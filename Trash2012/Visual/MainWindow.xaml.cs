﻿using System;
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
using System.Windows.Shapes;
using Trash2012.Engine;
using Trash2012.Model;

namespace Trash2012.Visual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            City ct = new City(MapLoader.loadMapFromFile(@"Resources\custom.trash-map"));
            MyMap.MyCity = ct;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MyMap.MyTravel.Count > 0)
            {
                MyMap.Animate();
            }
        }
    }
}