using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.IRepositories;
using YashfeenMedical.DAL.QueryModels;
using YashfeenMedical.DAL.Shared.Entities;
using YashfeenMedical.Infrastructure.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace YashfeenMedical.BLL.Services;

public abstract class TEntityService<TEntity, TId, TDto, TCreationDto, TUpdateDto> :
    IEntityServices<TId, TDto, TCreationDto, TUpdateDto>

    where TEntity : class, IEntity<TId>
    where TId : struct
    where TDto : class, TIdType<TId>
    where TCreationDto : class
    where TUpdateDto : class, TIdType<TId>
{
    private readonly IRepository<TEntity, TId> _repository;
    private readonly IMapper _mapper;

    public TEntityService(IRepository<TEntity, TId> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual async Task<TDto> Add(TCreationDto creationDTO)
    {
        var entity = _mapper.Map<TEntity>(creationDTO);

        entity.CreatedOn = DateTimeOffset.UtcNow;

        await _repository.Add(entity);
        await _repository.SaveChanges();

        var result = _mapper.Map<TDto>(entity);

        return result;
    }

    public virtual async Task Delete(TId id)
    {
        var entity = await Details(id);

        await _repository.Delete(id);
        await _repository.SaveChanges();
    }

    public virtual async Task<TPaginationQueryModel<TDto>> GetAll(PaginationQuery query)
    {
        var entities = await _repository.GetAll();

        var result = await GetPaggedList(entities.ProjectToType<TDto>(), query);

        return result;
    }

    public virtual async Task<TDto?> Details(TId id)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
            throw new NotFoundException("The request entity dosen't exits");

        var result = _mapper.Map<TDto>(entity);

        return result;
    }

    public virtual async Task<TDto> Update(TId id, TUpdateDto updateDto)
    {
        var entity = await _repository.GetById(id);

        if (entity == null)
            throw new NotFoundException("The request entity dosen't exits");

        var mappedEntity = _mapper.Map(updateDto, entity);

        mappedEntity.UpdatedOn = DateTimeOffset.UtcNow;

        await _repository.Update(mappedEntity);
        await _repository.SaveChanges();

        var result = _mapper.Map<TDto>(mappedEntity);
        return result;
    }

    public async Task<TPaginationQueryModel<TDto>> GetPaggedList(IQueryable<TDto> entities, PaginationQuery query)
    {
        var pageNumber = query?.PageNumber > 0 ? query.PageNumber : 1;
        var pageSize = query?.PageSize > 0 ? query.PageSize : 10;
        pageSize = Math.Min(pageSize, 50);

        var totalCount = await entities.CountAsync();

        var pagedList = await entities.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ProjectToType<TDto>()
            .ToListAsync();

        var result = new TPaginationQueryModel<TDto>
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

