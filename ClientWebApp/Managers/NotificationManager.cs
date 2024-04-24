using AXT_WebComunication.WebResponse;
using Radzen;

namespace ClientWebApp.Managers
{
    /// <summary>
    /// Classe utilizzata per inviare al client delle notifiche per avvisarlo dello stato del sistema o delle operazioni
    /// che avvengono sotto. Si basa sul servizio di Notifiche di Radzen e lo estende per l'utilizzo di questo progetto
    /// </summary>
    public class NotificationManager(NotificationService notificationService)
    {
        private readonly NotificationService _notificationService = notificationService;

        /// <summary>
        /// Metodo per creare una notifica da far visualizzare al client in base al risultato 
        /// della richiesta effettuata (AXT_WebResponse)
        /// </summary>
        /// <param name="response">AXT_WebResponse</param>
        public void Notify(AXT_WebResponse response)
        {
            var messaggio = CreateMessage(response);
            _notificationService.Notify(messaggio);
        }

        private static NotificationMessage CreateMessage(AXT_WebResponse response)
        {
            NotificationMessage notification;
            try
            {
                notification = response.Code switch
                {
                    "0" => new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Successo",
                        Detail = "L'operazione è stata eseguita con successo",
                        Duration = 5000
                    },
                    //Already Exist
                    "Ex89842DFE" => new NotificationMessage
                    {
                        Severity = NotificationSeverity.Warning,
                        Summary = "Nessuna modifica",
                        Detail = "L'operazione non è stata eseguita perchè il record è già presente",
                        Duration = 5000
                    },
                    //HTTP ERROR
                    "Ex8995951C" => new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Errore richiesta HTTP",
                        Detail = "La richiesta http non è ben formattata",
                        Duration = 5000
                    },
                    // NO RESPONSE
                    "Ex8995BA25" => new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Errore di comunicazione",
                        Detail = "richiesta in timeout, il server non ha risposto.",
                        Duration = 5000
                    },
                    //DEFAULT
                    _ => new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Errore sconosciuto",
                        Detail = "Si è presentato un errore, si prega di riprovare",
                        Duration = 5000
                    }
                };
            }
            catch (Exception e)
            {
                notification = new()
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Eccezione sul server",
                    Detail = e.Message,
                    Duration = 5000
                };
            }

            return notification;
        }
    }
}
