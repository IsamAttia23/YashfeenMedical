using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.DTOs.Specialties;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyController : BaseController<int, ISpecialtyServices, SpecialtyDto, SpecialtyCreationDto, SpecialtyUpdateDto>
    {
        private readonly ISpecialtyServices _services;

        public SpecialtyController(ISpecialtyServices services) : base(services)
        {
            _services = services;
        }

        [HttpPost]
        public async Task<IActionResult> Create(SpecialtyCreationDto creationDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await Add(creationDto);
            return CreatedAtAction(nameof(Details), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecialties([FromQuery] PaginationQuery paginationQuery)
        {
            var result = await GetAll(paginationQuery);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, SpecialtyUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!id.Equals(updateDto.Id))
                return BadRequest("Id in URL does not match Id in body.");

            var result = await Edit(id, updateDto);
            return Ok(result);
        }
    }
}
