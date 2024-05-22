using System.Text.Json;
using ConstructionSiteLibrary.Managers;
using Shared;

namespace ConstructionSiteLibrary.Repositories;

public class CompaniesRepository(HttpManager httpManager)
{
    List<CompanyModel> Companies = [];

    private HttpManager _httpManager = httpManager;

    public async Task<List<CompanyModel>> GetCompanies()
    {
        if (Companies.Count == 0)
        {
            var response = await _httpManager.SendHttpRequest("Company/CompaniesList", "");
            if (response.Code.Equals("0"))
            {
                Companies = JsonSerializer.Deserialize<List<CompanyModel>>(response.Content.ToString() ?? "") ?? [];
            }
        }

        return Companies;
    }

    public async Task<bool> SaveCompany(CompanyModel company)
    {
        var response = await _httpManager.SendHttpRequest("Company/SaveCompany", company);

        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Companies.Clear();
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateCompanies(List<CompanyModel> companies)
    {
        var response = await _httpManager.SendHttpRequest("Company/UpdateCompanies", companies);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Companies.Clear();
            return true;
        }

        return false;
    }

    
    public async Task<bool> HideCompanies(List<CompanyModel> companies)
    {
        var response = await _httpManager.SendHttpRequest("Company/HideCompanies", companies);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Companies.Clear();
            return true;
        }

        return false;
    }
}

