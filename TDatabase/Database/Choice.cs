using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Choice
{
    public int Id { get; set; }

    public string Value { get; set; } = null!;

    public string Tag { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<QuestionChoice> QuestionChoices { get; set; } = new List<QuestionChoice>();

    public virtual ICollection<QuestionChosen> QuestionChosens { get; set; } = new List<QuestionChosen>();
}
