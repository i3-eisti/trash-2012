using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace Trash2012.Engine
{
    public class ImageManager
    {
        public static Image Bitmap2Image(Bitmap bmp)
        {
            var dimension = new Rect(
                new Point(0.0, 0.0), 
                new Size(bmp.Width, bmp.Height));
            return Bitmap2Image(bmp, dimension);
        }

        public static Image Bitmap2Image(
            Bitmap bmp, 
            Rect dimension)
        {
            return new Image
            {
                Source = Bitmap2ImageSource(bmp),
                Width = dimension.Width,
                Height = dimension.Height,
                Stretch = Stretch.UniformToFill
            };
        }

        public static ImageSource Bitmap2ImageSource(Bitmap bmp)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }


        private ImageManager() {}
    }
}
