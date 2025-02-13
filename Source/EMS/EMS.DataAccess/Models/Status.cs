using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<UserStatus> UserStatuses { get; set; } = new List<UserStatus>();
}
