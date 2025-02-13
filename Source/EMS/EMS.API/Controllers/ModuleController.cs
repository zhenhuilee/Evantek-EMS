using EMS.DAL.Implementation;
using EMS.DAL.Interface;
using EMS.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleMgr _moduleMgr;
        private readonly ILogger<ModuleController> _logger;

        public ModuleController(IModuleMgr moduleMgr, ILogger<ModuleController> logger)
        {
            _moduleMgr = moduleMgr;
            _logger = logger;
        }


        [Authorize]
        [HttpGet("GetRoles")]

        public async Task<ActionResult<List<RoleDTO>>> GetRoles()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userIdClaim = identity.FindFirst(ClaimTypes.PrimarySid)?.Value;

                if (int.TryParse(userIdClaim, out int userId))
                {
                    var roles = await _moduleMgr.GetRolesByUserIdAsync(userId);
                    return Ok(roles);
                }

            }
            return BadRequest("Invalid user ID");
        }


        [Authorize]
        [HttpGet("GetModulesByRole/{roleId}")]
        public async Task<ActionResult<List<ModuleDTO>>> GetModulesByRole(int roleId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userIdClaim = identity.FindFirst(ClaimTypes.PrimarySid)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    try
                    {
                        var modules = await _moduleMgr.GetModulesByRoleIdAsync(userId, roleId);
                        return Ok(modules);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        _logger.LogWarning(ex, ex.Message);
                        return StatusCode(StatusCodes.Status403Forbidden, "User does not have access to the specified role.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.StackTrace);
                        return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error! Please contact the Server Administrator!");
                    }
                }
            }
            return BadRequest("Invalid user ID");
        }

        [Authorize]
        [HttpGet("IsAdmin/{userId}")]
        public async Task<ActionResult<bool>> IsAdmin(int userId)
        {
            try
            {
                // Check if the user is an admin
                var isAdmin = await _moduleMgr.IsAdminAsync(userId);

                if (isAdmin)
                {
                    return Ok(true);
                }
                return Ok(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error! Please contact the Server Administrator!");
            }
        }



    }
}
