using EMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Interface
{
    public interface IModuleMgr
    {
        Task<List<RoleDTO>> GetRolesByUserIdAsync(int userId);

        Task<List<ModuleDTO>> GetModulesByRoleIdAsync(int userId, int roleId);
        Task<bool> IsAdminAsync(int userId);
    }
}
