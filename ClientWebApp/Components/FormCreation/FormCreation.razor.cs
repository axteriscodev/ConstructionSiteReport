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
    }

    private async Task ReloadTable()
    {
        //DialogService.Close();
        await LoadData();
        //await grid!.Reload();
    }

    private void Change(IEnumerable<int>  args, int index)
    {
        //var values = args.ToList();

        //Console.WriteLine(string.Join( ",", values));
        //Console.WriteLine(index);

        var asd = selected.Where(x => x.Id == index).FirstOrDefault();
        
        if(args is not null) 
        {
            asd.QuestionIds.Append(args.First());
        }

        Console.WriteLine(asd.QuestionIds.Count());
        
    }

    class IdCategoryAndQuestions
    {
        public int Id { get; set;}

        public IEnumerable<int> QuestionIds { get; set; } = [];
    }

}
