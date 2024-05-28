using AXT_WebComunication.WebResponse;
using Microsoft.AspNetCore.Mvc;
using ServerHost.Services;
using Shared;
using TDatabase.Queries;

namespace ServerHost.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TemplateController : DefaultController
{
    [LogAction]
    [HttpPost()]
    public AXT_WebResponse TemplatesList([FromBody]int idTemplate)
    {
        var response = new AXT_WebResponse();
            var stopwatch = StartTime();
            ConfigureLog("", 0);

            try
            {
                var db = GetDbConnection();
                var list = TemplateDbHelper.Select(db, idTemplate);
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
    [HttpPost]
    public async Task<AXT_WebResponse> SaveTemplate(TemplateModel newTemplate)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var q = await TemplateDbHelper.Insert(db, newTemplate);
            response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), q);

        }
        catch (Exception ex)
        {
            response = ExceptionWebResponse(ex, "");
        }
        StopTime(stopwatch);
        return response;
    }

    // [LogAction]
    // [HttpPost]
    // public async Task<AXT_WebResponse> UpdateDocument(List<DocumentModel> documents)
    // {
    //     var response = new AXT_WebResponse();
    //     var stopwatch = StartTime();
    //     ConfigureLog("", 0);

    //     try
    //     {
    //         var db = GetDbConnection();
    //         var q = await DocumentDbHelper.Update(db, documents);
    //         response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), q);

    //     }
    //     catch (Exception ex)
    //     {
    //         response = ExceptionWebResponse(ex, "");
    //     }
    //     StopTime(stopwatch);
    //     return response;
    // }

    [LogAction]
    [HttpPost]
    public async Task<AXT_WebResponse> HideTemplates(List<TemplateModel> templates)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var q = await TemplateDbHelper.Hide(db, templates);
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
