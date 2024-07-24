using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class AttachmentNote
{
    public int IdAttachment { get; set; }

    public int IdNote { get; set; }

    public string? Name { get; set; }

    public bool Active { get; set; }

    public virtual Attachment IdAttachmentNavigation { get; set; } = null!;

    public virtual Note IdNoteNavigation { get; set; } = null!;
}
