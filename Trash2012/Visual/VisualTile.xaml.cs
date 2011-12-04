using System;
using System.Drawing;
using Trash2012.Model;
using Trash2012.Engine;
using Size = System.Windows.Size;

namespace Trash2012.Visual
{
    /// <summary>
    /// Interaction logic for Tile.xaml
    /// </summary>
    public partial class VisualTile
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool Visited { get; set; }
        public IMapTile ModelTile { get; private set; }

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
                bmp = Animations.FindResource(type);
            }
            else
            {
                throw new ArgumentException("First layer must be filled with a bitmap");
            }

            if (ImageAnimator.CanAnimate(bmp))
            {
                FirstLayer.StartAnimation(bmp, loop: true);
            }
            else
            {
                FirstLayer.Source = ImageManager.Bitmap2ImageSource(bmp);
            }

            FirstLayer.Width = dimension.Width;
            FirstLayer.Height = dimension.Height;
            SecondLayer.Width = dimension.Width;
            SecondLayer.Height = dimension.Height;
            ThirdLayer.Width = dimension.Width;
            ThirdLayer.Height = dimension.Height;
            FourthLayer.Width = dimension.Width;
            FourthLayer.Height = dimension.Height;

            Update();
        }

        public void Update()
        {
            if (ModelTile is IHouseTile)
            {
                var houseTile = ModelTile as IHouseTile;
                var bmpHouse = Animations.FindResource(houseTile.HouseType);
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
            Visited = true;
        }

    }
}
