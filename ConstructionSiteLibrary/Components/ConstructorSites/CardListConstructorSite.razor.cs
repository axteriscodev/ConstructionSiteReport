using ConstructionSiteLibrary.Utility;
using Radzen;
using Shared.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionSiteLibrary.Components.ConstructorSites
{
    public partial class CardListConstructorSite
    {

        
        private List<ConstructorSiteModel> sites = [];
        private List<ConstructorSiteModel> displayedSites = [];
        private bool onLoading = false;
        private string search = "";
        private string pagingSummaryFormat = "";
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


    }
}
