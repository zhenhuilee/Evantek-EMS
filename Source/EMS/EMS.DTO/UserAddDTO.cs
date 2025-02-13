using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class UserAddDTO
    {
        //public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string LoginName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public List<int> RoleIds { get; set; }

        public string EmailAddress { get; set; } = null!;
    }
}
