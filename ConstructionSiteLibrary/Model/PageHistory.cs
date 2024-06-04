using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionSiteLibrary.Model
{

    /// <summary>
    /// Classe per gestire le pagine visualizzate durante la navigazione 
    /// dell'app e per permettere di tornare indietro
    /// </summary>
    public class PageHistory
    {
        private readonly List<string> previousPages = [];


        /// <summary>
        /// Metodo per aggiungere il nome di una pagina nella lista 
        /// delle pagine visualizzate
        /// </summary>
        /// <param name="page">l'url parziale della pagina</param>
        public void AddPageToHistory(string page)
        {
            previousPages.Add(page);
        }

        /// <summary>
        /// Metodo per aggiungere una lista di pagine alle pagine 
        /// visualizzate
        /// </summary>
        /// <param name="pages">lista di pagine da aggiungere</param>
        public void AddPagesToHistory(List<string> pages)
        {
            previousPages.AddRange(pages);
        }

        /// <summary>
        /// Metodo per tornare indietro alla pagina precedente,
        /// se non ho pagina indietro torna stringa vuota
        /// </summary>
        /// <returns></returns>
        public string PreviousPage()
        {
            var page = "";
            if (CanGoBack())
            {
                page = previousPages[^2];
                //rimuovo la pagina attuale dallo storico
                previousPages.RemoveAt(previousPages.Count - 1);
            }
            else
            {
                previousPages.Add("");
            }
            return page;
        }

        /// <summary>
        /// Metodo per verificare che posso tornare indietro di pagina
        /// </summary>
        /// <returns></returns>
        public bool CanGoBack()
        {
            return previousPages.Count > 1;
        }
    }
}

