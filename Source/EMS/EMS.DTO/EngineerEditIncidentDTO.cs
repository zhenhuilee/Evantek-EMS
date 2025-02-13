using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class EngineerEditIncidentDTO
    {

        public int Id { get; set; }
        //public string? RefNum { get; set; }
        //public int StatusId { get; set; }
        public int RequestTypeId { get; set; }
        public string Company { get; set; } = null!;
        public int CompanyTypeId { get; set; }
        public string SubItem { get; set; } = null!;
        public string Customer { get; set; } = null!;
        public string CustomerPhone { get; set; } = null!;
        public int SubjectId { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? IpAddress { get; set; }
        //public int EngineerId { get; set; }
        //public List<int> EngineerId { get; set; }
        //public int AdminId { get; set; }
        public string? WorkOrderNo { get; set; }
        public string? Solution { get; set; }
        public DateTime? ResponseDateTime { get; set; }
        public DateTime? CompletedDateTime { get; set; }
        public DateTime? ArrivalDateTime { get; set; }
        public List<ReplacementDTO> Replacements { get; set; } = new List<ReplacementDTO>();

    }
}
