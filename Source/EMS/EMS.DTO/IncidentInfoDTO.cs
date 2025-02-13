using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class IncidentInfoDTO
    {
        public int IncidentId { get; set; }
        public string? RefNum { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;

        public string CompanyName { get; set; } = null!;

        public string CompanyType { get; set; } = null!;

        public string SubItem { get; set; } = null!;

        public string CustomerName { get; set; } = null!;

        public string CustomerPhone { get; set; } = null!;

        public int SubjectId { get; set; }

        public string SubjectName { get; set; } = null!;

        public int IncidentCategoryId { get; set; }

        public string IncidentCategoryName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? IpAddress { get; set; }

        public string Address { get; set; } = null!;

        public int? WorkOrderNo { get; set; }

        public int EngineerId { get; set; }    

        public DateTime Date { get; set; }
    }
}
