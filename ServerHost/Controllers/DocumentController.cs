using AXT_WebComunication.WebResponse;
using Microsoft.AspNetCore.Mvc;
using ServerHost.Services;
using Shared.Documents;
using TDatabase.Queries;
using Shared.ApiRouting;

namespace ServerHost.Controllers;

[ApiController]
public class DocumentController : DefaultController
{
    [LogAction]
    [Route(ApiRouting.DocumentsList)]
    [HttpPost()]
    public AXT_WebResponse DocumentsList([FromBody]int idDocument)
    {
        var response = new AXT_WebResponse();
            var stopwatch = StartTime();
            ConfigureLog("", 0);

            try
            {
                var db = GetDbConnection();
                var list = DocumentDbHelper.Select(db, idDocument);
                response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), list);

            }
            catch (Exception ex)
            {
                response = ExceptionWebResponse(ex, "");
            }
            StopTime(stopwatch);
            return response;
    }

    [LogAction]
    [Route(ApiRouting.SiteDocumentsList)]
    [HttpPost()]
    public AXT_WebResponse SiteDocumentsList([FromBody] int idSite)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var list = DocumentDbHelper.SelectFromSite(db, idSite);
            response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), list);

        }
        catch (Exception ex)
        {
            response = ExceptionWebResponse(ex, "");
        }
        StopTime(stopwatch);
        return response;
    }

    [LogAction]
    [Route(ApiRouting.SaveDocument)]
    [HttpPost]
    public async Task<AXT_WebResponse> SaveDocument(DocumentModel newDocument)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var q = await DocumentDbHelper.Insert(db, newDocument);
            response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), q);

        }
        catch (Exception ex)
        {
            response = ExceptionWebResponse(ex, "");
        }
        StopTime(stopwatch);
        return response;
    }

    [LogAction]
    [Route(ApiRouting.UpdateDocument)]
    [HttpPost]
    public async Task<AXT_WebResponse> UpdateDocument(List<DocumentModel> documents)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var q = await DocumentDbHelper.Update(db, documents);
            response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), q);

        }
        catch (Exception ex)
        {
            response = ExceptionWebResponse(ex, "");
        }
        StopTime(stopwatch);
        return response;
    }

    [LogAction]
    [Route(ApiRouting.HideDocuments)]
    [HttpPost]
    public async Task<AXT_WebResponse> HideDocuments(List<DocumentModel> documents)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var q = await DocumentDbHelper.Hide(db, documents);
            response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), q);

        }
        catch (Exception ex)
        {
            response = ExceptionWebResponse(ex, "");
        }
        StopTime(stopwatch);
        return response;
    }

}
