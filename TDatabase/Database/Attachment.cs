using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Attachment
{
    public int Id { get; set; }

    public int IdType { get; set; }

    public int IdDocument { get; set; }

    public DateTime DateTime { get; set; }

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public bool Active { get; set; }

    public virtual Document IdDocumentNavigation { get; set; } = null!;

    public virtual AttachmentType IdTypeNavigation { get; set; } = null!;
}
