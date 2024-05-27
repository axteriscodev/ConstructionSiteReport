using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class ReportedCompany
{
    public int IdCompany { get; set; }

    public int IdCurrentChoice { get; set; }

    public int IdQuestionChosen { get; set; }

    public int IdDocument { get; set; }

    public string? Note { get; set; }

    public virtual Company IdCompanyNavigation { get; set; } = null!;

    public virtual QuestionAnswered QuestionAnswered { get; set; } = null!;
}
