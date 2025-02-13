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
    public class ChangePasswordMgr : IChangePasswordMgr
    {
        private readonly EmsDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ChangePasswordMgr> _logger;


        public ChangePasswordMgr(EmsDbContext dbContext, IMapper mapper, ILogger<ChangePasswordMgr> logger)
        {
            _context = dbContext;
            _mapper = mapper;
            _logger = logger;
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

        private bool IsStrongPassword(string password)
        {
            return password.Length >= 8 && password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit);
        }

    }
}
