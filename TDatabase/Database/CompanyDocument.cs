using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class CompanyDocument
{
    public int IdDocument { get; set; }

    public int IdCompany { get; set; }

    public bool Present { get; set; }

    public virtual Company IdCompanyNavigation { get; set; } = null!;

    public virtual Document IdDocumentNavigation { get; set; } = null!;
}
