using EMS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class AuditDTO
    {
        public int UserId { get; set; }
        public int ModuleId { get; set; }
        public ActivityTypeEnum WebActivity { get; set; }
        public string Description { get; set; } = string.Empty;             
    }
}


