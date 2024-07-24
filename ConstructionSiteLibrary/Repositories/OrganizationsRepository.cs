using System.Text.Json;
using ConstructionSiteLibrary.Managers;
using Shared.ApiRouting;
using Shared.Organizations;

namespace ConstructionSiteLibrary.Repositories;

public class OrganizationsRepository(HttpManager httpManager)
{
    OrganizationModel Organization = new();

    private HttpManager _httpManager = httpManager;

    public async Task<OrganizationModel> GetOrganization(OrganizationModel organization)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.Organization, organization);
        if (response.Code.Equals("0"))
        {
            Organization = JsonSerializer.Deserialize<OrganizationModel>(response.Content.ToString() ?? "") ?? new();
        }

        return Organization;
    }

    public async Task<bool> UpdateOrganization(OrganizationModel organization)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.UpdateOrganization, organization);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Organization = JsonSerializer.Deserialize<OrganizationModel>(response.Content.ToString() ?? "") ?? new();
            return true;
        }

        return false;
    }
}
