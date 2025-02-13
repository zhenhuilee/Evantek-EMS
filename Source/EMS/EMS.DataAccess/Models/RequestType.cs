using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public partial class RequestType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}
