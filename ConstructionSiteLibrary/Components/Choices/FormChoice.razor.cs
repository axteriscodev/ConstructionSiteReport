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
    public object? Object { get; set; }

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

    /// <summary>
    /// Metodo invocato quando il componente è pronto per essere avviato
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
      
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if(!CreationMode && Object is not null)
        {
            Setup();
        }
    }
    /// <summary>
    /// Metodo che inizializza le impostazioni iniziali del componente sia in caso di creazione 
    /// che in modifica
    /// </summary>
    private void Setup()
    {
        var choice = (TemplateChoiceModel)Object!;
        form = new FormChoiceData()
        {
            Id = choice.Id,
            Tag = choice.Tag,
            Value = choice.Value,
        };
    }

    private async Task Save()
    {
        onSaving = true;
        bool response;
        if (CreationMode)
        {
            response = await QuestionRepository.SaveChoice(new ChoiceModel
            {
                Tag = form.Tag ?? "",
                Value = form.Value ?? "",
            });
        }
        else
        {
            List<ChoiceModel> list = [];
            list.Add(new()
            {
                Tag = form.Tag ?? "",
                Value = form.Value ?? "",
                Id = form.Id,
            });
            response = await QuestionRepository.UpdateChoices(list);
        }

        if (response)
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
    }
}
