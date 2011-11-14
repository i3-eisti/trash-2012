using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Trash2012.Model;
<<<<<<< HEAD
=======
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using System.Windows.Threading;
>>>>>>> FETCH_HEAD

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
        //private Dictionary<Tile, IMapTile> TilesVisualToModel;
        public Tile[][] TilesVisual;

        public Map()
        {
            InitializeComponent();
            UserInitialization();
        }

        private void UserInitialization()
        {
            //Selection handler
            {
                Canvas.SetZIndex(OuterBorder, -5);
            }
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        public DispatcherTimer timer;
        public int pos;
        public int posmax;

        public void Animate()
        {
            pos = 0;
            posmax = MyTravel.Count;
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (pos < posmax)
            {
                if (pos > 0)
                {
                    TilesVisual[MyTravel.Get(pos - 1).Position.Y][MyTravel.Get(pos - 1).Position.X].CleanAnimation();
                }
                TilesVisual[MyTravel.Get(pos).Position.Y][MyTravel.Get(pos).Position.X].Animate();
                pos++;
            }
            else
            {
                timer.Stop();
            }
        }

        public void SetCanvas()
        {
            //compute tile size
            double tileWidth = 
                (this.Width - 
                    OuterBorder.BorderThickness.Left -
                    OuterBorder.BorderThickness.Right ) / MyCity.Width;
            double tileHeight = 
                (this.Height - 
                    OuterBorder.BorderThickness.Top -
                    OuterBorder.BorderThickness.Bottom) / MyCity.Height;

            //reset graphic tiles's array
            TilesVisual = new Tile[MyCity.Height][];
            
            try
            {
                for (int i = MyCity.Height; i-- > 0; )
                {
                    TilesVisual[i] = new Tile[MyCity.Width];
                    for (int j = MyCity.Width; j-- > 0; )
                    {
                        Tile t = new Tile(MyCity, i, j, tileWidth, tileHeight);
                        t.img.MouseDown += SelectTile_MouseDown;
                        TilesVisual[i][j] = t;

                        Border bd = new Border();
                        bd.BorderBrush = new SolidColorBrush(BORDER_COLOR);
                        bd.BorderThickness = new Thickness(BORDER_THICKNESS_UNACTIVATED);
                        bd.CornerRadius = new CornerRadius(BORDER_RADIUS_UNACTIVATED);
                        bd.Child = t;
                        Canvas.SetLeft(bd, Math.Ceiling(j * tileWidth) - j);
                        Canvas.SetTop(bd, Math.Ceiling(i * tileHeight) - i);

                        MapContainer.Children.Add(bd);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when loading City map [converting ressource into image] : " + e.Message);
            }
        }

        #region TileSelectionHandler

        public Travel MyTravel  = new Travel();

        #region DO NOT LOOK

        private static readonly Color BORDER_COLOR = Colors.Black;

        /// <summary>
        /// Image's Border thickness when selection is not activated
        /// </summary>
        private const int BORDER_THICKNESS_UNACTIVATED = 0;

        /// <summary>
        /// Image's Border radius when selection is not activated
        /// </summary>
        private const int BORDER_RADIUS_UNACTIVATED = 0;

        /// <summary>
        /// Image's Border thickness when selection is activated
        /// </summary>
        private const int BORDER_THICKNESS_ACTIVATED = 1;
        /// <summary>
        /// Image's Border radius when selection is activated
        /// </summary>
        private const int BORDER_RADIUS_ACTIVATED = 7;

        

        #endregion

        /// <summary>
        /// Handle tile selection by mouse pressure
        /// </summary>
        /// <param name="sender">Image on which which mouse-clicked (love this verb !)</param>
        /// <param name="e">whatever bro</param>
        private void SelectTile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Tile selectedVisualTile = 
                (Tile)(
                    (
                        (Canvas)(
                            (
                                (Image)sender
                            ).Parent
                        )
                    ).Parent
                );

            IMapTile selectedTile = MyCity.Map[selectedVisualTile.X][selectedVisualTile.Y];
            Border imgContainer = (Border)selectedVisualTile.Parent;

            if (!MyTravel.Contains(selectedTile))
            {
                if (MyTravel.Add(selectedTile))
                {

                    //selectedVisualTile.Animate();
                    imgContainer.BorderThickness = new Thickness(BORDER_THICKNESS_ACTIVATED);
                    imgContainer.CornerRadius = new CornerRadius(BORDER_RADIUS_ACTIVATED);


                    //make it stand out above
                    Canvas.SetLeft(imgContainer, Canvas.GetLeft(imgContainer) + BORDER_THICKNESS_UNACTIVATED - BORDER_THICKNESS_ACTIVATED);
                    Canvas.SetTop(imgContainer, Canvas.GetTop(imgContainer) + BORDER_THICKNESS_UNACTIVATED - BORDER_THICKNESS_ACTIVATED);
                    Canvas.SetZIndex(imgContainer, 1);
                }
            }
            else //already selected
            {
                if (MyTravel.Remove(selectedTile))
                {
                    //selectedVisualTile.CleanAnimation();

                    //clean previous effect
                    imgContainer.BorderThickness = new Thickness(BORDER_THICKNESS_UNACTIVATED);
                    imgContainer.CornerRadius = new CornerRadius(BORDER_RADIUS_UNACTIVATED);

                    //put it down
                    Canvas.SetLeft(imgContainer, Canvas.GetLeft(imgContainer) + BORDER_THICKNESS_ACTIVATED - BORDER_THICKNESS_UNACTIVATED);
                    Canvas.SetTop(imgContainer, Canvas.GetTop(imgContainer) + BORDER_THICKNESS_ACTIVATED - BORDER_THICKNESS_UNACTIVATED);
                    Canvas.SetZIndex(imgContainer, 0);
                }
            }
        }

        #endregion
    }
}
