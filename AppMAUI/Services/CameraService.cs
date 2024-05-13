using AppMAUI.Camera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionSiteLibrary.Interfaces;

namespace AppMAUI.Services
{
    public class CameraService : ICameraService
    {

        public async Task OpenCamera()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new PlatformCamera());
        }
    }
}
