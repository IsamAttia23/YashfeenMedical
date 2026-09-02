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
        public async Task<IActionResult> GetPatientsAsync([FromQuery] PatientQueryModel patientQuery, [FromQuery] PaginationQuery paginationQuery)
        {
            var patients = await _patientServices.GetFilterdOrders(patientQuery, paginationQuery);
            return Ok(patients);
        }
    }
}
