using AutoMapper;
using EMS.DAL.Interface;
using EMS.DataAccess.Models;
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
    public class AuditMgr : IAuditMgr
    {
        private readonly EmsDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AuditMgr> _logger;

        public AuditMgr(EmsDbContext dbContext, IMapper mapper, ILogger<AuditMgr> logger)
        {
            _context = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateAuditAsync(int userId, int moduleId, ActivityTypeEnum webActivity, string description)
        {
            try
            {
                var audit = new Audit
                {
                    TimeStamp = DateTime.Now,
                    UserId = userId,
                    ModuleId = moduleId,
                    WebActivity = webActivity.ToString(),
                    Description = description
                };

                _context.Audits.Add(audit);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Audit record created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the audit record.");
                throw;
            }
        } 
    }
}
