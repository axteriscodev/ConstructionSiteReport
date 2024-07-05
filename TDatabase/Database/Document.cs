using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Document
{
    public int Id { get; set; }

    public int IdConstructorSite { get; set; }

    public int? IdClient { get; set; }

    public int IdTemplate { get; set; }

    public int? IdMeteo { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Cse { get; set; }

    public string? DraftedIn { get; set; }

    public string? CompletedIn { get; set; }

    public DateTime? CompilationDate { get; set; }

    public DateTime? LastEditDate { get; set; }

    public DateTime? CreationDate { get; set; }

    public bool ReadOnly { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual ICollection<CompanyDocument> CompanyDocuments { get; set; } = new List<CompanyDocument>();

    public virtual Client? IdClientNavigation { get; set; }

    public virtual ConstructorSite IdConstructorSiteNavigation { get; set; } = null!;

    public virtual MeteoCondition? IdMeteoNavigation { get; set; }

    public virtual Template IdTemplateNavigation { get; set; } = null!;

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual ICollection<QuestionAnswered> QuestionAnswereds { get; set; } = new List<QuestionAnswered>();
}
