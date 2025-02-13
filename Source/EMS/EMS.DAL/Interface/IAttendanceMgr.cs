using EMS.DataAccess.Models;
using EMS.DTO;
using System.Collections.Generic;

namespace EMS.DAL.Interface
{
    public interface IAttendanceMgr
    {
        // Attendance Page
        UserStatusDTO GetUserAttendance(int userId);
        List<CategoryStatusDTO> GetStatusList();
        UserUpdateStatusDTOResult UpdateUserAttendance(int userId, int statusId, string note);
    }
}