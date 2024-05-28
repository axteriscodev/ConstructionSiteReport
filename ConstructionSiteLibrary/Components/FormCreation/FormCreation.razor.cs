using System.Security.Cryptography.X509Certificates;
using ConstructionSiteLibrary.Repositories;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared;

namespace ConstructionSiteLibrary.Components.FormCreation;

public partial class FormCreation
{

    private List<CategoryModel> categories = [];

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
    /// il design degli elementi della form
    /// </summary>
    readonly Variant variant = Variant.Outlined;

    [Parameter]
    public string Param { get; set; } = "";

    protected override async Task OnInitializedAsync()
    {
        initialLoading = true;
        await base.OnInitializedAsync();
        //await LoadData();
        initialLoading = false;
    }

    // private async Task LoadData()
    // {
    //     categories = await CategoriesRepository.GetCategories();
    //     count = categories.Count;
    //     foreach (var category in categories)
    //     {

    //         //OrderElements(category.Questions.Cast<DocumentQuestionModel>());
    //         groups.Add(new() { Id = category.Id, Order=category.Order, Text = category.Text, Questions = category.Questions });
    //     }
    // }

    private async Task ReloadTable()
    {
        //DialogService.Close();
        //await LoadData();
        //await grid!.Reload();
    }

    private async Task CreateForm()
    {
        onSaving = true;
        List<CategoryModel> documentCategories = [];

        foreach (var group in groups)
        {
            if (group.SelectedQuestionIds.Any())
            {
                var category = new CategoryModel()
                {
                    Id = group.Id,
                    Text = group.Text,
                };

                foreach (var selectedQuestionId in group.SelectedQuestionIds)
                {
                    var selectedQuestion = group.Questions.First(x => x.Id == selectedQuestionId);
                    //category.Questions.Add(selectedQuestion);
                }

                documentCategories.Add(category);
            }
        }

        var document = new DocumentModel()
        {
            Categories = documentCategories,
        };

        await DocumentsRepository.SaveDocument(document);
        onSaving = false;
    }

    #region Visualizzazione

    private static string CategoryText(IdCategoryAndQuestions group)
    {
        return group.Order + ". " + group.Text;
    }

    private static string QuestionText(IdCategoryAndQuestions group, string questionText, int order)
    {
        return group.Order + "."  + order + " " + questionText;
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

        var items = groups.Where(x=>x.Id == groupNumber).SingleOrDefault().Questions;
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
        public List<QuestionModel> Questions { get; set; } = [];
    }

    #endregion
}

