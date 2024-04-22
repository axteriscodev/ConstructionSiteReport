using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string Vatcode { get; set; } = null!;

    public virtual ICollection<Document> IdDocuments { get; set; } = new List<Document>();
}
