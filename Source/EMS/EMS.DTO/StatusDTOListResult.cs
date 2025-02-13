using EMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{


    public class StatusDTOListResult
    {
        public int ResultCode { get; set; }
        public string ResultDescription { get; set; }
        public List<StatusDTO> StatusDTOs { get; set; }
    }

}
