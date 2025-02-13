using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EMS.DTO
{
    public class UserEditDTO
    {
        //public string StatusName { get;  set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int StatusId { get; set; }
        public string Note { get; set; } = null!;
        public List<int> RoleIds { get; set; } = new List<int>();
        public string EmailAddress { get; set; } = null!;
    }
}
