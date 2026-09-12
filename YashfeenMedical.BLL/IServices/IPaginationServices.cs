using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.DAL.QueryModels;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.IServices
{
    public interface IPaginationServices
    {
        Task<TPaginationQueryModel<T>> GetPaggedList<T>(IQueryable<T> entities, PaginationQuery query) where T : class;
    }
}
