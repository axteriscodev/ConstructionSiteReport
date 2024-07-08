using System.Text.Json;
using ConstructionSiteLibrary.Managers;
using Shared.ApiRouting;
using Shared.Documents;

namespace ConstructionSiteLibrary.Repositories;

public class CompaniesRepository(HttpManager httpManager)
{
    List<CompanyModel> Companies = [];

    private HttpManager _httpManager = httpManager;

    public async Task<List<CompanyModel>> GetCompanies()
    {
        if (Companies.Count == 0)
        {
            var response = await _httpManager.SendHttpRequest(ApiRouting.CompaniesList, 0);
            if (response.Code.Equals("0"))
            {
                Companies = JsonSerializer.Deserialize<List<CompanyModel>>(response.Content.ToString() ?? "") ?? [];
            }
        }

        return Companies;
    }

    public async Task<CompanyModel> GetCompanyById(int companyId)
    {
        CompanyModel selectedCompany = new();
       var  response = await _httpManager.SendHttpRequest(ApiRouting.CompaniesList, companyId);
       if (response.Code.Equals("0"))
       {
            var companies = JsonSerializer.Deserialize<List<CompanyModel>>(response.Content.ToString() ?? "") ?? []; 

            selectedCompany = companies.FirstOrDefault() ?? new();
       }

       return selectedCompany;
    }

    public async Task<bool> SaveCompany(CompanyModel company)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.SaveCompany, company);

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
        var response = await _httpManager.SendHttpRequest(ApiRouting.UpdateCompanies, companies);
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
        var response = await _httpManager.SendHttpRequest(ApiRouting.HideCompanies, companies);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Companies.Clear();
            return true;
        }

        return false;
    }
}

