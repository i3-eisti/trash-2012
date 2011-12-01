using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Trash2012.Engine;
using Trash2012.Model;
using System.Windows.Threading;

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
        public VisualTile[][] TilesVisual;

        public MainWindow MyMainWindow;

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
        }

        private Travel.Extremity _formerFrom;
        int _pos;
        int _posmax;

        private delegate void AnimationEventHandler();

        public void Animate()
        {
            var truck = ((TruckButton)MyMainWindow.MyAssets.MyListView.SelectedItem).MyTruck;
            _pos = 0;
            _posmax = truck.Travel.Count - 1;
            Dispatcher.BeginInvoke(DispatcherPriority.Send,
                                   new AnimationEventHandler(NextAnimation));
        }

        private void NextAnimation()
        {
            var truck = ((TruckButton)MyMainWindow.MyAssets.MyListView.SelectedItem).MyTruck;
            var myTravel = truck.Travel;
            if(_pos == _posmax)
            {
                //c : current, , n : next
                int cx = myTravel[_pos].ModelTile.Position.X,
                    cy = myTravel[_pos].ModelTile.Position.Y;

                var from =
                    _formerFrom == Travel.Extremity.Right ? Travel.Extremity.Left :
                    _formerFrom == Travel.Extremity.Left ? Travel.Extremity.Right :
                    _formerFrom == Travel.Extremity.Top ? Travel.Extremity.Bottom :
                                                    Travel.Extremity.Top; ;
                var to =
                    cx == 0 ? Travel.Extremity.Left :
                    cx == my_city.Width ? Travel.Extremity.Right :
                    cy == 0 ? Travel.Extremity.Top :
                              Travel.Extremity.Bottom;

                var animationLayer = TilesVisual[cy][cx].SecondLayer;
                //cancel callback for next animation
                animationLayer.BeforeEndCallback =
                    () => Dispatcher.BeginInvoke(DispatcherPriority.Send,
                                                new AnimationEventHandler(NextAnimation));
                //Stop animation at end
                animationLayer.EndCallback =
                    animationLayer.StopAnimation;

                _pos++;

                //After all those computation, start animation
                animationLayer.StartAnimation(
                    Animations.FindNext(from, to));
                myTravel[_pos].Update();

                truck.Travel = new Travel();
            }
            else if(_pos == 0)
            {
                //c : current, , n : next
                int cx = myTravel[_pos].ModelTile.Position.X,
                    cy = myTravel[_pos].ModelTile.Position.Y,
                    nx = myTravel[_pos + 1].ModelTile.Position.X,
                    ny = myTravel[_pos + 1].ModelTile.Position.Y;

                var from =
                    cx == 0 ? Travel.Extremity.Left :
                    cx == my_city.Width ? Travel.Extremity.Right :
                    cy == 0 ? Travel.Extremity.Top :
                              Travel.Extremity.Bottom;

                var to =
                    cx < nx ? Travel.Extremity.Right :
                    cx > nx ? Travel.Extremity.Left :
                    cy < ny ? Travel.Extremity.Bottom :
                              Travel.Extremity.Top;

                var animationLayer = TilesVisual[cy][cx].SecondLayer;

                //Slightly before animation's end start the next one
                animationLayer.BeforeEndCallback =
                    () => Dispatcher.BeginInvoke(DispatcherPriority.Send,
                                                new AnimationEventHandler(NextAnimation));
                //Stop animation at end
                animationLayer.EndCallback =
                    animationLayer.StopAnimation;

                //remember direction
                _formerFrom = to;
                //increment travel idx
                _pos++;

                //After all those computation, start animation
                animationLayer.StartAnimation(
                    Animations.FindNext(from, to));
                myTravel[_pos].Update();
            }
            else if(_pos < _posmax)
            {
                //c : current, , n : next
                int cx = myTravel[_pos].ModelTile.Position.X,
                    cy = myTravel[_pos].ModelTile.Position.Y,
                    nx = myTravel[_pos + 1].ModelTile.Position.X,
                    ny = myTravel[_pos + 1].ModelTile.Position.Y;

                var from = 
                    _formerFrom == Travel.Extremity.Right ? Travel.Extremity.Left :
                    _formerFrom == Travel.Extremity.Left ? Travel.Extremity.Right :
                    _formerFrom == Travel.Extremity.Top ? Travel.Extremity.Bottom :
                                                    Travel.Extremity.Top;
                var to =
                    cx < nx ? Travel.Extremity.Right :
                    cx > nx ? Travel.Extremity.Left :
                    cy < ny ? Travel.Extremity.Bottom :
                              Travel.Extremity.Top;

                var animationLayer = TilesVisual[cy][cx].SecondLayer;

                //Slightly before animation's end start the next one
                animationLayer.BeforeEndCallback = 
                    () => Dispatcher.BeginInvoke(DispatcherPriority.Send,
                                                new AnimationEventHandler(NextAnimation));
                //Stop animation at end
                animationLayer.EndCallback =
                    animationLayer.StopAnimation;

                //remember direction
                _formerFrom = to;
                //increment travel idx
                _pos++;

                //After all those computation, start animation
                animationLayer.StartAnimation(
                    Animations.FindNext(from, to));
                myTravel[_pos].Update();
            }
        }

        public void SetCanvas()
        {
            //compute tile size
            double tileWidth = 
                (Width - 
                    OuterBorder.BorderThickness.Left -
                    OuterBorder.BorderThickness.Right ) / MyCity.Width;
            double tileHeight = 
                (Height - 
                    OuterBorder.BorderThickness.Top -
                    OuterBorder.BorderThickness.Bottom) / MyCity.Height;

            //reset graphic tiles's array
            TilesVisual = new VisualTile[MyCity.Height][];
            
            try
            {
                for (var i = MyCity.Height; i-- > 0; )
                {
                    TilesVisual[i] = new VisualTile[MyCity.Width];
                    for (var j = MyCity.Width; j-- > 0; )
                    {
                        var tile = new VisualTile(MyCity.Map[i][j], i, j, tileWidth, tileHeight);
                        tile.MouseDown += SelectTile_MouseDown;
                        TilesVisual[i][j] = tile;

                        var border = new Border
                        {
                            BorderBrush = new SolidColorBrush(BORDER_COLOR),
                            BorderThickness = new Thickness(BORDER_THICKNESS_UNACTIVATED),
                            CornerRadius = new CornerRadius(BORDER_RADIUS_UNACTIVATED),
                            Child = tile
                        };
                        Canvas.SetLeft(border, Math.Ceiling(j * tileWidth) - j);
                        Canvas.SetTop(border, Math.Ceiling(i * tileHeight) - i);

                        MapContainer.Children.Add(border);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when loading City map [converting ressource into image] : " + e.Message);
            }
        }

        #region TileSelectionHandler

        //public Travel MyTravel  = new Travel();

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
            var truckButton = MyMainWindow.MyAssets.MyListView.SelectedItem as TruckButton;
            if (truckButton == null) return;
            Truck MyTruck = truckButton.MyTruck;

            VisualTile selectedVisualTile = (VisualTile)sender;
            IMapTile selectedTile = MyCity.Map[selectedVisualTile.X][selectedVisualTile.Y];
            Border imgContainer = (Border)selectedVisualTile.Parent;

            if (!MyTruck.Travel.Contains(selectedVisualTile.ModelTile))
            {
                if (MyTruck.Travel.Add(selectedVisualTile))
                {
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
                if (MyTruck.Travel.Remove(selectedTile))
                {
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

        public void SetTravel(Travel MyTravel)
        {
            ClearTravel();
            for(int i=0; i< MyTravel.Count; i++)
            {
                int X = MyTravel.Get(i).ModelTile.Position.X;
                int Y = MyTravel.Get(i).ModelTile.Position.Y;
                Border imgContainer = (Border)TilesVisual[Y][X].Parent;
                imgContainer.BorderThickness = new Thickness(BORDER_THICKNESS_ACTIVATED);
                imgContainer.CornerRadius = new CornerRadius(BORDER_RADIUS_ACTIVATED);

                //make it stand out above
                Canvas.SetLeft(imgContainer, Canvas.GetLeft(imgContainer) + BORDER_THICKNESS_UNACTIVATED - BORDER_THICKNESS_ACTIVATED);
                Canvas.SetTop(imgContainer, Canvas.GetTop(imgContainer) + BORDER_THICKNESS_UNACTIVATED - BORDER_THICKNESS_ACTIVATED);
                Canvas.SetZIndex(imgContainer, 1);
            }
        }

        public void ClearTravel()
        {
            for (int i = MyCity.Height; i-- > 0; )
            {
                for (int j = MyCity.Width; j-- > 0; )
                {
                    Border imgContainer = (Border)TilesVisual[i][j].Parent;
                    if (Canvas.GetZIndex(imgContainer) == 1) // Tile is selected
                    {
                        imgContainer.BorderThickness = new Thickness(BORDER_THICKNESS_UNACTIVATED);
                        imgContainer.CornerRadius = new CornerRadius(BORDER_RADIUS_UNACTIVATED);

                        //put it down
                        Canvas.SetLeft(imgContainer,
                                       Canvas.GetLeft(imgContainer) + BORDER_THICKNESS_ACTIVATED -
                                       BORDER_THICKNESS_UNACTIVATED);
                        Canvas.SetTop(imgContainer,
                                      Canvas.GetTop(imgContainer) + BORDER_THICKNESS_ACTIVATED -
                                      BORDER_THICKNESS_UNACTIVATED);
                        Canvas.SetZIndex(imgContainer, 0);
                    }
                }
            }
        }
    }
}
