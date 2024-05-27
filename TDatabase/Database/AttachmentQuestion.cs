using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class AttachmentQuestion
{
    public int IdAttachment { get; set; }

    public int IdQuestion { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }
}
