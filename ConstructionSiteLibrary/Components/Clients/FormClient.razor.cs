using ConstructionSiteLibrary.Repositories;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Documents;

namespace ConstructionSiteLibrary.Components.Clients;

public partial class FormClient
{
    [Parameter]
    public EventCallback OnSaveComplete { get; set; }

    [Parameter]
    public object? Object { get; set; }

    /// <summary>
    /// Classe utilizzata per incapsulare le informazioni inserite dall'utente nalla form
    /// </summary>
    private FormClientData form = new();

    /// <summary>
    /// il design degli elementi della form 
    /// </summary>
    readonly Variant variant = Variant.Outlined;

    /// <summary>
    /// indica se siamo in fase di salvataggio o meno
    /// </summary>
    private bool onSaving = false;


    /// <summary>
    /// Metodo invocato quando il componente è pronto per essere avviato
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await Setup();
    }

     /// <summary>
    /// Metodo che inizializza le impostazioni iniziali del componente sia in caso di creazione 
    /// che in modifica
    /// </summary>
    private async Task Setup()
    {
        if (Object is not null)
        {
            form = new FormClientData()
            {
                Id = ((ClientModel)Object).Id,
                Name = ((ClientModel)Object).Name,
            };
        }

    }

    /// <summary>
    /// Metodo richiamato dal bottone per salvare il salvataggio del nuovo record
    /// o della modifica di quello esistente (dipende se il componente è stato aperto
    /// in creazione o modifica)
    /// </summary>
    /// <returns></returns>
    private async Task Save()
    {
        onSaving = true;
        bool response;
        if (Object is null)
        {
            response = await ClientsRepository.SaveClient(new ClientModel { Name = form.Name ?? "" });
        }
        else
        {
            response = await ClientsRepository.UpdateClients([new ClientModel() { Id = form.Id, Name = form.Name ?? "" }]);
        }

        //NotificationService.Notify(response);
        if (response)
        {
            await OnSaveComplete!.InvokeAsync();
        }
        onSaving = false;
    }

    private class FormClientData
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}
