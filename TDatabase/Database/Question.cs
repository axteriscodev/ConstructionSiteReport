using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Question
{
    public int Id { get; set; }

    public int IdSubCategory { get; set; }

    public string Text { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<AttachmentQuestion> AttachmentQuestions { get; set; } = new List<AttachmentQuestion>();

    public virtual SubCategory IdSubCategoryNavigation { get; set; } = null!;

    public virtual ICollection<QuestionChosen> QuestionChosens { get; set; } = new List<QuestionChosen>();

    public virtual ICollection<Choice> IdChoices { get; set; } = new List<Choice>();
}
