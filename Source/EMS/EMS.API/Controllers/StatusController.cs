using EMS.API.Hubs.IHubs;
using EMS.API.Hubs;
using EMS.DAL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using EMS.DAL.Implementation;
using EMS.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusMgr _statusMgr;
       private readonly IModuleMgr _moduleMgr;
        private readonly ILogger<StatusController> _logger;


        public StatusController(IStatusMgr statusMgr, IModuleMgr moduleMgr, ILogger<StatusController> logger)
        {
            _statusMgr = statusMgr;
            _moduleMgr = moduleMgr;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("GetStatusList")]
        public async Task<ActionResult<List<AllStatusesDTO>>> GetStatusList()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    var userIdClaim = identity.FindFirst(ClaimTypes.PrimarySid)?.Value;
                    if (int.TryParse(userIdClaim, out int userId))
                    {
                        // Check if the current user is an admin
                        var isAdmin = await _moduleMgr.IsAdminAsync(userId);
                        if (!isAdmin)
                        {
                            return StatusCode(StatusCodes.Status403Forbidden, "Invalid user permissions");
                        }

                        // Proceed to get the status list if the user is an admin
                        var statuses = _statusMgr.GetStatusList(); // Make sure to await this if it's an async method
                        return Ok(statuses);
                    }
                }

                return BadRequest("Invalid user ID");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error! Please contact the Server Administrator!");
            }
        }



        [Authorize]
        [HttpPost("AddStatus")]
        public async Task<ActionResult<StatusDTOListResult>> AddStatus([FromBody] StatusAddDTO statusAddDTO)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    var userIdClaim = identity.FindFirst(ClaimTypes.PrimarySid)?.Value;
                    if (int.TryParse(userIdClaim, out int userId))
                    {
                        var isAdmin = await _moduleMgr.IsAdminAsync(userId);
                        if (!isAdmin)
                        {
                            return StatusCode(StatusCodes.Status403Forbidden, "Invalid user permissions");
                        }

                        var result = await _statusMgr.AddStatusAsync(statusAddDTO, userId);
                        if (result.ResultCode == 0)
                        {
                            return Ok(result);
                        }
                        return BadRequest(result.ResultDescription);
                    }
                }
                return BadRequest("Invalid user ID");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error! Please contact the Server Administrator!");
            }
        }


        [Authorize]
        [HttpPut("EditStatus")]
        public async Task<IActionResult> EditStatus([FromBody] StatusEditDTO statusEditDTO)
        {
            if (statusEditDTO == null || statusEditDTO.StatusId <= 0 || string.IsNullOrEmpty(statusEditDTO.StatusName))
            {
                return BadRequest("Invalid status details.");
            }

            // Retrieve the current user ID from claims
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userIdClaim = identity.FindFirst(ClaimTypes.PrimarySid)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    // Check if the user is an admin
                    var isAdmin = await _moduleMgr.IsAdminAsync(userId);
                    if (!isAdmin)
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, "Invalid user permissions");
                    }

                    // Proceed with editing the status
                    var result = await _statusMgr.EditStatus(statusEditDTO, userId);
                    if (result.ResultCode == 0)
                    {
                        return Ok(result.ResultDescription);
                    }
                    else
                    {
                        return StatusCode(500, result.ResultDescription);
                    }
                }
            }

            return BadRequest("Invalid user ID");
        }


        [Authorize]
        [HttpDelete("DeleteStatus/{statusId}")]
        public async Task<IActionResult> DeleteStatusAsync(int statusId)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    var userIdClaim = identity.FindFirst(ClaimTypes.PrimarySid)?.Value;
                    if (int.TryParse(userIdClaim, out int userId))
                    {
                        var isAdmin = await _moduleMgr.IsAdminAsync(userId);
                        if (!isAdmin)
                        {
                            return StatusCode(StatusCodes.Status403Forbidden, "Invalid user permissions");
                        }

                        var result = await _statusMgr.DeleteStatusAsync(statusId,userId);
                        if (result.ResultCode == 0)
                        {
                            return Ok(result); 
                        }
                        return BadRequest(result.ResultDescription); 
                    }
                }
                return BadRequest("Invalid user ID");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error! Please contact the Server Administrator!");
            }

        }
    }
}
