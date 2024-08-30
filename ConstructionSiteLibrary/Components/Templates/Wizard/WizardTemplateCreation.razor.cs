using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Model.TemplateWizard;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Templates.Wizard;


public partial class WizardTemplateCreation
{


    private TemplateModel? _template = new();


    /// <summary>
    /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
    /// </summary>
    private bool initialLoading;

    ScreenComponent screenComponent;

    private RadzenSteps? _stepsComponent;

    // [Parameter]
    // public int SiteId { get; set; }


    
    private void OnTemplateChanged(TemplateModel? template)
    {
        if(template is not null)
        {
            _template = template;
        }
        else
        {
            _template.IdTemplate = 0;
        }
        StateHasChanged();
    }

    // private void Forward(TemplateStepArgs args)
    // {

    //     switch (args.Step)
    //     {
    //         case TemplateStep.TemplateSelection:
    //             if (args.Object is not null)
    //             {
    //                 _template = (args.Object as TemplateModel)!;
    //             }
    //             break;
    //         case TemplateStep.Description:
    //             if (args.Object is not null)
    //             {
    //                 _template.Description = (args.Object as TemplateDescriptionModel)!;
    //             }
    //             break;
    //         case TemplateStep.Questions:
    //             if (args.Object is not null)
    //             {
    //                 _template = (args.Object as TemplateModel)!;
    //             }
    //             break;
    //         case TemplateStep.Title:
    //             break;
    //         default:
    //             break;
    //     }
    //     _stepsComponent?.NextStep();

    // }

    // private void Back()
    // {
    //     _stepsComponent?.PrevStep();
    // }

}
