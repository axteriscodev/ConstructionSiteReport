using System;
using System.Collections.Generic;
using System.Linq;
using ConstructionSiteLibrary.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ConstructionSiteLibrary.Services
{
    public class NavigationService(NavigationManager navManager, InvokeJSRuntime js)
    {
        /// <summary>
        /// Oggetto che gestisce lo storico delle pagine su cui si è navigato
        /// </summary>
        private PageHistory _pageHistory = new();
        /// <summary>
        /// NavigationManager per navigare tra le varie pagine
        /// </summary>
        private NavigationManager _navigationManager = navManager;
        /// <summary>
        /// Oggetto per utilizzare JSRuntime
        /// </summary>
        private readonly InvokeJSRuntime jsRuntime = js;

        private string DefaultPage = "/";

        #region Gestione Storico e Navigazione Pagine


        /// <summary>
        /// Metodo per cambiare pagina
        /// </summary>
        /// <param name="page">la pagina che si vuole visualizzare</param>
        /// <param name="forceReload">se true obbliga il reload della nuova pagina</param>
        /// <param name="openNewTab">se la pagina va aperta in una nuova tab o no</param>
        public void ChangePage(string page, bool forceReload = false, bool openNewTab = false)
        {
            if (openNewTab)
            {
                _ = OpenNewTab(page);
            }
            else
            {
                AddPage(page);
                _navigationManager.NavigateTo(page, forceReload);
            }
        }

        /// <summary>
        /// Metodo per aggiungere una pagina allo storico delle 
        /// pagine visitate
        /// </summary>
        /// <param name="page">il nome della pagina con il quale la posso richiamare</param>
        public void AddPage(string page)
        {
            _pageHistory.AddPageToHistory(page);
        }

        /// <summary>
        /// Metodo per tornare indietro alla pagina precedente,
        /// se non ho pagina indietro torna stringa vuota
        /// </summary>
        /// <returns></returns>
        public void Back()
        {
            var pagina = _pageHistory.PreviousPage();
            if (!pagina.Equals(""))
            {
                _navigationManager.NavigateTo(pagina);
            }
            else
            {
                _navigationManager.NavigateTo(DefaultPage);
            }

        }

        private async Task OpenNewTab(string page)
        {
            await jsRuntime.OpenNewTab(page);
        }
        #endregion
    }
}
