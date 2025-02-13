using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class DeleteIncidentDTO
    {
        public int RequestTypeId { get; set; } // from requestType table
        public string CompanyName { get; set; } = null!; // from company table
        public int CompanyTypeId { get; set; } // from companycategory table
        public string SubItem { get; set; } = null!; // from incident table
        public string CustomerName { get; set; } = null!;  // from incident table
        public string CustomerPhone { get; set; } = null!;  // from incident table
        public int SubjectId { get; set; } // from subject table
        public int IncidentCategoryId { get; set; } // from incident category table
        public string Description { get; set; } = null!;  // from incident table
        public string Address { get; set; } = null!;  // from incident table
        public int? WorkOrderNo { get; set; }
        public string? IpAddress { get; set; }
        public int EngineerId { get; set; } // from user table
        public DateTime? ResponseDateTime { get; set; }
        public DateTime IncidentCreatedDateTime { get; set; }
    }
}
