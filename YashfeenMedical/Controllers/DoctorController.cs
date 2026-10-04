using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.DTOs.Appointments;
using YashfeenMedical.BLL.DTOs.Doctors;
using YashfeenMedical.BLL.DTOs.DoctorSchedules;
using YashfeenMedical.BLL.DTOs.Patients;
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
        public async Task<IActionResult> CreateAsync(DoctorCreationDto creationDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await Add(creationDto);
            return CreatedAtAction(nameof(Details), new { id = result.Id }, result);
        }

        [HttpGet("{id}/schedule")]
        public async Task<IActionResult> GetDoctorScheduleAsync(int id,[FromQuery] PaginationQuery paginationQuery)
        {
            var result = await _services.GetDoctorSchedule(id, paginationQuery);

            return Ok(result);
        }

        [HttpGet("{id}/available-slots")]
        public async Task<IActionResult> GetAvailableSlots(int id, DateOnly date)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _services.GetDoctorScheduleOnDayAsync(id, date);
            return Ok(result);
        }

        [HttpPost("{id}/schedule")]
        public async Task<IActionResult> UpsertDoctorSchedule(int id, DoctorScheduleUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _services.UpsertSchedule(id, updateDto);
            return Ok(result);
        }

        [HttpPatch("{id}/toggle-activity")]
        public async Task<IActionResult> ToggleDoctorActivity(int id)
        {
            var result = await _services.ToggleDoctorActivitiy(id);
            return Ok(result);
        }

        [HttpPost("{id}/Photo")]
        public async Task<IActionResult> UploadPatientPhoto(int id, IFormFile profilePhoto)
        {
            var result = await _services.UploadDoctorPhoto(id, profilePhoto);
            return Ok("Doctor photo uploaded successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, DoctorUpdateDto updateDto)
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
