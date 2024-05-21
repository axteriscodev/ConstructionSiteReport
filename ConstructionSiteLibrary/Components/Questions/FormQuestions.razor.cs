using AXT_WebComunication.WebResponse;
using ConstructionSiteLibrary.Managers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConstructionSiteLibrary.Components.Questions;

public partial class FormQuestions
{
    [Parameter]
    public EventCallback OnSaveComplete { get; set; }
    [Parameter]
    public bool CreationMode { get; set; }
    [Parameter]
    public QuestionModel? Question { get; set; }

    /// <summary>
    /// Classe utilizzata per incapsulare le informazioni relative alla scelta dell'utente
    /// </summary>
    private FormQuestionData form = new();

    private List<CategoryModel> categories = [];
    private List<SubjectModel> subjects = [];
    private List<ChoiceModel> choices = [];

    private RadzenDropDownDataGrid<List<ChoiceModel>>? choicesDropDown;

    /// <summary>
    /// il design degli elementi della form
    /// </summary>
    readonly Variant variant = Variant.Outlined;
    /// <summary>
    /// 
    /// </summary>
    private bool onSaving = false;
    private bool onLoading = false;

    /// <summary>
    /// Metodo invocato quando il componente è pronto per essere avviato
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        onLoading = true;
        await base.OnInitializedAsync();
        await CaricaDati();
        Setup();
        onLoading = false;
    }

    /// <summary>
    /// Metodo che inizializza le impostazioni iniziali del componente sia in caso di creazione 
    /// che in modifica
    /// </summary>
    private void Setup()
    {
        if (!CreationMode && Question is not null)
        {
            form = new FormQuestionData()
            {
                Id = Question.Id,
                Text = Question.Text,
                IdCategory = Question.IdCategory,
                IdSubject = Question.IdSubject,
                Choices = Question.Choices,
                CurrentChoice = Question.CurrentChoice,
                Note = Question.Note,
                Printable = Question.Printable,
                Hidden = Question.Hidden,
            };
        }
    }

    private async Task CaricaDati()
    {
       categories = await CategoriesRepository.GetCategories();
       subjects = await CategoriesRepository.GetSubjects();
        choices = await QuestionRepository.GetChoices();
    }

    private async Task Save()
    {
        onSaving = true;

        var newQuestion = new QuestionModel()
        {
            Id = CreationMode ? 0 : form.Id,
            Text = form.Text,
            IdCategory = form.IdCategory!.Value,
            IdSubject = form.IdSubject,
            Choices = form.Choices,
            CurrentChoice = form.CurrentChoice,
            Note = form.Note ?? "",
            Printable = form.Printable,
            Hidden = form.Hidden,
        };

        bool success = CreationMode ? await QuestionRepository.SaveQuestion(newQuestion)
                                    : await QuestionRepository.UpdateQuestions([newQuestion]);
        if (success)
        {
            await OnSaveComplete!.InvokeAsync();
        }
        onSaving = false;
    }

    #region Gestione Checkbox choices

    private bool ChoiceCheckBoxValue(ChoiceModel choice)
    {
        return form.Choices.Find(x => x.Id == choice.Id ) is not null;
    }

    private async Task<bool> ChoiceCheckBoxChange(bool value, ChoiceModel choice)
    {
        if (value)
        {
            form.Choices.Add(choice);
        }
        else
        {
            var c = form.Choices.Where(x => x.Id == choice.Id).FirstOrDefault();
            if(c is not null)
            {
                form.Choices.Remove(c);
            }
        }
        return value;
    }

    #endregion

    private class FormQuestionData
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public int? IdCategory { get; set; }
        public int? IdSubject { get; set; }
        public List<ChoiceModel> Choices { get; set; } = [];
        public ChoiceModel? CurrentChoice { get; set; }
        public string? Note { get; set; }
        public bool Printable { get; set; }
        public bool Hidden { get; set; }
    }
}
