using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public class EngineerIncidentMapper
{
    public int Id { get; set; }

    public int EngineerId { get; set; }

    public int IncidentId { get; set; }

    public virtual User Engineer { get; set; } = null!;

    public virtual Incident Incident { get; set; } = null!;
}
