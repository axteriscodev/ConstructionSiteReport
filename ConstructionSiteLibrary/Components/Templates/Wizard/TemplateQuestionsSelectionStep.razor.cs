using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Model;
using ConstructionSiteLibrary.Model.TemplateWizard;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Shared.Documents;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Templates.Wizard;

public partial class TemplateQuestionsSelectionStep
{


    [Parameter]
    required public TemplateModel CurrentTemplate { get; set; }

    [Parameter]
    public EventCallback<TemplateStepArgs> OnForward { get; set; }
    [Parameter]
    public EventCallback OnBack { get; set; }

    private QuestionsSelection? questionsSelection;

    //private string title = "";


    /// <summary>
    /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
    /// </summary>
    private bool initialLoading;

    

    ScreenComponent screenComponent;

    [Parameter]
    public string Param { get; set; } = "";


    protected override async Task OnInitializedAsync()
    {
        initialLoading = true;
        await base.OnInitializedAsync();
        //await LoadData(CurrentTemplate);
        //InitData();
        initialLoading = false;
    }

    protected override async Task OnParametersSetAsync()
    {
        StateHasChanged();
    }

    public void OnSave()
    {
        questionsSelection!.OnSave();
    }


    public void Back()
    {
        OnBack.InvokeAsync();
    }

}
