using System;
using System.Collections.Generic;

namespace EMS.DataAccess.Models;

public partial class Incident
{
    public int Id { get; set; }

    public string Customer { get; set; } = null!;

    public string CustomerPhone { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? WorkOrderNo { get; set; }

    public string? IpAddress { get; set; }

    public DateTime? ResponseDateTime { get; set; }

    public int SubjectId { get; set; }

    public int IncidentCategoryId { get; set; }

    public int IncidentStatusId { get; set; }

    public string SubItem { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Company { get; set; } = null!;

    public int CompanyTypeId { get; set; }

    public int? RequestTypeId { get; set; }

    public string? Solution { get; set; }

    public DateTime IncidentCreatedDateTime { get; set; }

    public string? RefNum { get; set; }

    public int AdminId { get; set; }

    public DateTime? CompletedDateTime { get; set; }

    public DateTime? ArrivalDateTime { get; set; }

    public string? Signature { get; set; }

    public virtual User Admin { get; set; } = null!;

    public virtual CompanyType CompanyType { get; set; } = null!;

    public virtual IncidentCategory IncidentCategory { get; set; } = null!;

    public virtual IncidentStatus IncidentStatus { get; set; } = null!;

    public virtual ICollection<Replacement> Replacements { get; set; } = new List<Replacement>();

    public virtual RequestType? RequestType { get; set; }

    public virtual Subject Subject { get; set; } = null!;

    public virtual ICollection<User> Engineers { get; set; } = new List<User>();
}
