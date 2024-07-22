using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using ConstructionSiteLibrary.Repositories;
using ConstructionSiteLibrary.Components.Utilities;
using Shared.Templates;
using ConstructionSiteLibrary.Utility;

namespace ConstructionSiteLibrary.Components.Choices
{
    public partial class TableChoice
    {
        /// <summary>
        /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
        /// </summary>
        private bool initialLoading;
        /// <summary>
        /// Booleano che è impostata durante una ricerca
        /// </summary>
        private bool isLoading = false;
        /// <summary>
        /// Intero che ci dice quanti sono gli elementi
        /// </summary>
        private int count;
        /// <summary>
        /// Intero che ci dice quanti elementi possono stare in una pagina
        /// </summary>
        private int pageSize = GlobalVariables.PageSize;
        /// <summary>
        /// Stringa indica la pagina e gli elementi
        /// </summary>
        private string pagingSummaryFormat = "Pagina {0} di {1} (Totale {2} scelte)";
        /// <summary>
        /// Riferimento al componente tabella
        /// </summary>
        private RadzenDataGrid<TemplateChoiceModel>? grid;
        /// <summary>
        /// Riferimento alla lista di choices
        /// </summary>
        private List<TemplateChoiceModel> list = [];


        ScreenComponent screenComponent;

        private async Task OpenNewForm()
        {
            var width = screenComponent.ScreenSize.Width;

            // creo uno style aggiuntivo da inviare al componente caricato con il popup come options
            var additionalStyle = $"min-height:fit-content;height:fit-content;width:{width}px;max-width:600px";
            var newOptions = new DialogOptions
            {
                Style = additionalStyle
            };
            //creo parametri da inviare al componente caricato con il popup
            var param = new Dictionary<string, object>
            {
                //tra i parametri che invio al dialog creo un EventCallback da passare al componente
                { "OnSaveComplete", EventCallback.Factory.Create(this, ReloadTable) },
                { "CreationMode", true },
            };
            await DialogService.OpenAsync<FormChoice>("Nuova scelta", parameters: param, options: newOptions);
        }

        private async Task OpenUpdateForm(TemplateChoiceModel model)
        {
            var width = screenComponent.ScreenSize.Width;

            //creo uno style aggiuntivo da inviare al componente caricato con il popup come options
            var additionalStyle = $"min-height:fit-content;height:fit-content;width:{width}px;max-width:600px";
            var newOptions = new DialogOptions
            {
                Style = additionalStyle
            };
            //creo parametri da inviare al componente caricato con il popup
            var param = new Dictionary<string, object>
            {
                //tra i parametri che invio al dialog creo un EventCallback da passare al componente
                { "OnSaveComplete", EventCallback.Factory.Create(this, ReloadTable) },
                { "Choice", model},
                { "CreationMode", false },
            };
            await DialogService.OpenAsync<FormChoice>("Aggiorna scelta", parameters: param, options: newOptions);
        }

        private async Task Disable(TemplateChoiceModel model)
        {
            var titolo = "Disattivazione scelta";
            var text = "Vuoi disattivare la scelta: " + model.Value + "?";
            var confirmationResult = await DialogService.Confirm(text, titolo,
            new ConfirmOptions { OkButtonText = "Si", CancelButtonText = "No" });
            Console.WriteLine("cliccato: " + confirmationResult);
            if (confirmationResult == true)
            {
                var response = await QuestionRepository.HideChoices([model]);
                if (response)
                {
                    await ReloadTable();
                }
            }
        }
        private async Task ReloadTable()
        {
            DialogService.Close();
            await LoadData();
            await grid!.Reload();
        }

        protected override async Task OnInitializedAsync()
        {
            initialLoading = true;
            await base.OnInitializedAsync();
            await LoadData();
            initialLoading = false;
        }
        private async Task LoadData()
        {
            list = await QuestionRepository.GetChoices();
            count = list.Count;
        }
    }
}
