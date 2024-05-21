using AXT_WebComunication.WebResponse;
using System.Text.Json;
using System.Text;

namespace ServerHost.Services
{

    /// <summary>
    /// Classe utilizzata per la creazione dei log
    /// </summary>
    public class Logger
    {
        #region Campi

        // private static readonly string ZERO = "0";
        private static readonly string Ex89644BD0 = "Ex89644BD0";
        private static readonly string API_LOGS = "ApiLogs.txt";
        private static readonly string HTTP_ERROR_LOGS = "HttpErrorLogs.txt";
        private static readonly string API_EXCEPTION_LOGS = "ApiExceptionLogs.txt";
        private static readonly string NOTIFICATION_LOGS = "NotificationLogs.txt";

        #endregion

        #region Scrittura LOG

        /// <summary>
        /// Metodo che scrive il log delle richieste API che sono terminate con codice della WebResponse diversa da 0
        /// </summary>
        /// <param name="context">contesto della richiesta http</param>
        /// <param name="ip">ip di chi ha effettuato la richiesta</param>
        public static void WriteAPILog(int logMode, string request, AXT_WebResponse response, string ip, string rqMethod, string rqController, int milliseconds)
        {
            try
            {
                var sb = new StringBuilder();
                string log = "";
                string logPath = "";
                string content;

                switch (logMode)
                {
                    //non stampo ne la request ne il content della response
                    case 0:
                        request = "";
                        content = "";
                        break;
                    //stampo la request ma non il content della response
                    case 1:
                        content = "";
                        break;
                    //stampo il content della response ma non la request
                    case 2:
                        request = "";
                        content = response.Content is not null ? JsonSerializer.Serialize(response.Content) : "";
                        break;
                    //stampo sia la request che il content della response
                    case 3:
                        content = response.Content is not null ? JsonSerializer.Serialize(response.Content) : "";
                        break;
                    default:
                        request = "";
                        content = "";
                        break;
                }

                // creo la stringa da aggiungere al file di log
                log = sb.AppendFormat("[{0}] | OperationTime: {7} ms | Controller: {1} - Method: {2} | ClientIP: {3} | Response: {4} - {5} - {6} | Request: {8}",
                                   DateTime.Now, rqController, rqMethod, ip, response.Code, response.Message, content, milliseconds, request).ToString();

                //seleziono il file da scrivere in base a se è una eccezione o meno
                logPath = response.Code.Equals(Ex89644BD0) ? GetFileLogPath(API_EXCEPTION_LOGS) : GetFileLogPath(API_LOGS);

                using StreamWriter stream = new(logPath, true);
                stream.WriteLine(log);
            }
            catch (Exception) { }
        }


        /// <summary>
        /// Metodo che scrive il log delle richieste API che sono terminate con codice della WebResponse diversa da 0
        /// </summary>
        /// <param name="context">contesto della richiesta http</param>
        /// <param name="ip">ip di chi ha effettuato la richiesta</param>
        public static void WriteHTTPLogError(string ip, string rqMethod, string rqController, int httpStatus, string title, string errors)
        {
            try
            {
                var sb = new StringBuilder();

                // creo la stringa da aggiungere al file di log
                var log = sb.AppendFormat("[{0}] | HTTP STATUS: {1} | Controller: {2} - Method: {3} | ClientIP: {4} | {5} - ERRORS: {6}",
                                   DateTime.Now, httpStatus, rqController, rqMethod, ip, title, errors).ToString();

                //seleziono il file da scrivere in base a se è una eccezione o meno
                var logPath = GetFileLogPath(HTTP_ERROR_LOGS);

                using StreamWriter stream = new(logPath, true);
                stream.WriteLine(log);
            }
            catch (Exception) { }
        }

        #endregion

        /// <summary>
        /// Metodo che scrive sul file di log per la gestione delle notifiche
        /// </summary>
        /// <param name="txt"></param>
        public static void WriteNotificheLog(string txt, bool isError = false)
        {
            var sb = new StringBuilder();

            var log = isError ? sb.AppendFormat("[{0}] EXCEPTION: {1}", DateTime.Now, txt).ToString()
                              : sb.AppendFormat("[{0}] {1}", DateTime.Now, txt).ToString();
            var logPath = GetFileLogPath(NOTIFICATION_LOGS);
            using StreamWriter stream = new(logPath, true);
            stream.WriteLine(log);
        }

        #region Metodi Privati

        /// <summary>
        /// Metodo che restituisce il percorso dove salvare il file di log
        /// </summary>
        /// <param name="fileName">il nome del file</param>
        /// <returns>stringa con il percorso del file di log</returns>
        private static string GetFileLogPath(string fileName)
        {
            var path = ConfigurationService.GetLogsFolder();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return Path.Combine(path, fileName);
        }

        #endregion
    }
}
