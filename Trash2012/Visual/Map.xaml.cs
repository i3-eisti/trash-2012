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
using System.Windows.Media.Effects;

namespace Trash2012.Visual
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : UserControl
    {
        private City my_city;
        public City MyCity
        {
            get
            {
                return my_city;
            }
            set
            {
                my_city = value;
                SetCanvas();
            }
        }

        /// <summary>
        /// Every image of the component
        /// </summary>
        private List<Image> GraphicTiles;

        /// <summary>
        /// Selected Image
        /// </summary>
        public Image SelectedImage { get; private set; }
        /// <summary>
        /// Selected Tile
        /// </summary>
        public IMapTile SelectedMapTile { get; private set; }

        public Map()
        {
            InitializeComponent();
            UserInitialization();
        }

        private void UserInitialization()
        {
            GraphicTiles = new List<Image>();
            SelectedImage = null;
            SelectedMapTile = null;
        }

        public void SetCanvas()
        {
            //compute tile size
            double tileWidth = RootContainer.Width / MyCity.Width;
            double tileHeight = RootContainer.Height / MyCity.Height;
            //reset graphic tiles's array
            GraphicTiles.Clear();
            
            try
            {
                for (int i = MyCity.Height; i-- > 0; )
                {
                    for (int j = MyCity.Width; j-- > 0; )
                    {
                            Image img = new Image();
                            img.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                MyCity.Map[i][j].Tile.GetHbitmap(),
                                IntPtr.Zero,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());
                            img.Width = tileWidth;
                            img.Height = tileHeight;
                            img.Stretch = Stretch.Fill;
                            img.SetValue(Grid.ColumnProperty, j);
                            img.SetValue(Grid.RowProperty, i);
                            /// Supervise mouse event on every image
                            img.MouseDown += SelectTile_MouseDown;

                            //record every image
                            GraphicTiles.Add(img);

                            Canvas.SetLeft(img, Math.Ceiling(j * tileWidth));
                            Canvas.SetTop(img, Math.Ceiling(i * tileHeight) - i);
                            RootContainer.Children.Add(img);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when loading City map [converting ressource into image] : " + e.Message);
            }
        }

        private void SelectTile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //clean previous effect
            if (SelectedImage != null)
            {
                Canvas.SetZIndex(SelectedImage, 0);
                SelectedImage.Effect = null;
            }

            SelectedImage = (Image)sender;
            Console.WriteLine(SelectedImage + " selected, parent: " + SelectedImage.Parent + ", z-index: " + Canvas.GetZIndex(SelectedImage));

            //outer glow effect
            DropShadowEffect outerGlow = new DropShadowEffect();

            outerGlow.BlurRadius = SelectedImage.Width;

            Color red = Color.FromRgb((byte)255,0,50);
            outerGlow.Color = red;

            SelectedImage.Effect = outerGlow;
            Canvas.SetZIndex(SelectedImage, 1);
        }
    }
}
