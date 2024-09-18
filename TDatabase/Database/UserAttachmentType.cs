using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class UserAttachmentType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Active { get; set; }

    public bool Hidden { get; set; }

    public virtual ICollection<UserAttachment> UserAttachments { get; set; } = new List<UserAttachment>();
}
