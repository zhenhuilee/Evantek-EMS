using EMS.DAL.Interface;
using EMS.DTO;
using EMS.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditMgr _auditMgr;
        private readonly ILogger<AuditController> _logger;

        public AuditController(IAuditMgr auditMgr, ILogger<AuditController> logger)
        {
            _auditMgr = auditMgr;
            _logger = logger;
        }


        [HttpPost("CreateAudit")]
        public async Task<IActionResult> CreateAudit([FromBody] AuditDTO auditDto)
        {
            if (auditDto == null)
            {
                _logger.LogWarning("Audit data is null.");
                return BadRequest("Audit data is required.");
            }

            try
            {
                await _auditMgr.CreateAuditAsync(auditDto.UserId, auditDto.ModuleId, auditDto.WebActivity, auditDto.Description);
                _logger.LogInformation("Audit record created successfully.");
                return Ok("Audit record created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the audit record.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
