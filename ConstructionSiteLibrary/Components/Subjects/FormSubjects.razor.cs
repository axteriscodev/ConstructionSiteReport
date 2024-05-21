using ConstructionSiteLibrary.Managers;
using ConstructionSiteLibrary.Repositories;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared;

namespace ConstructionSiteLibrary.Components.Subjects;

public partial class FormSubjects
{
    [Parameter]
    public EventCallback OnSaveComplete { get; set; }
    [Parameter]
    public bool CreationMode { get; set; }
    [Parameter]
    public object? Object { get; set; }

    /// <summary>
    /// Classe utilizzata per incapsulare le informazioni inserite dall'utente nalla form
    /// </summary>
    private FormSubjectsData form = new();
    /// <summary>
    /// il design degli elementi della form 
    /// </summary>
    readonly Variant variant = Variant.Outlined;
    /// <summary>
    /// indica se siamo in fase di salvataggio o meno
    /// </summary>
    private bool onSaving = false;
    /// <summary>
    /// FieldSet da mostrare nella form (modifica o creazione)
    /// </summary>
    private string fieldSet = "";



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
        if (!CreationMode && Object is not null)
        {
            fieldSet = "Modifica";
            form = new FormSubjectsData()
            {
                Id = ((SubjectModel)Object).Id,
                Nome = ((SubjectModel)Object).Text,
            };
        }
        else
        {
            fieldSet = "Nuovo";
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
        if (CreationMode)
        {
            response = await CategoriesRepository.SaveSubject(new SubjectModel { Text = form.Nome ?? "" });
     
        }
        else
        {
            response = await CategoriesRepository.UpdateSubjects([new SubjectModel() { Id = form.Id, Text = form.Nome ?? "" }]);
            
        }

        //NotificationService.Notify(response);
        if (response)
        {
            await OnSaveComplete!.InvokeAsync();
        }
        onSaving = false;
    }



    private class FormSubjectsData
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
    }
}
