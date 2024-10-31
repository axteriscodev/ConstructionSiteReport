using AXT_WebComunication.WebResponse;
using Microsoft.AspNetCore.Mvc;
using ServerHost.Services;
using Shared.ApiRouting;
using TDatabase.Queries;


namespace ServerHost.Controllers
{

    [ApiController]
    public class FuctionalityController : DefaultController
    {
        [LogAction]
        [Route(ApiRouting.CheckOnline)]
        [HttpPost]
        public AXT_WebResponse Check()
        {
            var response = new AXT_WebResponse();
            response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), "OK");
            return response;
        }

        [LogAction]
        [Route(ApiRouting.Test)]
        [HttpPost]
        public async Task<AXT_WebResponse> Test()
        {
            var response = new AXT_WebResponse();
            response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), "OK");
            return response;
        }
    }
}
