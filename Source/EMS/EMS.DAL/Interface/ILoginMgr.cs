using EMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Interface
{
    public interface ILoginMgr
    {
        // Login Page
        Task<UserDTOListResult> AuthenticateUser(LoginDTO credentials);
    }
}