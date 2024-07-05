using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Templates;

public partial class TableTemplates
{
    private List<TemplateModel> templates = [];

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

    /// <summary>
    /// Intero che ci dice quanti elementi possono stare in una pagina
    /// </summary>
    private int pageSize = 8;

    /// <summary>
    /// Stringa indica la pagina e gli elementi
    /// </summary>
    private string pagingSummaryFormat = "Pagina {0} di {1} (Totale {2} categorie)";

    /// <summary>
    /// Riferimento al componente tabella
    /// </summary>
    private RadzenDataGrid<TemplateModel>? grid;

    [Parameter]
    public string Param { get; set; } = "";

    [Parameter] 
    public EventCallback<TemplateModel> GetTemplate { get; set; }

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
        templates = await TemplatesRepository.GetTemplates();
        count = templates.Count;
    }

    private async Task Hide(TemplateModel template)
    {
        var titolo = "Disattivazione sezione";
        var text = "Vuoi disattivare la sezione: " + template.TitleTemplate + "?";
        var confirmationResult = await DialogService.Confirm(text, titolo, new ConfirmOptions { OkButtonText = "Si", CancelButtonText = "No" });
        Console.WriteLine("cliccato: " + confirmationResult);
        if (confirmationResult == true)
        {
            var response = await TemplatesRepository.HideTemplate([template]);
            //NotificationService.Notify(response);
            if (response)
            {
                await ReloadTable();
            }
        }
    }

    private void OpenWizard()
    {
        NavigationService.ChangePage(PageRouting.TemplateWizardPage);
    }


    private void OnTemplateSelected(TemplateModel selectedTemplate)
    {
        GetTemplate.InvokeAsync(selectedTemplate);
    }

    private async Task ReloadTable()
    {
        DialogService.Close();
        await LoadData();
        await grid!.Reload();
    }
}
