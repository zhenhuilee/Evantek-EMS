using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class ReplacementDTO
    {
        //public int Id { get; set; } 
        public string? Model { get; set; }
        public string? OldSerialNo { get; set; }
        public string? NewSerialNo { get; set; }
        public string? Remarks { get; set; }
    }
}
