using ConstructionSiteLibrary.Interfaces;
using Microsoft.Maui.Graphics.Platform;
using System.Reflection;
using IImage = Microsoft.Maui.Graphics.IImage;
using Microsoft.Maui.Graphics.Skia;
using Font = Microsoft.Maui.Graphics.Font;
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
                        IImage newImage = image.Downsize(1000, true); //TODO mettere 1000
                        int height = (int)newImage.Height;
                        int width = (int)newImage.Width;

                        //Aggiunta testo all'immagine
                        SkiaBitmapExportContext bmp = new(width: width, height: height, 1.0f);

                        // bmp.Image.Draw(bmp.Canvas.DrawImage(newImage, 0, 0, width: width, height, height));

                        // ICanvas imagecCanvas = bmp.Canvas;
                        // imagecCanvas.DrawImage(newImage, x: 0, y: 0, width: width, height: height);

                        // imagecCanvas.SetFillPaint(newImage.AsPaint(), RectF.Zero);
                        // imagecCanvas.FillRectangle(0, 0, width, height);

                        // imagecCanvas.FillRectangle(0, 0, width, height);

                        // Rect myImageRectangle = new(x:0, y:0, width: width, height: height);
                        // bmp.Image.Draw(imagecCanvas, myImageRectangle);

                        // PathF path = new PathF();

                        ICanvas writeCanvas = bmp.Canvas;

                        // Measure a string
                        string myText = "Hello, Maui.Graphics!";
                        Font myFont = new("Impact");
                        float myFontSize = 48;
                        writeCanvas.Font = myFont;
                        SizeF textSize = writeCanvas.GetStringSize(myText, myFont, myFontSize);

                        // Draw a rectangle to hold the string
                        Point point = new(
                            x: (bmp.Width - textSize.Width) / 2,
                            y: (bmp.Height - textSize.Height) / 2);
                        Rect myTextRectangle = new(point, textSize);

                        // Daw the string itself
                        writeCanvas.FontSize = myFontSize * .9f; // smaller than the rectangle
                        writeCanvas.FontColor = Colors.White;
                        writeCanvas.DrawString(myText, myTextRectangle,
                            HorizontalAlignment.Center, VerticalAlignment.Center, TextFlow.OverflowBounds);

                        // bmp.WriteToFile(localFilePath);
                        // Stream bmpStream = Stream.Null;
                        // bmp.WriteToStream(bmpStream);
                        // bmp.Image.AsBase64

                        // IImage writtenImage;
                        // writtenImage = PlatformImage.FromStream(bmpStream);
                        // var PhotoPath = string.Format("data:image/png;base64,{0}", newImage.AsBase64());

                        var PhotoPath = string.Format("data:image/png;base64,{0}", bmp.Image.AsBase64());

                        return PhotoPath;
                    }
                }
            }
            return "";
        }
    }
}
