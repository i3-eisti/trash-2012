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
    public partial class Tile : UserControl
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Image FirstLayer { get; set; }
        public GifImage SecondLayer { get; set; }
        public GifImage ThirdLayer { get; set; }

        public Tile(
            IMapTile modelTile, 
            int i, int j, 
            double tileWidth, double tileHeight)
        {
            InitializeComponent();
            var dimension = new Size(tileWidth, tileHeight);

            X = i;
            Y = j;
            Width = dimension.Width;
            Height = dimension.Height;

            Bitmap bmp;

            if (modelTile is IBackgroundTile)
            {
                var type = ((IBackgroundTile)modelTile).Type;
                bmp = StaticTileLayer.SelectBitmap(type);
            }
            else if (modelTile is IRoadTile)
            {
                var type = ((IRoadTile)modelTile).Type;
                bmp = StaticTileLayer.SelectBitmap(type);
            }
            else
            {
                throw new ArgumentException("First layer must be filled with a bitmap");
            }
            FirstLayer = new StaticTileLayer(bmp, dimension);
            SecondLayer = new GifImage()
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
            //Canvas.SetZIndex(ThirdLayer,  -3);

            TileCanvas.Children.Add(SecondLayer);
            TileCanvas.Children.Add(FirstLayer);
            //TileCanvas.Children.Add(ThirdLayer);
        }

        public class StaticTileLayer : Image
        {
            public StaticTileLayer(Bitmap bmp, Size dimension) : base()
            {
                Source = ImageManager.Bitmap2ImageSource(bmp);
                Width = dimension.Width;
                Height = dimension.Height;
                Stretch = Stretch.UniformToFill;
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

            public static Bitmap SelectBitmap(BackgroundTile.BackgroundType type)
            {
                switch (type)
                {
                    case BackgroundTile.BackgroundType.Plain:
                        return Properties.Resources.TilePlain;
                    default:
                        throw new ArgumentException("Unhandled animation bitmap tile: " + type);
                }
            }
        }

    }
}
