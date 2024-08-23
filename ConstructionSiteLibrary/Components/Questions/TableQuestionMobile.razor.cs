using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Managers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Shared.Defaults;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Questions;

public partial class TableQuestionMobile
{
    /// <summary>
    /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
    /// </summary>
    private bool initialLoading;

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
    private string pagingSummaryFormat = "Pagina {0} di {1} (Totale {2} domande)";
    /// <summary>
    /// Riferimento alla lista di questions
    /// </summary>
    private List<TemplateQuestionModel> questions = [];

    ScreenComponent? screenComponent;

    private async Task OpenNewForm()
    {
        var width = screenComponent.ScreenSize.Width;

        // creo uno style aggiuntivo da inviare al componente caricato con il popup come options
        var additionalStyle = $"min-height:fit-content;height:fit-content;width:{width}px;max-width:600px";
        var newOptions = new DialogOptions
        {
            Style = additionalStyle
        };
        //creo parametri da inviare al componente caricato con il popup
        var param = new Dictionary<string, object>
            {
                //tra i parametri che invio al dialog creo un EventCallback da passare al componente
                { "OnSaveComplete", EventCallback.Factory.Create(this, ReloadTable) },
                { "CreationMode", true },
            };
        await DialogService.OpenAsync<FormQuestions>("Aggiorna domanda", parameters: param, options: newOptions);
    }

    private async Task OpenUpdateForm(QuestionModel model)
    {
        var width = screenComponent.ScreenSize.Width;

        //creo uno style aggiuntivo da inviare al componente caricato con il popup come options
        var additionalStyle = $"min-height:fit-content;height:fit-content;width:{width}px;max-width:600px";
        var newOptions = new DialogOptions
        {
            Style = additionalStyle
        };
        //creo parametri da inviare al componente caricato con il popup
        var param = new Dictionary<string, object>
            {
                //tra i parametri che invio al dialog creo un EventCallback da passare al componente
                { "OnSaveComplete", EventCallback.Factory.Create(this, ReloadTable) },
                { "Question", model},
                {"CreationMode", false },
            };
        await DialogService.OpenAsync<FormQuestions>("Aggiorna domanda", parameters: param, options: newOptions);
    }

    private async Task Disable(TemplateQuestionModel model)
    {
        var titolo = "Disattivazione domanda";
        var text = "Vuoi disattivare la domanda: " + model.Text + "?";
        var confirmationResult = await DialogService.Confirm(text, titolo,
        new ConfirmOptions { OkButtonText = "Si", CancelButtonText = "No" });
        Console.WriteLine("cliccato: " + confirmationResult);
        if (confirmationResult == true)
        {
            var response = await QuestionRepository.HideQuestions([model]);
            if (response)
            {
                await ReloadTable();
            }
        }
    }
    private async Task ReloadTable()
    {
        DialogService.Close();
        await LoadData();
    }

    protected override async Task OnInitializedAsync()
    {
        initialLoading = true;
        await base.OnInitializedAsync();
        await LoadData();
        initialLoading = false;
    }
    private async Task LoadData()
    {
        questions = await QuestionRepository.GetQuestions();
        count = questions.Count;
    }


    private string PrintSubject(int? idSubject)
    {
        return idSubject is not null && idSubject > 0 ? idSubject.Value.ToString() : "-";
    }
}
