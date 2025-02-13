using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public partial class Audit
{
    public int Id { get; set; }

    public DateTime TimeStamp { get; set; }

    public string WebActivity { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int UserId { get; set; }

    public int ModuleId { get; set; }

    public virtual Module Module { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
