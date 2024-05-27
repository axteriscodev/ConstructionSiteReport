using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class QuestionChoice
{
    public int IdQuestion { get; set; }

    public int IdChoice { get; set; }

    public string? Note { get; set; }

    public virtual Choice IdChoiceNavigation { get; set; } = null!;

    public virtual Question IdQuestionNavigation { get; set; } = null!;
}
