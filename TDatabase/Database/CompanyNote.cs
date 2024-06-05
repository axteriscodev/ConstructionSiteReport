using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class CompanyNote
{
    public int IdNote { get; set; }

    public int IdCompany { get; set; }

    public string? Note { get; set; }

    public bool Active { get; set; }
}
