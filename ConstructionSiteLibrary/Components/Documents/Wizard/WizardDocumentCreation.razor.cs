using ConstructionSiteLibrary.Model.DocumentWizard;
using ConstructionSiteLibrary.Repositories;
using Microsoft.AspNetCore.Components;
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
        [Parameter]
        public int SiteId { get; set; }

        private TemplateModel? _selectedTemplate;
        private DocumentModel _document = new();

        private ConstructorSiteModel _constructorSite = new();

        private RadzenSteps? _stepsComponent;

        protected override async Task OnParametersSetAsync()
        {
            _constructorSite = await ConstructorSitesRepository.GetConstructorSiteInfo(SiteId);
            await base.OnParametersSetAsync();
        }


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
                    if (args.Object is not null)
                    {
                        _document = (args.Object as DocumentModel)!;
                        _document.ConstructorSite = _constructorSite;
                        _document.IdTemplate = _selectedTemplate.IdTemplate;
                    }
                    break;
                case DocumentStep.Companies:
                    if (args.Object is not null)
                    {
                        _document.Companies = (args.Object as List<CompanyModel>)!;
                    }
                    break;
                case DocumentStep.Save:
                    break;
                default:
                    break;
            }
            _stepsComponent?.NextStep();

        }


    }
}
