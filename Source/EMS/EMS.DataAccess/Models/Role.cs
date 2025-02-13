using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<RoleModuleMapper> RoleModuleMappers { get; set; } = new List<RoleModuleMapper>();

    public virtual ICollection<UserRoleMapper> UserRoleMappers { get; set; } = new List<UserRoleMapper>();
}
