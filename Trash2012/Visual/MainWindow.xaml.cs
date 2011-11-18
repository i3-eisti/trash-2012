using System;
using System.Collections.Generic;
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

        public List<ShopItem> BuyableItems { get; private set; }

        public MainWindow(Game game)
        {
            InitializeComponent();

            List<ShopItem> items = new List<ShopItem>(1);
            items.Add(PaperTruckBuyer);
            BuyableItems = items;

            _game = game;
            OnGameStart(_game);
        }

        #region Game Events

        private void OnGameStart(Game game)
        {
            MyMap.MyCity = _game.City;
            UpdateGameDashboard(game);
            UpdateBuyableItem(game);
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


        private List<ShopItem> currentlyPushed = new List<ShopItem>(1);
        private void CheckOtherBuyableItem(ShopItem item)
        {
            currentlyPushed.Add(item);
            foreach (var buyableItem in BuyableItems)
            {
                if (currentlyPushed.Contains(buyableItem)) continue;
                buyableItem.IsEnabled = buyableItem.Price <= _game.Company.Gold.Current;
            }
        }

        private void bNextDay_Click(object sender, RoutedEventArgs e)
        {
            //1. Shop interactions
            foreach (var buyableItem in BuyableItems)
            {
                if(buyableItem.IsBuyed) // User pressed the button
                {
                    var buyedTruck = buyableItem.GetArticle();
                    _game.Company.Gold -= buyableItem.Price;
                    _game.Company.Trucks.Add(buyedTruck);
                }
            }

            //2. Truck Travel
            var dailyTravel = MyMap.MyTravel;
            //Travel dailyTravel = null;
            if(dailyTravel != null)
            {
                var companyTruck = _game.Company.Trucks[0];
                int collectedGarbage = _game.ApplyTravel(dailyTravel, companyTruck);
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
            UpdateTimeline(_game);
            UpdateGameDashboard(_game);
            UpdateBuyableItem(_game);
            currentlyPushed.Clear();
        }

        #region UI Update

        private void UpdateTimeline(Game game)
        {
            GameTimeline.CurrentDate = game.CurrentDate;
        }

        private void UpdateGameDashboard(Game game)
        {
            GameDashboard.TruckQuantity = game.Company.Trucks.Count;
            GameDashboard.InhabitantQuantity = game.City.PeopleNumber;
            GameDashboard.MoneyQuantity = game.Company.Gold.Current;
        }

        private void UpdateBuyableItem(Game game)
        {
            foreach (var buyableItem in BuyableItems)
            {
                //Buyable item are enabled only if the company has enough money to buy it
                buyableItem.IsBuyed = false;
                buyableItem.IsEnabled = buyableItem.Price <= game.Company.Gold.Current;
            }
        }

        #endregion

    }
}