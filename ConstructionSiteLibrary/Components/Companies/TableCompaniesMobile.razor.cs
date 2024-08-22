using Microsoft.AspNetCore.Components;
using ConstructionSiteLibrary.Components.Utilities;
using Radzen.Blazor;
using Shared.Documents;
using ConstructionSiteLibrary.Utility;
using Radzen;

namespace ConstructionSiteLibrary.Components.Companies;

public partial class TableCompaniesMobile
{
    private List<CompanyModel> companies = [];

    private List<CompanyModel> displayedCompanies = [];

    private bool initialLoading;

    private bool isLoading = false;

    private int count;

    private int pageSize = GlobalVariables.PageSize;

    private string pagingSummaryFormat = "Pagina {0} di {1} (Totale {2} ditte)";

    private string search = ""; 

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
        companies = await CompaniesRepository.GetCompanies();
        FilterCompanies();
    }

    private void SearchChanged(string args)
    {
        search = args;
        FilterCompanies();
    }

     private void FilterCompanies()
    {
        displayedCompanies = companies;
        search = search.TrimStart().TrimEnd();
        if(!string.IsNullOrEmpty(search))
        {
            displayedCompanies = companies.Where(x => x.CompanyName.Contains(search, StringComparison.InvariantCultureIgnoreCase) || x.SelfEmployedName.Contains(search, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        count = displayedCompanies.Count;
    }

    private void OpenAddCompanyPage()
    {
        Navigation.ChangePage(PageRouting.AddCompanyPage);
    }

    private void OpenUpdateCompany(CompanyModel company)
    {
        Navigation.ChangePage(PageRouting.AddCompanyPage + company.Id);
    }

    private async Task Hide(CompanyModel company)
    {
        var titolo = "Elimina Azienda";
        var text = "Vuoi eliminare l'azienda selezionata?";
        var confirmationResult = await DialogService.Confirm(text, titolo, new ConfirmOptions { OkButtonText = "Sì", CancelButtonText = "No" });
        if (confirmationResult == true)
        {
            var response = await CompaniesRepository.HideCompanies([company]);
            if (response)
            {
                await ReloadTable();
            }
        }
    }

    private string GetName(CompanyModel company)
    {
        return string.IsNullOrEmpty(company.CompanyName) ? company.SelfEmployedName : company.CompanyName;
    }

    private async Task ReloadTable()
    {
        DialogService.Close();
        await LoadData();
        //await grid.Reload();
    }
}
