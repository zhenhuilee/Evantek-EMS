using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public class CategoryDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;


    //public virtual ICollection<Status> Statuses { get; set; } = new List<Status>();

    public List<StatusDTO> StatusDTOs { get; set; } = new List<StatusDTO>();
}
