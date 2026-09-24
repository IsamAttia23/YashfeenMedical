using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AppointmentController : BaseController<int, IAppointmentServices, AppointmentDto, AppointmentCreationDto, AppointmentUpdateDto>
    {
        private readonly IAppointmentServices _appointmentServices;

        public AppointmentController(IAppointmentServices services) : base(services)
        {
            _appointmentServices = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointmentsAsync([FromQuery] AppointmentQueryModel queryModel)
        {
            var result = await _appointmentServices.GetFilterdAppointmentsAsync(queryModel);
            return Ok(result);
        }

        [HttpGet("/today")]
        public async Task<IActionResult> BringAppointmentsTodayAsync([FromQuery] AppointmentQueryModel queryModel)
        {
            var result = await _appointmentServices.BringAppointmentsTodayAsync(queryModel);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(AppointmentCreationDto creationDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await Add(creationDto);
            return CreatedAtAction(nameof(Details), new { id = result.Id }, result);
        }
    }
}
