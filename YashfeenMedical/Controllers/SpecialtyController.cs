using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.DTOs.Specialties;
using YashfeenMedical.BLL.IServices;

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
    }
}
