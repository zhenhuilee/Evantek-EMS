using EMS.DataAccess.Models;
using EMS.DAL.Interface;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using EMS.DTO;
using Microsoft.EntityFrameworkCore;
using EMS.Helper;

namespace EMS.DAL.Implementation
{
    public class AttendanceMgr : IAttendanceMgr
    {
        private readonly EmsDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AttendanceMgr> _logger;
        private readonly IAuditMgr _auditMgr;

        public AttendanceMgr(EmsDbContext dbContext, IMapper mapper, ILogger<AttendanceMgr> logger, IAuditMgr auditMgr)
        {
            _context = dbContext;
            _mapper = mapper;
            _logger = logger;
            _auditMgr = auditMgr;
        }

        public List<CategoryStatusDTO> GetStatusList()
        {
            var categories = _context.Categories.ToList();
            var statuses = _context.Statuses.ToList();

            var statusList = categories.Select(category => new CategoryStatusDTO
            {
                CategoryName = category.Name,
                Statuses = statuses
                    .Where(status => status.CategoryId == category.Id)
                    .Select(status => new StatusDTO
                    {
                        Id = status.Id,
                        Name = status.Name
                    })
                    .ToList()
            }).ToList();

            return statusList;
        }

        public UserStatusDTO GetUserAttendance(int userId)
        {
            var userAttendance = _context.UserStatuses
                .Where(us => us.UserId == userId)
                .Select(us => new UserStatusDTO
                {
                    Note = us.Note,
                    StatusId = us.StatusId
                }).FirstOrDefault();

            return userAttendance;
        }


        public UserUpdateStatusDTOResult UpdateUserAttendance(int userId, int statusId, string note)
        {
            UserUpdateStatusDTOResult userUpdateStatusDTOResult = new UserUpdateStatusDTOResult();
            userUpdateStatusDTOResult.ResultCode = 0;
            userUpdateStatusDTOResult.ResultDescription = "User status updated successfully!";

            // Validate the length of the note
            if (note.Length > 50)
            {
                _logger.LogWarning($"Note exceeds 50 characters for userId {userId}");
                userUpdateStatusDTOResult.ResultCode = 2;
                userUpdateStatusDTOResult.ResultDescription = "Note cannot exceed 50 characters.";
                return userUpdateStatusDTOResult;
            }

            var userStatus = _context.UserStatuses.Include(x => x.Status).FirstOrDefault(us => us.UserId == userId);
            if (userStatus == null)
            {
                _logger.LogError($"User status not found for userId {userId}");
                userUpdateStatusDTOResult.ResultCode = 1;
                userUpdateStatusDTOResult.ResultDescription = $"User status not found for userId {userId}";
                return userUpdateStatusDTOResult;
            }

            // Fetch the old status and category
            var oldStatus = _context.Statuses.Include(s => s.Category).FirstOrDefault(s => s.Id == userStatus.StatusId);
            if (oldStatus == null)
            {
                _logger.LogError("Old status not found.");
                throw new Exception("Old status not found.");
            }

            var oldNote = userStatus.Note;
            // Update the status and note
            userStatus.StatusId = statusId;
            userStatus.Note = note;
            userStatus.LastUpdated = DateTime.Now;

            var update = _context.UserStatuses.Update(userStatus);
            _context.SaveChanges();

            string auditDescription = $"Edited my attendance: ";
            bool hasChanges = false;

            UserUpdateStatusDTO userUpdateStatusDTO = new UserUpdateStatusDTO
            {
                UserId = (update.Entity.UserId ?? 0),
                Note = update.Entity.Note,
                LastUpdated = update.Entity.LastUpdated
            };

            // Fetch the new status and category
            var newStatus = _context.Statuses.Include(s => s.Category).FirstOrDefault(s => s.Id == update.Entity.StatusId);
            if (newStatus == null)
            {
                _logger.LogError("New status not found.");
                throw new Exception("New status not found.");
            }

            userUpdateStatusDTO.StatusName = newStatus.Name;
            userUpdateStatusDTOResult.UserUpdateStatusDTO = userUpdateStatusDTO;


            // Check if status or category has changed
            if (userStatus.StatusId != oldStatus.Id)
            {
                auditDescription += $"Status changed from '{oldStatus.Category.Name} - {oldStatus.Name}' " +
                                    $"to '{newStatus.Category.Name} - {newStatus.Name}'. ";
                hasChanges = true;
            }


            // Check if note has changed
            if (oldNote != note)
            {
                auditDescription += $"Note changed from '{oldNote}' to '{note}'. ";
                hasChanges = true;
            }
             
            if (!hasChanges)
            {
                auditDescription = "No changes made to user's info.";
            }

            _auditMgr.CreateAuditAsync(userId, 6, ActivityTypeEnum.EDIT, auditDescription);

            return userUpdateStatusDTOResult;
        }


    }
}
