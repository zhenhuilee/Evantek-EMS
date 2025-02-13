using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public partial class UserRoleMapper
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int UserId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
