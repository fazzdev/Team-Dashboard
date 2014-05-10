using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TeamDashboard
{
    public static class ImageHelper
    {
        public static string ImageToBase64(Image image)
        {
            if (image == null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Png);
                var imageBytes = memoryStream.ToArray();

                return Convert.ToBase64String(imageBytes);
            }
        }

        public static Image Base64ToImage(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;

            var imageBytes = Convert.FromBase64String(base64String);
            using (var memoryStream = new MemoryStream(imageBytes))
            {
                return Image.FromStream(memoryStream);
            }
        }

        public static ImageSource ToImageSource(this Image image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}
