using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class AdminEditInfoDTO
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Note { get; set; }
        public int StatusId { get; set; }

        public DateTime LastUpdated { get; set; }
        public string StatusName { get; set; }
        public int RoleId { get; set; }
    }
}
