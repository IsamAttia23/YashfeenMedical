using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.QueryModels;
using YashfeenMedical.DAL.Shared.Entities;

namespace YashfeenMedical.API.Controllers
{
    [ApiController]
    public class BaseController<TId, TServcices, TDto, TCreationDto, TUpdateDto> : ControllerBase
        where TId : struct
        where TServcices : class, IEntityServices<TId, TDto, TCreationDto, TUpdateDto>
        where TDto : class, TIdType<TId>
        where TCreationDto : class
        where TUpdateDto : class, TIdType<TId>
    {
        protected readonly TServcices _services;

        public BaseController(TServcices services)
        {
            _services = services;
        }


        protected virtual async Task<TPaginationQueryModel<TDto>> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var result = await _services.GetAll(paginationQuery);

            return result;
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Details(TId id)
        {
            var entity = await _services.Details(id);

            return Ok(entity);
        }

        [HttpPost]
        protected virtual async Task<TDto> Add([FromBody] TCreationDto creationDto)
        {
            var entity = await _services.Add(creationDto);

            return entity;
        }

        [HttpPut("{id}")]
        protected virtual async Task<TDto> Edit(TId id, [FromBody] TUpdateDto updateDto)
        {
            var result = await _services.Update(id, updateDto);

            return result;
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(TId id)
        {
            await _services.Delete(id);

            return NoContent();
        }
    }
}
