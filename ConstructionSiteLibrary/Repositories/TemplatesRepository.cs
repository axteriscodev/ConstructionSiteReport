using System.Linq.Expressions;
using System.Text.Json;
using ConstructionSiteLibrary.Managers;
using ConstructionSiteLibrary.Model;
using ConstructionSiteLibrary.Services;
using Shared.ApiRouting;
using Shared.Templates;

namespace ConstructionSiteLibrary.Repositories;

public class TemplatesRepository(HttpManager httpManager, IndexedDBService indexedDBService)
{

    #region Campi

    private List<TemplateModel> _templates = [];
    private List<TemplateDescriptionModel> _templateDescriptions = [];
    private bool online = true;
    private const int TUTTI = 0;
    private readonly HttpManager _httpManager = httpManager;
    private readonly IndexedDBService _indexedDBService = indexedDBService;

    #endregion

    #region Metodi per la compilazione dei template (SI OFFLINE)

    public async Task<List<TemplateModel>> GetTemplates()
    {
        try
        {
            if (!online)
            {
                await CheckIfOnline();
            }

            if (online)
            {
                var response = await _httpManager.SendHttpRequest(ApiRouting.TemplatesList, TUTTI);
                if (response.Code.Equals("0"))
                {
                    _templates = JsonSerializer.Deserialize<List<TemplateModel>>(response.Content.ToString() ?? "") ?? [];
                    _ = await _indexedDBService.Insert(IndexedDBTables.templates, _templates.Cast<object>().ToArray());
                }
                else if (response.Code.Equals("Ex8995BA25"))
                {
                    online = false;
                }

                if (!online)
                {
                    var content = await _indexedDBService.ReadObjectStore(IndexedDBTables.templates);
                    _templates = content is not null ? JsonSerializer.Deserialize<List<TemplateModel>>(content) ?? [] : [];
                }
            }
        }
        catch (Exception) { }
        return _templates;
    }

     public async Task<TemplateModel> GetTemplateById(int idTemplate = TUTTI)
    {
        var template = new TemplateModel();
        try
        {
            if(!online)
            {
                await CheckIfOnline();
            }
            if(online)
            {
                var response = await _httpManager.SendHttpRequest(ApiRouting.TemplatesList, idTemplate);
                if(response.Code.Equals("0"))
                {
                    var templates = JsonSerializer.Deserialize<List<TemplateModel>>(response.Content.ToString() ?? "") ?? [];
                    template = templates.FirstOrDefault() ?? new();
                }
                else if (response.Code.Equals("Ex8995BA25"))
                {
                    online = false;
                }
            }
            if(!online)
            {
                var content = await _indexedDBService.Read(IndexedDBTables.templates, idTemplate);
                template = content is not null ? JsonSerializer.Deserialize<TemplateModel>(content) ?? new() : new();
            }
        }
        catch (Exception) { }

        return template;
        
    }
    #endregion

    #region Metodi per la creazione e cancellazione (NO OFFLINE)

    public async Task<bool> SaveTemplate(TemplateModel template)
    {
        var result = false;
        try
        {
            var response = await _httpManager.SendHttpRequest(ApiRouting.SaveTemplate, template);
            if(response.Code.Equals("0"))
            {
                _templates.Clear();
                result = true;
            }
        }
        catch (Exception) { }

        return result;
    }

    public async Task<bool> HideTemplate(List<TemplateModel> templates)
    {
        var result = false;
        try
        {
            var response = await _httpManager.SendHttpRequest(ApiRouting.HideTemplates, templates);
            if(response.Code.Equals("0"))
            {
                _templates.Clear();
                result = true;
            }
        }
        catch (Exception) { }

        return result;
    }

    #endregion

    #region Template description 

    public async Task<List<TemplateDescriptionModel>> GetTemplatesDescriptions()
    {
        try
        {
            if (!online)
            {
                await CheckIfOnline();
            }

            if (online)
            {
                var response = await _httpManager.SendHttpRequest(ApiRouting.TemplatesDescriptionsList, TUTTI); //TODO
                if (response.Code.Equals("0"))
                {
                    _templateDescriptions = JsonSerializer.Deserialize<List<TemplateDescriptionModel>>(response.Content.ToString() ?? "") ?? [];
                    _ = await _indexedDBService.Insert(IndexedDBTables.templateDescriptions, _templateDescriptions.Cast<object>().ToArray());
                }
                else if (response.Code.Equals("Ex8995BA25"))
                {
                    online = false;
                }

                if (!online)
                {
                    var content = await _indexedDBService.ReadObjectStore(IndexedDBTables.templateDescriptions);
                    _templates = content is not null ? JsonSerializer.Deserialize<List<TemplateModel>>(content) ?? [] : [];
                }
            }
        }
        catch (Exception) { }
        return _templateDescriptions;
    }

    public async Task<bool> SaveTemplateDescription(TemplateDescriptionModel desc)
    {
        var result = false;
        try
        {
            var response = await _httpManager.SendHttpRequest(ApiRouting.SaveTemplatesDescriptions, desc);
            if (response.Code.Equals("0"))
            {
                result = true;
            }
        }
        catch (Exception) { }

        return result;
    }

    #endregion 

    #region Metodi per effettuare la sincronizzazione con il server

    public async Task<bool> DownloadTemplates()
    {
        var result = false;
        if (online)
        {
            try
            {
                var response = await _httpManager.SendHttpRequest(ApiRouting.TemplatesList, TUTTI);
                if (response.Code.Equals("0"))
                {
                    _templates = JsonSerializer.Deserialize<List<TemplateModel>>(response.Content.ToString() ?? "") ?? [];
                    var count = await _indexedDBService.Insert(IndexedDBTables.templates, _templates.Cast<object>().ToArray());
                    result = count == _templates.Count;
                }
            }
            catch (Exception) { }
        }
        return result;
    }

   

    public async Task<bool> UploadTemplates()
    {
        var result = true;
        if (online)
        {
            try
            {
                var content = await _indexedDBService.SelectByIndex(IndexedDBTables.templates, "offlineChange", 1);
                var modifiedTemplates = content is not null ? JsonSerializer.Deserialize<List<TemplateModel>>(content) ?? [] : [];
                if (modifiedTemplates.Count > 0)
                {
                    //TODO i template possono solo essere creati, la save prende in ingresso un valore alla volta
                    //dobbiamo decidere come muoverci
                    var response = await _httpManager.SendHttpRequest(ApiRouting.SaveTemplate, modifiedTemplates);
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
            await UploadTemplates();
        }
    }

    private void SetOfflineChange(List<TemplateModel> list)
    {
        foreach (var doc in list)
        {
            //doc.ChangedOffline = 1;
        }
    }

    #endregion

}
