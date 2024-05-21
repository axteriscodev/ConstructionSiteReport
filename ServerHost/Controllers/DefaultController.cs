using AXT_WebComunication.WebResponse;
using Microsoft.AspNetCore.Mvc;
using ServerHost.Services;
using System.Diagnostics;
using System.Text.Json;
using TDatabase.Database;
using TDatabase.Utilities;

namespace ServerHost.Controllers
{
    public class DefaultController : Controller
    {

        protected const int ZERO = 0;


        [NonAction]
        protected DbCsclAxteriscoContext GetDbConnection()
        {
            var connectionString = ConfigurationService.GetConnection() ?? "";
            return DatabaseContextFactory.Create(connectionString);
        }

        /// <summary>
        /// Metodo che inizializza un timer per il conto del tempo impiegato ad eseguire la richiesta
        /// del client
        /// </summary>
        /// <returns>il timer</returns>
        [NonAction]
        protected Stopwatch StartTime()
        {
            var stopwatch = Stopwatch.StartNew();
            return stopwatch;
        }

        /// <summary>
        /// Metodo utilizzato per fermare il timer e salvare il tempo trascorso 
        /// nell' item del HttpContext ["OPERATION_TIME"] per scriverlo nei log
        /// </summary>
        /// <param name="stopwatch">il timer</param>
        [NonAction]
        protected void StopTime(Stopwatch stopwatch)
        {
            stopwatch.Stop();
            var time = stopwatch.Elapsed.Milliseconds;
            HttpContext.Items["OPERATION_TIME"] = time;
        }

        /// <summary>
        /// Metodo utilizzato per creare una WebResponse in caso di Eccezione del server per l'invio dell'errore
        /// all'APP e segnarla nei log
        /// </summary>
        /// <param name="e">l'eccezione</param>
        /// <param name="rq">la richiesta inviata dal client</param>
        /// <returns></returns>
        protected AXT_WebResponse ExceptionWebResponse(Exception e, object rq)
        {
            //In caso di exception faccio il log sempre sia della Richiesta sia della Risposta
            ConfigureLog(rq, 3);
            var content = e.InnerException is not null ? e.InnerException.Message : "";
            AXT_WebResponse response = new(StatusResponse.GetStatus(Status.EXCEPTION), content)
            {
                Message = e.Message
            };
            return response;
        }

        /// <summary>
        /// Metodo utilizzato per aggiungere ai log la richiesta del client e la risposta
        /// </summary>
        /// <remarks>
        /// Di default non viene stampato ne la richiesta ne la risposta se non nel caso di risposta con Status.EXCEPTION.
        /// se si imposta il campo logRequest nella chiamata di un metodo si possono modificare le impostazioni di log solo per quel metodo
        /// altrimenti per impostare il tipo di log a livello di progetto si può modificare nell'appsettings.json il campo "DebugMode".
        /// 
        /// valori consentiti per "DebugMode" e logRequest:
        /// 0 => NON viene stampata ne la richiesta ne il content della risposta
        /// 1 => viene stampata la richiesta, NON viene stampato il content della risposta
        /// 2 => NON viene stampata la richiesta, viene stampato il content della risposta
        /// 3 => vengono stampate sia la richiesta che il content della risposta 
        /// 
        /// NOTA: se viene impostato il campo logRequest con un valore != da ZERO in un metodo, questo ha la precedenza 
        /// sulle impostazioni generali di "DebugMode" es:(DebugMode = 3, logReques=2  => viene utilizzato logRequest=2).
        /// Se invece logRequest == ZERO allora viene usato il valore di DebugMode
        /// </remarks>
        /// <param name="rq"></param>
        /// <param name="logRequest"></param>
        protected void ConfigureLog(object rq, int logRequest = ZERO)
        {
            var debugMode = GetDebugSetting();
            //se logRequest != ZERO uso il suo valore, altrimenti debugMode
            HttpContext.Items["LOG_MODE"] = logRequest != ZERO ? logRequest : debugMode;
            HttpContext.Items["CLIENT_REQUEST"] = JsonSerializer.Serialize(rq);
        }

        private static int GetDebugSetting()
        {
            var debugMode = ConfigurationService.GetConfigurationInt("DebugMode");
            return debugMode;
        }
    }
}
