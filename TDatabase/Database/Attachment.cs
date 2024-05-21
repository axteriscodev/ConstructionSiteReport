using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Attachment
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AttachmentQuestion> AttachmentQuestions { get; set; } = new List<AttachmentQuestion>();
}
