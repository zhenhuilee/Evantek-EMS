using EMS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Interface
{
    public interface IAuditMgr
    {
        Task CreateAuditAsync(int userId, int moduleId, ActivityTypeEnum webActivity, string description);
    }
}
