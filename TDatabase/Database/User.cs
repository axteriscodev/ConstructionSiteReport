using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class User
{
    public int Id { get; set; }

    public int IdOrganization { get; set; }

    public int IdRole { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Phone { get; set; }

    public bool Active { get; set; }

    public bool Hidden { get; set; }

    public virtual Organization IdOrganizationNavigation { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<UserAttachment> UserAttachments { get; set; } = new List<UserAttachment>();
}
