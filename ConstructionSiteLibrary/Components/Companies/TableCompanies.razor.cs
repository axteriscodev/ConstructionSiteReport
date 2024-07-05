using Microsoft.AspNetCore.Components;
using ConstructionSiteLibrary.Components.Utilities;
using Radzen.Blazor;
using Shared.Documents;
using ConstructionSiteLibrary.Utility;

namespace ConstructionSiteLibrary.Components.Companies;

public partial class TableCompanies
{
    private List<CompanyModel> companies = [];

    private bool initialLoading;

    private bool isLoading = false;

    private int count;

    private int pageSize = 8;

    private string pagingSummaryFormat = "Pagina {0} di {1} (Totale {2} aziende)";

    private RadzenDataGrid<CompanyModel>? grid;

    [Parameter]
    public string Param { get; set; } = "";

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
        count = companies.Count;
    }

    private void OpenAddCompanyPage()
    {
        Navigation.ChangePage(PageRouting.AddCompanyPage);
    }

    private async Task ReloadTable()
    {
        await LoadData();
        await grid.Reload();
    }
}
