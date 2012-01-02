using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Trash2012.Engine;
using Trash2012.Model;
using System;

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
            buttons.Clear();
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
            //MyListView.KeyDown;
            int selected = MyListView.SelectedIndex;
            MyListView.UnselectAll();
            MyListView.ItemsSource = null;
            MyListView.ItemsSource = buttons;
            foreach ( TruckButton tb in MyListView.ItemsSource)
                Console.WriteLine(tb.ToString());
            MyListView.SelectedIndex = selected;
        }

        private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyListView.SelectedIndex != -1)
            {
                Truck MyTruck = ((TruckButton)MyListView.SelectedItem).MyTruck;
                MyMainWindow.MyMap.SetTravel(MyTruck.Travel);
            }
        }

        private void UserControl_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            Keyboard.Focus(MyMainWindow.MyMap);
        }

        private void UserControl_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Keyboard.Focus(MyMainWindow.MyMap);
        }
    }
}
