using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.QueryModels;
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
            entity.DeletedOn = DateTimeOffset.UtcNow;
            await Update(entity);
        }

        public async Task<TPaginationQueryModel<TEntity>> GetAll(PaginationQuery query)
        {
            var list = GetPaggedList(FinalQuery, query);
            return await list;
        }

        public async Task<TEntity?> GetById(TId id)
        {
            return await FinalQuery.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> IsExists(TId id)
        {
            return await FinalQuery.AnyAsync(x => x.Id.Equals(id));
        }

        public Task Update(TEntity entity)
        {
            _context.Update(entity);
            return Task.CompletedTask;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<TPaginationQueryModel<TEntity>> GetPaggedList(IQueryable<TEntity> entities, PaginationQuery query)
        {
            var pageNumber = query?.PageNumber > 0 ? query.PageNumber : 1;
            var pageSize = query?.PageSize > 0 ? query.PageSize : 10;
            pageSize = Math.Min(pageSize, 50);

            var totalCount = await entities.CountAsync();

            var pagedList = await entities.Skip((pageNumber - 1) * pageSize)
                .Take(query.PageSize).ToListAsync();

            var result = new TPaginationQueryModel<TEntity>
            {
                Data = pagedList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };

            return result;
        }
    }
}
