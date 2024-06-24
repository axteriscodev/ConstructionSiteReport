using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Model;
using ConstructionSiteLibrary.Repositories;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Defaults;
using Shared.Documents;


namespace ConstructionSiteLibrary.Components.Documents;

public partial class DocumentCompilation
{

    public DocumentModel documentModel = new();

    /// <summary>
    /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
    /// </summary>
    private bool initialLoading;
    private bool onSaving = false;

    private string ImgFirma = "";
    private List<string> ImgAllegati = [];
    private ScreenComponent? screenComponent;
    private ScreenSize? canvasSize = new() { Width = 600, Height = 150 };

    //IEnumerable<int> DocumentsList = [];
    List<VisualCategory> visualCategories = [];

    [Parameter]
    public string Param { get; set; } = "";

    [Parameter]
    public int CurrentSelection { get; set; }

    protected override async Task OnInitializedAsync()
    {
        initialLoading = true;
        await base.OnInitializedAsync();
        await LoadData();
        initialLoading = false;
    }

    private async Task LoadData()
    {
        // List<DocumentModel> docLists = await DocumentsRepository.GetDocuments();

        // if (docLists.Count != 0)
        // {
        //     DocumentsList = docLists.Select(x => x.Id);
        //     if(CurrentSelection == 0)
        //     {
        //         CurrentSelection = docLists.First().Id;
        //     }

        //     documentModel = docLists.Where(x => x.Id == CurrentSelection).FirstOrDefault() ?? new();
        //     CreateVisualCategories();
        // }

        documentModel = await DocumentsRepository.GetDocumentById(CurrentSelection);
        CreateVisualCategories();
        ImgFirma = "";
        ImgAllegati = [];
    }

    // private async Task OnDocumentSelected()
    // {
    //     documentModel = await DocumentsRepository.GetDocumentById(CurrentSelection);
    //     CreateVisualCategories();
    //     ImgFirma = "";
    //     ImgAllegati = [];
    // }

    private void CreateVisualCategories()
    {
        visualCategories = [];
        foreach (var cat in documentModel.Categories)
        {
            visualCategories.Add(new() { Category = cat });
        }
    }

    private async Task SaveForm()
    {
        onSaving = true;
        //documentModel.LastModified = DateTime.Now;
        await DocumentsRepository.UpdateDocuments([documentModel]);
        //await LoadData();
        onSaving = false;
    }

    private void SavedSignature(Signature signature)
    {
        DialogService.Close();
        ImgFirma = signature.Image;
        StateHasChanged();
    }

    private void ScreenSizeObservable(ScreenSize? size)
    {
        if (size is not null)
        {
            if (size.Width < canvasSize.Width)
            {
                Console.WriteLine("cv - " + canvasSize.Width);
                canvasSize.Width = size.Width - 60;
                Console.WriteLine("cv new - " + canvasSize.Width);
                StateHasChanged();
            }
        }
    }

    private async Task TakePicture()
    {
        var img = await CameraService.OpenCamera();
        ImgAllegati.Add(img);
        StateHasChanged();
    }

    #region Visualizzazione

    private static string CategoryText(CategoryModel cat)
    {
        return cat.Order + ". " + cat.Text;
    }

    private static string QuestionText(CategoryModel cat, string questionText, int order)
    {
        return cat.Order + "." + order + " " + questionText;
    }

    private static void ShowQuestions(VisualCategory cat)
    {
        cat.ShowQuestion = !cat.ShowQuestion;
    }

    private static string AccordionIcon(VisualCategory cat)
    {
        return cat.ShowQuestion ? "remove" : "add";
    }

    #endregion

    #region Classe interna 

    class VisualCategory
    {
        public bool ShowQuestion = true;
        public DocumentCategoryModel Category { get; set; } = new();
    }

    #endregion
}
