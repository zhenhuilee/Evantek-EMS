using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class AllStatusesDTO
    {
        public int statusId { get; set; }
        public string statusName { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
    }
}
