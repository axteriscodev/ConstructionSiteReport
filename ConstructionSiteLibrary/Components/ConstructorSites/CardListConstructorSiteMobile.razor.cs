using ConstructionSiteLibrary.Components.Questions;
using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Utility;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Documents;


namespace ConstructionSiteLibrary.Components.ConstructorSites
{
    public partial class CardListConstructorSiteMobile
    {
        ScreenComponent? screenComponent;

        private List<ConstructorSiteModel> sites = [];
        private List<ConstructorSiteModel> displayedSites = [];
        private bool onLoading = false;
        private string search = "";
        private string pagingSummaryFormat = "Pagina {0} di {1} (Totale {2} cantieri)";
        private int count = 0;
        private int pageSize = GlobalVariables.PageSize;
        private int pageIndex = 0;

        protected override async Task OnInitializedAsync()
        {
            onLoading = true;
            await base.OnInitializedAsync();
            await LoadData();
            onLoading = false;
        }

        private async Task LoadData()
        {
            sites = await SiteRepository.GetConstructorSites();
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
            displayedSites = sites;
            if (!string.IsNullOrEmpty(search))
            {
                displayedSites = sites.Where(x => x.Name.Contains(search)).ToList();
            }
            count = displayedSites.Count;
            SelectCurrentPage();
        }

        private void SelectCurrentPage()
        {
            var skip = pageIndex * pageSize;
            displayedSites = displayedSites.Skip(skip).Take(pageSize).ToList();
        }

        private async Task OpenForm()
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
                { "OnSaveComplete", EventCallback.Factory.Create(this, Reload) },
                { "CreationMode", true },
            };
            await DialogService.OpenAsync<FormConstructorSite>("Nuovo cantiere", parameters: param, options: newOptions);
        }

        private async Task Reload()
        {
            DialogService.Close();
            await LoadData();
            StateHasChanged();
        }
    }
}
