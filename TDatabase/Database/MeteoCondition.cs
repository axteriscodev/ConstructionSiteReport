using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class MeteoCondition
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public string? Note { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
