using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.DTOs.MedicalRecords;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.BLL.IStateMachines;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AppointmentController : BaseController<int, IAppointmentServices, AppointmentDto, AppointmentCreationDto, AppointmentUpdateDto>
    {
        private readonly IAppointmentServices _appointmentServices;
        private readonly IAppointmentStateMachine _stateMachine;

        public AppointmentController(IAppointmentServices services, IAppointmentStateMachine stateMachine) : base(services)
        {
            _appointmentServices = services;
            _stateMachine = stateMachine;
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await Add(creationDto);
            return CreatedAtAction(nameof(Details), new { id = result.Id }, result);
        }

        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> ConfirmAppointmentAsync(int id)
        {
            var result = await _stateMachine.ConfirmAppointmentAsync(id);
            return Ok(result);

        }

        [HttpPut("{id}/start")]
        public async Task<IActionResult> StartAppointmentAsync(int id)
        {
            var result = await _stateMachine.StartAppointmentAsync(id);
            return Ok(result);

        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteAppointmentAsync(int id,MedicalRecordCreationDto medicalRecord)
        {
            var result = await _stateMachine.CompleteAppointmentAsync(id,medicalRecord);
            return Ok(result);

        }
    }
}
