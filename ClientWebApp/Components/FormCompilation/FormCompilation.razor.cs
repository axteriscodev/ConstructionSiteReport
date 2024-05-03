using ClientWebApp.Repositories;
using Microsoft.AspNetCore.Components;
using Shared;

namespace ClientWebApp.Components.FormCompilation;

public partial class FormCompilation
{

    private DocumentModel documentModel = new();

    /// <summary>
    /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
    /// </summary>
    private bool initialLoading;

    [Parameter]
    public string Param { get; set; } = "";

    protected override async Task OnInitializedAsync()
    {
        initialLoading = true;
        await base.OnInitializedAsync();
        await LoadData();
        initialLoading = false;
    }

    private async Task LoadData()
    {
        documentModel = await DocumentsRepository.GetDocumentById(1);
    }

    private async Task SaveForm()
    {
       DocumentsRepository.UpdateDocuments([documentModel]);
    }
}
