using ConstructionSiteLibrary.Components.Utilities;
using ConstructionSiteLibrary.Model;


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
            if(size is not null)
            {
                Console.WriteLine("x: " + size.Width + " -  y: " + size.Height);
            }
        }

        private async void OpenCameraPage()
        {
           PhotoPath = await CameraService.OpenCamera();
        
            StateHasChanged();
        }


    }
}
