using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Note
{
    public int Id { get; set; }

    public int IdDocument { get; set; }

    public string? Text { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<AttachmentNote> AttachmentNotes { get; set; } = new List<AttachmentNote>();

    public virtual ICollection<CompanyNote> CompanyNotes { get; set; } = new List<CompanyNote>();

    public virtual Document IdDocumentNavigation { get; set; } = null!;
}
