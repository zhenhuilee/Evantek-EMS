using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public class ModuleDTO
{
    public int Id { get; set; }
    public string? Url { get; set; }
    public string ModuleName { get; set; } = null!;
}
