using AutoMapper;
using EMS.DAL.Interface;
using EMS.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EMS.DTO;
using EMS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.DAL.Implementation
{
    public class UserMgr : IUserMgr
    {
        private readonly EmsDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UserMgr> _logger;
        private readonly IAuditMgr _auditMgr;

        public UserMgr(EmsDbContext dbContext, IMapper mapper, ILogger<UserMgr> logger, IAuditMgr auditMgr)
        {
            _context = dbContext;
            _mapper = mapper;
            _logger = logger;
            _auditMgr = auditMgr;
        }

        /*
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
                    .ToListAsync();Fadd

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

       
        public async Task<ResultDTO> ChangePasswordAsync(int userId, string currentPassword, string newPassword, string confirmPassword)
        {
            ResultDTO result = new ResultDTO();
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null || user.IsDeleted)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "User not found!";
                    return result;
                }


                // Check if old password is entered correctly
                string currentPasswordCheck = currentPassword + user.PasswordSalt;
                if (user.Password != HelperFunctions.SHA256_Hash(currentPasswordCheck))
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "Current password is incorrect!";
                    return result;
                }


                // Check if new password == confirm password
                if (newPassword != confirmPassword)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "New password and confirm password do not match!";
                    return result;
                }


                // Check new password is not the same as old password
                string newPasswordCheck = newPassword + user.PasswordSalt;
                if (user.Password == HelperFunctions.SHA256_Hash(newPasswordCheck))
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "New password cannot be the same as the old password!";
                    return result;
                }


                // Check the strength of the new password
                if (!IsStrongPassword(newPassword))
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "New password is weak!";
                    return result;
                }

                user.Password = HelperFunctions.SHA256_Hash(newPasswordCheck);

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                result.ResultCode = 0;
                result.ResultDescription = "Password changed successfully!";
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultDescription = "Internal Server Error! Please contact the Server Administrator!";
                _logger.LogError(ex, ex.StackTrace);
            }
            return result;
        }


        public async Task<bool> ValidateCurrentPasswordAsync(int userId, string currentPassword)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null || user.IsDeleted)
                {
                    // User not found or user is deleted
                    return false;
                }

                // Hash the provided current password with the user's salt
                string hashedCurrentPassword = HelperFunctions.SHA256_Hash(currentPassword + user.PasswordSalt);

                // Check if the hashed password matches the stored password
                return user.Password == hashedCurrentPassword;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                throw new Exception("Internal Server Error! Please contact the Server Administrator!");
            }
        }

        */

        public List<AllUsersDTO> GetUsersList()
        {
            var usersList = (from user in _context.Users
                             join userStatus in _context.UserStatuses on user.Id equals userStatus.UserId
                             join status in _context.Statuses on userStatus.StatusId equals status.Id
                             select new
                             {
                                 user,
                                 userStatus,
                                 status,
                                 
                             })
                             .AsEnumerable()
                             .Select(x => new AllUsersDTO
                             {
                                 UserId = x.user.Id,
                                 UserName = x.user.Name,
                                 EmailAddress = x.user.EmailAddress,
                                 StatusName = x.status.Name,
                                 StatusId = x.status.Id,
                                 Note = x.userStatus.Note,
                                 RoleId = _context.UserRoleMappers
                                            .Where(urm => urm.UserId == x.user.Id)
                                            .Select(urm => urm.RoleId)
                                            .ToList(), // Fetch role IDs and map to a list
                                 RoleName = _context.UserRoleMappers
                                            .Where(urm => urm.UserId == x.user.Id)
                                            .Select(urm => urm.Role.Name)
                                            .ToList() // Fetch role names and map to a list
                             }).ToList();
            return usersList;
        }


        public async Task<UserDTOListResult> AddUserAsync(UserAddDTO userDetails, int loggedUserId) 
        {
            UserDTOListResult oResult = new UserDTOListResult
            {
                ResultCode = 0,
                ResultDescription = "User added successfully!"
            };

            if (string.IsNullOrWhiteSpace(userDetails.Name) || string.IsNullOrWhiteSpace(userDetails.LoginName) || string.IsNullOrWhiteSpace(userDetails.Password) || string.IsNullOrWhiteSpace(userDetails.EmailAddress))
            {
                oResult.ResultCode = 1;
                oResult.ResultDescription = "Failed to add user. Provided input is empty!";
                return oResult;
            }
            else if (!IsStrongPassword(userDetails.Password))
            {
                oResult.ResultCode = 1;
                oResult.ResultDescription = "Failed to add user. Provided password is weak!"; 
                return oResult;
            }

            User user;
            try
            {
                User newUser = new User
                {
                    Name = userDetails.Name,
                    LoginName = userDetails.LoginName,
                    IsDeleted = false,
                    PasswordSalt = HelperFunctions.RandomString(12),
                    EmailAddress = userDetails.EmailAddress,    
                };

                string passwordCheck = userDetails.Password + newUser.PasswordSalt;
                newUser.Password = HelperFunctions.SHA256_Hash(passwordCheck);

                var defaultStatus = await _context.Statuses.FirstOrDefaultAsync(x => x.Name == "Office" && !x.IsDeleted);

                if (defaultStatus == null)
                {
                    defaultStatus = await _context.Statuses.FirstOrDefaultAsync(x => !x.IsDeleted);
                }

                UserStatus newStatus = new UserStatus
                {
                    Note = "",
                    StatusId = 1,
                    LastUpdated = DateTime.Now,
                    User = newUser,
                    Status = defaultStatus,
                    EndTime = DateTime.Now,
                };

                user = _context.Users.Add(newUser).Entity;
                _context.UserStatuses.Add(newStatus);

                // Save changes to get the new UserId
                await _context.SaveChangesAsync();

                int userId = newUser.Id;

                // Add the user role mappings
                List<string> roleNames = new List<string>(); // To store role names
                if (userDetails.RoleIds != null && userDetails.RoleIds.Count > 0)
                {
                    foreach (var roleId in userDetails.RoleIds)
                    {
                        var role = await _context.Roles.FindAsync(roleId);
                        if (role != null)
                        {
                            UserRoleMapper userRole = new UserRoleMapper
                            {
                                UserId = newUser.Id, // Use the new UserId
                                RoleId = roleId
                            };
                            _context.UserRoleMappers.Add(userRole);
                            roleNames.Add(role.Name); // Add role name to the list
                        }
                        else
                        {
                            oResult.ResultCode = 1;
                            oResult.ResultDescription = $"Failed to add user. Provided role ID {roleId} is invalid!";
                            return oResult;
                        }
                    }
                }

                await _context.SaveChangesAsync();

                // Join the role names into a string for logging
                string roleNamesStr = string.Join(", ", roleNames);

                // Audit the add user action
                await _auditMgr.CreateAuditAsync(loggedUserId, 5, ActivityTypeEnum.ADD, $"Added an employee with the following information - Name: '{userDetails.Name}({userId})', Username: '{userDetails.LoginName}', Roles: '{roleNamesStr}', Email Address: '{userDetails.EmailAddress}'.");

                oResult.UserDTO = _mapper.Map<UserDTO>(user);
                return oResult;
            }
            catch (Exception ex)
            {
                oResult.ResultCode = 1;
                oResult.ResultDescription = "Internal Server Error! Please contact the Server Administrator!";
                _logger.LogError(ex, ex.StackTrace);
                throw;
            }
        }

        private bool IsStrongPassword(string password)
        {
            return password.Length >= 8 && password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit);
        }


        public async Task<ResultDTO> EditUserInfoAsync(UserEditDTO userEditDTO, int loggedUserId)  
        {
            ResultDTO result = new ResultDTO();
            try
            {
                var user = await _context.Users
                    .Include(x => x.UserRoleMappers)
                    .ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync(X => X.Id == userEditDTO.UserId); 

                if (user == null || user.IsDeleted)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "User not found!";
                    return result;
                }

                var originalRoles = user.UserRoleMappers.Select(x => x.Role.Name).ToList(); // Get original role names

                string originalUserName = user.Name;
                // Update user name
                user.Name = userEditDTO.UserName;

                string originalEmail = user.EmailAddress;
                user.EmailAddress = userEditDTO.EmailAddress;


                // Update status and note
                var userStatus = await _context.UserStatuses.FirstOrDefaultAsync(us => us.UserId == userEditDTO.UserId);
                if (userStatus == null)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "User status not found!";
                    return result;
                }

                int originalStatusId = userStatus.StatusId;
                string originalNote = userStatus.Note;

                userStatus.StatusId = userEditDTO.StatusId;
                userStatus.Note = userEditDTO.Note;
                userStatus.LastUpdated = DateTime.Now;

                // Fetch statusName based on StatusId
                var statusName = await _context.Statuses
                    .Where(s => s.Id == userEditDTO.StatusId)
                    .Select(s => s.Name)
                    .FirstOrDefaultAsync();

                if (statusName == null)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "Invalid Status ID!";
                    return result;
                }

                // Fetch the new role names corresponding to the RoleIds from userEditDTO
                var newRoleNames = await _context.Roles
                    .Where(r => userEditDTO.RoleIds.Contains(r.Id))
                    .Select(r => r.Name)
                    .ToListAsync();

                // Update roles
                var existingRoles = _context.UserRoleMappers.Where(urm => urm.UserId == userEditDTO.UserId);
                _context.UserRoleMappers.RemoveRange(existingRoles);

                if (userEditDTO.RoleIds != null && userEditDTO.RoleIds.Count > 0)
                {
                    foreach (var roleId in userEditDTO.RoleIds)
                    {
                        var role = await _context.Roles.Where(x => x.Id == roleId).FirstOrDefaultAsync();
                        if (role != null)
                        {
                            UserRoleMapper userRole = new UserRoleMapper
                            {
                                UserId = userEditDTO.UserId,
                                RoleId = roleId
                            };
                            _context.UserRoleMappers.Add(userRole);
                        }
                        else
                        {
                            result.ResultCode = 1;
                            result.ResultDescription = $"Invalid role ID {roleId}";
                            return result;
                        }
                    }
                }

                // Save changes
                await _context.SaveChangesAsync();

                string auditDescription = $"Edited {originalUserName}'s({user.Id}) information: "; 
                bool hasChanges = false;

                if (originalUserName != userEditDTO.UserName)
                {
                    auditDescription += $"Name changed from '{originalUserName}' to '{userEditDTO.UserName}'. "; 
                    hasChanges = true;
                }

                if (originalEmail != userEditDTO.EmailAddress) {
                    auditDescription += $"Email changed from '{originalEmail}' to '{userEditDTO.EmailAddress}'. ";
                    hasChanges = true;

                }
                if (originalStatusId != userEditDTO.StatusId)
                {
                    var originalStatusName = await _context.Statuses 
                        .Where(s => s.Id == originalStatusId)
                        .Select(s => s.Name)
                        .FirstOrDefaultAsync();
                    auditDescription += $"Status changed from '{originalStatusName}' to '{statusName}'. ";
                    hasChanges = true;
                }
                if (originalNote != userEditDTO.Note)
                {
                    auditDescription += $"Note changed from '{originalNote}' to '{userEditDTO.Note}'. ";
                    hasChanges = true;
                }

                // Compare originalRoles with newRoleNames
                if (!originalRoles.SequenceEqual(newRoleNames))
                {
                    auditDescription += $"Role changed from '{string.Join(", ", originalRoles)}' to '{string.Join(", ", newRoleNames)}'. ";
                    hasChanges = true;
                }


                if (!hasChanges)
                {
                    auditDescription = "No changes made to user's info.";
                }

                // Audit the edit user action
                await _auditMgr.CreateAuditAsync(loggedUserId, 5, ActivityTypeEnum.EDIT, auditDescription);

                result.ResultCode = 0;
                result.ResultDescription = "User information updated successfully!";
                result.Data = statusName; // Include statusName in the result
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultDescription = "Internal Server Error! Please contact the Server Administrator!";
                _logger.LogError(ex, ex.StackTrace);
            }
            return result;
        }


        public async Task<ResultDTO> AdminChangePasswordAsync(int userId, string newPassword)
        {
            ResultDTO result = new ResultDTO();
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null || user.IsDeleted)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "User not found!";
                    return result;
                }

                // Check the strength of the new password
                if (!IsStrongPassword(newPassword))
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "New password is weak!";
                    return result;
                }

                // Hash the new password with the user's salt
                string newPasswordCheck = newPassword + user.PasswordSalt;
                user.Password = HelperFunctions.SHA256_Hash(newPasswordCheck);

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                result.ResultCode = 0;
                result.ResultDescription = "Password changed successfully!"; 
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultDescription = "Internal Server Error! Please contact the Server Administrator!";
                _logger.LogError(ex, ex.StackTrace);
            }
            return result;
        }


        public async Task<ResultDTO> DeleteUserAsync(int userId, int loggedUserId)
        {
            ResultDTO result = new ResultDTO();
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null || user.IsDeleted)
                {
                    result.ResultCode = 1;
                    result.ResultDescription = "User not found or already deleted!";
                    return result;
                }

                string userName = user.Name;

                // Update the IsDeleted flag
                user.IsDeleted = true;

                // Remove entries from UserRoleMapper table
                var userRoles = _context.UserRoleMappers.Where(urm => urm.UserId == userId);
                _context.UserRoleMappers.RemoveRange(userRoles);

                // Remove entries from UserStatus table
                var userStatus = _context.UserStatuses.Where(us => us.UserId == userId);
                _context.UserStatuses.RemoveRange(userStatus);

                // Save changes
                await _context.SaveChangesAsync();

                // Audit the add user action
                await _auditMgr.CreateAuditAsync(loggedUserId, 5, ActivityTypeEnum.DELETE, "Deleted " + userName + "(" + userId + ") and all its relevant information.");


                result.ResultCode = 0;
                result.ResultDescription = "User deleted successfully!";
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultDescription = "Internal Server Error! Please contact the Server Administrator!";
                _logger.LogError(ex, ex.StackTrace);
            }
            return result;
        }




        /*
        public async Task<UserDetailsDTO> GetUserDetailsByIdAsync(int userId)
        {
            try
            {
                // Fetch user information
                var user = await _context.Users
                    .Include(u => u.UserStatus)
                    .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

                if (user == null)
                {
                    throw new Exception("User not found!");
                }

                // Fetch user roles
                var userRoles = await (from userRole in _context.UserRoleMappers
                                       where userRole.UserId == userId
                                       select userRole.RoleId).ToListAsync();

                // Map to DTO
                var userDetails = new UserDetailsDTO
                {
                    Name = user.Name,
                    Status = user.UserStatus.StatusId,
                    Note = user.UserStatus.Note,
                    RoleIds = userRoles
                };

                return userDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                throw new Exception("Internal Server Error! Please contact the Server Administrator!");
            }
        }
        */










        /*
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


        public async Task<StatusDTOListResult> AddStatusAsync(StatusAddDTO statusDetails)
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

        public async Task<ResultDTO> EditStatus(StatusEditDTO statusEditDTO)
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

                // Find the category by ID to ensure it's valid
                var category = await _context.Categories.FindAsync(statusEditDTO.CategoryId);
                if (category == null)
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
            }
            catch (Exception ex)
            {
                result.ResultCode = 1;
                result.ResultDescription = "Internal Server Error! Please contact the Server Administrator!";
                _logger.LogError(ex, ex.StackTrace);
            }
            return result;
        }

        public async Task<ResultDTO> DeleteStatusAsync(int statusId)
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
        */



    }
}

