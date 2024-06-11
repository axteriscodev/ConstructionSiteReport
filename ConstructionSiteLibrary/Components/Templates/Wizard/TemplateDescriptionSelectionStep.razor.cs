using ConstructionSiteLibrary.Components.Utilities;
using Microsoft.AspNetCore.Components;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Templates.Wizard;

public partial class TemplateDescriptionSelectionStep
{
    List<TemplateDescriptionModel> TemplatesDescriptions = [];

    TemplateDescriptionModel? CurrentSelection;

    [Parameter]
    public EventCallback<TemplateStepArgs> OnForward { get; set; }
    [Parameter]
    public EventCallback OnBack { get; set; }

    /// <summary>
    /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
    /// </summary>
    private bool initialLoading;

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
        TemplatesDescriptions = await TemplatesRepository.GetTemplatesDescriptions();
    }


    public void Forward()
    {
        TemplateStepArgs args = new()
        {
            // Object = Document,
            // Step = DocumentStep.Description
        };

        OnForward.InvokeAsync(args);
    }

    public void Back()
    {
        OnBack.InvokeAsync();
    }
}
