using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class UserDetailsDTO
    {
        public string Name { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public List<int> RoleIds { get; set; }
    }
}


