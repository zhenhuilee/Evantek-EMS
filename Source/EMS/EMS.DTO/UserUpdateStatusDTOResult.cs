using EMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class UserUpdateStatusDTOResult : BasicResult
    {
        public UserUpdateStatusDTO UserUpdateStatusDTO { get; set; } = new UserUpdateStatusDTO();
    }
}