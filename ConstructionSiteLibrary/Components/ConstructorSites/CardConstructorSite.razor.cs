using Shared.Documents;
using Microsoft.AspNetCore.Components;

namespace ConstructionSiteLibrary.Components.ConstructorSites
{
    public partial class CardConstructorSite
    {
        [Parameter]
        public ConstructorSiteModel Site { get; set; } = new();




        private string PrintDate(DateTime? date)
        {
            return date is null? "" : date.Value.ToString("dd/MM/yyyy");
        }
    }
}
