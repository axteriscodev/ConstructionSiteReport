using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class AttachmentQuestion
{
    public int IdAttachment { get; set; }

    public int IdQuestion { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public virtual Attachment IdAttachmentNavigation { get; set; } = null!;

    public virtual Question IdQuestionNavigation { get; set; } = null!;
}
