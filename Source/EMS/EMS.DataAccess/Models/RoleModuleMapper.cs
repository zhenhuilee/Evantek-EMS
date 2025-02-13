﻿using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public partial class RoleModuleMapper
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int ModuleId { get; set; }

    public virtual Module Module { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
