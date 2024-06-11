using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Templates.Wizard;

public partial class TemplateSelectionStep
{
    List<TemplateModel> Templates = [];

    TemplateModel? CurrentSelection;

    /// <summary>
    /// booleano che indica se la pagina sta eseguendo il caricamento iniziale
    /// </summary>
    private bool initialLoading;

    ScreenComponent screenComponent;

}
