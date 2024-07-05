using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class CompanyConstructorSite
{
    public int IdCompany { get; set; }

    public int IdConstructorSite { get; set; }

    public string? JobsDescription { get; set; }

    public string? Note { get; set; }

    public virtual Company IdCompanyNavigation { get; set; } = null!;

    public virtual ConstructorSite IdConstructorSiteNavigation { get; set; } = null!;
}
