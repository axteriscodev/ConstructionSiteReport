using System.Text.Json;
using ConstructionSiteLibrary.Managers;
using Shared;

namespace ConstructionSiteLibrary.Repositories;

public class ClientsRepository(HttpManager httpManager)
{
    List<ClientModel> Clients = [];

    private HttpManager _httpManager = httpManager;

    public async Task<List<ClientModel>> GetClients()
    {
        if (Clients.Count == 0)
        {
            var response = await _httpManager.SendHttpRequest("Client/ClientsList", "");
            if (response.Code.Equals("0"))
            {
                Clients = JsonSerializer.Deserialize<List<ClientModel>>(response.Content.ToString() ?? "") ?? [];
            }
        }

        return Clients;
    }

    public async Task<bool> SaveClient(ClientModel client)
    {
        var response = await _httpManager.SendHttpRequest("Client/SaveClient", client);

        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Clients.Clear();
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateClients(List<ClientModel> clients)
    {
        var response = await _httpManager.SendHttpRequest("Client/UpdateClients", clients);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Clients.Clear();
            return true;
        }

        return false;
    }

    
    public async Task<bool> HideClients(List<ClientModel> clients)
    {
        var response = await _httpManager.SendHttpRequest("Client/HideClients", clients);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Clients.Clear();
            return true;
        }

        return false;
    }
}
