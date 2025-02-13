using EMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class AllUsersDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } //shown
        public int StatusId { get; set; }
        public string StatusName { get; set; } //shown
        public string Note { get; set; } //shown

        //public List<string> Roles { get; set; }
        public List<int> RoleId { get; set; } // from role table 
        public List<string> RoleName { get; set; } // shown
        public string EmailAddress { get; set; } = null!;
    }
}