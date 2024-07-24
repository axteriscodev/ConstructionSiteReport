using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Role
{
    public int Id { get; set; }

    public int IdOrganization { get; set; }

    public string Name { get; set; } = null!;

    public bool DocumentsManagement { get; set; }

    public bool UsersManagement { get; set; }

    public bool Active { get; set; }

    public bool Hidden { get; set; }

    public virtual Organization IdOrganizationNavigation { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
