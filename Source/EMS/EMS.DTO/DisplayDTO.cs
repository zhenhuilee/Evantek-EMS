using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class DisplayDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;

        public string StatusName { get; set; } = null!;

        public string Note { get; set; } = null!;

        public DateTime LastUpdated { get; set; }


    }
}
