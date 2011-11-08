using System.Windows;
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
            MyMap.MyCity = new City(MapLoader.loadMapFromFile(@"Resources\custom.trash-map")); ;
        }
    }
}