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
            
            

            this.Width = tileWidth;
            this.Height = tileHeight;
            
        }

        public void Animate()
        {
            //Canvas.SetZIndex(truckimg, 1);
            //if (Model is IRoadTile)
            //{
            //    IRoadTile road = (IRoadTile) Model;
            //    if (road.Type == RoadTile.RoadType.Horizontal)
            //    {
            //        VisualStateManager.GoToState(this, "LeftRight", true);
            //    }
            //    if (road.Type == RoadTile.RoadType.BottomLeft)
            //    {
            //        VisualStateManager.GoToState(this, "LeftBottom", true);
            //    }
            //    if (road.Type == RoadTile.RoadType.TopLeft)
            //    {
            //        VisualStateManager.GoToState(this, "TopLeft", true);
            //    }
            //}
            //Canvas.SetZIndex(truckimg, 1);
            
        }

        public void CleanAnimation()
        {
            VisualStateManager.GoToState(this, "Base", true);
            truckimg.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
