using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Choice
{
    public int Id { get; set; }

    public string Value { get; set; } = null!;

    public string Tag { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<QuestionChosen> QuestionChosens { get; set; } = new List<QuestionChosen>();

    public virtual ICollection<Question> IdQuestions { get; set; } = new List<Question>();
}
