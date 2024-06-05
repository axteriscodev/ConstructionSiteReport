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

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            if(Site is not null)
            {
                form = new FormSite()
                { 
                    Id = Site.Id,
                    Name = Site.Name,
                    JobDescription = Site.JobDescription,
                    Address = Site.Address,
                    StartDate = Site.StartDate,
                };
            }

            await base.OnParametersSetAsync();
        }

        private async Task Save()
        {
            Site = new()
            {
                Id = form.Id,
                Name = form.Name!,
                JobDescription = form.JobDescription ?? "",
                Address = form.Address!,
                StartDate = form.StartDate.HasValue ? form.StartDate.Value : DateTime.Today,
            };
            var success = CreationMode ? await SiteRepository.SaveContructorSite(Site)
                                       : await SiteRepository.UpdateContructorSites([Site]);
            if(success)
            {
                await OnSaveComplete.InvokeAsync();
            }
        }

        private class FormSite
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? JobDescription { get; set; }
            public string? Address { get; set; }
            public DateTime? StartDate { get; set; }
        }
    }
}
