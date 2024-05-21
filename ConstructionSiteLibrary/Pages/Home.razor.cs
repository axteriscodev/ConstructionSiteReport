using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Interfaces;
using ConstructionSiteLibrary.Model;

using Microsoft.AspNetCore.Components.Routing;
using System.Drawing;

namespace ConstructionSiteLibrary.Pages
{
    public partial class Home
    {

        ScreenDimension? dim;
        ScreenComponent? screen;

        string PhotoPath = "";

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
            Console.WriteLine("x: " + size.Width + " -  y: " + size.Height);
        }

        private async void OpenCameraPage()
        {
           PhotoPath = await CameraService.OpenCamera();
        
            StateHasChanged();
        }
    }
}
