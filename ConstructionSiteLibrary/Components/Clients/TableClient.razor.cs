using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Shared;

namespace ConstructionSiteLibrary.Components.Clients;

public partial class TableClient
{
    private List<ClientModel> clients = [];

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
    private string pagingSummaryFormat = "Pagina {0} di {1} (Totale {2} clienti)";

    /// <summary>
    /// Riferimento al componente tabella
    /// </summary>
    private RadzenDataGrid<ClientModel>? grid;

    [Parameter]
    public string Param { get; set; } = "";

    ScreenComponent screenComponent;

    protected override async Task OnInitializedAsync()
    {
        initialLoading = true;
        await base.OnInitializedAsync();
        await LoadData();
        initialLoading = false;
    }

    private async Task LoadData()
    {
        clients = await ClientsRepository.GetClients();
        count = clients.Count;
    }

}
