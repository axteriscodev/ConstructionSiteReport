using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class QuestionChosen
{
    public int Id { get; set; }

    public int IdDocument { get; set; }

    public int? IdCurrentChoice { get; set; }

    public int IdQuestion { get; set; }

    public string? Note { get; set; }

    public bool Printable { get; set; }

    public bool Hidden { get; set; }

    public virtual Choice? IdCurrentChoiceNavigation { get; set; }

    public virtual Document IdDocumentNavigation { get; set; } = null!;

    public virtual Question IdQuestionNavigation { get; set; } = null!;
}
