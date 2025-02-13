using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class AdminEditInfoDTOResult : BasicResult
    {
        public AdminEditInfoDTO AdminEditInfoDTO { get; set; } = new AdminEditInfoDTO();
    }
}
