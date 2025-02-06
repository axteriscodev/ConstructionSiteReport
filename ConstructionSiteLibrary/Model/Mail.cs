using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionSiteLibrary.Model
{
    public class Mail
    {
        public string? Sender { get; set; }

        public string? Receiver { get; set; }

        public List<string> Cc { get; set; } = [];

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        public List<string> Attachments { get; set; } = [];


        public void CreateQRCodeEmail(string plName, string qrPath, string title = "", string content = "")
        {
            Title = !string.IsNullOrEmpty(title) ? title : DefaultTitle(plName);
            Content = !string.IsNullOrEmpty(content) ? content : DefaultContent(plName);
            Attachments.Add(qrPath);
        }
        public void CreateTokenEmail(string token)
        {
            Title = "Codice di Verifica CheckList";
            Content = $"Il codice per il reset della password è: {token}";
        }


        private static string DefaultTitle(string playlistName)
        {
            return $"Playlist {playlistName}";
        }

        private static string DefaultContent(string playlistName)
        {
            return $"";
        }
    }
}
