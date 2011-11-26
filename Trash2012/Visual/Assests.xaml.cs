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
using System.Windows.Controls.Primitives;

namespace Trash2012.Visual
{
    /// <summary>
    /// Interaction logic for Assests.xaml
    /// </summary>
    public partial class Assests : UserControl
    {
        public List<TruckButton> buttons;
        public MainWindow MyMainWindow;
        public Assests()
        {
            InitializeComponent();
            buttons = new List<TruckButton>();
        }

        public void UpdateAssests(Game g)
        {
            foreach (Truck t in g.Company.Trucks)
            {   
                switch(t.HandledResource)
                {
                    case TrashType.Paper:
                        
                        TruckButton tb = new TruckButton();
                        tb.Icon.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                Properties.Resources.TruckPaper.GetHbitmap(),
                                IntPtr.Zero,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());
                        tb.ItemLabel.Text = "Papier";
                        tb.MyTruck = t;
                        buttons.Add(tb);
                        break;
                }
            }
            MyListView.ItemsSource = buttons;
        }

        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Truck MyTruck = ((TruckButton)MyListView.SelectedItem).MyTruck;
            MyMainWindow.MyMap.SetTravel(MyTruck.Travel);
        }
    }
}
