using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class SubCategory
{
    public int Id { get; set; }

    public int IdMacroCategory { get; set; }

    public string Text { get; set; } = null!;

    public bool Active { get; set; }

    public virtual MacroCategory IdMacroCategoryNavigation { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
