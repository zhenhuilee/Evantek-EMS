using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class FilterIncidentsRequestDTO
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? StatusId { get; set; }
        public List<int> EngineerIds { get; set; }
    }
}
