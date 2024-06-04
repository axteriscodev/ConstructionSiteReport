using AXT_WebComunication.WebResponse;
using Microsoft.AspNetCore.Mvc;
using ServerHost.Services;
using Shared.ApiRouting;


namespace ServerHost.Controllers
{

    [ApiController]
    public class FuctionalityController : Controller
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
    }
}
