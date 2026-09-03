using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.DTOs.Patients;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : BaseController<int, IPatientServices, PatientDto, PatientCreationDto, PatientUpdateDto>
    {
        private readonly IPatientServices _patientServices;

        public PatientsController(IPatientServices services) : base(services)
        {
            _patientServices = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatientsAsync([FromQuery] PatientQueryModel patientQuery)
        {
            var patients = await _patientServices.GetFilterdPatients(patientQuery);
            return Ok(patients);
        }

        [HttpGet("{id}/appointments")]
        public async Task<IActionResult> GetPatientAppointments(int id, [FromQuery] PatientAppointmentsQueryModel queryModel)
        {
            var result = await _patientServices.GetPaitentAppointments(queryModel, id);
            return Ok(result);
        }
    }
}
