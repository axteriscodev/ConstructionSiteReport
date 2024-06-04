using Microsoft.AspNetCore.Components;
using Shared.Documents;

namespace ConstructionSiteLibrary.Components.ConstructorSites
{
    public partial class FormConstructorSite
    {
        [Parameter]
        public EventCallback OnSaveComplete { get; set; }
        [Parameter]
        public bool CreationMode { get; set; }
        [Parameter]
        public ConstructorSiteModel? Site { get; set; }

        private FormSite form = new();

        private bool onSaving = false;
        private bool onLoading = false;


        private async Task Save()
        {
            Site = new()
            {
                Name = form.Name!,
                JobDescription = form.JobDescription ?? "",
                Address = form.Address!,
                StartDate = form.StartDate.HasValue ? form.StartDate.Value : DateTime.Today,
            };
            var success = await SiteRepository.SaveContructorSite(Site);
            if(success)
            {
                await OnSaveComplete.InvokeAsync();
            }
        }

        private class FormSite
        {
            public string? Name { get; set; }
            public string? JobDescription { get; set; }
            public string? Address { get; set; }
            public DateTime? StartDate { get; set; }
        }
    }
}
