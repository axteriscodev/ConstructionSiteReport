using AXT_WebComunication.WebResponse;
using ClientWebApp.Managers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared;

namespace ClientWebApp.Components.Questions;

public partial class FormQuestions
{
    [Parameter]
    public EventCallback OnSaveComplete { get; set; }
    [Parameter]
    public bool CreationMode { get; set; }
    [Parameter]
    public object? Object { get; set; }

    /// <summary>
    /// Classe utilizzata per incapsulare le informazioni relative alla scelta dell'utente
    /// </summary>
    private FormQuestionData form = new();
    /// <summary>
    /// il design degli elementi della form
    /// </summary>
    readonly Variant variant = Variant.Outlined;
    /// <summary>
    /// 
    /// </summary>
    private bool onSaving = false;

    /// <summary>
    /// Metodo invocato quando il componente è pronto per essere avviato
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Setup();
    }

    /// <summary>
    /// Metodo che inizializza le impostazioni iniziali del componente sia in caso di creazione 
    /// che in modifica
    /// </summary>
    private void Setup()
    {
        if (!CreationMode && Object is not null)
        {
            form = new FormQuestionData()
            {
                Id = ((QuestionModel)Object).Id,
                Text = ((QuestionModel)Object).Text,
                IdCategory = ((QuestionModel)Object).IdCategory,
                IdSubject = ((QuestionModel)Object).IdSubject,
                Choices = ((QuestionModel)Object).Choices,
                CurrentChoice = ((QuestionModel)Object).CurrentChoice,
                Note = ((QuestionModel)Object).Note,
                Printable = ((QuestionModel)Object).Printable,
                Hidden = ((QuestionModel)Object).Hidden,
            };
        }
    }

    private async Task Save()
    {
        onSaving = true;
        AXT_WebResponse response;
        if (CreationMode)
        {
            response = await HttpManager.SendHttpRequest("Question/SaveQuestion", new QuestionModel
            {
                Text = ((QuestionModel)Object).Text, 
                IdCategory = ((QuestionModel)Object).IdCategory,
                IdSubject = ((QuestionModel)Object).IdSubject,
                Choices = ((QuestionModel)Object).Choices,
                CurrentChoice = ((QuestionModel)Object).CurrentChoice,
                Note = ((QuestionModel)Object).Note,
                Printable = ((QuestionModel)Object).Printable,
                Hidden = ((QuestionModel)Object).Hidden,
            });
        }
        else
        {
            List<QuestionModel> list = [];
            list.Add(new()
            {
                Text = ((QuestionModel)Object).Text,
                IdCategory = ((QuestionModel)Object).IdCategory,
                IdSubject = ((QuestionModel)Object).IdSubject,
                Choices = ((QuestionModel)Object).Choices,
                CurrentChoice = ((QuestionModel)Object).CurrentChoice,
                Note = ((QuestionModel)Object).Note,
                Printable = ((QuestionModel)Object).Printable,
                Hidden = ((QuestionModel)Object).Hidden,
            });
            response = await HttpManager.SendHttpRequest("Question/UpdateQuestion", list);
        }

        if (response.Code.Equals("0"))
        {
            await OnSaveComplete!.InvokeAsync();
        }
        onSaving = false;
    }



    private class FormQuestionData
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int IdCategory { get; set; }
        public int? IdSubject { get; set; }
        public List<ChoiceModel> Choices { get; set; }
        public ChoiceModel CurrentChoice { get; set; }
        public string? Note { get; set; }
        public bool Printable { get; set; }
        public bool Hidden { get; set; }
    }
}
