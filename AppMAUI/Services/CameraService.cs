using ConstructionSiteLibrary.Interfaces;
using Microsoft.Maui.Graphics.Platform;
using System.Reflection;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace AppMAUI.Services
{
    public class CameraService : ICameraService
    {

        public async Task<string> OpenCamera()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                    using Stream sourceStream = await photo.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                    IImage image;
                    Assembly assembly = GetType().GetTypeInfo().Assembly;
                    image = PlatformImage.FromStream(sourceStream);
                    sourceStream.Dispose();
                    localFileStream.Dispose();

                    if (image != null)
                    {
                        IImage newImage = image.Downsize(1000, true);
                        var PhotoPath = string.Format("data:image/png;base64,{0}", newImage.AsBase64());

                        return PhotoPath;
                    }
                }
            }
            return "";
        }
    }
}
