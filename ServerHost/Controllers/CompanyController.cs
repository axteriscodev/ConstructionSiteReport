using AXT_WebComunication.WebResponse;
using Microsoft.AspNetCore.Mvc;
using ServerHost.Services;
using Shared.ApiRouting;
using Shared.Documents;
using TDatabase.Queries;

namespace ServerHost.Controllers;

[ApiController]
public class CompanyController : DefaultController
{
    [LogAction]
    [Route(ApiRouting.CompaniesList)]
    [HttpPost]
    public AXT_WebResponse CompaniesList([FromBody]int idCompany)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var list = CompanyDbHelper.Select(db, idCompany);
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
    [Route(ApiRouting.SaveCompany)]
    [HttpPost]
    public async Task<AXT_WebResponse> SaveCompany(CompanyModel newCompany)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var list = await CompanyDbHelper.Insert(db, newCompany);
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
    [Route(ApiRouting.UpdateCompanies)]
    [HttpPost]
    public async Task<AXT_WebResponse> UpdateCompanies(List<CompanyModel> companies)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var list = await CompanyDbHelper.Update(db, companies);
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
    [Route(ApiRouting.HideCompanies)]
    [HttpPost]
    public async Task<AXT_WebResponse> HideCompanies(List<CompanyModel> companies)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var list = await CompanyDbHelper.Hide(db, companies);
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
