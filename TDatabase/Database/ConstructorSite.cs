using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class ConstructorSite
{
    public int Id { get; set; }

    public int IdClient { get; set; }

    public string Name { get; set; } = null!;

    public string JobDescription { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual Client IdClientNavigation { get; set; } = null!;
}
