using System.Text.Json;
using ClientWebApp.Managers;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using Shared;

namespace ClientWebApp.Components.Choices
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
        private int pageSize = 8;
        /// <summary>
        /// Stringa indica la pagina e gli elementi
        /// </summary>
        private string pagingSummaryFormat = "Pagina {0} di {1} (Totale {2} campi misurazione)";
        /// <summary>
        /// Riferimento al componente tabella
        /// </summary>
        private RadzenDataGrid<ChoiceModel>? grid;
        /// <summary>
        /// Riferimento alla lista di choices
        /// </summary>
        private List<ChoiceModel> list = [];

        private async Task OpenNewForm()
        {
            // creo uno style aggiuntivo da inviare al componente caricato con il popup come options
            var additionalStyle = "min-width:600px;min-height:fit-content;height:fit-content;width:600px;";
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
            await DialogService.OpenAsync<FormChoice>("Aggiorna argomento", parameters: param, options: newOptions);
        }

        private async Task OpenUpdateForm(ChoiceModel model)
        {
            //creo uno style aggiuntivo da inviare al componente caricato con il popup come options
            var additionalStyle = "min-width:600px;min-height:fit-content;height:fit-content;width:600px;";
            var newOptions = new DialogOptions
            {
                Style = additionalStyle
            };
            //creo parametri da inviare al componente caricato con il popup
            var param = new Dictionary<string, object>
            {
                //tra i parametri che invio al dialog creo un EventCallback da passare al componente
                { "OnSaveComplete", EventCallback.Factory.Create(this, ReloadTable) },
                { "Object", model},
                {"CreationMode", false },
            };
            await DialogService.OpenAsync<FormChoice>("Aggiorna scelta", parameters: param, options: newOptions);
        }

        private async Task Disable(ChoiceModel model)
        {
            var titolo = "Disattivazione scelta";
            var text = "Vuoi disattivare la scelta: " + model.Value + "?";
            var confirmationResult = await DialogService.Confirm(text, titolo,
            new ConfirmOptions { OkButtonText = "Si", CancelButtonText = "No" });
            Console.WriteLine("cliccato: " + confirmationResult);
            if (confirmationResult == true)
            {
                var response = await HttpManager.SendHttpRequest("Question/HideChoice", new[] { model });
                if (response.Code.Equals("0"))
                {
                    await ReloadTable();
                }
            }
        }
        private async Task ReloadTable()
        {
            DialogService.Close();
            await GetData();
            await grid!.Reload();
        }

        protected override async Task OnInitializedAsync()
        {
            initialLoading = true;
            await base.OnInitializedAsync();
            await GetData();
            initialLoading = false;
        }
        private async Task GetData()
        {
            var response = await HttpManager.SendHttpRequest("Question/ChoicesList", "");
            if (response.Code.Equals("0"))
            {
                list = JsonSerializer.Deserialize<List<ChoiceModel>>(response.Content.ToString() ?? "") ?? [];
                count = list.Count;

            }
        }
    }
}
