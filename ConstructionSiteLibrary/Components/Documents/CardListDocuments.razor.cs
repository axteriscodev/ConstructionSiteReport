using ConstructionSiteLibrary.Utility;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Documents;


namespace ConstructionSiteLibrary.Components.Documents
{
    public partial class CardListDocuments
    {

        [Parameter]
        public int SiteId { get; set; }

        private List<DocumentModel> documents = [];
        private List<DocumentModel> displayedDocuments = [];
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
            documents = await DocumentRepository.GetSiteDocuments(SiteId);
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
    }
}
