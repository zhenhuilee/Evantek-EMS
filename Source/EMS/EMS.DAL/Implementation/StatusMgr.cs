using AutoMapper;
using EMS.DAL.Interface;
using EMS.DataAccess.Models;
using EMS.DTO;
using EMS.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Implementation
{
    public class StatusMgr : IStatusMgr
    {
        private readonly EmsDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<StatusMgr> _logger;
        private readonly IAuditMgr _auditMgr;

        public StatusMgr(EmsDbContext dbContext, IMapper mapper, ILogger<StatusMgr> logger, IAuditMgr auditMgr)
        {
            _context = dbContext;
            _mapper = mapper;
            _logger = logger;
            _auditMgr = auditMgr;
        }

        public List<AllStatusesDTO> GetStatusList()
        {
            var statusList = (from status in _context.Statuses
                              join category in _context.Categories on status.CategoryId equals category.Id
                              select new AllStatusesDTO
                              {
                                  statusId = status.Id,
                                  statusName = status.Name,
                                  categoryId = category.Id,
                                  categoryName = category.Name
                              }).ToList();

            return statusList;
        }


        public async Task<StatusDTOListResult> AddStatusAsync(StatusAddDTO statusDetails, int loggedUserId)
        {
            StatusDTOListResult result = new StatusDTOListResult();
            try
            {
                // Validate the input
                if (string.IsNullOrWhiteSpace(statusDetails.StatusName) || statusDetails.CategoryId <= 0)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "Invalid input data.";
                    return result;
                }

                // Check if the category exists
                var category = await _context.Categories.FindAsync(statusDetails.CategoryId);
                if (category == null)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "Category not found.";
                    return result;
                }

                // Create new status entity
                Status newStatus = new Status
                {
                    Name = statusDetails.StatusName,
                    CategoryId = statusDetails.CategoryId,

                };

                // Add the status to the context and save changes
                _context.Statuses.Add(newStatus);
                await _context.SaveChangesAsync();

                // Audit the add user action
                await _auditMgr.CreateAuditAsync(loggedUserId, 4, ActivityTypeEnum.ADD, $"Added '{category.Name} - {newStatus.Name}'.");

                // Fetch the updated status list to return

                result.ResultCode = 0;
                result.ResultDescription = "Status added successfully!";
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultDescription = "Internal Server Error! Please contact the Server Administrator!";
                _logger.LogError(ex, ex.StackTrace);
            }
            return result;
        }

        public async Task<ResultDTO> EditStatus(StatusEditDTO statusEditDTO, int loggedUserId)
        {
            ResultDTO result = new ResultDTO();
            try
            {
                // Find the status by ID
                var status = await _context.Statuses.FindAsync(statusEditDTO.StatusId);
                if (status == null)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "Status not found!";
                    return result;
                }

                // Find the original category associated with the status
                var originalCategory = await _context.Categories.FindAsync(status.CategoryId);
                if (originalCategory == null)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "Original category not found!";
                    return result;
                }

                string originalStatusName = status.Name;
                string originalCategoryName = originalCategory.Name;

                // Find the new category by ID to ensure it's valid
                var newCategory = await _context.Categories.FindAsync(statusEditDTO.CategoryId);
                if (newCategory == null)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "Invalid category ID!";
                    return result;
                }

                // Update the status details
                status.Name = statusEditDTO.StatusName;
                status.CategoryId = statusEditDTO.CategoryId;

                // Save the changes to the database
                _context.Statuses.Update(status);
                await _context.SaveChangesAsync();

                result.ResultCode = 0;
                result.ResultDescription = "Status updated successfully!";

                // Audit the edit status action
                await _auditMgr.CreateAuditAsync(
                    loggedUserId,
                    4,
                    ActivityTypeEnum.EDIT,
                    $"Edited status's info: '{originalCategoryName} - {originalStatusName}' was changed to '{newCategory.Name} - {status.Name}'."
                );
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultDescription = "Internal Server Error! Please contact the Server Administrator!";
                _logger.LogError(ex, ex.StackTrace);
            }
            return result;
        }




        public async Task<ResultDTO> DeleteStatusAsync(int statusId, int loggedUserId)
        {
            ResultDTO result = new ResultDTO();
            try
            {
                // Fetch the status to be deleted
                var status = await _context.Statuses.FindAsync(statusId);
                if (status == null)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "Status not found!";
                    return result;
                }

                // Fetch the category associated with the status
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == status.CategoryId);

                if (category == null)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "Associated category not found!";
                    return result;
                }

                // Remove the status
                _context.Statuses.Remove(status);

                // Check if the category has any other statuses
                var remainingStatuses = await _context.Statuses
                    .Where(s => s.CategoryId == category.Id)
                    .ToListAsync();

                if (!remainingStatuses.Any())
                {
                    // No more statuses in the category, so remove the category as well
                    _context.Categories.Remove(category);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Audit the add user action
                await _auditMgr.CreateAuditAsync(loggedUserId, 4, ActivityTypeEnum.DELETE, $"Deleted '{category.Name} - {status.Name}'.");

                result.ResultCode = 0;
                result.ResultDescription = "Status and associated category deleted successfully!";
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultDescription = "Internal Server Error! Please contact the Server Administrator!";
                _logger.LogError(ex, ex.StackTrace);
            }
            return result;
        }

    }
}
