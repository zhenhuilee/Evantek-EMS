using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class FilterIncidentDTO
    {
        public string RefNum { get; set; }
        public string Company { get; set; }
        public string SubItem { get; set; }
        public string Status { get; set; }
        public string? IpAddress { get; set; }
        public List<string> Engineer { get; set; }
        public DateTime IncidentCreatedDateTime { get; set; }
        public string Description { get; set; }
        public string? Solution { get; set; }
    }
}