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
using Trash2012.Engine;
using Trash2012.Model;

namespace Trash2012.Visual
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void bStart_Click(object sender, RoutedEventArgs e)
        {
            Game newGame = new Game(MapLoader.loadDefaultMap());
            //MainWindow gameWindow = new MainWindow(newGame);
            //gameWindow.Show();
            this.Close();
        }

        private void bLoad_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
