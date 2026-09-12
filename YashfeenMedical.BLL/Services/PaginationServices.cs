using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.QueryModels;
using Microsoft.EntityFrameworkCore;

namespace YashfeenMedical.BLL.Services
{
    public class PaginationServices : IPaginationServices
    {
        public async Task<TPaginationQueryModel<T>> GetPaggedList<T>(IQueryable<T> entities, PaginationQuery query) where T : class
        {
            var pageNumber = query?.PageNumber > 0 ? query.PageNumber : 1;
            var pageSize = query?.PageSize > 0 ? query.PageSize : 10;
            pageSize = Math.Min(pageSize, 50);

            var totalCount = await entities.CountAsync();

            var pagedList = await entities.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new TPaginationQueryModel<T>
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
