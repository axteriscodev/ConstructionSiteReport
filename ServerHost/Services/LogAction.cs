using AXT_WebComunication.WebResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text.Json;

namespace ServerHost.Services
{

    /// <summary>
    /// Classe che estende da ActionFilterAttribute, che crea un attributo da aggiungere alle Action dei controller
    /// per la creazione dei log.
    /// La classe va l'override del metodo OnResultExecuted che viene eseguito all'invio della risposta che cattura i 
    /// dati del context e crea un task asincrono per la creazione del log (in questo modo la risposta non viene rallentata
    /// dalla scrittua dei log)
    /// </summary>
    public class LogAction :ActionFilterAttribute
    {
        #region Override di OnResultExecuted

        /// <summary>
        /// Metodo chiamato dopo che è stato eseguito il risultato della richiesta HTTP
        /// richiama il metodo del logger indicato con un Task.
        /// </summary>
        /// <param name="context">contesto http</param>
        override public void OnResultExecuted(ResultExecutedContext context)
        {
            try
            {
                var httpStatus = context.HttpContext.Response.StatusCode;
                var actionDescriptor = context.ActionDescriptor.DisplayName;
                string rqController = actionDescriptor is not null ? GetControllerNameAndAction(actionDescriptor) : "";
                var rqMethod = context.HttpContext.Request.Method;
                var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
                string ip = remoteIp is not null ? remoteIp.ToString() : "";

                // se la chiamata HTTP è andata a buon fine (STATUS 200)
                if (httpStatus == (int)HttpStatusCode.OK)
                {
                    int time = (int)(context.HttpContext.Items["OPERATION_TIME"] ?? 0);
                    string request = (string)(context.HttpContext.Items["CLIENT_REQUEST"] ?? "");
                    int logMode = (int)(context.HttpContext.Items["LOG_MODE"] ?? 0);
                    var response = (context.Result as ObjectResult)?.Value as AXT_WebResponse ?? new();
                    Task.Run(() => Logger.WriteAPILog(logMode, request, response, ip, rqMethod, rqController, time));
                }
                // chiamata HTTP STATUS != 200
                else
                {
                    var b = context.HttpContext.Request;
                    var result = context.Result as BadRequestObjectResult;
                    var value = result?.Value as ValidationProblemDetails;
                    var title = JsonSerializer.Serialize(value?.Title);
                    var errors = JsonSerializer.Serialize(value?.Errors);
                    var temp = JsonSerializer.Serialize(result?.Value);

                    Task.Run(() => Logger.WriteHTTPLogError(ip, rqMethod, rqController, httpStatus, title, errors));
                }

            }
            catch (Exception) { }

            base.OnResultExecuted(context);
        }

        #endregion

        #region Metodi Privati

        /// <summary>
        /// Metodo che prende dall'action descriptor controller a azione chiamate dal client
        /// </summary>
        /// <param name="actionDescriptor">action description della chiamata http</param>
        /// <returns>stringa contenente il controller e l'azione chiamata</returns>
        private static string GetControllerNameAndAction(string actionDescriptor)
        {
            string result = "";
            try
            {
                string[] temp = actionDescriptor.Split(" ");
                string[] temp2 = temp[0].Split(".");
                result = temp2[^2] + "." + temp2[^1];
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        #endregion
    }
}
