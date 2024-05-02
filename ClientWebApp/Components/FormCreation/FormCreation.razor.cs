using System.Security.Cryptography.X509Certificates;
using ClientWebApp.Repositories;
using Microsoft.AspNetCore.Components;
using Shared;

namespace ClientWebApp.Components.FormCreation;

public partial class FormCreation
{

    private List<CategoryModel> categories = [];

    //private IEnumerable<int> selected = [1,3,6];

    private List<IdCategoryAndQuestions> selected = [];

    /// <summary>
    /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
    /// </summary>
    private bool initialLoading;

    /// <summary>
    /// Booleano che è impostata durante una ricerca
    /// </summary>
    private bool isLoading = false;

    /// <summary>
    /// Intero che ci dice quanti sono gli elementi
    /// </summary>
    private int count;


    [Parameter]
    public string Param { get; set; } = "";

    protected override async Task OnInitializedAsync()
    {
        initialLoading = true;
        await base.OnInitializedAsync();
        await LoadData();
        initialLoading = false;
    }

    private async Task LoadData()
    {
        categories = await CategoriesRepository.GetCategories();
        count = categories.Count;
        foreach (var category in categories)
        {
            selected.Add(new() { Id = category.Id, Text = category.Text, Questions = category.Questions });
        }
    }

    private async Task ReloadTable()
    {
        //DialogService.Close();
        await LoadData();
        //await grid!.Reload();
    }

    private async Task CreateForm()
    {
        List<CategoryModel> documentCategories = [];

        foreach (var selection in selected)
        {
            if (selection.SelectedQuestionIds.Any())
            {
                var category = new CategoryModel()
                {
                    Id = selection.Id,
                    Text = selection.Text,
                };

                foreach (var selectedQuestionId in selection.SelectedQuestionIds)
                {
                    var selectedQuestion = selection.Questions.First(x => x.Id == selectedQuestionId);
                    category.Questions.Add(selectedQuestion);
                }

                documentCategories.Add(category);
            }
        }

        var document = new DocumentModel()
        {
            Categories = documentCategories,
        };

        await DocumentsRepository.SaveDocument(document);
    }

}

class IdCategoryAndQuestions
{
    public int Id { get; set; }
    public string Text { get; set; } = "";
    public IEnumerable<int> SelectedQuestionIds { get; set; } = [];
    public List<QuestionModel> Questions { get; set; } = [];
}
