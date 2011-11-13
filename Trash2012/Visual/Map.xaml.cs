using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Trash2012.Model;

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
        private Dictionary<Image,IMapTile> GraphicTiles;

        public Map()
        {
            InitializeComponent();
            UserInitialization();
        }

        private void UserInitialization()
        {
            //Selection handler
            {
                GraphicTiles = new Dictionary<Image, IMapTile>();
                Canvas.SetZIndex(OuterBorder, -5);
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
                            // Supervise mouse event on every image
                            img.MouseDown += SelectTile_MouseDown;

                            //wrap it into a border container,
                            // to enable selection later
                            Border bd = new Border();
                            bd.BorderBrush = new SolidColorBrush(BORDER_COLOR);
                            bd.BorderThickness = new Thickness(BORDER_THICKNESS_UNACTIVATED);
                            bd.CornerRadius = new CornerRadius(BORDER_RADIUS_UNACTIVATED);
                            bd.Child = img;

                            //record every image
                            GraphicTiles.Add(img,MyCity.Map[i][j]);

                            Canvas.SetLeft(bd, Math.Ceiling(j * tileWidth) - j);
                            Canvas.SetTop(bd, Math.Ceiling(i * tileHeight) - i);
                            //Canvas.SetLeft(img, Math.Ceiling(j * tileWidth) - j);
                            //Canvas.SetTop(img, Math.Ceiling(i * tileHeight) - i);
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

        private List<Image> selectedImages = new List<Image>();
        private List<IMapTile> selectedMapTiles = new List<IMapTile>();
        public Travel MyTravel = new Travel();

        /// <summary>
        /// Last selected image
        /// </summary>
        public Image LastSelectedImage 
        {
            get
            {
                if (selectedImages.Count == 0)
                {
                    return null;
                }
                else
                {
                    return selectedImages.Last();
                }
            }
        }
        /// <summary>
        /// All selected images, ordered from the first selected to the last selected.
        /// INFO : Don't use internally
        /// </summary>
        public Stack<Image> SelectedImages 
        {
            get
            {
                Stack<Image> imgStack = new Stack<Image>(selectedImages);
                return imgStack;
            }
        }
        /// <summary>
        /// Last corresponding selected IMapTile
        /// </summary>
        public IMapTile LastSelectedMapTile
        {
            get
            {
                if (selectedMapTiles.Count == 0)
                {
                    return null;
                }
                else
                {
                    return selectedMapTiles.Last();
                }
            }
        }
        /// <summary>
        /// All corresponding selected map tiles, ordered from the first selected to the last selected
        /// INFO : Don't use internally
        /// </summary>
        public Stack<IMapTile> SelectedMapTiles
        {
            get
            {
                Stack<IMapTile> tileStack = new Stack<IMapTile>(selectedMapTiles);
                return tileStack;
            }
        }
        /// <summary>
        /// Fast checking is any image is selected on the map
        /// </summary>
        public Boolean IsAnyTileSelected 
        {
            get
            {
                return selectedImages.Count != 0;
            }
        }

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
        private const int BORDER_THICKNESS_ACTIVATED = 4;

        /// <summary>
        /// Image's Border radius when selection is activated
        /// </summary>
        private const int BORDER_RADIUS_ACTIVATED = 7;

        /// <summary>
        /// Check wheter an image is already selected or not
        /// </summary>
        /// <param name="imgTile">Image to check</param>
        /// <returns>if image is selected or not</returns>
        private bool IsImageSelected(Image imgTile)
        {
            return SelectedImages.Contains(imgTile);
        }

        /// <summary>
        /// Handle tile selection by mouse pressure
        /// </summary>
        /// <param name="sender">Image on which which mouse-clicked (love this verb !)</param>
        /// <param name="e">whatever bro</param>
        private void SelectTile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image selectedImage = (Image)sender;
            IMapTile selectedTile = GraphicTiles[selectedImage];
            Border imgContainer = (Border)selectedImage.Parent;

            //int height_pos = (int) (e.GetPosition(MapContainer).X / selectedImage.Height);
            //int width_pos = (int)(e.GetPosition(MapContainer).Y / selectedImage.Width);

            if (!IsImageSelected(selectedImage)) //not yet selected
            {
                if (MyTravel.Add(selectedTile))
                {
                    //Update properties
                    selectedImages.Add(selectedImage);
                    selectedMapTiles.Add(selectedTile);



                    imgContainer.BorderThickness = new Thickness(BORDER_THICKNESS_ACTIVATED);
                    imgContainer.CornerRadius = new CornerRadius(BORDER_RADIUS_ACTIVATED);


                    //make it stand out above
                    Canvas.SetLeft(imgContainer, Canvas.GetLeft(imgContainer) + BORDER_THICKNESS_UNACTIVATED - BORDER_THICKNESS_ACTIVATED);
                    Canvas.SetTop(imgContainer, Canvas.GetTop(imgContainer) + BORDER_THICKNESS_UNACTIVATED - BORDER_THICKNESS_ACTIVATED);
                    Canvas.SetZIndex(imgContainer, selectedImages.Count);
                }
            }
            else //already selected
            {
                selectedImages.Remove(selectedImage);
                selectedMapTiles.Remove(selectedTile);

                //clean previous effect
                imgContainer.BorderThickness = new Thickness(BORDER_THICKNESS_UNACTIVATED);
                imgContainer.CornerRadius = new CornerRadius(BORDER_RADIUS_UNACTIVATED);

                //put it down
                Canvas.SetLeft(imgContainer, Canvas.GetLeft(imgContainer) + BORDER_THICKNESS_ACTIVATED - BORDER_THICKNESS_UNACTIVATED);
                Canvas.SetTop(imgContainer, Canvas.GetTop(imgContainer) + BORDER_THICKNESS_ACTIVATED - BORDER_THICKNESS_UNACTIVATED);
                Canvas.SetZIndex(imgContainer, 0);
            }
        }

        #endregion
    }
}
