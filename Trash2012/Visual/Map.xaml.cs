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
                InitDefinitions();
                SetGrid();
            }
        }

        public Map()
        {
            InitializeComponent();
        }

        private void InitDefinitions()
        {
            //ColumnDefinition defcol = new ColumnDefinition();
            //defcol.Width = new GridLength(30);
            for (int i = 0; i < MyCity.Width; i++)
            {
                //RootGrid.ColumnDefinitions.Add(defcol);
                RootGrid.ColumnDefinitions.Add(new ColumnDefinition());
                
            }

            //RowDefinition defrow = new RowDefinition();
            //defrow.Height = new GridLength(30);
            for (int i = 0; i < MyCity.Height; i++)
            {
                RootGrid.RowDefinitions.Add(new RowDefinition());
            }
        }

        public void SetGrid()
        {
            for (int i = MyCity.Height; i-- > 0; )
            {
                for (int j = MyCity.Width; j-- > 0; )
                {
                    Image img = new Image();
                    img.Width = this.Width / MyCity.Width;
                    img.Height = this.Height / MyCity.Height;
                    img.Stretch = Stretch.Fill;
                    img.SetValue(Grid.ColumnProperty, j);
                    img.SetValue(Grid.RowProperty, i);

                    try
                    {
                        img.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                            MyCity.Map[i][j].Tile.GetHbitmap(),
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error when converting ressource into image : " + e.Message);
                    }

                    RootGrid.Children.Add(img);
                    
                }
            }
        }
    }
}
