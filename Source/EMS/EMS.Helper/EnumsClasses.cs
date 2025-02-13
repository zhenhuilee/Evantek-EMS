using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Helper
{
    public enum RoleEnum
    {
        User = 1,
        Admin,
        Engineer,
    }

    public enum ActivityTypeEnum
    {
        ADD = 1,
        EDIT,
        DELETE
    }

    public enum ModuleTypeEnum
    {
        ATTENDANCE = 1,
        USERS,
        STATUSES
    } 
}
