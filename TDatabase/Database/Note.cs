using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Note
{
    public int Id { get; set; }

    public int IdDocument { get; set; }

    public string? Text { get; set; }

    public bool Active { get; set; }

    public virtual Document IdDocumentNavigation { get; set; } = null!;
}
