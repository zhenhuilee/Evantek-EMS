using EMS.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DTO
{
    public class CategoryStatusDTO
    {

        public string CategoryName { get; set; }
        public List<StatusDTO> Statuses { get; set; }
    }
}

