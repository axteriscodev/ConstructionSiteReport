using System.Text.Json;
using ClientWebApp.Managers;
using Shared;

namespace ClientWebApp.Repositories;

public class DocumentsRepository(HttpManager httpManager)
{
    List<DocumentModel> Documents = [];

    private HttpManager _httpManager = httpManager;

    public async Task<List<DocumentModel>> GetDocuments()
    {
        if (Documents.Count == 0)
        {
            var response = await _httpManager.SendHttpRequest("Document/DocumentsList", "");
            if (response.Code.Equals("0"))
            {
                Documents = JsonSerializer.Deserialize<List<DocumentModel>>(response.Content.ToString() ?? "") ?? [];
            }

        }

        return Documents;
    }

    public async Task<bool> SaveDocument(DocumentModel document)
    {
        var response = await _httpManager.SendHttpRequest("Document/SaveDocument", document);

        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Documents.Clear();
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateDocuments(List<DocumentModel> documents)
    {
        var response = await _httpManager.SendHttpRequest("Document/UpdateDocument", documents);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Documents.Clear();
            return true;
        }

        return false;
    }

    public async Task<bool> HideDocuments(List<DocumentModel> documents)
    {
        var response = await _httpManager.SendHttpRequest("Document/HideQuestion", documents);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Documents.Clear();
            return true;
        }

        return false;
    }
}
