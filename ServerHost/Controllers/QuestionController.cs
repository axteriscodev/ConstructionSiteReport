using AXT_WebComunication.WebResponse;
using Microsoft.AspNetCore.Mvc;
using ServerHost.Services;
using Shared.ApiRouting;
using Shared.Templates;
using TDatabase.Queries;

namespace ServerHost.Controllers
{

    [ApiController]
    public class QuestionController : DefaultController
    {

        #region Questions

        [LogAction]
        [Route(ApiRouting.QuestionsList)]
        [HttpPost]
        public AXT_WebResponse QuestionsList()
        {
            var response = new AXT_WebResponse();
            var stopwatch = StartTime();
            ConfigureLog("", 0);

            try
            {
                var db = GetDbConnection();
                var list = QuestionDbHelper.Select(db);
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
        [Route(ApiRouting.SaveQuestion)]
        [HttpPost]
        public async Task<AXT_WebResponse> SaveQuestion(TemplateQuestionModel newQuestion)
        {
            var response = new AXT_WebResponse();
            var stopwatch = StartTime();
            ConfigureLog("", 0);

            try
            {
                var db = GetDbConnection();
                var q = await QuestionDbHelper.Insert(db, newQuestion);
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
        [Route(ApiRouting.UpdateQuestions)]
        [HttpPost]
        public async Task<AXT_WebResponse> UpdateQuestions(List<TemplateQuestionModel> questions)
        {
            var response = new AXT_WebResponse();
            var stopwatch = StartTime();
            ConfigureLog("", 0);

            try
            {
                var db = GetDbConnection();
                var q = await QuestionDbHelper.Update(db, questions);
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
        [Route(ApiRouting.HideQuestions)]
        [HttpPost]
        public async Task<AXT_WebResponse> HideQuestions(List<TemplateQuestionModel> questions)
        {
            var response = new AXT_WebResponse();
            var stopwatch = StartTime();
            ConfigureLog("", 0);

            try
            {
                var db = GetDbConnection();
                var q = await QuestionDbHelper.Hide(db, questions);
                response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), q);

            }
            catch (Exception ex)
            {
                response = ExceptionWebResponse(ex, "");
            }
            StopTime(stopwatch);
            return response;
        }

        #endregion


        #region Choice

        [LogAction]
        [Route(ApiRouting.ChoicesList)]
        [HttpPost]
        public AXT_WebResponse ChoicesList()
        {
            var response = new AXT_WebResponse();
            var stopwatch = StartTime();
            ConfigureLog("", 0);

            try
            {
                var db = GetDbConnection();
                var list = ChoiceDbHelper.Select(db);
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
        [Route(ApiRouting.SaveChoice)]
        [HttpPost]
        public async Task<AXT_WebResponse> SaveChoice(TemplateChoiceModel newChoice)
        {
            var response = new AXT_WebResponse();
            var stopwatch = StartTime();
            ConfigureLog("", 0);

            try
            {
                var db = GetDbConnection();
                var c = await ChoiceDbHelper.Insert(db, newChoice);
                response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), c);

            }
            catch (Exception ex)
            {
                response = ExceptionWebResponse(ex, "");
            }
            StopTime(stopwatch);
            return response;
        }

        [LogAction]
        [Route(ApiRouting.UpdateChoices)]
        [HttpPost]
        public async Task<AXT_WebResponse> UpdateChoices(List<TemplateChoiceModel> choices)
        {
            var response = new AXT_WebResponse();
            var stopwatch = StartTime();
            ConfigureLog("", 0);

            try
            {
                var db = GetDbConnection();
                var c = await ChoiceDbHelper.Update(db, choices);
                response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), c);

            }
            catch (Exception ex)
            {
                response = ExceptionWebResponse(ex, "");
            }
            StopTime(stopwatch);
            return response;
        }

        [LogAction]
        [Route(ApiRouting.HideChoices)]
        [HttpPost]
        public async Task<AXT_WebResponse> HideChoices(List<TemplateChoiceModel> choices)
        {
            var response = new AXT_WebResponse();
            var stopwatch = StartTime();
            ConfigureLog("", 0);

            try
            {
                var db = GetDbConnection();
                var c = await ChoiceDbHelper.Hide(db, choices);
                response.AddResponse(StatusResponse.GetStatus(Status.SUCCESS), c);

            }
            catch (Exception ex)
            {
                response = ExceptionWebResponse(ex, "");
            }
            StopTime(stopwatch);
            return response;
        }

        #endregion
    }
}
