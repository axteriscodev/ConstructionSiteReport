using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Model.TemplateWizard;
using ConstructionSiteLibrary.Utility;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Templates.Wizard;


public partial class WizardTemplateCreation
{


    private TemplateModel? _template = new();

    private bool onSaving;


    /// <summary>
    /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
    /// </summary>
    private bool initialLoading;

    private TemplateQuestionsSelectionStep templateQuestionsSelectionStep;

    ScreenComponent screenComponent;

    private RadzenSteps? _stepsComponent;

    // [Parameter]
    // public int SiteId { get; set; }


    
    private void OnTemplateChanged(TemplateModel? template)
    {
        if(template is not null)
        {
            _template.Categories = template.Categories;
            _template.Description = template.Description;
        }
        else
        {
            _template.Categories = [];
            _template.Description = new();
        }
        StateHasChanged();
    }

    private async Task SaveTemplate()
    {
        templateQuestionsSelectionStep.OnSave();

        if (_template.NameTemplate is not null && !string.IsNullOrEmpty(_template.NameTemplate.Trim()))
        {
            onSaving = true;

            _template.TitleTemplate = _template.NameTemplate;

            await TemplatesRepository.SaveTemplate(_template);

            onSaving = false;

            NavigationService.ChangePage(PageRouting.TemplatePage);
        }
        else
        {
            //Title = null;
        }
    }

}
