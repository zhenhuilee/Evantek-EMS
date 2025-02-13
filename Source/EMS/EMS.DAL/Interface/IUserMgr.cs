using EMS.DataAccess.Models;
using EMS.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.DAL.Interface
{
    public interface IUserMgr
    {
        // Module Page
        //Task<List<RoleDTO>> GetRolesByUserIdAsync(int userId);
        //Task<List<ModuleDTO>> GetModulesByRoleIdAsync(int userId, int roleId);

        // Change Password Page
        //Task<ResultDTO> ChangePasswordAsync(int userId, string currentPassword, string newPassword, string confirmPassword);
        //Task<bool> ValidateCurrentPasswordAsync(int userId, string currentPassword);

        //List<AllStatusesDTO> GetStatusList();
        List<AllUsersDTO> GetUsersList();
        Task<UserDTOListResult> AddUserAsync(UserAddDTO userDetails, int loggedUserId);
        Task<ResultDTO> AdminChangePasswordAsync(int userId, string newPassword);
        Task<ResultDTO> DeleteUserAsync(int userId, int loggedUserId);
        //Task<UserDetailsDTO> GetUserDetailsByIdAsync(int userId); 
        Task<ResultDTO> EditUserInfoAsync(UserEditDTO userEditDTO, int loggedUserId);

        //Task<StatusDTOListResult> AddStatusAsync(StatusAddDTO statusDetails);
        //Task<ResultDTO> EditStatus(StatusEditDTO statusEditDTO);
        //Task<ResultDTO> DeleteStatusAsync(int statusId);
        //Task<bool> IsAdminAsync(int userId); 
    }
}