using ConstructionSiteLibrary.Model.DocumentWizard;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Shared.Documents;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Documents.Wizard
{
    public partial class CompaniesStep
    {
        [Parameter]
        public EventCallback<DocumentStepArgs> OnForward { get; set; }
        [Parameter]
        public EventCallback OnBack { get; set; }
        [Parameter]
        public List<CompanyModel> SelectedCompanies { get; set; } = [];

        private List<CompanyModel> companies = [];

        private RadzenDropDown<CompanyModel>? companiesDropDown;
        private bool onloading = false;
        private bool forwardDisabled = true;



        protected override async Task OnInitializedAsync()
        {
            onloading = true;
            await base.OnInitializedAsync();
            await LoadData();
            onloading = false;
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        public async Task LoadData()
        {
            companies = await CompaniesRepository.GetCompanies();
        }


        public void OnSelectedCompany(object comp)
        {
            var temp = comp as CompanyModel;
            if(temp is not null)
            {
                companies.Remove(temp);
                SelectedCompanies.Add(temp);
                companiesDropDown.Reset();
            }
        }

        public void RemoveSelectedCompany(CompanyModel company)
        {
            SelectedCompanies.Remove(company);
            companies.Add(company);
        }



        #region Avanti e Indietro 

        public void Forward()
        {
            DocumentStepArgs args = new()
            {
                Object = SelectedCompanies,
                Step = DocumentStep.Companies
            };

            OnForward.InvokeAsync(args);
        }


        public void Back()
        {
            OnBack.InvokeAsync();
        }

        #endregion
    }
}
