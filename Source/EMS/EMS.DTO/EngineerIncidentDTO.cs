using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class EngineerIncidentDTO
    {
        public int Id { get; set; } // all fields are from incident table

        public string? RefNum { get; set; }

        public string Company { get; set; } = null!;

        public DateTime IncidentCreatedDateTime { get; set; }
    }
}
