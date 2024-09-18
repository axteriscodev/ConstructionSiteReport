using Microsoft.AspNetCore.Components;
using ConstructionSiteLibrary.Components.Utilities;
using Radzen.Blazor;
using Shared.Documents;
using ConstructionSiteLibrary.Utility;
using Radzen;
using ConstructionSiteLibrary.Model;

namespace ConstructionSiteLibrary.Components.Companies;

public partial class TableCompanies
{
    ScreenComponent? screenComponent;

    private List<CompanyModel> companies = [];

    private List<CompanyModel> displayedCompanies = [];

    private bool onLoading = false;

    private string search = "";

    private int count = 0;

    private int pageSize = GlobalVariables.PageSize;

    private int pageIndex = 0;

    private string pagingSummaryFormat = "Pagina {0} di {1} (Totale {2} aziende)";

   

    protected override async Task OnInitializedAsync()
    {
        onLoading = true;
        await base.OnInitializedAsync();
        await LoadData();
        onLoading = false;
    }

    private async Task LoadData()
    {
        companies = await CompaniesRepository.GetCompanies();
        FilterCompanies();
    }

    
    private void PageChanged(AxtPagerEventArgs args)
    {
        pageIndex = args.CurrentPage;
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
        SelectCurrentPage();
    }

    private void SelectCurrentPage()
    {
        var skip = pageIndex * pageSize;
        displayedCompanies = displayedCompanies.Skip(skip).Take(pageSize).ToList();
    }

    private void OpenAddCompanyPage()
    {
        Navigation.ChangePage(PageRouting.AddCompanyPage);
    }

    private void OpenUpdateCompany(object item)
    {
        var company = item as CompanyModel;
        
        Navigation.ChangePage(PageRouting.AddCompanyPage + company.Id);
    }

    private async Task OpenDeleteCompany(object company)
    {
        var c = company as CompanyModel;

        await Hide(c ?? new());
    }
    
    private async Task Hide(CompanyModel company)
    {
        var titolo = "Elimina Azienda";
        var text = "Vuoi eliminare l'azienda selezionata?";
        var confirmationResult = await DialogService.Confirm(text, titolo, new ConfirmOptions{OkButtonText = "Sì", CancelButtonText = "No"});
        if(confirmationResult == true)
        {
            var response = await CompaniesRepository.HideCompanies([company]);
            if(response)
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
