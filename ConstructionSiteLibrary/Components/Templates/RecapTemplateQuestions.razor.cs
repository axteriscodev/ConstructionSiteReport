using Microsoft.AspNetCore.Components;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Templates;

public partial class RecapTemplateQuestions
{

    [Parameter]
    public TemplateModel? SelectedTemplate { get; set; }

    private List<CategoryQuestionsGroup> groups = [];

    private bool onloading = false;

    protected override async Task OnInitializedAsync()
    {
        onloading = true;
        await base.OnInitializedAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        onloading = true;
        await base.OnParametersSetAsync();
        LoadData();
        onloading = false;
    }

    public void LoadData()
    {
        groups = [];

        if (SelectedTemplate is not null)
        {
            foreach (var category in SelectedTemplate!.Categories)
            {
                OrderElements(category.Questions);
                groups.Add(new() { Id = category.Id, Text = category.Text, Order = category.Order, Questions = category.Questions });
            }
        }


    }


    private static void OrderElements(List<TemplateQuestionModel> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            lista[i].Order = i + 1;
        }
    }

    private static string CategoryText(CategoryQuestionsGroup group)
    {
        return group.Order + ". " + group.Text;
    }

    private static string QuestionText(CategoryQuestionsGroup group, string questionText, int order)
    {
        return group.Order + "." + order + " " + questionText;
    }

    private void ShowQuestions(CategoryQuestionsGroup group)
    {
        group.ShowQuestion = !group.ShowQuestion;
    }

    private string AccordionIcon(CategoryQuestionsGroup group)
    {
        return group.ShowQuestion ? "remove" : "add";
    }

}
