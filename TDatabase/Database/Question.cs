using System;
using System.Collections.Generic;

namespace TDatabase.Database;

public partial class Question
{
    public int Id { get; set; }

    public int IdCategory { get; set; }

    public string Text { get; set; } = null!;

    public bool Active { get; set; }

    public int? IdOrganization { get; set; }

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual Organization? IdOrganizationNavigation { get; set; }

    public virtual ICollection<QuestionChoice> QuestionChoices { get; set; } = new List<QuestionChoice>();

    public virtual ICollection<QuestionChosen> QuestionChosens { get; set; } = new List<QuestionChosen>();
}
