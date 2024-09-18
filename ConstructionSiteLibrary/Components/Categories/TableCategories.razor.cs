using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Model;
using ConstructionSiteLibrary.Repositories;
using ConstructionSiteLibrary.Utility;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Shared.Defaults;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Categories;

public partial class TableCategories
{
    ScreenComponent screenComponent;

    private List<TemplateCategoryModel> categories = [];

    private List<TemplateCategoryModel> displayedCategories = [];

    private bool onLoading = false;

    private string search = "";

    private int count = 0;

    private int pageSize = GlobalVariables.PageSize;

    private int pageIndex = 0;

    private string pagingSummaryFormat = "Pagina {0} di {1} (Totale {2} categorie)";

    

    

    protected override async Task OnInitializedAsync()
    {
        onLoading = true;
        await base.OnInitializedAsync();
        await LoadData();
        onLoading = false;
    }

    private async Task LoadData()
    {
        categories = await CategoriesRepository.GetCategories();
        FilterCategories();
    }

    private void PageChanged(AxtPagerEventArgs args)
    {
        pageIndex = args.CurrentPage;
        FilterCategories();
    }

    private void SearchChanged(string args)
    {
        search = args;
        FilterCategories();
    }

    private void FilterCategories()
    {
        displayedCategories = categories;
        search = search.TrimStart().TrimEnd();
        if(!string.IsNullOrEmpty(search))
        {
            displayedCategories = categories.Where(x => x.Text.Contains(search, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        count = displayedCategories.Count;
        SelectCurrentPage();
    }

    private void SelectCurrentPage()
    {
        var skip = pageIndex * pageSize;
        displayedCategories = displayedCategories.Skip(skip).Take(pageSize).ToList();
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


    private async Task OpenUpdateForm(object category)
    {
        var item = category as TemplateCategoryModel;

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
                { "Object", item ?? new()},
                {"CreationMode", false },
            };
        await DialogService.OpenAsync<FormCategories>("Aggiorna categoria", parameters: param, options: newOptions);
    }

    private async Task Hide(object category)
    {
        var item = category as TemplateCategoryModel;

        var titolo = "Disattivazione categoria";
        var text = "Vuoi disattivare la categoria: " + item.Text + "?";
        var confirmationResult = await DialogService.Confirm(text, titolo, new ConfirmOptions { OkButtonText = "Si", CancelButtonText = "No" });
        Console.WriteLine("cliccato: " + confirmationResult);
        if (confirmationResult == true)
        {
            var response = await CategoriesRepository.HideCategories([item]);
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
