using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class AttachmentType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
