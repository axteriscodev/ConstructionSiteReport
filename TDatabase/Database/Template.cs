using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Template
{
    public int Id { get; set; }

    public int? IdDescription { get; set; }

    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public DateTime Date { get; set; }

    public string? Note { get; set; }

    public bool Active { get; set; }

    public int? IdOrganization { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual TemplateDescription? IdDescriptionNavigation { get; set; }

    public virtual Organization? IdOrganizationNavigation { get; set; }

    public virtual ICollection<QuestionChosen> QuestionChosens { get; set; } = new List<QuestionChosen>();
}
