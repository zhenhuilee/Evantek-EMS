using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LoginName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public int? UserStatusId { get; set; }

    public string EmailAddress { get; set; } = null!;

    public virtual ICollection<Audit> Audits { get; set; } = new List<Audit>();

    public virtual ICollection<Incident> IncidentsNavigation { get; set; } = new List<Incident>();

    public virtual ICollection<UserRoleMapper> UserRoleMappers { get; set; } = new List<UserRoleMapper>();

    public virtual UserStatus? UserStatus { get; set; }

    public virtual ICollection<UserStatus> UserStatuses { get; set; } = new List<UserStatus>();

    public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}
