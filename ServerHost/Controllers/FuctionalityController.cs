using AXT_WebComunication.WebResponse;
using Microsoft.AspNetCore.Mvc;
using ServerHost.Services;
using System.Collections.Generic;
using TDatabase.Queries;

namespace ServerHost.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class FuctionalityController : Controller
    {
        [LogAction]
        [HttpPost]
        public AXT_WebResponse Check()
        {
            var response = new AXT_WebResponse();
            response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), "OK");
            return response;
        }
    }
}
