using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class QuestionChosen
{
    public int Id { get; set; }

    public int IdTemplate { get; set; }

    public int IdQuestion { get; set; }

    public int Order { get; set; }

    public string? Note { get; set; }

    public virtual Question IdQuestionNavigation { get; set; } = null!;

    public virtual Template IdTemplateNavigation { get; set; } = null!;

    public virtual ICollection<QuestionAnswered> QuestionAnswereds { get; set; } = new List<QuestionAnswered>();
}
