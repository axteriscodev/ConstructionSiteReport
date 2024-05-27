using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Template
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime Date { get; set; }

    public string? Note { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<QuestionChosen> QuestionChosens { get; set; } = new List<QuestionChosen>();
}
