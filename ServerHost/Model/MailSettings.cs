namespace ServerHost.Model
{
    public class MailSettings
    {
        public string DefaultSender { get; set; } = "";

        public string DefaultReceiver { get; set; } = "";

        public string Host { get; set; } = "";

        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public int Port { get; set; } = 0;

        public EmailCryptography Cryptography { get; set; }

        public bool SecurityProtocol { get; set; }

    }
}
