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
    public class LoginMgr : ILoginMgr
    {
        private readonly EmsDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<LoginMgr> _logger;


        public LoginMgr(EmsDbContext dbContext, IMapper mapper, ILogger<LoginMgr> logger)
        {
            _context = dbContext;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<UserDTOListResult> AuthenticateUser(LoginDTO credentials)
        {
            UserDTOListResult oResult = new UserDTOListResult();
            oResult.ResultCode = 0;
            oResult.ResultDescription = "User authenticated successfully!";

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.LoginName.ToLower() == credentials.LoginName.ToLower());

                if (user == null)
                {
                    oResult.ResultCode = 1;
                    oResult.ResultDescription = "User not found";
                    return oResult;
                }

                string passwordCheck = credentials.Password + user.PasswordSalt;
                var hashedPassword = HelperFunctions.SHA256_Hash(passwordCheck);

                if (hashedPassword != user.Password)
                {
                    oResult.ResultCode = 1;
                    oResult.ResultDescription = "Invalid password!";
                    return oResult;
                }

                UserDTO userDTO = _mapper.Map<UserDTO>(user);
                oResult.UserDTO = userDTO;
            }
            catch (System.Exception ex)
            {
                oResult.ResultCode = 1;
                oResult.ResultDescription = "Internal Server Error! Please contact the Server Administrator!";
                _logger.LogError(ex, ex.StackTrace);
            }
            return oResult;
        }

        private string GenerateToken(User user)
        {
            // You can use any token generation mechanism here (e.g., JWT)
            // For demonstration purposes, let's generate a simple token
            return Guid.NewGuid().ToString();
        }
    }
}