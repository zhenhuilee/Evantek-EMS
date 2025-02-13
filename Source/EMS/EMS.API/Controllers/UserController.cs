using EMS.API.Hubs.IHubs;
using EMS.API.Hubs;
using EMS.DAL.Implementation;
using EMS.DAL.Interface;
using EMS.DataAccess.Models;
using EMS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserMgr _userMgr;
        private readonly IModuleMgr _moduleMgr;
        private readonly ILogger<UserController> _logger;
        private readonly IHubContext<DisplayHub, IDisplayHub> _displayHub;

        public UserController(IUserMgr userMgr, IModuleMgr moduleMgr,ILogger<UserController> logger, IHubContext<DisplayHub, IDisplayHub> displayHub)
        {
            _userMgr = userMgr;
            _moduleMgr = moduleMgr;
            _logger = logger;
            _displayHub = displayHub;
        }

        /*
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
                    var roles = await _userMgr.GetRolesByUserIdAsync(userId);
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
                        var modules = await _userMgr.GetModulesByRoleIdAsync(userId, roleId);
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
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<ResultDTO>> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userIdClaim = identity.FindFirst(ClaimTypes.PrimarySid)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    var result = await _userMgr.ChangePasswordAsync(userId, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword, changePasswordDTO.ConfirmPassword);
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
                    var isValid = await _userMgr.ValidateCurrentPasswordAsync(userId, validatePasswordDTO.CurrentPassword);
                    return Ok(isValid);
                }
            }
            return BadRequest("Invalid user ID");
        }
        */

        // /api/User/GetName
        [Authorize]
        [HttpGet("GetName")]
        public async Task<ActionResult<string>> GetName()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var name = identity.FindFirst(ClaimTypes.Name)?.Value;
                //var id = identity.FindFirst(ClaimTypes.PrimarySid)?.Value;

                return Ok(name);

                //return Ok(name + "(" + id + ")");
            }
            return BadRequest();
        }


        [Authorize]
        [HttpGet("GetUserList")]
        public async Task<ActionResult<List<AllUsersDTO>>> GetUsersList()
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

                        // Proceed to get the user list if the user is an admin
                        var users = _userMgr.GetUsersList(); // Make sure to await this if it's an async method
                        return Ok(users);
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
        [HttpPost("AddUser")]
        public async Task<ActionResult<UserDTOListResult>> AddUser([FromBody] UserAddDTO newUser)
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

                        var addedUserResult = await _userMgr.AddUserAsync(newUser,userId);

                        if (addedUserResult.ResultCode == 0)
                        {
                            await _displayHub.Clients.All.AddUser(addedUserResult.UserDTO.Id, addedUserResult.UserDTO.Name, DateTime.Now);
                            return Ok(addedUserResult);
                        }
                        return BadRequest(addedUserResult.ResultDescription);
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
        [HttpPost("AdminChangePassword")]
        public async Task<ActionResult<ResultDTO>> AdminChangePassword([FromBody] AdminChangePasswordDTO adminChangePasswordDTO)
        {
            // First, verify if the current user making the request is an admin
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var currentUserIdClaim = identity.FindFirst(ClaimTypes.PrimarySid)?.Value;
                if (int.TryParse(currentUserIdClaim, out int currentUserId))
                {
                    // Check if the current user is an admin
                    var isAdmin = await _moduleMgr.IsAdminAsync(currentUserId);

                    if (!isAdmin)
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, "Invalid user permissions");
                    }

                    // Proceed with changing the password if the user is an admin
                    var result = await _userMgr.AdminChangePasswordAsync(adminChangePasswordDTO.UserId, adminChangePasswordDTO.NewPassword);
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
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<ActionResult<ResultDTO>> DeleteUser(int userId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userIdClaim = identity.FindFirst(ClaimTypes.PrimarySid)?.Value;
                if (int.TryParse(userIdClaim, out int currentUserId))
                {
                    // Check if the current user is an admin
                    var isAdmin = await _moduleMgr.IsAdminAsync(currentUserId);
                    if (!isAdmin)
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, "Invalid user permissions");
                    }

                    // Proceed with deletion if the current user is an admin
                    var result = await _userMgr.DeleteUserAsync(userId, currentUserId);
                    if (result.ResultCode == 0)
                    {
                        await _displayHub.Clients.All.DeleteUser(userId);
                        return Ok(result);
                    }
                    return BadRequest(result.ResultDescription);
                }
            }
            return BadRequest("Invalid user ID");
        }



        [Authorize]
        [HttpPost("EditUserInfo")]
        public async Task<ActionResult<ResultDTO>> EditUserInfo([FromBody] UserEditDTO userEditDTO)
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

                        // Perform the user edit operation
                        var result = await _userMgr.EditUserInfoAsync(userEditDTO,userId);

                        if (result.ResultCode == 0)
                        {
                            await _displayHub.Clients.All.EditUser(
                                userEditDTO.UserId,
                                userEditDTO.UserName,
                                result.Data?.ToString(),
                                userEditDTO.Note,
                                DateTime.Now
                            );

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

        /*
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
                        var statuses =  _userMgr.GetStatusList(); // Make sure to await this if it's an async method
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

                        var result = await _userMgr.AddStatusAsync(statusAddDTO);
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
                        
                    }

                    // Proceed with editing the status
                    var result = await _userMgr.EditStatus(statusEditDTO);
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

                        var result = await _userMgr.DeleteStatusAsync(statusId);
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
        [HttpGet("IsAdmin/{userId}")]
        public async Task<ActionResult<bool>> IsAdmin(int userId)
        {
            try
            {
                // Check if the user is an admin
                var isAdmin = await _userMgr.IsAdminAsync(userId);

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
        */
    }
}
