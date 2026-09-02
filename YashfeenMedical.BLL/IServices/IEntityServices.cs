using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.QueryModels;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.BLL.IServices
{
    public interface IEntityServices<TEntity, TId, TDto, TCreationDto, TUpdateDto>
        where TEntity : class, IEntity<TId>
        where TId : struct
        where TDto : class , TIdType<TId>
        where TCreationDto : class
        where TUpdateDto : class, TIdType<TId>
    {
        Task<TDto?> Details(TId id);
        Task<TPaginationQueryModel<TDto>> GetAll(PaginationQuery query);
        Task<TDto> Add(TCreationDto creationDTO);
        Task Delete(TId id);
        Task<TDto> Update(TId id, TUpdateDto updateDto);
    }
}
