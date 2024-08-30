using ConstructionSiteLibrary.Model;
using ConstructionSiteLibrary.Model.TemplateWizard;
using ConstructionSiteLibrary.Repositories;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Shared.Documents;
using Shared.Templates;
using ConstructionSiteLibrary.Components.Utilities;

namespace ConstructionSiteLibrary.Components.Templates;

public partial class QuestionsSelection
{
    private List<TemplateCategoryModel> categories = [];



    private List<CategoryQuestionsGroup> groups = [];

    [Parameter]
    required public TemplateModel CurrentTemplate { get; set; }




    /// <summary>
    /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
    /// </summary>
    private bool initialLoading;

    /// <summary>
    /// Booleano che è impostata durante una ricerca
    /// </summary>
    private bool isLoading = false;

    private bool onSaving = false;

    /// <summary>
    /// Intero che ci dice quanti sono gli elementi
    /// </summary>
    private int count;

    /// <summary>
    /// Riferimento al componente tabella
    /// </summary>
    private RadzenDataGrid<TemplateCategoryModel>? grid;


    /// <summary>
    /// il design degli elementi della form
    /// </summary>
    readonly Variant variant = Variant.Outlined;

    ScreenComponent screenComponent;

    [Parameter]
    public string Param { get; set; } = "";


    protected override async Task OnInitializedAsync()
    {
        initialLoading = true;
        await base.OnInitializedAsync();
        await LoadData(CurrentTemplate);
        //InitData();
        initialLoading = false;
    }

    protected override async Task OnParametersSetAsync()
    {
        await LoadData(CurrentTemplate);
    }


    private void InitData()
    {
        foreach (var group in groups)
        {
            group.State = true;
            foreach (var question in group.Questions)
            {
                group.SelectedQuestionIds.Add(question.Id);
            }
        }
    }

    private async Task LoadData(TemplateModel selectedTemplate)
    {
        groups = [];
        categories = await CategoriesRepository.GetCategories();
        count = categories.Count;

        //title = selectedTemplate?.TitleTemplate ?? "";

        foreach (var category in categories)
        {
            List<int> templateSelectedId = [];

            if (selectedTemplate.Categories.Count != 0)
            {
                var tempCat = selectedTemplate.Categories.Where(c => c.Id == category.Id).FirstOrDefault();

                if (tempCat is not null)
                {
                    foreach (var question in tempCat.Questions)
                    {
                        templateSelectedId.Add(question.Id);
                    }
                }
            }
            else
            {
                foreach (var question in category.Questions)
                {
                    templateSelectedId.Add(question.Id);
                }
            }

            bool? groupState = null;

            if (templateSelectedId.Count == 0)
            {
                groupState = false;
            }
            else if (templateSelectedId.Count == category.Questions.Count)
            {
                groupState = true;
            }

            OrderElements(category.Questions);
            groups.Add(new() { Id = category.Id, Order = category.Order, Text = category.Text, State = groupState, Questions = category.Questions, SelectedQuestionIds = templateSelectedId });

        }


    }


    private async Task ReloadTable()
    {
        DialogService.Close();
        await LoadData(CurrentTemplate);
        await grid!.Reload();
    }

    public TemplateStepArgs OnSave()
    {
        onSaving = true;
        List<TemplateCategoryModel> templateCategories = [];

        foreach (var group in groups)
        {
            if (group.SelectedQuestionIds.Any())
            {
                var category = new TemplateCategoryModel()
                {
                    Id = group.Id,
                    Text = group.Text,
                    Order = group.Order,
                };

                foreach (var selectedQuestionId in group.SelectedQuestionIds)
                {
                    var selectedQuestion = group.Questions.First(x => x.Id == selectedQuestionId);
                    category.Questions.Add(selectedQuestion);
                }

                templateCategories.Add(category);
            }
        }

        CurrentTemplate.Categories = templateCategories;

        TemplateStepArgs args = new()
        {
            Object = CurrentTemplate,
            Step = TemplateStep.Questions,
        };

        return args;


    }

    #region Visualizzazione

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

    #endregion

    #region Selezione categorie e domande 

    private async Task<bool> ChangeCheckBoxCategory(bool? value, CategoryQuestionsGroup group)
    {
        value ??= true;
        if (value.Value)
        {
            foreach (var q in group.Questions)
            {
                group.SelectedQuestionIds.Add(q.Id);
            }
        }
        else
        {
            group.SelectedQuestionIds = [];
        }
        group.State = value.Value;
        return false;
    }

    private bool ValoreCheckBoxQuestion(int id, CategoryQuestionsGroup group)
    {
        return group.SelectedQuestionIds.Contains(id);
    }

    private async Task<bool> ChangeCheckBoxQuestion(bool value, int id, CategoryQuestionsGroup group)
    {
        if (value)
        {
            group.SelectedQuestionIds.Add(id);
            group.State = group.SelectedQuestionIds.Count == group.Questions.Count ? true : null;
        }
        else
        {
            group.SelectedQuestionIds.Remove(id);
            group.State = group.SelectedQuestionIds.Count > 0 ? null : false;
        }
        return false;
    }

    #endregion

    #region Ordinamento domande 


    private void OrderCategories(ChangeObjectIndex indici)
    {

        var items = groups;
        var itemToMove = items[indici.OldIndex];
        items.RemoveAt(indici.OldIndex);

        if (indici.NewIndex < items.Count)
        {
            items.Insert(indici.NewIndex, itemToMove);
        }
        else
        {
            items.Add(itemToMove);
        }

        for (int i = 0; i < groups.Count; i++)
        {
            groups[i].Order = i + 1;
        }
    }

    private void OrderList(ChangeObjectIndex indice)
    {
        var items = groups.Where(x => x.Id == indice.GroupNumber).SingleOrDefault().Questions;
        var itemToMove = items[indice.OldIndex];
        items.RemoveAt(indice.OldIndex);

        if (indice.NewIndex < items.Count)
        {
            items.Insert(indice.NewIndex, itemToMove);
        }
        else
        {
            items.Add(itemToMove);
        }

        OrderElements(items);
        StateHasChanged();
    }

    private static void OrderElements(List<TemplateQuestionModel> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            lista[i].Order = i + 1;
        }
    }


    #endregion
}
