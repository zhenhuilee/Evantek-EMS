using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public partial class Display
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid ApiKey { get; set; }
}
