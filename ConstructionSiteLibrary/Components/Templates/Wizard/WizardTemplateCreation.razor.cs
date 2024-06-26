using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Model.TemplateWizard;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Templates.Wizard;


public partial class WizardTemplateCreation
{


    private TemplateModel? _template;


    /// <summary>
    /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
    /// </summary>
    private bool initialLoading;

    ScreenComponent screenComponent;

    private RadzenSteps? _stepsComponent;

    [Parameter]
    public int SiteId { get; set; }


    private void Back()
    {
        _stepsComponent?.PrevStep();
    }

    private void Forward(TemplateStepArgs args)
    {

        switch (args.Step)
        {
            case TemplateStep.TemplateSelection:
                if (args.Object is not null)
                {
                    _template = (args.Object as TemplateModel)!;
                }
                break;
            case TemplateStep.Description:
                if (args.Object is not null)
                {
                    _template.Description = (args.Object as TemplateDescriptionModel)!;
                }
                break;
            case TemplateStep.Questions:
                if (args.Object is not null)
                {
                    _template = (args.Object as TemplateModel)!;
                }
                break;
            case TemplateStep.Title:
                break;
            default:
                break;
        }
        _stepsComponent?.NextStep();

    }
}
