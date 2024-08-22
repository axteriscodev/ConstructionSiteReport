using ConstructionSiteLibrary.Components.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Shared.Defaults;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Categories;

public partial class TableCategoriesMobile
{
     private List<TemplateCategoryModel> categories = [];

    /// <summary>
    /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
    /// </summary>
    private bool initialLoading;


    /// <summary>
    /// Intero che ci dice quanti sono gli elementi
    /// </summary>
    private int count;

    /// <summary>
    /// Intero che ci dice quanti elementi possono stare in una pagina
    /// </summary>
    private int pageSize = 8;

    /// <summary>
    /// Stringa indica la pagina e gli elementi
    /// </summary>
    private string pagingSummaryFormat = "Pagina {0} di {1} (Totale {2} categorie)";

    [Parameter]
    public string Param { get; set; } = "";

    ScreenComponent screenComponent;

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

    private void SearchChanged()
    {
        
    }

    private async Task OpenOrderForm()
    {   
        var width = screenComponent.ScreenSize.Width;

        // creo uno style aggiuntivo da inviare al componente caricato con il popup come options
        var additionalStyle = $"min-height:fit-content;height:fit-content;width:{width}px;max-width:600px";
        var newOptions = new DialogOptions
        {
            Style = additionalStyle
        };
        //creo parametri da inviare al componente caricato con il popup
        var param = new Dictionary<string, object>
            {
                //tra i parametri che invio al dialog creo un EventCallback da passare al componente
                { "OnSaveComplete", EventCallback.Factory.Create(this, ReloadTable) },
                { "Categories", categories },
            };
        await DialogService.OpenAsync<SortCategories>("Ordinamento", parameters: param, options: newOptions);
    }

    /// <summary>
    /// Metodo che apre la form per aggiungere un record al sistema
    /// </summary>
    /// <returns></returns>
    private async Task OpenNewForm()
    {
        var width = screenComponent.ScreenSize.Width;

        // creo uno style aggiuntivo da inviare al componente caricato con il popup come options
        var additionalStyle = $"min-height:fit-content;height:fit-content;width:{width}px;max-width:600px";
        var newOptions = new DialogOptions
        {
            Style = additionalStyle
        };
        //creo parametri da inviare al componente caricato con il popup
        var param = new Dictionary<string, object>
            {
                //tra i parametri che invio al dialog creo un EventCallback da passare al componente
                { "OnSaveComplete", EventCallback.Factory.Create(this, ReloadTable) },
                { "CreationMode", true },
            };
        await DialogService.OpenAsync<FormCategories>("Nuova categoria", parameters: param, options: newOptions);
    }


    private async Task OpenUpdateForm(CategoryModel item)
    {
        var width = screenComponent.ScreenSize.Width;

        //creo uno style aggiuntivo da inviare al componente caricato con il popup come options
        var additionalStyle = $"min-height:fit-content;height:fit-content;width:{width}px;max-width:600px";
        var newOptions = new DialogOptions
        {
            Style = additionalStyle
        };
        //creo parametri da inviare al componente caricato con il popup
        var param = new Dictionary<string, object>
            {
                //tra i parametri che invio al dialog creo un EventCallback da passare al componente
                { "OnSaveComplete", EventCallback.Factory.Create(this, ReloadTable) },
                { "Object", item},
                {"CreationMode", false },
            };
        await DialogService.OpenAsync<FormCategories>("Aggiorna categoria", parameters: param, options: newOptions);
    }

    private async Task Hide(TemplateCategoryModel category)
    {
        var titolo = "Disattivazione sezione";
        var text = "Vuoi disattivare la sezione: " + category.Text + "?";
        var confirmationResult = await DialogService.Confirm(text, titolo, new ConfirmOptions { OkButtonText = "Si", CancelButtonText = "No" });
        Console.WriteLine("cliccato: " + confirmationResult);
        if (confirmationResult == true)
        {
            var response = await CategoriesRepository.HideCategories([category]);
            //NotificationService.Notify(response);
            if (response)
            {
                await ReloadTable();
            }
        }
    }

    private async Task ReloadTable()
    {
        DialogService.Close();
        await LoadData();
    }
}
