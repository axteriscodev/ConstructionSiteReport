using ConstructionSiteLibrary.Interfaces;
using Microsoft.Maui.Graphics.Platform;
using IImage = Microsoft.Maui.Graphics.IImage;
using Microsoft.Maui.Graphics.Skia;
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

                    var image = PlatformImage.FromStream(sourceStream);
                    //var skiaImage = SkiaImage.FromStream(sourceStream, ImageFormat.Png);
                    sourceStream.Dispose();
                    localFileStream.Dispose();

                    if (image != null)
                    {
                        image = image.Downsize(1000, true);
                        image.AsStream();
                        var skiaImage = SkiaImage.FromStream(image.AsStream(), ImageFormat.Png);

                        //Aggiunta testo all'immagine
                        SkiaBitmapExportContext bmp = new(width: (int)skiaImage.Width, height: (int)skiaImage.Height, 1.0f);

                        DateTime dt = DateTime.Now.ToLocalTime();
                        string dateString = dt.ToString("HH:mm - dd/MM/yyyy");

                        ICanvas canvas = bmp.Canvas;
                        bmp.Canvas.DrawImage(skiaImage, 0, 0, skiaImage.Width, skiaImage.Height);

                        Microsoft.Maui.Graphics.Font myFont = new("Impact");
                        float myFontSize = 30;
                        canvas.Font = myFont;
                        SizeF textSize = canvas.GetStringSize(dateString, myFont, myFontSize);
                        // Draw a rectangle to hold the string
                        Point point = new(
                            x: (bmp.Width - textSize.Width),
                            y: (bmp.Height - textSize.Height));
                        Rect myTextRectangle = new(point, textSize);
                        // Daw the string itself
                        canvas.FontSize = myFontSize * .9f; // smaller than the rectangle
                        canvas.FontColor = Colors.Fuchsia;
                        canvas.DrawString(dateString, myTextRectangle, HorizontalAlignment.Center, VerticalAlignment.Center, TextFlow.OverflowBounds);

                        bmp.Canvas.SaveState();
                        var temp = bmp.Image;



                        var PhotoPath = string.Format("data:image/png;base64,{0}", temp.AsBase64());

                        return PhotoPath;
                    }
                }
            }
            return "";
        }
    }
}
