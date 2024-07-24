using System.Text.Json;
using ConstructionSiteLibrary.Managers;
using Shared.ApiRouting;
using Shared.Organizations;

namespace ConstructionSiteLibrary.Repositories;

public class RolesRepository(HttpManager httpManager)
{
    List<RoleModel> Roles = [];

    private HttpManager _httpManager = httpManager;

    public async Task<List<RoleModel>> GetRoles()
    {
        if(Roles.Count == 0)
        {
           var response = await _httpManager.SendHttpRequest(ApiRouting.RolesList, "");
            if (response.Code.Equals("0"))
            {
                Roles = JsonSerializer.Deserialize<List<RoleModel>>(response.Content.ToString() ?? "") ?? [];
            } 
        }

        return Roles;
    }

    public async Task<bool> SaveRole(RoleModel role)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.SaveRole, role);

        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Roles.Clear();
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateRoles(List<RoleModel> roles)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.UpdateRoles, roles);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Roles.Clear();
            return true;
        }

        return false;
    }

    
    public async Task<bool> HideRoles(List<RoleModel> roles)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.HideRoles, roles);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Roles.Clear();
            return true;
        }

        return false;
    }
}
