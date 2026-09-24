using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.DTOs.DoctorSchedules;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorScheduleController : BaseController<int, IDoctorScheduleServices, DoctorScheduleDto, DoctorScheduleCreationDto, DoctorScheduleUpdateDto>
    {
        private readonly IDoctorScheduleServices _services;

        public DoctorScheduleController(IDoctorScheduleServices services) : base(services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSchedules([FromQuery] PaginationQuery paginationQuery)
        {
            return await GetAll(paginationQuery);
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetDoctorSchedules(int doctorId, [FromQuery] PaginationQuery paginationQuery)
        {
            var result = await _services.GetDoctorSchedule(doctorId, paginationQuery);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] DoctorScheduleCreationDto creationDto)
        {
           var result = await Add(creationDto);
           return Ok(result);
        }
    }
}
