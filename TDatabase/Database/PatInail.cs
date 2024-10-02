using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class PatInail
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Active { get; set; }

    public bool Hidden { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
}
