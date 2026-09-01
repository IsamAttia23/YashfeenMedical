using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.DAL.IRepositories
{
    public interface IRepository<TEntity, TId>
        where TEntity : class
        where TId : struct
    {
        Task<TEntity?> GetById(TId id);
        Task<TPaginationQueryModel<TEntity>> GetAll(PaginationQuery query);
        Task Add(TEntity entity);
        Task Delete(TId id);
        Task Update(TEntity entity);
        Task<bool> IsExists(TId id);
        Task<TPaginationQueryModel<TEntity>> GetPaggedList(IQueryable<TEntity> entities, PaginationQuery query);
        Task SaveChanges();
    }
}
