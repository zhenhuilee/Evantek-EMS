using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class AllIncidentDTO
    {

        public int Id { get; set; }
        public string? RefNum { get; set; } // from incident table

        public string CompanyName { get; set; } = null!; // from incident table

        public string SubItem { get; set; } = null!; // from incident table

        //public int IncidentStatusId { get; set; } // from incident status table

        public string StatusName { get; set; } = null!; // from status table

        //public int SubjectId { get; set; } // from subject table

        public string SubjectName { get; set; } = null!; // from subject table

        public string? IpAddress { get; set; } // from incident table

        //public int EngineerId { get; set; } // from incident table

        public string EngineerName { get; set; } = null!; // from user table

        //blic DateTime? ReponseDateTime { get; set; } // from incident table

        public DateTime IncidentCreatedDateTime { get; set; }
    }
}
