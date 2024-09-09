using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionSiteLibrary.Model.DocumentCompilation
{
    /// <summary>
    /// Classe che contiene i nomi da assegnare alle varie parti del documento 
    /// per permetterne la navigazione senza dover scrollare su e giù obbligatoriamente
    /// </summary>
    public class DocumentAnchor
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Text { get; set; } = "";
        public string Anchor { get; set; } = "";

        public DocumentAnchor() { }

        public DocumentAnchor(string text, string anchor) : this (0, text, anchor) { }
        

        public DocumentAnchor(int id, string text, string anchor)
        {
            Id = id;
            Text = text;
            Anchor = anchor;
        }
    }
}
