using System.Windows;
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
            Game newGame = new Game(MapLoader.loadMapFromFile(@"D:\devel\Trash2012\Trash2012\Resources\default.trash-map"));
            MainWindow gameWindow = new MainWindow(newGame);
            this.Close();
            gameWindow.Show();
        }
    }
}
