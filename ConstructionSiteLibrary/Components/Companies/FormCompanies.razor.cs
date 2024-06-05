using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Documents;

namespace ConstructionSiteLibrary.Components.Companies;

public partial class FormCompanies
{
    [Parameter]
    public CompanyModel Company { get; set; } = new();

    /// <summary>
    /// il design degli elementi della form
    /// </summary>
    readonly Variant variant = Variant.Outlined;


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
