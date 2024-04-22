using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class MacroCategory
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
}
