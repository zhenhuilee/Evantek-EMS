using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public partial class Counter
{
    public int Id { get; set; }

    public string Keys { get; set; } = null!;

    public string Value { get; set; } = null!;
}
