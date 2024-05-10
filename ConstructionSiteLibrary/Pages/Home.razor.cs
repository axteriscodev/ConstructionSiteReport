using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Model;
using System.Drawing;

namespace ConstructionSiteLibrary.Pages
{
    public partial class Home
    {

        ScreenDimension? dim;
        ScreenComponent? screen;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private void ScreenDimensionChanged(ScreenDimension? dimension)
        {
            dim = dimension;
            Console.WriteLine(dimension.ToString());
        }

        private void ScreenSizeChanged(ScreenSize? size)
        {
           Console.WriteLine("x: " + size.Width + " -  y: " + size.Height );
        }
    }
}
