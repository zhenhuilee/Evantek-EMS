using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class BasicResult
    {
        public int ResultCode { get; set; }

        public string ResultDescription { get; set; } = string.Empty;
    }
}