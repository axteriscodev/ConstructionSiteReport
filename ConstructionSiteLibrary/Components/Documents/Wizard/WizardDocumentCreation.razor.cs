using ConstructionSiteLibrary.Model.DocumentWizard;
using Radzen.Blazor;
using Shared.Documents;
using Shared.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionSiteLibrary.Components.Documents.Wizard
{
    public partial class WizardDocumentCreation
    {

        private TemplateModel? _selectedTemplate;
        private DocumentModel _document = new();

        private RadzenSteps? _stepsComponent;

        private void Back()
        {
            _stepsComponent?.PrevStep();
        }

        private void Forward(DocumentStepArgs args)
        {

            switch (args.Step)
            {
                case DocumentStep.ChoiceTemplate:
                    _selectedTemplate = args.Object as TemplateModel;
                    break;
                case DocumentStep.Description:
                    if(args.Object is not null)
                    {
                        _document = (args.Object as DocumentModel)!;
                    }
                    break;
                case DocumentStep.Companies:
                    if(args.Object is not null)
                    {
                        _document.Companies = (args.Object as List<CompanyModel>)!;
                    }
                    break;
                default:
                    break;
            }
            _stepsComponent?.NextStep();

        }


    }
}
