using AutoMapper;
using EMS.DAL.Interface;
using EMS.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Implementation
{
    public class ModuleMgr : IModuleMgr
    {
        private readonly EmsDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ModuleMgr> _logger;

        public ModuleMgr(EmsDbContext dbContext, IMapper mapper, ILogger<ModuleMgr> logger)
        {
            _context = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<RoleDTO>> GetRolesByUserIdAsync(int userId)
        {
            // Fetch the roles based on the user ID
            var roles = await _context.UserRoleMappers
                .Where(urm => urm.UserId == userId)
                .Select(urm => new RoleDTO
                {
                    Id = urm.Role.Id,
                    Name = urm.Role.Name
                })
                .ToListAsync();

            return roles;
        }

        public async Task<List<ModuleDTO>> GetModulesByRoleIdAsync(int userId, int roleId)
        {
            try
            {
                // Fetch roles assigned to the user
                var userRoles = await GetRolesByUserIdAsync(userId);

                // Check if the provided roleId is within the user's roles
                if (!userRoles.Any(role => role.Id == roleId))
                {
                    throw new UnauthorizedAccessException("User does not have access to the specified role.");
                }

                // Fetch modules mapped to the role
                var modules = await _context.RoleModuleMappers
                    .Where(rm => rm.RoleId == roleId)
                    .Select(rm => rm.Module)
                    .ToListAsync();

                return _mapper.Map<List<ModuleDTO>>(modules);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                throw new UnauthorizedAccessException("User does not have access to the specified role.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                throw new Exception("Internal Server Error! Please contact the Server Administrator!");
            }
        }

        public async Task<bool> IsAdminAsync(int userId)
        {
            try
            {
                // Fetch roles for the user
                var roles = await GetRolesByUserIdAsync(userId);

                // Check if the user has an "Admin" role
                return roles.Any(role => role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                throw new Exception("Internal Server Error! Please contact the Server Administrator!");
            }
        }
    }
}
