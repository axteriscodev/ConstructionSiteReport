using ConstructionSiteLibrary.Components.Utilities;
using Microsoft.AspNetCore.Components;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Templates.Wizard;

public partial class TemplateSelectionStep
{
    List<TemplateModel> Templates = [];

    TemplateModel? CurrentSelection;

    /// <summary>
    /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
    /// </summary>
    private bool initialLoading;

    [Parameter]
    public EventCallback<TemplateStepArgs> OnForward { get; set; }
    

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
        Templates = await TemplatesRepository.GetTemplates();
    }

    public void Forward()
    {
        TemplateStepArgs args = new()
            {
                // Object = SelectedTemplate,
                // Step = DocumentStep.ChoiceTemplate
            };

        OnForward.InvokeAsync(args);
    }

}
