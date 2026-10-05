using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.DTOs.Prescriptions;
using YashfeenMedical.BLL.IServices;
using YashfeenMedical.BLL.IStateMachines;

namespace YashfeenMedical.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : BaseController<int, IPrescriptionServices, PrescriptionDto, PrescriptionCreationDto, PrescriptionUpdateDto>
    {
        private readonly IPrescriptionServices _services;
        private readonly IPrescriptionStateMachine _stateMachine;

        public PrescriptionsController(IPrescriptionServices services, IPrescriptionStateMachine stateMachine) : base(services)
        {
            _services = services;
            _stateMachine = stateMachine;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PrescriptionCreationDto creationDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await Add(creationDto);
            return CreatedAtAction(nameof(Details), new { id = created.Id }, created);
        }

        [HttpPost("{id}/dispense")]
        public async Task<IActionResult> Dispense(int id)
        {
            var result = await _stateMachine.Dispense(id);
            return Ok(result);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _stateMachine.Cancel(id);
            return Ok(result);
        }
    }
}
