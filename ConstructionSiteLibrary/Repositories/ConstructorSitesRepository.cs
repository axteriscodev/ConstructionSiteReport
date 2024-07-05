using System.Diagnostics;
using System.Text.Json;
using ConstructionSiteLibrary.Managers;
using Shared.ApiRouting;
using Shared.Documents;

namespace ConstructionSiteLibrary.Repositories;

public class ConstructorSitesRepository(HttpManager httpManager)
{
    List<ConstructorSiteModel> ConstructorSites = [];

    private HttpManager _httpManager = httpManager;

    public async Task<List<ConstructorSiteModel>> GetConstructorSites()
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.ConstructorSitesList, "");
        if (response.Code.Equals("0"))
        {
            ConstructorSites = JsonSerializer.Deserialize<List<ConstructorSiteModel>>(response.Content.ToString() ?? "") ?? [];
        }

        return ConstructorSites;
    }

    public async Task<ConstructorSiteModel> GetConstructorSiteInfo(int idConstructorSite)
    {
        ConstructorSiteModel site = new();
        var response = await _httpManager.SendHttpRequest(ApiRouting.ConstructorSiteInfo, idConstructorSite);
        if (response.Code.Equals("0"))
        {
            var list = JsonSerializer.Deserialize<List<ConstructorSiteModel>>(response.Content.ToString() ?? "") ?? [];
            site = list.FirstOrDefault() ?? new();
        }

        return site;
    }

    public async Task<bool> SaveContructorSite(ConstructorSiteModel constructorSite)
    {
         var response = await _httpManager.SendHttpRequest(ApiRouting.SaveConstructorSite, constructorSite);

        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            ConstructorSites.Clear();
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateContructorSites(List<ConstructorSiteModel> constructorSites)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.UpdateConstructorSites, constructorSites);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            ConstructorSites.Clear();
            return true;
        }

        return false;
    }

    public async Task<bool> HideConstructorSites(List<ConstructorSiteModel> constructorSites)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.HideConstructorSites, constructorSites);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            ConstructorSites.Clear();
            return true;
        }

        return false;
    }
}
