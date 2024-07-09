using ConstructionSiteLibrary.Components.Choices;
using ConstructionSiteLibrary.Interfaces;
using Radzen;


namespace ClientWebApp.Services
{
    public class CameraService(DialogService dialogService) : ICameraService
    {

        private DialogService _dialogService = dialogService;

        public async Task<string> OpenCamera()
        {
            await _dialogService.Confirm("", "titolo",
            new ConfirmOptions { OkButtonText = "Si", CancelButtonText = "No" });
            return "";
        }
    }
}
