using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class UserAttachment
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public int Type { get; set; }

    public string Path { get; set; } = null!;

    public bool Active { get; set; }

    public bool Hidden { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual UserAttachmentType TypeNavigation { get; set; } = null!;
}
