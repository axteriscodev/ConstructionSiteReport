using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Model;
using ConstructionSiteLibrary.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Templates;

public partial class TableTemplates
{
    ScreenComponent? screenComponent;

    private List<TemplateModel> templates = [];

    private List<TemplateModel> displayedTemplates = [];

    private bool onLoading = false;

    private string search = "";

    private int count = 0;

    private int pageSize = GlobalVariables.PageSize;

    private int pageIndex = 0;

    private string pagingSummaryFormat = "Pagina {0} di {1} (Totale {2} template)";


    [Parameter]
    public EventCallback<TemplateModel> GetTemplate { get; set; }



    protected override async Task OnInitializedAsync()
    {
        onLoading = true;
        await base.OnInitializedAsync();
        await LoadData();
        onLoading = false;
    }

    private async Task LoadData()
    {
        templates = await TemplatesRepository.GetTemplates();
        FilterTemplates();
    }

    private async Task Hide(TemplateModel template)
    {
        var titolo = "Disattivazione template";
        var text = "Vuoi disattivare il template: " + template.TitleTemplate + "?";
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

    private void PageChanged(AxtPagerEventArgs args)
    {
        pageIndex = args.CurrentPage;
        FilterTemplates();
    }

    private void SearchChanged(string args)
    {
        search = args;
        FilterTemplates();
    }

    private void FilterTemplates()
    {
        displayedTemplates = templates;
        search = search.TrimStart().TrimEnd();
        if (!string.IsNullOrEmpty(search))
        {
            displayedTemplates = templates.Where(x => x.TitleTemplate.Contains(search, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        count = displayedTemplates.Count;
        SelectCurrentPage();
    }


    private void SelectCurrentPage()
    {
        var skip = pageIndex * pageSize;
        displayedTemplates = displayedTemplates.Skip(skip).Take(pageSize).ToList();
    }


    private void OnTemplateSelected(TemplateModel selectedTemplate)
    {
        GetTemplate.InvokeAsync(selectedTemplate);
    }

    private async Task OpenModifyTemplate(object question)
    {
        //var q = question as TemplateModel;
        Console.WriteLine("non è prevista la modifica dei template");
    }

    private async Task OpenDeleteTemplate(object template)
    {
        var t = template as TemplateModel;

        await Hide(t ?? new());
    }

    private async Task ReloadTable()
    {
        DialogService.Close();
        await LoadData();
        //await grid!.Reload();
    }
}
