using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class IncidentDTO
    {
        public int Id { get; set; }

        public string CustomerName { get; set; } = null!;

        public string CustomerPhone { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Solution { get; set; } = null!;

        public string? WorkOrderNo { get; set; }

        public byte[] Signature { get; set; } = null!;

        public string? IpAddress { get; set; }

        public DateTime ResponseDateTime { get; set; }

        public int RequestTypeId { get; set; }

        public int SubjectId { get; set; }

        public int IncidentCategoryId { get; set; }

        public int IncidentStatusId { get; set; }
    }
}
