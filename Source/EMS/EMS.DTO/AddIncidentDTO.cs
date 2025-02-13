using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class AddIncidentDTO
    {
        public int RequestTypeId { get; set; } 
        public string Company { get; set; } = null!;
        public int CompanyTypeId { get; set; } 
        public string SubItem { get; set; } = null!; 
        public string Customer { get; set; } = null!;  
        public string CustomerPhone { get; set; } = null!;  
        public int SubjectId { get; set; } 
        public int IncidentCategoryId { get; set; } 
        public string Description { get; set; } = null!;  
        public string Address { get; set; } = null!;  
        public string? WorkOrderNo { get; set; }
        public string? IpAddress { get; set; }
        public List<int> EngineerId { get; set; }
        public int AdminId { get; set; } 
        public DateTime? ResponseDateTime { get; set; }
        public DateTime IncidentCreatedDateTime { get; set; }
    }
} 