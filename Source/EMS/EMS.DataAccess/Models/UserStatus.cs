using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public partial class UserStatus
{
    public int Id { get; set; }

    public string Note { get; set; } = null!;

    public int StatusId { get; set; }

    public int? UserId { get; set; }

    public DateTime LastUpdated { get; set; }

    public DateTime? EndTime { get; set; }

    public virtual Status Status { get; set; } = null!;

    public virtual User? User { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
