using EMS.API.Hubs;
using EMS.API.Hubs.IHubs;
using EMS.DAL.Interface;
using EMS.DataAccess.Models;
using EMS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceMgr _attendanceMgr;
        private readonly ILogger<AttendanceController> _logger;
        private readonly IHubContext<DisplayHub, IDisplayHub> _displayHub;


        public AttendanceController(IAttendanceMgr attendanceMgr, ILogger<AttendanceController> logger, IHubContext<DisplayHub, IDisplayHub> displayHub)
        {
            _attendanceMgr = attendanceMgr;
            _logger = logger;
            _displayHub = displayHub;
        }

        // GET: api/<AttendanceController>/statuslist
        [Authorize]
        [HttpGet("GetStatuslist")]
        public ActionResult<List<CategoryStatusDTO>> GetStatusList()
        {
            var statusList = _attendanceMgr.GetStatusList();
            return Ok(statusList);
        }


        // GET: api/<AttendanceController>/userattendance
        [Authorize]
        [HttpGet("GetUserAttendance")]
        public ActionResult<UserStatusDTO> GetUserAttendance()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.PrimarySid);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                _logger.LogError("User ID claim not found or invalid.");
                return BadRequest("User ID claim not found or invalid.");
            }

            var userAttendance = _attendanceMgr.GetUserAttendance(userId);
            return Ok(userAttendance);
        }



        // PUT: api/<AttendanceController>/updateattendance
        [Authorize]
        [HttpPut("UpdateAttendance")]
        public ActionResult UpdateAttendance([FromBody] UserStatusDTO userStatusDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.PrimarySid);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                _logger.LogError("User ID claim not found or invalid.");
                return BadRequest("User ID claim not found or invalid.");
            }

            var updateResult = _attendanceMgr.UpdateUserAttendance(userId, userStatusDto.StatusId, userStatusDto.Note);
            if (updateResult.ResultCode != 0)
            {
                _logger.LogError($"Failed to update attendance for userId {userId}");
                return StatusCode(500, "Internal server error");
            }
            else
            {
                _displayHub.Clients.All.UpdateUserStatus(updateResult.UserUpdateStatusDTO.UserId, updateResult.UserUpdateStatusDTO.StatusName, updateResult.UserUpdateStatusDTO.Note, updateResult.UserUpdateStatusDTO.LastUpdated);
            }
            return Ok(updateResult);
        }
    }
}