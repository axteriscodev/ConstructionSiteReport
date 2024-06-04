using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;
using ConstructionSiteLibrary.Repositories;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Shared.Defaults;
using Shared.Documents;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Templates;

public partial class TemplateCreation
{

    private List<TemplateCategoryModel> categories = [];

    //private IEnumerable<int> selected = [1,3,6];

    private List<IdCategoryAndQuestions> groups = [];

    private string title = "";


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

    [Parameter]
    public string Param { get; set; } = "";

    [Parameter]
    public TemplateModel? SelectedTemplate { get; set; }

    protected override async Task OnInitializedAsync()
    {
        initialLoading = true;
        await base.OnInitializedAsync();
        await LoadData();
        InitData();
        initialLoading = false;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (SelectedTemplate is not null)
        {
            initialLoading = true;
            await LoadData(SelectedTemplate);
            initialLoading = false;
        }
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

    private async Task LoadData(TemplateModel? selectedTemplate = null)
    {
        groups = [];
        categories = await CategoriesRepository.GetCategories();
        count = categories.Count;

        title = selectedTemplate?.TitleTemplate ?? "";

        foreach (var category in categories)
        {
            List<int> templateSelectedId = [];

            if (selectedTemplate is not null)
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

            bool? groupState = null;

            if (templateSelectedId.Count == 0)
            {
                groupState = false;
            }
            else if (templateSelectedId.Count == category.Questions.Count)
            {
                groupState = true;
            }

            //OrderElements(category.Questions.Cast<DocumentQuestionModel>());
            groups.Add(new() { Id = category.Id, Order = category.Order, Text = category.Text, State = groupState, Questions = category.Questions, SelectedQuestionIds = templateSelectedId });
        }


    }

    private async void OnTemplateSelected(int templateId)
    {
        var template = await TemplatesRepository.GetTemplateById(templateId);
    }

    private async Task ReloadTable()
    {
        DialogService.Close();
        await LoadData();
        await grid!.Reload();
    }

    private async Task CreateForm()
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
                };

                foreach (var selectedQuestionId in group.SelectedQuestionIds)
                {
                    var selectedQuestion = group.Questions.First(x => x.Id == selectedQuestionId);
                    category.Questions.Add(selectedQuestion);
                }

                templateCategories.Add(category);
            }
        }

        var document = new TemplateModel()
        {
            TitleTemplate = title,
            Categories = templateCategories,
        };

        await TemplatesRepository.SaveDocument(document);
        onSaving = false;
    }

    #region Visualizzazione

    private static string CategoryText(IdCategoryAndQuestions group)
    {
        return group.Order + ". " + group.Text;
    }

    private static string QuestionText(IdCategoryAndQuestions group, string questionText, int order)
    {
        return group.Order + "." + order + " " + questionText;
    }

    private void ShowQuestions(IdCategoryAndQuestions group)
    {
        group.ShowQuestion = !group.ShowQuestion;
    }

    private string AccordionIcon(IdCategoryAndQuestions group)
    {
        return group.ShowQuestion ? "remove" : "add";
    }

    #endregion

    #region Selezione categorie e domande 

    private async Task<bool> ChangeCheckBoxCategory(bool? value, IdCategoryAndQuestions group)
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

    private bool ValoreCheckBoxQuestion(int id, IdCategoryAndQuestions group)
    {
        return group.SelectedQuestionIds.Contains(id);
    }

    private async Task<bool> ChangeCheckBoxQuestion(bool value, int id, IdCategoryAndQuestions group)
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


    private void OrderCategories((int oldIndex, int newIndex) indici)
    {
        // spezzo la tupla
        var (oldIndex, newIndex) = indici;

        var items = groups;
        var itemToMove = items[oldIndex];
        items.RemoveAt(oldIndex);

        if (newIndex < items.Count)
        {
            items.Insert(newIndex, itemToMove);
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

    private void OrderList((int oldIndex, int newIndex, int groupNumber) indici)
    {
        // spezzo la tupla
        var (oldIndex, newIndex, groupNumber) = indici;

        var items = groups.Where(x => x.Id == groupNumber).SingleOrDefault().Questions;
        var itemToMove = items[oldIndex];
        items.RemoveAt(oldIndex);

        if (newIndex < items.Count)
        {
            items.Insert(newIndex, itemToMove);
        }
        else
        {
            items.Add(itemToMove);
        }

        //OrderElements(items);
        StateHasChanged();
    }

    private static void OrderElements(List<DocumentQuestionModel> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            //lista[i].Order = i + 1;
        }
    }

    #endregion

    #region Classe interna 

    class IdCategoryAndQuestions
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Text { get; set; } = "";
        public bool? State { get; set; } = false;
        public bool ShowQuestion = true;
        public List<int> SelectedQuestionIds { get; set; } = [];
        public List<TemplateQuestionModel> Questions { get; set; } = [];
    }

    #endregion
}

