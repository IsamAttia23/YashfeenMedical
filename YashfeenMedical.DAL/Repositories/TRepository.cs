using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.DAL.Repositories
{
    public abstract class TRepository<TEntity, TId> : IRepository<TEntity, TId>
         where TEntity : class, IEntity<TId>
         where TId : struct
    {
        private readonly DbContext _context;

        public abstract IQueryable<TEntity> SelectQuery { get; }

        protected IQueryable<TEntity> FinalQuery => SelectQuery.Where(x => x.DeletedOn == null);

        public TRepository(DbContext context)
        {
            _context = context;
        }


        public virtual async Task Add(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public virtual async Task Delete(TId id)
        {
            var entity = await GetById(id);
            entity.DeletedOn = DateTimeOffset.Now;
            await Update(entity);
        }

        public virtual async Task<TEntity?> GetById(TId id)
        {
            return await FinalQuery.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<bool> IsExists(TId id)
        {
            return await FinalQuery.AnyAsync(x => x.Id.Equals(id));
        }

        public virtual Task Update(TEntity entity)
        {
            _context.Update(entity);
            return Task.CompletedTask;
        }

        public virtual async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
        public void SetRowVersion<Entity>(Entity entity, byte[] rowVersion) where Entity : class, IRowVersionProperty
        {
            _context.Entry(entity).Property(e => e.RowVersion).OriginalValue = rowVersion;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return FinalQuery;
        }

        public async Task<bool> HasActiveRelationsAsync(TEntity entity)
        {
            var entityType = _context.Model.FindEntityType(typeof(TEntity));
            if (entityType == null)
                return false;

            var method = typeof(TRepository<TEntity, TId>)
                .GetMethod(nameof(AnyDependentAsync), BindingFlags.NonPublic | BindingFlags.Instance)!;

            foreach (var fk in entityType.GetReferencingForeignKeys())
            {
                var principalKey = fk.PrincipalKey.Properties[0];
                var foreignKey = fk.Properties[0];

                var id = _context.Entry(entity).Property(principalKey.Name).CurrentValue;
                if (id == null)
                    continue;

                var dependentType = fk.DeclaringEntityType.ClrType;
                var hasDeletedOn = fk.DeclaringEntityType.FindProperty("DeletedOn") != null;

                var task = (Task<bool>)method
                    .MakeGenericMethod(dependentType)
                    .Invoke(this, new object[] { foreignKey, id, hasDeletedOn })!;

                if (await task)
                    return true;
            }

            return false;
        }

        private async Task<bool> AnyDependentAsync<TDependent>(
            IProperty foreignKey, object id, bool hasDeletedOn) where TDependent : class
        {
            var param = Expression.Parameter(typeof(TDependent), "x");

            // EF.Property<TFk>(x, "ForeignKeyName") == id
            var fkAccess = Expression.Call(
                typeof(EF), nameof(EF.Property), new[] { foreignKey.ClrType },
                param, Expression.Constant(foreignKey.Name));

            Expression body = Expression.Equal(fkAccess, Expression.Constant(id, foreignKey.ClrType));

            // && EF.Property<DateTimeOffset?>(x, "DeletedOn") == null
            if (hasDeletedOn)
            {
                var deletedOn = Expression.Call(
                    typeof(EF), nameof(EF.Property), new[] { typeof(DateTimeOffset?) },
                    param, Expression.Constant("DeletedOn"));

                body = Expression.AndAlso(
                    body,
                    Expression.Equal(deletedOn, Expression.Constant(null, typeof(DateTimeOffset?))));
            }

            var lambda = Expression.Lambda<Func<TDependent, bool>>(body, param);
            return await _context.Set<TDependent>().AnyAsync(lambda);
        }
    }
}
