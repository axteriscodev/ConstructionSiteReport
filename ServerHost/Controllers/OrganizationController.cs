using AXT_WebComunication.WebResponse;
using Microsoft.AspNetCore.Mvc;
using ServerHost.Services;
using Shared.ApiRouting;
using Shared.Organizations;
using TDatabase.Queries;

namespace ServerHost.Controllers;

[ApiController]
public class OrganizationController : DefaultController
{
    [LogAction]
    [Route(ApiRouting.Organization)]
    [HttpPost]
    public AXT_WebResponse Organization(OrganizationModel organization)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var org = OrganizationDbHelper.Select(db, organization.Id);
            response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), org);
        }
        catch (Exception ex)
        {
            response = ExceptionWebResponse(ex, "");
        }
        StopTime(stopwatch);
        return response;
    }

    [LogAction]
    [Route(ApiRouting.UpdateOrganization)]
    [HttpPost]
    public async Task<AXT_WebResponse> UpdateOrganization(OrganizationModel organization)
    {
        var response = new AXT_WebResponse();
        var stopwatch = StartTime();
        ConfigureLog("", 0);

        try
        {
            var db = GetDbConnection();
            var list = await OrganizationDbHelper.Update(db, organization);
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
