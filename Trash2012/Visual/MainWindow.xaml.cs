using System;
using System.Windows;
using Trash2012.Engine;
using Trash2012.Model;
using Trash2012.Properties;

namespace Trash2012.Visual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public City GameCity
        {
            get { return _game.City; }
        }

        public MainWindow(Game game)
        {
            InitializeComponent();
            _game = game;
            OnGameStart(_game);
        }

        #region Game Events

        private void OnGameStart(Game game)
        {
            MyMap.MyCity = _game.City;
            GameDashboard.TruckQuantity = game.Company.Trucks.Count;
            GameDashboard.InhabitantQuantity = game.City.PeopleNumber;
            GameDashboard.MoneyQuantity = game.Company.Gold.Current;
        }

        #endregion

        #region Hidden members

        private readonly Game _game;
        private bool displayAnnounce = true;

        #endregion

        private void Trash2012_Loaded(object sender, RoutedEventArgs e)
        {
            if (!displayAnnounce) return;
            MessageBox.Show(
                this,
                "Une ville est en proie à la saleté et aux déchets qui s'accumulent !\nAidez la en faisant les choix judicieux pour nettoyer au mieux cette ville !",
                "Nouvelle Partie",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            displayAnnounce = false;
        }

        private void TruckShopItemHandler(ShopItem item)
        {
            Console.WriteLine(string.Format(
                "Bought a '{0}' truck which costs {1:C}!",
                item.Text,
                item.Price
            ));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MyMap.MyTravel.Count > 0)
            {
                MyMap.Animate();
            }
        }

        private void bNextDay_Click(object sender, RoutedEventArgs e)
        {
            //1. Shop interactions

            //2. Truck Travel
            //Travel dailyTravel = MyMap.MyTravel;
            Travel dailyTravel = null;
            if(dailyTravel != null)
            {
                //TODO Apply travel on current map
                int collectedGarbage = _game.ApplyTravel(dailyTravel);
                //TODO Summarize collected garbage
                //TODO Display a beautiful screen for that
            }
            else
            {
                Console.WriteLine(string.Format(
                    "No daily travel on {0}", _game.CurrentDate
                ));
            }

            //3. City garbage update
            //_game.ApplyDailyGarbage();

            //4. Go to the next day
            _game.CurrentDate = _game.CurrentDate.AddDays(1);

            //5. Update UI Components
            GameTimeline.CurrentDate = _game.CurrentDate;
        }

    }
}