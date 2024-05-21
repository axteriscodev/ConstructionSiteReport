using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Document
{
    public int Id { get; set; }

    public int? IdConstructorSite { get; set; }

    public int? IdClient { get; set; }

    public string Title { get; set; } = null!;

    public DateTime Date { get; set; }

    public virtual ICollection<CompanyDocument> CompanyDocuments { get; set; } = new List<CompanyDocument>();

    public virtual Client? IdClientNavigation { get; set; }

    public virtual ConstructorSite? IdConstructorSiteNavigation { get; set; }

    public virtual ICollection<QuestionChosen> QuestionChosens { get; set; } = new List<QuestionChosen>();
}
