using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string Vatcode { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<CompanyConstructorSite> CompanyConstructorSites { get; set; } = new List<CompanyConstructorSite>();

    public virtual ICollection<CompanyDocument> CompanyDocuments { get; set; } = new List<CompanyDocument>();

    public virtual ICollection<ReportedCompany> ReportedCompanies { get; set; } = new List<ReportedCompany>();
}
