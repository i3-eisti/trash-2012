using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Trash2012.Model;
using Trash2012.Engine;

namespace Trash2012.Visual
{
    /// <summary>
    /// Interaction logic for Tile.xaml
    /// </summary>
    public partial class Tile : UserControl
    {
        public int X { get; set; }
        public int Y { get; set; }
        public IMapTile Model;

        GifImage gif;

        public Tile(City MyCity, int i, int j, double tileWidth, double tileHeight)
        {
            X = i;
            Y = j;
            Model = MyCity.Map[i][j];

            InitializeComponent();
            img.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                Model.Tile.GetHbitmap(),
                                IntPtr.Zero,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());
            img.Width = tileWidth;
            img.Height = tileHeight;
            img.Stretch = Stretch.Fill;
            Canvas.SetZIndex(img, 1);

            gif = new GifImage(Properties.Resources.LeftRight, img.Width, img.Height);
            if (Model is IRoadTile)
            {
                IRoadTile road = (IRoadTile)Model;
                if (road.Type == RoadTile.RoadType.Horizontal)
                {
                    gif = new GifImage(Properties.Resources.LeftRight_long_, img.Width, img.Height);
                }
                if (road.Type == RoadTile.RoadType.BottomLeft)
                {
                    gif = new GifImage(Properties.Resources.LeftBottom_long_, img.Width, img.Height);
                }
                if (road.Type == RoadTile.RoadType.Vertical)
                {
                    gif = new GifImage(Properties.Resources.TopBottom_long_, img.Width, img.Height);
                }
                if (road.Type == RoadTile.RoadType.TopRight)
                {
                    gif = new GifImage(Properties.Resources.TopLeft_long_, img.Width, img.Height);
                }
                
            }

            Canvas.SetZIndex(gif, 2);
            TileCanvas.Children.Add(gif);

            
            

            this.Width = tileWidth;
            this.Height = tileHeight;
            
        }

        public void StartAnimate()
        {
            gif.StartAnimate();            
        }

        public void StopAnimate()
        {
            gif.StopAnimate();
        }
    }
}
