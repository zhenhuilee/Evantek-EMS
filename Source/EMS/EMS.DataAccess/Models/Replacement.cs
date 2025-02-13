using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public partial class Replacement
{
    public int Id { get; set; }

    public string Model { get; set; } = null!;

    public string OldSerialNo { get; set; } = null!;

    public string NewSerialNo { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public int IncidentId { get; set; }

    public virtual Incident Incident { get; set; } = null!;
}
