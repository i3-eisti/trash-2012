using System.Collections.Generic;
using System.Windows.Controls;
using Trash2012.Engine;
using Trash2012.Model;

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

                        TruckButton tb = new TruckButton
                        {
                            Icon =
                            {
                                Source =
                                    ImageManager.Bitmap2ImageSource(
                                        Properties.Resources.TruckPaper)
                            },
                            ItemLabel = {Text = "Papier"},
                            MyTruck = t
                        };
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
