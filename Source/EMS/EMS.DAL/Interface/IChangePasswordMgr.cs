using EMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Interface
{
    public interface IChangePasswordMgr
    {
        Task<ResultDTO> ChangePasswordAsync(int userId, string currentPassword, string newPassword, string confirmPassword);
        Task<bool> ValidateCurrentPasswordAsync(int userId, string currentPassword);
    }
}
