using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class ConstructorSite
{
    public int Id { get; set; }

    public int? IdOrganization { get; set; }

    public int? IdClient { get; set; }

    public string Name { get; set; } = null!;

    public string? JobDescription { get; set; }

    public string? Address { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? IdSico { get; set; }

    public string? IdSicoInProgress { get; set; }

    public DateTime? PreliminaryNotificationStart { get; set; }

    public DateTime? PreliminaryNotificationInProgress { get; set; }

    public string? Note { get; set; }

    public string? Rl { get; set; }

    public string? Dl { get; set; }

    public string? Cse { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<CompanyConstructorSite> CompanyConstructorSites { get; set; } = new List<CompanyConstructorSite>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual Client? IdClientNavigation { get; set; }

    public virtual Organization? IdOrganizationNavigation { get; set; }
}
