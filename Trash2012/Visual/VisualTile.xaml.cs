using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using Trash2012.Model;
using Trash2012.Engine;
using Image = System.Windows.Controls.Image;
using Size = System.Windows.Size;

namespace Trash2012.Visual
{
    /// <summary>
    /// Interaction logic for Tile.xaml
    /// </summary>
    public partial class VisualTile : UserControl
    {
        public int X { get; set; }
        public int Y { get; set; }

        public IMapTile ModelTile { get; private set; }

        public GifImage FirstLayer { get; set; }
        public GifImage SecondLayer { get; set; }
        public GifImage ThirdLayer { get; set; }

        public VisualTile(
            IMapTile modelTile, 
            int i, int j, 
            double tileWidth, double tileHeight)
        {
            InitializeComponent();
            var dimension = new Size(tileWidth, tileHeight);

            X = i;
            Y = j;
            ModelTile = modelTile;
            Width = dimension.Width;
            Height = dimension.Height;

            Bitmap bmp;
            
            if (ModelTile is IBackgroundTile)
            {
                var type = ((IBackgroundTile)ModelTile).Type;
                bmp = Animations.FindResource(type);
            }
            else if (ModelTile is IRoadTile)
            {
                var type = ((IRoadTile)ModelTile).Type;
                bmp = SelectBitmap(type);
            }
            else
            {
                throw new ArgumentException("First layer must be filled with a bitmap");
            }

            FirstLayer = new GifImage
            {
                Width = dimension.Width,
                Height = dimension.Height,
                Stretch = Stretch.UniformToFill
            }; 
            if (ImageAnimator.CanAnimate(bmp))
            {
                FirstLayer.StartAnimation(bmp, loop: true);
            }
            else
            {
                FirstLayer.Source = ImageManager.Bitmap2ImageSource(bmp);
            }

            SecondLayer = new GifImage
            {
                Width = dimension.Width,
                Height = dimension.Height,
                Stretch = Stretch.UniformToFill
            };
            ThirdLayer = new GifImage
            {
                Width = dimension.Width, 
                Height = dimension.Height,
                Stretch = Stretch.UniformToFill
            };

            Canvas.SetZIndex(FirstLayer, 10);
            Canvas.SetZIndex(SecondLayer, 40);
            Canvas.SetZIndex(ThirdLayer,  70);

            TileCanvas.Children.Add(SecondLayer);
            TileCanvas.Children.Add(FirstLayer);
            TileCanvas.Children.Add(ThirdLayer);

            Update();
        }

        public void Update()
        {
            if (ModelTile is IHouseTile)
            {
                var houseTile = ModelTile as IHouseTile;
                var bmpHouse = SelectBitmap(houseTile.HouseType);
                if (ImageAnimator.CanAnimate(bmpHouse))
                {
                    //Here put some code for animated background
                }
                else
                {
                    SecondLayer.Source = ImageManager.Bitmap2ImageSource(bmpHouse);
                }

                var bmpGarbage = houseTile.Garbage.Amount > 0
                    ? Properties.Resources.GarbageFull
                    : Properties.Resources.GarbageEmpty;
                if (ImageAnimator.CanAnimate(bmpGarbage))
                {
                    //Here put some code for animated background
                }
                else
                {
                    ThirdLayer.Source = ImageManager.Bitmap2ImageSource(bmpGarbage);
                }
            }
        }

        public static Bitmap SelectBitmap(RoadTile.RoadType type)
        {
            switch (type)
            {
                case RoadTile.RoadType.Horizontal:
                    return Properties.Resources.TileRoadHorizontal;
                case RoadTile.RoadType.Vertical:
                    return Properties.Resources.TileRoadVertical;
                case RoadTile.RoadType.TopLeft:
                    return Properties.Resources.TileRoadTopLeft;
                case RoadTile.RoadType.TopRight:
                    return Properties.Resources.TileRoadTopRight;
                case RoadTile.RoadType.BottomLeft:
                    return Properties.Resources.TileRoadBottomLeft;
                case RoadTile.RoadType.BottomRight:
                    return Properties.Resources.TileRoadBottomRight;
                case RoadTile.RoadType.TopBottomLeft:
                    return Properties.Resources.TileRoadTopBottomLeft;
                case RoadTile.RoadType.TopBottomRight:
                    return Properties.Resources.TileRoadTopBottomRight;
                case RoadTile.RoadType.TopLeftRight:
                    return Properties.Resources.TileRoadTopLeftRight;
                case RoadTile.RoadType.BottomLeftRight:
                    return Properties.Resources.TileRoadBottomLeftRight;
                default:
                    throw new ArgumentException("Unhandled animation bitmap tile: " + type);
            }
        }

        public static Bitmap SelectBitmap(HouseTile.THouse type)
        {
            switch (type)
            {
                case HouseTile.THouse.Normal:
                    return Properties.Resources.NormalHouse;
                default:
                    throw new ArgumentException("Unhandled animation bitmap tile: " + type);
            }
        }

    }
}
