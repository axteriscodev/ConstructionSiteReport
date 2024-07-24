using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Organization
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public bool Active { get; set; }

    public bool Hidden { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Choice> Choices { get; set; } = new List<Choice>();

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<ConstructorSite> ConstructorSites { get; set; } = new List<ConstructorSite>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<TemplateDescription> TemplateDescriptions { get; set; } = new List<TemplateDescription>();

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
