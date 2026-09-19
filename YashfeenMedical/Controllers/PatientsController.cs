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

        [HttpGet("{id}/medical-records")]
        public async Task<IActionResult> GetPatientMedicalRecords(int id, [FromQuery] PaginationQuery queryModel)
        {
            var result = await _patientServices.GetPaitentMedicalRecords(queryModel, id);
            return Ok(result);
        }

        [HttpGet("{id}/prescriptions")]
        public async Task<IActionResult> GetPatientPrescriptions(int id, [FromQuery] PaginationQuery queryModel)
        {
            var result = await _patientServices.GetPaitentPrescriptions(queryModel, id);
            return Ok(result);
        }

        [HttpGet("{id}/invoices")]
        public async Task<IActionResult> GetPatientInvoices(int id, [FromQuery] PaginationQuery queryModel)
        {
            var result = await _patientServices.GetPaitentInvoices(queryModel, id);
            return Ok(result);
        }

        [HttpGet("{id}/files")]
        public async Task<IActionResult> GetPatientMedicalFiles(int id, [FromQuery] PaginationQuery queryModel)
        {
            var result = await _patientServices.GetPaitentMedicalFiles(queryModel, id);
            return Ok(result);
        }

        [HttpPatch("{id}/toggle-activity")]
        public async Task<IActionResult> TogglePatientActivity(int id)
        {
            var result = await _patientServices.TogglePatientActivitiy(id);
            return Ok(result);
        }

        [HttpPost("{id}/Photo")]
        public async Task<IActionResult> UploadPatientPhoto(int id, IFormFile profilePhoto)
        {
            var result = await _patientServices.UploadPatientPhoto(id, profilePhoto);
            return Ok("Patient photo uploaded successfully.");
        }
    }
}
