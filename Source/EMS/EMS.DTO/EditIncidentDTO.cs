using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class EditIncidentDTO
    {
        public int Id { get; set; } 
        public int StatusId { get; set; } // incident status table
        public int RequestTypeId {get; set;} // request type table        
        public int CompanyTypeId { get; set; } // company type table
        public int SubjectId { get; set; } // subject table
        public int CategoryId { get; set; } // incident category table
        //public int EngineerId { get; set; } // user table

        public List<int> EngineerId { get; set; }
        public int AdminId { get; set; }    // user table
        public string Company { get; set; } = null!; // incident table
        public string SubItem { get; set; } = null!; // incident table
        public string Customer { get; set; } = null!; // incident table
        public string CustomerPhone { get; set; } = null!; // incident table
        public string Description { get; set; } = null!; // incident table
        public string Address { get; set; } = null!; // incident table
        public string? IpAddress { get; set; } // incident table
        public string? WorkOrderNo { get; set; }  // incident table
        public DateTime? ResponseDateTime { get; set; } // incident table
        //public DateTime? CompletedDateTime { get; set; } // incident table
        //public DateTime? ArrivalDateTime { get; set; } // incident table 
        //public string? Solution { get; set; } // incident table
        //public List<ReplacementDTO> Replacements { get; set; } = new List<ReplacementDTO>();

    }
}
