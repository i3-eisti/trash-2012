using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Trash2012.Engine;
using Trash2012.Model;
using System.Windows.Threading;
using System.Collections.Generic;

namespace Trash2012.Visual
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map
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

        public bool IsSelectionEnabled { get; set; }

        /// <summary>
        /// Every image of the component
        /// </summary>
        //private Dictionary<Tile, IMapTile> TilesVisualToModel;
        public VisualTile[][] TilesVisual;

        public MainWindow MyMainWindow;

        public int Position_X = 0;
        public int Position_Y = 0;
        public int MaxTiles = 5;
        public int Tile_Size = 50;

        public Map()
        {
            InitializeComponent();
            Canvas.SetZIndex(OuterBorder, -5);
            IsSelectionEnabled = true;
            this.MouseEnter += new MouseEventHandler(Map_MouseEnter);
            //this.MouseDown += new MouseEventHandler(Map_MouseEnter);
        }

        void Map_MouseEnter(object sender, MouseEventArgs e)
        {
            MyMainWindow.Focus();
        }

        private delegate void AnimationEventHandler(object[] args);

        public void Animate(Truck truck, Action animationEndCallback)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Send,
                                   new AnimationEventHandler(NextAnimation), 
                                   new object[] { truck, 0, Travel.Extremity.Top, animationEndCallback });
        }

        private void NextAnimation(object[] args)
        {
            var truck = args[0] as Truck;
            var pos = (int) args[1];
            var formerFrom = (Travel.Extremity) args[2];
            var animationEndCallback = args[3] as Action;

            var myTravel = truck.Travel;
            var posmax = truck.Travel.Count - 1; 
            if(posmax != 0 && pos == posmax)
            {
                //c : current, , n : next
                int cx = myTravel[pos].ModelTile.Position.X,
                    cy = myTravel[pos].ModelTile.Position.Y;

                var from =
                    formerFrom == Travel.Extremity.Right ? Travel.Extremity.Left :
                    formerFrom == Travel.Extremity.Left ? Travel.Extremity.Right :
                    formerFrom == Travel.Extremity.Top ? Travel.Extremity.Bottom :
                                                    Travel.Extremity.Top; ;
                var to =
                    cx == 0 ? Travel.Extremity.Left :
                    cx == my_city.Width ? Travel.Extremity.Right :
                    cy == 0 ? Travel.Extremity.Top :
                              Travel.Extremity.Bottom;

                var animationLayer = TilesVisual[cy][cx].FourthLayer;
                //cancel callback for next animation
                animationLayer.BeforeEndCallback = delegate {};
                //Stop animation at end
                animationLayer.EndCallback = delegate
                {
                    animationLayer.StopAnimation();
                    animationEndCallback();
                };

                //After all those computation, start animation
                animationLayer.StartAnimation(
                    Animations.FindNextFrom(myTravel[pos].ModelTile, from));
                
                myTravel[pos].Update();

                truck.Travel = new Travel();
            }
            else if(pos == 0)
            {
                //c : current, , n : next
                int cx = myTravel[pos].ModelTile.Position.X,
                    cy = myTravel[pos].ModelTile.Position.Y,
                    nx = myTravel[pos + 1].ModelTile.Position.X,
                    ny = myTravel[pos + 1].ModelTile.Position.Y;

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

                var animationLayer = TilesVisual[cy][cx].FourthLayer;

                //Slightly before animation's end start the next one
                animationLayer.BeforeEndCallback =
                    () => Dispatcher.BeginInvoke(DispatcherPriority.Send,
                                                new AnimationEventHandler(NextAnimation), 
                                                new object[]{truck, pos + 1, to, animationEndCallback});
                //Stop animation at end
                animationLayer.EndCallback =
                    animationLayer.StopAnimation;

                //After all those computation, start animation
                animationLayer.StartAnimation(
                    Animations.FindNextTo(myTravel[pos].ModelTile, to));
                myTravel[pos].Update();
            }
            else if(pos < posmax)
            {
                //c : current, , n : next
                int cx = myTravel[pos].ModelTile.Position.X,
                    cy = myTravel[pos].ModelTile.Position.Y,
                    nx = myTravel[pos + 1].ModelTile.Position.X,
                    ny = myTravel[pos + 1].ModelTile.Position.Y;

                var from = 
                    formerFrom == Travel.Extremity.Right ? Travel.Extremity.Left :
                    formerFrom == Travel.Extremity.Left ? Travel.Extremity.Right :
                    formerFrom == Travel.Extremity.Top ? Travel.Extremity.Bottom :
                                                    Travel.Extremity.Top;
                var to =
                    cx < nx ? Travel.Extremity.Right :
                    cx > nx ? Travel.Extremity.Left :
                    cy < ny ? Travel.Extremity.Bottom :
                              Travel.Extremity.Top;

                var animationLayer = TilesVisual[cy][cx].FourthLayer;

                //Slightly before animation's end start the next one
                animationLayer.BeforeEndCallback = 
                    () => Dispatcher.BeginInvoke(DispatcherPriority.Send,
                                                new AnimationEventHandler(NextAnimation),
                                                new object[] { truck, pos + 1, to, animationEndCallback });
                //Stop animation at end
                animationLayer.EndCallback =
                    animationLayer.StopAnimation;

                //After all those computation, start animation
                animationLayer.StartAnimation(
                    Animations.FindNext(from, to));
                myTravel[pos].Update();
            }
        }

        public void SetCanvas()
        {
            //compute tile size
            double tileWidth = 
                (Width - 
                    OuterBorder.BorderThickness.Left -
                    OuterBorder.BorderThickness.Right + MyCity.Width) / MyCity.Width;
            double tileHeight = 
                (Height - 
                    OuterBorder.BorderThickness.Top -
                    OuterBorder.BorderThickness.Bottom + MyCity.Height) / MyCity.Height;


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

                        Canvas.SetLeft(border, (Math.Ceiling(j * tileWidth) - j));
                        Canvas.SetTop(border, (Math.Ceiling(i * tileHeight) - i));
                        MapContainer.Children.Add(border);
                        
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when loading City map [converting ressource into image] : " + e.Message);
            }
        }

        public void UpdateCanvas()
        {
            Tile_Size = (ActualWidth > 0) ? (int)((ActualWidth - 5) / MaxTiles) : 0;

            try
            {
                for (var i = MyCity.Height; i-- > 0; )
                {
                    for (var j = MyCity.Width; j-- > 0; )
                    {
                        var tile = TilesVisual[i][j];
                        tile.Update(Tile_Size, Tile_Size);
                        var border = (Border) tile.Parent;

                        
                        bool add = true;
                        if ((j * Tile_Size - j) + Position_X * Tile_Size < -j
                            || (j * Tile_Size - j) + Position_X * Tile_Size > this.ActualWidth - Tile_Size - j
                            || (i * Tile_Size - i) + Position_Y * Tile_Size < -i
                            || (i * Tile_Size - i) + Position_Y * Tile_Size > this.ActualHeight - Tile_Size - i)
                        {
                            add = false;
                        }
                        if (add)
                        {
                            Canvas.SetLeft(border, j * (Tile_Size - 1) + Position_X * Tile_Size);
                            Canvas.SetTop(border, i * (Tile_Size - 1) + Position_Y * Tile_Size);
                            border.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            Canvas.SetLeft(border, (j * Tile_Size - j) + Position_X * Tile_Size);
                            Canvas.SetTop(border, (i * Tile_Size - i) + Position_Y * Tile_Size);
                            border.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when loading City map [converting ressource into image] : " + e.Message);
            }
        }

        public void UnUpdateCanvas()
        {
            double tileWidth =
                (Width -
                    OuterBorder.BorderThickness.Left -
                    OuterBorder.BorderThickness.Right + MyCity.Width) / MyCity.Width;
            double tileHeight =
                (Height -
                    OuterBorder.BorderThickness.Top -
                    OuterBorder.BorderThickness.Bottom + MyCity.Height) / MyCity.Height;

            try
            {
                for (var i = MyCity.Height; i-- > 0; )
                {
                    for (var j = MyCity.Width; j-- > 0; )
                    {
                        var tile = TilesVisual[i][j];
                        tile.Update(tileWidth, tileHeight);
                        var border = (Border)tile.Parent;


                        //bool add = true;
                        //if ((j * Tile_Size - j) + Position_X * Tile_Size < -j
                        //    || (j * Tile_Size - j) + Position_X * Tile_Size > this.ActualWidth - Tile_Size - j
                        //    || (i * Tile_Size - i) + Position_Y * Tile_Size < -i
                        //    || (i * Tile_Size - i) + Position_Y * Tile_Size > this.ActualHeight - Tile_Size - i)
                        //{
                        //    add = false;
                        //}
                        //if (add)
                        //{
                        Canvas.SetLeft(border, (Math.Ceiling(j * tileWidth) - j));
                        Canvas.SetTop(border, (Math.Ceiling(i * tileHeight) - i));
                        border.Visibility = System.Windows.Visibility.Visible;
                        //}
                        //else
                        //{
                        //    Canvas.SetLeft(border, (j * Tile_Size - j) + Position_X * Tile_Size);
                        //    Canvas.SetTop(border, (i * Tile_Size - i) + Position_Y * Tile_Size);
                        //    border.Visibility = System.Windows.Visibility.Collapsed;
                        //}


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
            if (!IsSelectionEnabled) return;

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
