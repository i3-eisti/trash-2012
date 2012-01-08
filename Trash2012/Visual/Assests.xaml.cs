using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Trash2012.Engine;
using Trash2012.Model;
using System;
using System.Windows.Controls.Primitives;

namespace Trash2012.Visual
{
    /// <summary>
    /// Interaction logic for Assests.xaml
    /// </summary>
    public partial class Assests : UserControl
    {
        public List<ToggleButton> buttons;
        public MainWindow MyMainWindow;
        public Assests()
        {
            InitializeComponent();
            buttons = new List<ToggleButton>();
        }

        public void UpdateAssests(Game g)
        {
            buttons.Clear();
            MyListButton.Children.Clear();
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
                        ToggleButton b = new ToggleButton();
                        b.Content = tb;
                        b.Focusable = false;
                        b.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        b.Height = 50;
                        b.Click += new System.Windows.RoutedEventHandler(b_Click);
                        buttons.Add(b);
                        MyListButton.Children.Add(b);
                        break;
                }
            }

            foreach (ToggleButton b in MyListButton.Children)
                Console.WriteLine(b.ToString());
            
        }

        void b_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ToggleButton b = (ToggleButton) sender;
            if((bool)b.IsChecked)
            {
                foreach (ToggleButton tb in MyListButton.Children)
                {
                    tb.IsChecked = false;
                }
                b.IsChecked = true;
                Truck MyTruck = ((TruckButton)b.Content).MyTruck;
                MyMainWindow.MyMap.SetTravel(MyTruck.Travel);
            }
            else
            {
                b.IsChecked = false;
                MyMainWindow.MyMap.ClearTravel();
            }
        }
    }
}
