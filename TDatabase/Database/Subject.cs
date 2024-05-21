using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Subject
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
