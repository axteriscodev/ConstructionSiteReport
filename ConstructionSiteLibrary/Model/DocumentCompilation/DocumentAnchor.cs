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
        public int Index { get; set; }
        public int TypeIndex { get; set; }
        public string Text { get; set; } = "";
        public string Anchor { get; set; } = "";

        public DocumentAnchor() { }

        public DocumentAnchor(string text, string anchor) : this (0, text, anchor) { }
        

        public DocumentAnchor(int idx, string text, string anchor) : this(0, idx, text, anchor) { }


        public DocumentAnchor(int typeIndex, int idx, string text, string anchor)
        {
            TypeIndex = typeIndex;
            Index = idx;
            Text = text;
            Anchor = anchor;
        }
    }
}
