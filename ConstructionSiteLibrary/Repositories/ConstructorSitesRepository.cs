using System.Diagnostics;
using System.Text.Json;
using ConstructionSiteLibrary.Managers;
using Shared.Documents;

namespace ConstructionSiteLibrary.Repositories;

public class ConstructorSitesRepository(HttpManager httpManager)
{
    List<ConstructorSiteModel> ConstructorSites = [];

    private HttpManager _httpManager = httpManager;

    public async Task<List<ConstructorSiteModel>> GetConstructorSites()
    {
        if(true)
        {
            var response = await _httpManager.SendHttpRequest("ConstructorSite/ConstructorSitesList" , "");
            if (response.Code.Equals("0"))
            {
                ConstructorSites = JsonSerializer.Deserialize<List<ConstructorSiteModel>>(response.Content.ToString() ?? "") ?? [];
            }
        }

        return ConstructorSites;
    }

    public async Task<bool> SaveContructorSite(ConstructorSiteModel constructorSite)
    {
         var response = await _httpManager.SendHttpRequest("ConstructorSite/SaveConstructorSite", constructorSite);

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
        var response = await _httpManager.SendHttpRequest("ConstructorSite/UpdateConstructorSites", constructorSites);
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
        var response = await _httpManager.SendHttpRequest("ConstructorSite/HideConstructorSites", constructorSites);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            ConstructorSites.Clear();
            return true;
        }

        return false;
    }
}
