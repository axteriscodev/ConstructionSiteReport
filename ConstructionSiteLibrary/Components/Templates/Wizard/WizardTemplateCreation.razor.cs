using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Model.TemplateWizard;
using Radzen.Blazor;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Templates.Wizard;


public partial class WizardTemplateCreation
{


    private TemplateModel? _selectedTemplate;

    private TemplateModel newTemplate = new();

    /// <summary>
    /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
    /// </summary>
    private bool initialLoading;

    ScreenComponent screenComponent;

    private RadzenSteps? _stepsComponent;


     private void Back()
        {
            _stepsComponent?.PrevStep();
        }

        private void Forward(TemplateStepArgs args)
        {

            switch (args.Step)
            {
                case TemplateStep.TemplateSelection:
                    if(args.Object is not null)
                    {
                        newTemplate = (args.Object as TemplateModel)!;
                    }
                    //_selectedTemplate = args.Object as TemplateModel;
                    break;
                case TemplateStep.Description:
                    if(args.Object is not null)
                    {
                        //_ = (args.Object as string)!;
                    }
                    break;
                case TemplateStep.Questions:
                    if(args.Object is not null)
                    {
                        
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
