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


        protected virtual async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _services.GetAll(paginationQuery);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Details(TId id)
        {
            var entity = await _services.Details(id);

            return Ok(entity);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Add([FromBody] TCreationDto creationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _services.Add(creationDto);

            return CreatedAtAction(nameof(Details), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Edit(TId id, [FromBody] TUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!id.Equals(updateDto.Id))
                return BadRequest("Id in URL does not match Id in body.");

            var result = await _services.Update(id, updateDto);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(TId id)
        {
            await _services.Delete(id);

            return NoContent();
        }
    }
}
