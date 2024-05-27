using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Choice
{
    public int Id { get; set; }

    public string Value { get; set; } = null!;

    public string Tag { get; set; } = null!;

    public string Color { get; set; } = null!;

    public bool Reportable { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<QuestionAnswered> QuestionAnswereds { get; set; } = new List<QuestionAnswered>();

    public virtual ICollection<Question> IdQuestions { get; set; } = new List<Question>();
}
