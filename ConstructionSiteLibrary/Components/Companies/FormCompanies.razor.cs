using ConstructionSiteLibrary.Utility;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Documents;

namespace ConstructionSiteLibrary.Components.Companies;

public partial class FormCompanies
{
    [Parameter]
    public CompanyModel Company { get; set; } = new();

    [Parameter]
    public int CompanyId { get; set; }

    private string title = "Aggiungi azienda/libero professionista";

    /// <summary>
    /// il design degli elementi della form
    /// </summary>
    readonly Variant variant = Variant.Outlined;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if(CompanyId != 0)
        {
            title = "Modifica azienda/libero professionista";
            await Setup(); 
        }
       
    }

    private async Task Setup()
    {
        Company = await CompaniesRepository.GetCompanyById(CompanyId);
    }


    private async Task Save()
    {
        bool response;

         if (Company.Id == 0)
        {
            response = await CompaniesRepository.SaveCompany(Company);
        }
        else
        {
            List<CompanyModel> list = [];
            list.Add(Company);
            response = await CompaniesRepository.UpdateCompanies(list);
        }

        if (response)
        {
            NavigationService.ChangePage(PageRouting.CompaniesPage);
        }
    }



    
}
