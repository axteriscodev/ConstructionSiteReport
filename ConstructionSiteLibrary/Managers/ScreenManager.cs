using ConstructionSiteLibrary.Model;
using Microsoft.JSInterop;

namespace ConstructionSiteLibrary.Managers
{
    public class ScreenManager
    {

        public event EventHandler<ScreenSize>? ResizeScreen;
        public event EventHandler<ScreenSize>? ResizeMain;
        private int navbarSize = 0;
        private IJSRuntime? JS;
        private ScreenSize? screenSize;
        private ScreenSize? mainSize;


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
                    screenSize = await jsModule.InvokeAsync<ScreenSize>("getWindowSize");
                    mainSize = new() { Height = screenSize.Height, Width = screenSize.Width - navbarSize };
                    
                    //invoco la funzione js per rimanere in ascolto del ridimensionamento dello schermo
                    await jsModule.InvokeAsync<string>("resizeListener", DotNetObjectReference.Create(this));
                }
            }catch(Exception) { }
        }

        public ScreenSize? GetScreenSize()
        {
            return screenSize;
        }

        /// <summary>
        /// Metodo che restituisce la grandezza della schermata togliendo la larghezza della navbar
        /// </summary>
        /// <returns></returns>
        public ScreenSize? GetMainSize()
        {
            return mainSize;
        }

        public void SetNavbarWidth(int width)
        {
            navbarSize = width;
            mainSize.Width = screenSize.Width - navbarSize;
            ResizeMain?.Invoke(this, mainSize);
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
            screenSize!.Width = jsBrowserWidth;
            screenSize!.Height = jsBrowserHeight;
            mainSize!.Width = jsBrowserWidth - navbarSize;
            mainSize!.Height = jsBrowserHeight;
            ResizeScreen?.Invoke(this, screenSize);
            ResizeMain?.Invoke(this, mainSize); 
        }
    }
}
