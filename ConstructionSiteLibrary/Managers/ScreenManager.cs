using ConstructionSiteLibrary.Model;
using Microsoft.JSInterop;

namespace ConstructionSiteLibrary.Managers
{
    public class ScreenManager
    {
        private IJSRuntime? JS;
        public event EventHandler<ScreenSize>? Resize;
        private ScreenSize? dimension;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="js"></param>
        public async Task Init(IJSRuntime js)
        {
            try
            {
                if (JS == null)
                {
                    JS = js;
                    var jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/ConstructionSiteLibrary/screenService.js");
                    //setto la dimensione corrente dello schermo
                    dimension = await jsModule.InvokeAsync<ScreenSize>("getWindowSize");
                    //invoco la funzione js per rimanere in ascolto del ridimensionamento dello schermo
                    await jsModule.InvokeAsync<string>("resizeListener", DotNetObjectReference.Create(this));
                }
            }catch(Exception) { }
        }

        public ScreenSize? GetScreenSize()
        {
            return dimension;
        }

        /// <summary>
        /// Metodo invocato da js quando viene modificata la dimensione dello schermo, 
        /// setta i nuovi valori di altezza e larghezza e poi tramite evento propaga la nuova
        /// dimensione a chi è in ascolto
        /// </summary>
        /// <param name="jsBrowserWidth"></param>
        /// <param name="jsBrowserHeight"></param>
        [JSInvokable]
        public void SetScreenDimensions(int jsBrowserWidth, int jsBrowserHeight)
        {
            dimension!.Width = jsBrowserWidth;
            dimension!.Height = jsBrowserHeight;
            Resize?.Invoke(this, dimension);
        }
    }
}
