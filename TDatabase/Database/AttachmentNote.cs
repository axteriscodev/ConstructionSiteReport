using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class AttachmentNote
{
    public int IdAttachment { get; set; }

    public int IdNote { get; set; }

    public string? Name { get; set; }

    public bool Active { get; set; }
}
