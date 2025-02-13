using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public partial class Module
{
    public int Id { get; set; }

    public string ModuleName { get; set; } = null!;

    public string? Url { get; set; }

    public virtual ICollection<Audit> Audits { get; set; } = new List<Audit>();

    public virtual ICollection<RoleModuleMapper> RoleModuleMappers { get; set; } = new List<RoleModuleMapper>();
}
