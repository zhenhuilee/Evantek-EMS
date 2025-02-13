using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class AddUserDTO
    {
        public int UserId { get; set; }

        public string UserName{ get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
