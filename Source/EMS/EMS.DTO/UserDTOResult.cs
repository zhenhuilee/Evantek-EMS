using EMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class UserDTOResult : BasicResult
    {
        public UserDTO UserDTO { get; set; } = new UserDTO();

    }
}