using AXT_WebComunication.WebResponse;
using Microsoft.AspNetCore.Mvc;
using ServerHost.Services;
using Shared.ApiRouting;
using Shared.Documents;
using TDatabase.Queries;

namespace ServerHost.Controllers;

[ApiController]
public class ConstructorSiteController : DefaultController
{
    [LogAction]
    [Route(ApiRouting.ConstructorSitesList)]
    [HttpPost]
    public AXT_WebResponse ConstructorSitesList()
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var list = ConstructorSiteDbHelper.Select(db);
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
    [Route(ApiRouting.ConstructorSiteInfo)]
    [HttpPost]
    public AXT_WebResponse ConstructorSiteInfo([FromBody] int idConstructorSite)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var list = ConstructorSiteDbHelper.Select(db, idConstructorSite);
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
    [Route(ApiRouting.SaveConstructorSite)]
    [HttpPost]
    public async Task<AXT_WebResponse> SaveConstructorSite(ConstructorSiteModel newContructorSite)
    {
         var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var list = await ConstructorSiteDbHelper.Insert(db, newContructorSite);
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
    [Route(ApiRouting.UpdateConstructorSites)]
    [HttpPost]
    public async Task<AXT_WebResponse> UpdateConstructorSites(List<ConstructorSiteModel> constructorSites)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var list = await ConstructorSiteDbHelper.Update(db, constructorSites);
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
    [Route(ApiRouting.HideConstructorSites)]
    [HttpPost]
    public async Task<AXT_WebResponse> HideConstructorSites(List<ConstructorSiteModel> constructorSites)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var list = await ConstructorSiteDbHelper.Hide(db, constructorSites);
            response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), list);
        }
        catch (Exception ex)
        {
            response = ExceptionWebResponse(ex, "");
        }
        StopTime(stopwatch);
        return response;
    }
}
