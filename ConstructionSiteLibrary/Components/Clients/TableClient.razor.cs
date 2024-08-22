using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Model;
using ConstructionSiteLibrary.Utility;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Shared.Documents;

namespace ConstructionSiteLibrary.Components.Clients;

public partial class TableClient
{
    ScreenComponent? screenComponent;

    private List<ClientModel> clients = [];

    private List<ClientModel> displayedClients = [];

    private bool onLoading = false;

    private string search = "";

    private int count = 0;

    private int pageSize = GlobalVariables.PageSize;

    private int pageIndex = 0;

    private string pagingSummaryFormat = "Pagina {0} di {1} (Totale {2} committenti)";


    protected override async Task OnInitializedAsync()
    {
        onLoading = true;
        await base.OnInitializedAsync();
        await LoadData();
        onLoading = false;
    }

    private async Task LoadData()
    {
        clients = await ClientsRepository.GetClients();
        FilterClients();
    }

    private void PageChanged(AxtPagerEventArgs args)
    {
        pageIndex = args.CurrentPage;
        FilterClients();
    }

    private void SearchChanged(string args)
    {
        search = args;
        FilterClients();
    }

    private void FilterClients()
    {
        displayedClients = clients;
        search = search.TrimStart().TrimEnd();
        if (!string.IsNullOrEmpty(search))
        {
            displayedClients = displayedClients.Where(x => x.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        count = displayedClients.Count;
        SelectCurrentPage();
    }

    private void SelectCurrentPage()
    {
        var skip = pageIndex * pageSize;
        displayedClients = displayedClients.Skip(skip).Take(pageSize).ToList();
    }

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
            };
        await DialogService.OpenAsync<FormClient>("Nuovo cliente", parameters: param, options: newOptions);
    }

    private async Task OpenUpdateForm(object item)
    {
        var client = item as ClientModel;

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
                { "Object", client},
            };
        await DialogService.OpenAsync<FormClient>("Aggiorna cliente", parameters: param, options: newOptions);
    }


    private async Task Hide(object item)
    {
        var client = item as ClientModel;

        var titolo = "Disattivazione cliente";
        var text = "Vuoi disattivare il cliente: " + client.Name + "?";
        var confirmationResult = await DialogService.Confirm(text, titolo, new ConfirmOptions { OkButtonText = "Si", CancelButtonText = "No" });
        Console.WriteLine("cliccato: " + confirmationResult);
        if (confirmationResult == true)
        {
            var response = await ClientsRepository.HideClients([client]);
            //NotificationService.Notify(response);
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
        //await grid!.Reload();
    }

}
