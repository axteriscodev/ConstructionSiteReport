using AXT_WebComunication.WebResponse;
using Microsoft.AspNetCore.Mvc;
using ServerHost.Services;
using TDatabase.Queries;
using TDatabase.Utilities;

namespace ServerHost.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class QuestionController : DefaultController
    {


        [LogAction]
        [HttpPost]
        public AXT_WebResponse ChoicesList() 
        {
            var response = new AXT_WebResponse();
            var stopwatch = StartTime();
            ConfigureLog("", 0);

            try
            {
                var connectionString = ConfigurationService.GetConnection() ?? "";
                var db = DatabaseContextFactory.Create(connectionString);
                var list = ChoiceDbHelper.Select(db);
                response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), list);

            }catch (Exception ex)
            {
                response = ExceptionWebResponse(ex,"");
            }
            StopTime(stopwatch);
            return response;
        }

    }
}
