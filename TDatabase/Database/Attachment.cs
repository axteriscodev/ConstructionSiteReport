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

    public string? FilePath { get; set; }

    public string? Image { get; set; }

    public bool Active { get; set; }

    public int? IdOrganization { get; set; }

    public virtual ICollection<AttachmentNote> AttachmentNotes { get; set; } = new List<AttachmentNote>();

    public virtual ICollection<AttachmentQuestion> AttachmentQuestions { get; set; } = new List<AttachmentQuestion>();

    public virtual Document IdDocumentNavigation { get; set; } = null!;

    public virtual Organization? IdOrganizationNavigation { get; set; }

    public virtual AttachmentType IdTypeNavigation { get; set; } = null!;
}
