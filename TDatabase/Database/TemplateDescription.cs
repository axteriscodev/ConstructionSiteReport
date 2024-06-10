using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class TemplateDescription
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();
}
