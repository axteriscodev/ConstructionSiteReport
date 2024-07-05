using System.Text.Json;
using ConstructionSiteLibrary.Managers;
using ConstructionSiteLibrary.Services;
using ConstructionSiteLibrary.Model;
using Shared.Documents;
using Shared.ApiRouting;

namespace ConstructionSiteLibrary.Repositories;

public class DocumentsRepository(HttpManager httpManager, IndexedDBService indexedDBService)
{

    #region Campi

    private List<DocumentModel> _documents = [];
    private List<MeteoConditionModel> _meteoConditions = [];
    private bool online = true;
    private const int TUTTI = 0;
    private readonly HttpManager _httpManager = httpManager;
    private readonly IndexedDBService _indexedDBService = indexedDBService;

    #endregion

    #region Metodi per la compilazione di documenti (SI OFFLINE)

    public async Task<List<DocumentModel>> GetDocuments()
    {
        try
        {
            //se non sono online controllo di essere ritornato online
            if (!online)
            {
                await CheckIfOnline();
            }
            //se sono online
            if (online)
            {
                var response = await _httpManager.SendHttpRequest(ApiRouting.DocumentsList, TUTTI);
                if (response.Code.Equals("0"))
                {

                    _documents = JsonSerializer.Deserialize<List<DocumentModel>>(response.Content.ToString() ?? "") ?? [];
                    _ = await _indexedDBService.Insert(IndexedDBTables.documents, _documents.Cast<object>().ToArray());
                }
                else if (response.Code.Equals("Ex8995BA25"))// problemi di connessione
                {
                    online = false;
                }
            } 
            //altrimenti cerco in locale
            if(!online)
            {
                var content = await _indexedDBService.ReadObjectStore(IndexedDBTables.documents);
                _documents = content is not null ? JsonSerializer.Deserialize<List<DocumentModel>>(content) ?? [] : [];
            }

        }
        catch (Exception) { }
        return _documents;
    }

    public async Task<List<MeteoConditionModel>> GetMeteoConditions()
    {
        try
        {
            //se non sono online controllo di essere ritornato online
            if (!online)
            {
                await CheckIfOnline();
            }
            //se sono online
            if (online)
            {
                var response = await _httpManager.SendHttpRequest(ApiRouting.MeteoConditionsList, TUTTI);
                if (response.Code.Equals("0"))
                {

                    _meteoConditions = JsonSerializer.Deserialize<List<MeteoConditionModel>>(response.Content.ToString() ?? "") ?? [];
                    _ = await _indexedDBService.Insert(IndexedDBTables.meteoConditions, _meteoConditions.Cast<object>().ToArray());
                }
                else if (response.Code.Equals("Ex8995BA25"))// problemi di connessione
                {
                    online = false;
                }
            } 
            //altrimenti cerco in locale
            if(!online)
            {
                var content = await _indexedDBService.ReadObjectStore(IndexedDBTables.meteoConditions);
                _meteoConditions = content is not null ? JsonSerializer.Deserialize<List<MeteoConditionModel>>(content) ?? [] : [];
            }

        }
        catch (Exception) { }
        return _meteoConditions;
    }

    /// <summary>
    /// Metodo per richiedere i documenti di un determinato constructorSite
    /// </summary>
    /// <param name="siteId"></param>
    /// <returns></returns>
    public async Task<List<DocumentModel>> GetSiteDocuments(int siteId)
    {
        List<DocumentModel> docs = [];
        var response = await _httpManager.SendHttpRequest(ApiRouting.SiteDocumentsList, siteId);
        if (response.Code.Equals("0"))
        {
            docs = JsonSerializer.Deserialize<List<DocumentModel>>(response.Content.ToString() ?? "") ?? [];
        }
        return docs;
    }

    public async Task<DocumentModel> GetDocumentById(int idDocument = TUTTI)
    {
        var document = new DocumentModel();
        try
        {
            //se non sono online controllo di essere ritornato online
            if (!online)
            {
                await CheckIfOnline();
            }
            if (online)
            {
                var response = await _httpManager.SendHttpRequest(ApiRouting.DocumentsList, idDocument);
                if (response.Code.Equals("0"))
                {
                    var documents = JsonSerializer.Deserialize<List<DocumentModel>>(response.Content.ToString() ?? "") ?? [];
                    document = documents.FirstOrDefault() ?? new();
                }
                else if (response.Code.Equals("Ex8995BA25"))// problemi di connessione
                {
                    online = false;
                }
            }
            if (!online)
            {
                var content = await _indexedDBService.Read(IndexedDBTables.documents, idDocument);
                document = content is not null ? JsonSerializer.Deserialize<DocumentModel>(content) ?? new() : new();
            }

        }
        catch (Exception) { }

        return document;
    }


    public async Task<bool> UpdateDocuments(List<DocumentModel> documents)
    {
        var result = false;
        try
        {
            //se non sono online controllo di essere ritornato online
            if (!online)
            {
                await CheckIfOnline();
            }
            if (online)
            {
                var response = await _httpManager.SendHttpRequest(ApiRouting.UpdateDocument, documents);
                if (response.Code.Equals("0"))
                {
                    _documents.Clear();
                    result = true;
                }
                else if (response.Code.Equals("Ex8995BA25"))// problemi di connessione
                {
                    online = false;
                }
            }
            if (!online)
            {
                SetOfflineChange(documents);
                var content = await _indexedDBService.Insert(IndexedDBTables.documents, documents.Cast<object>().ToArray());
                _documents.Clear();
                result = true;
            }
        }
        catch (Exception) { }

        return result;
    }

    #endregion

    #region Metodi per la creazione e cancellazione (NO OFFLINE)

    public async Task<bool> SaveDocument(DocumentModel document)
    {
        var result = false;
        try
        {
            var response = await _httpManager.SendHttpRequest(ApiRouting.SaveDocument, document);
            if (response.Code.Equals("0"))
            {
                _documents.Clear();
                result = true;
            }
        }
        catch (Exception) { }

        return result;
    }


    public async Task<bool> HideDocuments(List<DocumentModel> documents)
    {
        var result = false;
        try
        {
            var response = await _httpManager.SendHttpRequest(ApiRouting.HideDocuments, documents);
            if (response.Code.Equals("0"))
            {
                _documents.Clear();
                result = true;
            }
        }
        catch (Exception) { }

        return result;
    }

    #endregion

    #region Metodi per effettuare la sincronizzazione con il server

    public async Task<bool> DownloadDocuments()
    {
        var result = false;
        if (online)
        {
            try
            {
                var response = await _httpManager.SendHttpRequest(ApiRouting.DocumentsList, TUTTI);
                if (response.Code.Equals("0"))
                {
                    _documents = JsonSerializer.Deserialize<List<DocumentModel>>(response.Content.ToString() ?? "") ?? [];
                    //carico i documenti del db locale
                    var count = await _indexedDBService.Insert(IndexedDBTables.documents, _documents.Cast<object>().ToArray());
                    result = count == _documents.Count;
                }
            }
            catch (Exception) { }
        }
        return result;
    }

    public async Task<bool> UploadDocuments()
    {
        var result = true;
        if (online)
        {
            try
            {
                var content = await _indexedDBService.SelectByIndex(IndexedDBTables.documents, "offlineChange", 1);
                var modifiedDocuments = content is not null ? JsonSerializer.Deserialize<List<DocumentModel>>(content) ?? [] : [];
                if (modifiedDocuments.Count > 0)
                {
                    var response = await _httpManager.SendHttpRequest(ApiRouting.UpdateDocument, modifiedDocuments);
                    result = response.Code.Equals("0");
                }
            }
            catch (Exception) { result = false; }
        }
        else
        {
            result = false;
        }
        return result;
    }

    #endregion

    #region Metodi privati

    private async Task CheckIfOnline()
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.CheckOnline, "");
        if (response.Code.Equals("0"))
        {
            online = true;
            //allora aggiorno sul server se ho fatto delle modifiche nel frattempo
            await UploadDocuments();
        }
    }

    private void SetOfflineChange(List<DocumentModel> list)
    {
        foreach (var doc in list)
        {
            doc.ChangedOffline = 1;
        }
    }

    #endregion
}
