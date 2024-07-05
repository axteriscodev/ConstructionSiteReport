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

    /// <summary>
    /// il design degli elementi della form
    /// </summary>
    readonly Variant variant = Variant.Outlined;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if(CompanyId != 0)
        {
            Setup(); 
        }
       
    }

    private async Task Setup()
    {
        
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
    }

    
}
