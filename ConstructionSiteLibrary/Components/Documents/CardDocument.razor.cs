using ConstructionSiteLibrary.Utility;
using Microsoft.AspNetCore.Components;
using Shared.Documents;


namespace ConstructionSiteLibrary.Components.Documents
{
    public partial class CardDocument
    {

        [Parameter]
        public DocumentModel Document { get; set; } = new();


        private void OnDocumentClick()
        {
            Console.WriteLine("cliccato documento:" + Document.Id);
            Navigation.ChangePage(PageRouting.DocumentCompilationPage + Document.Id);
        }

    }
}
