using ConstructionSiteLibrary.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace ConstructionSiteLibrary.Components.Utilities
{
    public partial class SignatureComponent
    {
        [Parameter]
        public int CanvasHeight { get; set; } = 200;
        [Parameter]
        public int CanvasWidth { get; set; } = 300;
        [Parameter]
        public bool DebugImage { get; set; } = false;
        [Parameter]
        public EventCallback<Signature> OnSavedSignature { get; set; }
        private string canvasId = "canvasFirma";
        private bool onSaving = false;
        private Signature signature = new();
        IJSObjectReference? module;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/ConstructionSiteLibrary/signature.js");
                await module.InvokeVoidAsync("loadCanvas", canvasId);
            }
        }

        /// <summary>
        /// Metodo richiamato dal bottone di salvataggio per salvare la firma nel canvas 
        /// in un immagine
        /// </summary>
        /// <returns></returns>
        private async Task Save()
        {
            onSaving = true;
            if (await IsCanvasBlank())
            {
                signature = await module!.InvokeAsync<Signature>("SaveCanvas", canvasId);
                StateHasChanged();
                await OnSavedSignature.InvokeAsync(signature);
            }
            else
            {
                Console.WriteLine("il canvas è vuoto");
            }
            onSaving = false;
        }

        /// <summary>
        /// Metodo che richiama javascript per controllare se il canvas è vuoto
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsCanvasBlank()
        {
            bool empty = true;
            if (module is not null)
            {
                empty = await module.InvokeAsync<bool>("isCanvasBlank", canvasId);
            }
            return empty;
        }

    }
}
