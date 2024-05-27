using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class QuestionAnswered
{
    public int IdCurrentChoice { get; set; }

    public int IdQuestionChosen { get; set; }

    public int IdDocument { get; set; }

    public string? Note { get; set; }

    public virtual Choice IdCurrentChoiceNavigation { get; set; } = null!;

    public virtual Document IdDocumentNavigation { get; set; } = null!;

    public virtual QuestionChosen IdQuestionChosenNavigation { get; set; } = null!;

    public virtual ICollection<ReportedCompany> ReportedCompanies { get; set; } = new List<ReportedCompany>();
}
