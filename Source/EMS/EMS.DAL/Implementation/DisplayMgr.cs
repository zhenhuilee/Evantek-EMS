using AutoMapper;
using EMS.DAL.Interface;
using EMS.DataAccess.Models;
using EMS.DTO;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace EMS.DAL.Implementation
{
    public class DisplayMgr : IDisplayMgr
    {
        private readonly EmsDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DisplayMgr> _logger;

        public DisplayMgr(EmsDbContext dbContext, IMapper mapper, ILogger<DisplayMgr> logger)
        {
            _context = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public List<DisplayDTO> GetDisplayList()
        {
            var displayList = (from user in _context.Users
                               join userStatus in _context.UserStatuses on user.Id equals userStatus.UserId
                               join status in _context.Statuses on userStatus.StatusId equals status.Id

                               select new
                               {
                                   user,
                                   userStatus,
                                   status,
                                   userStatus.LastUpdated // Ensure this field exists in UserStatuses
                               })
                               .AsEnumerable()
                               .Select(x => new DisplayDTO
                               {
                                   UserId = x.user.Id,
                                   UserName = x.user.Name,
                                   StatusName = x.status.Name,
                                   Note = x.userStatus.Note,
                                   LastUpdated = x.userStatus.LastUpdated // Added field
                               }).ToList();
            return displayList;
        }
    }
}