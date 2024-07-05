using AXT_WebComunication.WebResponse;
using ConstructionSiteLibrary.Repositories;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Defaults;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Choices;

public partial class FormChoice
{
    [Parameter]
    public EventCallback OnSaveComplete { get; set; }
    [Parameter]
    public bool CreationMode { get; set; }
    [Parameter]
    public TemplateChoiceModel? Choice { get; set; }
    
    /// <summary>
    /// Classe utilizzata per incapsulare le informazioni relative alla scelta dell'utente
    /// </summary>
    private FormChoiceData form = new();
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
        onLoading = false;
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        Setup();
    }
    /// <summary>
    /// Metodo che inizializza le impostazioni iniziali del componente sia in caso di creazione 
    /// che in modifica
    /// </summary>
    private void Setup()
    {
        if (!CreationMode && Choice is not null)
        {
            form = new FormChoiceData()
            {
                Id = Choice.Id,
                Tag = Choice.Tag,
                Value = Choice.Value,
                Reportable = Choice.Reportable,
            };
        }
    }

    private async Task Save()
    {
        onSaving = true;
        var newChoice = new TemplateChoiceModel()

        {
            Id = CreationMode ? 0 : form.Id,
            Tag = form.Tag,
            Value = form.Value,
            Reportable = form.Reportable,
        };

        bool success = CreationMode ? await QuestionRepository.SaveChoice(newChoice)
                                    : await QuestionRepository.UpdateChoices([newChoice]);
        if (success)
        {
            await OnSaveComplete!.InvokeAsync();
        }
        onSaving = false;
    }

    private class FormChoiceData
    {
        public int Id { get; set; }
        public string? Tag { get; set; }
        public string? Value { get; set; }
        public bool Reportable { get; set; }
    }
}
