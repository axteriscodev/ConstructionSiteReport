using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Question
{
    public int Id { get; set; }

    public int IdMacroCategory { get; set; }

    public int? IdSubCategory { get; set; }

    public string Text { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<AttachmentQuestion> AttachmentQuestions { get; set; } = new List<AttachmentQuestion>();

    public virtual MacroCategory IdMacroCategoryNavigation { get; set; } = null!;

    public virtual SubCategory? IdSubCategoryNavigation { get; set; }

    public virtual ICollection<QuestionChoice> QuestionChoices { get; set; } = new List<QuestionChoice>();

    public virtual ICollection<QuestionChosen> QuestionChosens { get; set; } = new List<QuestionChosen>();
}
