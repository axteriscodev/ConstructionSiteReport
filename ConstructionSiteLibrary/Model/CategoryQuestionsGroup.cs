using Shared.Templates;

namespace ConstructionSiteLibrary;

public class CategoryQuestionsGroup
{
    public int Id { get; set; }
    public int Order { get; set; }
    public string Text { get; set; } = "";
    public bool? State { get; set; } = false;
    public bool ShowQuestion = true;
    public List<int> SelectedQuestionIds { get; set; } = [];
    public List<TemplateQuestionModel> Questions { get; set; } = [];
}
