using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{

    public class ChangePasswordDTO
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ResultDTO
    {
        public string Data;
        public int ResultCode { get; set; }
        public string ResultDescription { get; set; }
        public object UserDTO { get; set; }
        public object UserUpdateStatusDTO { get; set; }

        public object IncidentDTO { get; set; }
    }

}
