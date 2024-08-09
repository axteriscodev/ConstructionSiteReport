using ConstructionSiteLibrary.Components.ConstructorSites;
using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Model;
using ConstructionSiteLibrary.Utility;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Documents;

namespace ConstructionSiteLibrary.Components.Documents
{
    public partial class TableDocuments
    {

        [Parameter]
        public int SiteId { get; set; }

        ScreenComponent? screenComponent;


        private ConstructorSiteModel site = new();
        private List<DocumentModel> documents = [];
        private List<DocumentModel> displayedDocuments = [];
        private bool onLoading = false;
        private string search = "";
        private string pagingSummaryFormat = "Pagina {0} di {1} (Totale {2} documenti)";
        private int count = 0;
        private int pageSize = GlobalVariables.PageSize;
        private int pageIndex = 0;
        private ScreenSize? screenSize;
        private int documentColumn = 4;

        protected override async Task OnInitializedAsync()
        {
            onLoading = true;
            await base.OnInitializedAsync();
            await LoadData();
            onLoading = false;
        }

        private async Task LoadData()
        {
            site = await ConstructorSitesRepository.GetConstructorSiteInfo(SiteId);
            documents = await DocumentsRepository.GetSiteDocuments(SiteId);
            FilterSites();
        }


        private void PageChanged(PagerEventArgs args)
        {
            pageIndex = args.PageIndex;
            FilterSites();
        }

        private void SearchChanged(string args)
        {
            search = args;
            FilterSites();
        }

        private void FilterSites()
        {
            displayedDocuments = documents;
            if (!string.IsNullOrEmpty(search))
            {
                displayedDocuments = documents.Where(x => x.Title.Contains(search)).ToList();
            }
            count = displayedDocuments.Count;
            SelectCurrentPage();
        }

        private void SelectCurrentPage()
        {
            var skip = pageIndex * pageSize;
            displayedDocuments = displayedDocuments.Skip(skip).Take(pageSize).ToList();
        }

        private void CreaNuovo()
        {
            NavigationService.ChangePage(PageRouting.DocumentCreationPage + SiteId);
        }

        private async Task OnSettingsClick()
        {
            Console.WriteLine("cliccato settings");
            var width = screenComponent!.GetScreenSize().Width;

            // creo uno style aggiuntivo da inviare al componente caricato con il popup come options
            var additionalStyle = $"min-height:fit-content;height:fit-content;width:{width}px;max-width:800px";
            var newOptions = new DialogOptions
            {
                Style = additionalStyle
            };
            //creo parametri da inviare al componente caricato con il popup
            var param = new Dictionary<string, object>
            {
                //tra i parametri che invio al dialog creo un EventCallback da passare al componente
                { "OnSaveComplete", EventCallback.Factory.Create(this, Reload) },
                { "CreationMode", false },
                { "Site", site },
            };
             await DialogService.OpenAsync<FormConstructorSite>("Cantiere", parameters: param, options: newOptions);
        }

        private void Back()
        {
            NavigationService.ChangePage(PageRouting.HomePage);
        }
        private async Task Reload()
        {
            DialogService.Close();
            await LoadData();
            StateHasChanged();
        }


        private void MainSizeChanged(ScreenSize? size)
        {
            int cardDimension = 160;
            int marginX = 10;
            documentColumn = 1;

            if(size is not null)
            {
                var temp = ((double)size.Width / (cardDimension + marginX));
                Console.WriteLine($"decimal: {temp}");
                documentColumn = (int)temp - 1;

                Console.WriteLine($"numero colonne: {documentColumn}");

            }
            StateHasChanged();
        }


        private void MainDimensionChanged(ScreenDimension? newDimension)
        {

            documentColumn = newDimension switch
            {
                ScreenDimension.XL => 8,
                ScreenDimension.L => 5,
                ScreenDimension.M => 5,
                ScreenDimension.S => 2,
                _ => 1,
            };
            StateHasChanged();
        }
    }
}
