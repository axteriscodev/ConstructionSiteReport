namespace AppMAUI.Camera;

public partial class PlatformCamera : ContentPage
{
	public PlatformCamera()
	{
		InitializeComponent();
	}

	private void CameraView_CamerasLoaded(object sender, EventArgs e)
	{
		
		cameraView.Camera = cameraView.Cameras.First();
		cameraView.WidthRequest = DeviceDisplay.Current.MainDisplayInfo.Width; 
		cameraView.HeightRequest = DeviceDisplay.Current.MainDisplayInfo.Height;

        MainThread.BeginInvokeOnMainThread(async () => 
		{
			await cameraView.StartCameraAsync();
		});
	}
}