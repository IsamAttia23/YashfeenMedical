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
        Task<IQueryable<TEntity>> GetAll();
        Task<TEntity?> GetById(TId id);
        Task Add(TEntity entity);
        Task Delete(TId id);
        Task Update(TEntity entity);
        Task<bool> IsExists(TId id);
        Task SaveChanges();
    }
}
