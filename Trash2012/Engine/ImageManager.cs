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

        public static Bitmap Rotate(Bitmap b, float angle)
        {
            //create a new empty bitmap to hold rotated image
            var returnBitmap = new Bitmap(b.Width, b.Height);
            //make a graphics object from the empty bitmap
            var g = Graphics.FromImage(returnBitmap);
            //move rotation point to center of image
            g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
            //rotate
            g.RotateTransform(angle);
            //move image back
            g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
            //draw passed in image onto graphics object
            g.DrawImage(b, new System.Drawing.Point(0, 0));
            return returnBitmap;
        }


        private ImageManager() {}
    }
}
