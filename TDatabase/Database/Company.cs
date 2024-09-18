using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Company
{
    public int Id { get; set; }

    public int? IdOrganization { get; set; }

    public string? CompanyName { get; set; }

    public string? SelfEmployedName { get; set; }

    public string? Address { get; set; }

    public string? TaxId { get; set; }

    public string? Vatcode { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Pec { get; set; }

    public string? ReaNumber { get; set; }

    public string? WorkerWelfareFunds { get; set; }

    public string? Ccnl { get; set; }

    public string? InpsId { get; set; }

    public string? InailId { get; set; }

    public string? InailPat { get; set; }

    public string? JobsDescriptions { get; set; }

    public bool Active { get; set; }

    public int? PatInail { get; set; }

    public virtual ICollection<CompanyConstructorSite> CompanyConstructorSiteIdCompanyNavigations { get; set; } = new List<CompanyConstructorSite>();

    public virtual ICollection<CompanyConstructorSite> CompanyConstructorSiteSubcontractedByNavigations { get; set; } = new List<CompanyConstructorSite>();

    public virtual ICollection<CompanyDocument> CompanyDocuments { get; set; } = new List<CompanyDocument>();

    public virtual ICollection<CompanyNote> CompanyNotes { get; set; } = new List<CompanyNote>();

    public virtual Organization? IdOrganizationNavigation { get; set; }

    public virtual PatInail? PatInailNavigation { get; set; }

    public virtual ICollection<ReportedCompany> ReportedCompanies { get; set; } = new List<ReportedCompany>();
}
