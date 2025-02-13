using EMS.DAL.Interface;
using EMS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePasswordController : ControllerBase
    {
        private readonly IChangePasswordMgr _changepasswordMgr;
        private readonly ILogger<ChangePasswordController> _logger;

        public ChangePasswordController(IChangePasswordMgr changepasswordMgr, ILogger<ChangePasswordController> logger)
        {
            _changepasswordMgr = changepasswordMgr;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<ResultDTO>> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userIdClaim = identity.FindFirst(ClaimTypes.PrimarySid)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    var result = await _changepasswordMgr.ChangePasswordAsync(userId, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword, changePasswordDTO.ConfirmPassword);
                    if (result.ResultCode == 0)
                    {
                        return Ok(result);
                    }
                    return BadRequest(result.ResultDescription);
                }
            }
            return BadRequest("Invalid user ID");
        }


        [Authorize]
        [HttpPost("ValidateCurrentPassword")]
        public async Task<ActionResult<bool>> ValidateCurrentPassword([FromBody] ValidatePasswordDTO validatePasswordDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userIdClaim = identity.FindFirst(ClaimTypes.PrimarySid)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    var isValid = await _changepasswordMgr.ValidateCurrentPasswordAsync(userId, validatePasswordDTO.CurrentPassword);
                    return Ok(isValid);
                }
            }
            return BadRequest("Invalid user ID");
        }
    }
}
