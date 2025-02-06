using ConstructionSiteLibrary.Model;
using System;
using System.Collections.Generic;
using MailKit.Net.Smtp;
using System.Security.Cryptography.X509Certificates;
using ServerHost.Services.Interfaces;
using MailKit.Security;
using ServerHost.Model;
using System.Net;
using System.Net.Security;
using MimeKit;

namespace ServerHost.Services
{
    internal class MailKitService : IMailService
    {
        public async Task<bool> SendMail(Mail mail)
        {
            var result = true;
            try
            {
                //Recupero le impostazioni per l'invio delle mail
                var mailSettings = ConfigurationService.GetEmailSettings();
                //Creo la mail
                var email = CreateEmail(mail, mailSettings.DefaultSender, mailSettings.DefaultReceiver);
                //Accetto il certificato del server senza farmi domande (possibile problema sicurezza ma il server è il nostro)

#pragma warning disable CS8622
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain,
                                                                                    SslPolicyErrors sslPolicyErrors)
                { return true; };
#pragma warning restore CS8622

                //Creo il client smtp
                var smtp = new SmtpClient();
                //seleziono il metodo di criptazione
                SecureSocketOptions option = mailSettings.Cryptography switch
                {
                    EmailCryptography.None => SecureSocketOptions.None,
                    EmailCryptography.SSL => SecureSocketOptions.SslOnConnect,
                    EmailCryptography.STARTTLS => SecureSocketOptions.StartTls,
                    EmailCryptography.Automatic => SecureSocketOptions.Auto,
                    _ => SecureSocketOptions.None
                };

                //Connessione al server
                await smtp.ConnectAsync(mailSettings.Host, mailSettings.Port, option);
                //Autenticazione
                await smtp.AuthenticateAsync(mailSettings.Username, mailSettings.Password);
                //Invio mail
                await smtp.SendAsync(email);
                //Disconnessione
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }


        private static MimeMessage CreateEmail(Mail mail, string defaulSender, string defaultReceiver)
        {
            var email = new MimeMessage
            {
                Subject = mail.Title,
            };

            //Creo il corpo della mail aggiungento il testo e gli allegati
            var builder = new BodyBuilder
            {
                TextBody = mail.Content
            };
            foreach (var attachment in mail.Attachments)
            {
                builder.Attachments.Add(attachment);
            }
            email.Body = builder.ToMessageBody();

            //aggiungo il mittente
            var sender = string.IsNullOrEmpty(mail.Sender) ? defaulSender : mail.Sender;
            email.From.Add(MailboxAddress.Parse(sender));
            //Aggiungo destinatario + cc se presenti
            var receiver = string.IsNullOrEmpty(mail.Receiver) ? defaultReceiver : mail.Receiver;
            email.To.Add(MailboxAddress.Parse(receiver));
            foreach (var cc in mail.Cc)
            {
                email.Cc.Add(MailboxAddress.Parse(cc));
            }

            return email;
        }
    }
}