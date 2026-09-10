using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.DTOs.Doctors;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.BLL.Services;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : BaseController<int, IDoctorServices, DoctorDto, DoctorCreationDto, DoctorUpdateDto>
    {
        private readonly IDoctorServices _services;

        public DoctorController(IDoctorServices services) : base(services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorsAsync([FromQuery] DoctorQueryModel doctorQuery)
        {
            var patients = await _services.GetFilteredDoctorsWithPaginationAsync(doctorQuery);
            return Ok(patients);
        }

        [HttpPost]
        public async  Task<IActionResult> CreateAsync(DoctorCreationDto creationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await Add(creationDto);
            return CreatedAtAction(nameof(Details), new { id = result.Id }, result);
        }
    }
}
