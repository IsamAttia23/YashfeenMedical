using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.API.Controllers
{
    public class AppointmentController : BaseController<int, IAppointmentServices, AppointmentDto, AppointmentCreationDto, AppointmentUpdateDto>
    {
        private readonly IAppointmentServices _appointmentServices;

        public AppointmentController(IAppointmentServices services) : base(services)
        {
            _appointmentServices = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointmentsAsync(AppointmentQueryModel queryModel)
        {
            var result = await _appointmentServices.GetFilterdAppointmentsAsync(queryModel);
            return Ok(result);
        }

        [HttpGet("today")]
        public async Task<IActionResult> BringAppointmentsTodayAsync(AppointmentQueryModel queryModel , DateOnly date)
        {
            var result = await _appointmentServices.BringAppointmentsTodayAsync(queryModel, date);
            return Ok(result);
        }
    }
}
