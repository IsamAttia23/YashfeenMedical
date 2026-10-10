using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YashfeenMedical.BLL.RecycleBin;
using YashfeenMedical.DAL.QueryModels;

namespace YashfeenMedical.API.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class RecycleBinController : ControllerBase
    {
        private readonly RecycleBinService _service;

        public RecycleBinController(RecycleBinService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string entityType, [FromQuery] PaginationQuery query)
        {
            if (string.IsNullOrWhiteSpace(entityType)) return BadRequest("entityType is required");

            if (!_service.IsSupported(entityType)) return BadRequest("Unsupported entity type");

            var result = await _service.List(entityType, query);
            return Ok(result);
        }

        [HttpPost("{entityType}/{id}/restore")]
        public async Task<IActionResult> Restore(string entityType, int id)
        {
            await _service.Restore(entityType, id);
            return Ok();
        }

        [HttpDelete("{entityType}/{id}")]
        public async Task<IActionResult> HardDelete(string entityType, int id)
        {
            await _service.HardDelete(entityType, id);
            return NoContent();
        }
    }
}
