using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.DTOs.MedicalRecords;
using YashfeenMedical.BLL.IServices;

namespace YashfeenMedical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordsController : BaseController<int, IMedicalRecordServices, MedicalRecordDto, MedicalRecordCreationDto, MedicalRecordUpdateDto>
    {
        private readonly IMedicalRecordServices _services;

        public MedicalRecordsController(IMedicalRecordServices services) : base(services)
        {
            _services = services;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, MedicalRecordUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != updateDto.Id)
                return BadRequest("Id in URL does not match Id in body.");

            var result = await Edit(id, updateDto);
            return Ok(result);
        }
    }
}
