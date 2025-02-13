using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public class UserDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LoginName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public int? UserStatusId { get; set; }

    public DateTime lastUpdated { get; set; }

    //public string EmailAddress { get; set; } = null!;
}