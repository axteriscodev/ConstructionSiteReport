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

    private int CurrentSelection;

    IEnumerable<int> DocumentsList = [];

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
        List<DocumentModel> docLists = await DocumentsRepository.GetDocuments();

        if (docLists.Count != 0)
        {
            DocumentsList = docLists.Select(x => x.Id);
            CurrentSelection = docLists.First().Id;

            documentModel = await DocumentsRepository.GetDocumentById(CurrentSelection);
        }
    }

    private async Task OnDocumentSelected()
    {
        documentModel = await DocumentsRepository.GetDocumentById(CurrentSelection);
    }

    private async Task SaveForm()
    {
        DocumentsRepository.UpdateDocuments([documentModel]);
    }
}
