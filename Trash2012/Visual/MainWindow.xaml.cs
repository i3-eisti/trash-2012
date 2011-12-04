﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using Trash2012.Engine;
using Trash2012.Model;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Controls;

namespace Trash2012.Visual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Game Configuration
        
        //Configure the game here for more simplicity 
        private readonly IMapTile[][] _choosenMap = MapLoader.loadCustomMap();
        //Intro Animation timer interval
        private const bool PlayIntroAnimation = false;
        /// <summary>
        /// If new game announce should be displayed
        /// </summary>
        private bool _displayAnnounce = false;
        private readonly int[] _introInterval = {0, 0, 0, 0, 100}; 
        //Dashboard counter animation
        private const long DashboardAnimationTick = 730000;
        private readonly int[] _monthlyRevenueRange = {900, 1200};

        #endregion

        public City GameCity
        {
            get { return _game.City; }
        }

        private readonly DispatcherTimer _gameTimer = new DispatcherTimer();
        private readonly DispatcherTimer[] _dashboardTimer = new DispatcherTimer[4];

        public List<ShopItem> BuyableItems { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            BuyableItems = new List<ShopItem>(1) { PaperTruckBuyer };
			
            InitializeComponent();

            MyMap.MyMainWindow = this;
            MyAssets.MyMainWindow = this;

            if (PlayIntroAnimation)
            {
                _gameTimer.Interval = new TimeSpan(_introInterval[0], _introInterval[1], _introInterval[2],
                                                       _introInterval[3], _introInterval[4]);
                _gameTimer.Tick += intro_timer_Tick;
            }
            else
            {
                bStart.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }       

        #region Game Events

        private void OnGameStart(Game game)
        {
            MyMap.MyCity = game.City;
            game.DateChangeEvents.Add(PaydayEvent);

            UpdateGameDashboard(game, delegate {});
            UpdateBuyableItem(game, delegate { });
            MyAssets.UpdateAssests(game);
        }

        private void PaydayEvent(DateTime newDate)
        {
            //every month, receive payday
            if (newDate.Day == 1)
            {
                var r = new Random();
                var revenue = r.Next(_monthlyRevenueRange[0], _monthlyRevenueRange[1]);
                _game.Company.Gold += revenue; 
                MessageBox.Show(
                    this,
                    string.Format("It's payday ! You have earn {0:C} this month !", revenue),
                    "Monthly revenue",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
        }

        #endregion

        #region Hidden members

        /// <summary>
        /// Game backend model
        /// </summary>
        private Game _game;
        /// <summary>
        /// Memory to remember shop pushed buttons every turns
        /// </summary>
        private readonly List<ShopItem> _currentlyPushed = new List<ShopItem>(1);

        #endregion

        #region Intro Animation
        
        private void Trash2012_Loaded(object sender, RoutedEventArgs e)
        {
            if (PlayIntroAnimation)
            {
				Intro.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
									Properties.Resources.intro.GetHbitmap(),
									IntPtr.Zero,
									Int32Rect.Empty,
									BitmapSizeOptions.FromEmptyOptions());
				CamionIntro.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
									Properties.Resources.CamionIntro.GetHbitmap(),
									IntPtr.Zero,
									Int32Rect.Empty,
									BitmapSizeOptions.FromEmptyOptions());
				try
				{
				    music = new MediaElement
				    {
				        LoadedBehavior = MediaState.Manual,
				        UnloadedBehavior = MediaState.Manual,
				        Source = new Uri(@"D:\devel\Trash2012\Trash2012\Resources\Music\music.mp3")
				    };
				    music.Play();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}

                _gameTimer.Start();
            }
        }

        int intro_counter = 0;
        bool step1 = false;
        bool step2 = false;
        bool step3 = false;
        bool step4 = false;
        bool step5 = false;
        MediaElement music;

        void intro_timer_Tick(object sender, EventArgs e)
        {
            if (intro_counter > 10 && intro_counter < 100)
            {
                step1 = true;
            }
            if(step1)
            {
                this.StartGrid.Opacity -= 0.05;
                this.Intro.Opacity += 0.05;
                if (this.StartGrid.Opacity < 0)
                {
                    this.StartGrid.Visibility = Visibility.Collapsed;
                    step1 = false;
                    step2 = true;
                    CamionIntro.Visibility = Visibility.Visible;
                }
            }
            if (step2)
            {
                if (Canvas.GetLeft(CamionIntro) > 675)
                {
                    Canvas.SetLeft(CamionIntro, Canvas.GetLeft(CamionIntro) - 10);
                }
                else
                {
                    this.Intro.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                Properties.Resources.intro1.GetHbitmap(),
                                IntPtr.Zero,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());

                    step2 = false;
                    step3 = true;
                    intro_counter = 100;
                }
                
            }
            if (step3)
            {
                if (intro_counter > 105)
                {
                    if (Canvas.GetLeft(CamionIntro) > 405)
                    {
                        Canvas.SetLeft(CamionIntro, Canvas.GetLeft(CamionIntro) - 10);
                    }
                    else
                    {
                        this.Intro.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                Properties.Resources.intro2.GetHbitmap(),
                                IntPtr.Zero,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());

                        step3 = false;
                        step4 = true;
                        intro_counter = 100;
                    }

                }
            }
            if (step4)
            {
                if (intro_counter > 105)
                {
                    if (Canvas.GetLeft(CamionIntro) > 155)
                    {
                        Canvas.SetLeft(CamionIntro, Canvas.GetLeft(CamionIntro) - 10);
                    }
                    else
                    {
                        this.Intro.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                Properties.Resources.intro3.GetHbitmap(),
                                IntPtr.Zero,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());

                        step4 = false;
                        step5 = true;
                        intro_counter = 100;
                    }

                }
            }
            if (step5)
            {
                if (intro_counter > 105)
                {
                    if (Canvas.GetLeft(CamionIntro) > -160)
                    {
                        Canvas.SetLeft(CamionIntro, Canvas.GetLeft(CamionIntro) - 10);
                    }
                    else
                    {
                        CamionIntro.Visibility = Visibility.Collapsed;
                        Intro.Visibility = Visibility.Collapsed;
                        StartGrid.Visibility = Visibility.Visible;
                        StartGrid.Opacity = 100;
                        bStart.Visibility = Visibility.Visible;
                        _gameTimer.Stop();
                    }
                }

            }
            intro_counter++;
        }

        #endregion

        private void bStart_Click(object sender, RoutedEventArgs e)
        {
            _game = new Game(_choosenMap);
            OnGameStart(_game);
            StartCanvas.Visibility = Visibility.Collapsed;
            StartGrid.Visibility = Visibility.Collapsed;
            if (!_displayAnnounce) return;
            MessageBox.Show(
                this,
                "Une ville est en proie à la saleté et aux déchets qui s'accumulent !\nAidez la en faisant les choix judicieux pour nettoyer au mieux cette ville !",
                "Nouvelle Partie",
                MessageBoxButton.OK,
                MessageBoxImage.Information
                );
            _displayAnnounce = false;
        }

        private void CheckOtherBuyableItem(ShopItem item)
        {
            _currentlyPushed.Add(item);
            foreach (var buyableItem in BuyableItems.Where(
                                            buyableItem => !_currentlyPushed.Contains(buyableItem)))
            {
                buyableItem.IsEnabled = buyableItem.Price <= _game.Company.Gold.Current;
            }
        }

        private void NextDayHandler(object sender, RoutedEventArgs e)
        {
            GameUpdate();
            UIUpdate();
        }

        #region Game Update

        private void GameUpdate()
        {
            //1. Shop interactions
            ShopUpdate();

            //2. Truck Travel
            TravelUpdate();

            //3. City garbage update
            CityUpdate();

            //4. Go to the next day
            DateUpdate();
        }

        private void ShopUpdate()
        {
            foreach (var buyableItem in BuyableItems.Where(buyableItem => buyableItem.IsBuyed))
            {
                var buyedTruck = buyableItem.GetArticle();
                _game.Company.Gold -= buyableItem.Price;
                _game.Company.Trucks.Add(buyedTruck);
            }
        }

        private void TravelUpdate()
        {
            //LINQ FTW
            foreach (var collectedGarbage in 
                from truckButton in MyAssets.buttons 
                select truckButton.MyTruck.Travel into dailyTravel 
                let companyTruck = _game.Company.Trucks[0] 
                select _game.ApplyTravel(dailyTravel, companyTruck))
            {
                Console.WriteLine(string.Format("{0} garbage collected.", collectedGarbage));
            }
        }

        private void CityUpdate()
        {
            _game.ApplyDailyGarbage();
            _recentEvent = _game.ApplyRandomEvent();
            if (_recentEvent.HasValue)
            {
                Console.WriteLine("Recent event occured : " + _recentEvent.Value.Message);
                _recentEvent.Value.Effect();
            }
        }

        private void DateUpdate()
        {
            _game.CurrentDate = _game.CurrentDate.AddDays(1);
        }

        #endregion

        #region UI Update

        private Game.GameEvent? _recentEvent;

        private void UIUpdate()
        {
            MyMap.ClearTravel();
            MyMap.IsSelectionEnabled = false;
            foreach (var shopItem in BuyableItems)
            {
                shopItem.IsEnabled = false;
            }
            foreach (var button in MyAssets.buttons)
                button.IsEnabled = false;

            HandleRandomEvent(_game, _recentEvent, () =>
            UpdateMap(_game, () => 
            UpdateGameDashboard(_game, () => 
            UpdateBuyableItem(_game, () =>
            UpdateTimeline(_game, delegate
            {
                MyMap.IsSelectionEnabled = true;
                foreach (var button in MyAssets.buttons)
                    button.IsEnabled = true;
                //reset temporary values
                _currentlyPushed.Clear();
                _recentEvent = null;
            })))));
        }

        private void HandleRandomEvent(Game game, Game.GameEvent? gameEvent, Action doneCallback)
        {
            if(gameEvent.HasValue)
            {
                MessageBox.Show(
                    gameEvent.Value.Message,
                    "Un évènement inattendue !",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            UpdateGameDashboard(_game, doneCallback);
        }

        private void UpdateMap(Game game, Action doneCallback)
        {
            AnimateTruck(game, 0, doneCallback);
        }

        private void AnimateTruck(Game game, int truckPosition, Action doneCallback)
        {
            if (truckPosition < game.Company.Trucks.Count)
            {
                var truck = game.Company.Trucks[truckPosition];
                if (truck.Travel.Count > 0)
                    MyMap.Animate(
                        truck,
                        () => AnimateTruck(game, truckPosition + 1, doneCallback));
                else
                    AnimateTruck(game, truckPosition + 1, doneCallback);
            }
            else
            {
                doneCallback();
            }
        }

        private void UpdateTimeline(Game game, Action doneCallback)
        {
            GameTimeline.CurrentDate = game.CurrentDate;
            doneCallback();
        }

        #region Dashboard

        private void UpdateGameDashboard(Game game, Action doneCallback)
        {
            int
                deltat = _game.Company.Trucks.Count - GameDashboard.TruckQuantity,
                deltam = _game.Company.Gold.Current - GameDashboard.MoneyQuantity,
                deltap = _game.City.PeopleNumber - GameDashboard.PeopleQuantity,
                deltag = _game.City.GarbageQuantity - GameDashboard.GarbageQuantity;
                
            //trucks
            if (deltat != 0)
            {
                _dashboardTimer[0] = new DispatcherTimer();
                _dashboardTimer[0].Interval = new TimeSpan(DashboardAnimationTick / Math.Abs(deltat));
                _dashboardTimer[0].Tick += DashboardTruckAnimate;
                _dashboardTimer[0].Start();
            }

            //money
            if (deltam != 0)
            {
                _dashboardTimer[1] = new DispatcherTimer();
                _dashboardTimer[1].Interval = new TimeSpan(DashboardAnimationTick / Math.Abs(deltam));
                _dashboardTimer[1].Tick += DashboardMoneyAnimate;
                _dashboardTimer[1].Start();
            }

            //people
            if (deltap != 0)
            {
                _dashboardTimer[2] = new DispatcherTimer();
                _dashboardTimer[2].Interval = new TimeSpan(DashboardAnimationTick / Math.Abs(deltap));
                _dashboardTimer[2].Tick += DashboardPeopleAnimate;
                _dashboardTimer[2].Start();
            }

            //garbage
            if (deltag != 0)
            {
                _dashboardTimer[3] = new DispatcherTimer();
                _dashboardTimer[3].Interval = new TimeSpan(DashboardAnimationTick / Math.Abs(deltag));
                _dashboardTimer[3].Tick += DashboardGarbageAnimate;
                _dashboardTimer[3].Start();
            }

            doneCallback();
        }
        private void DashboardTruckAnimate(object sender, EventArgs e)
        {
            var timer = sender as DispatcherTimer;
            var adjusted = GameDashboard.TruckQuantity == _game.Company.Trucks.Count;
            var delta = _game.Company.Trucks.Count - GameDashboard.TruckQuantity;
            if (!adjusted)
                GameDashboard.TruckQuantity += Math.Sign(delta) * 1;
            else
                timer.Stop();
        }
        private void DashboardMoneyAnimate(object sender, EventArgs e)
        {
            var timer = sender as DispatcherTimer;
            var adjusted = GameDashboard.MoneyQuantity == _game.Company.Gold.Current;
            var delta = _game.Company.Gold.Current - GameDashboard.MoneyQuantity;
            if (!adjusted)
                GameDashboard.MoneyQuantity += Math.Sign(delta) * 1;
            else
                timer.Stop();
        }
        private void DashboardPeopleAnimate(object sender, EventArgs e)
        {
            var timer = sender as DispatcherTimer;
            var adjusted = GameDashboard.PeopleQuantity == _game.City.PeopleNumber;
            var delta = _game.City.PeopleNumber - GameDashboard.PeopleQuantity;
            if (!adjusted)
                GameDashboard.PeopleQuantity += Math.Sign(delta) * 1;
            else
                timer.Stop();
        }
        private void DashboardGarbageAnimate(object sender, EventArgs e)
        {
            var timer = sender as DispatcherTimer;
            var adjusted = GameDashboard.GarbageQuantity == _game.City.GarbageQuantity;
            var delta = _game.City.GarbageQuantity - GameDashboard.GarbageQuantity;
            if (!adjusted)
                    GameDashboard.GarbageQuantity += Math.Sign(delta) * 1;
            else
                timer.Stop(); 
        }

        #endregion

        private void UpdateBuyableItem(Game game, Action doneCallback)
        {
            foreach (var buyableItem in BuyableItems)
            {
                //Buyable item are enabled only if the company has enough money to buy it
                buyableItem.IsBuyed = false;
                buyableItem.IsEnabled = buyableItem.Price <= game.Company.Gold.Current;
            }
            doneCallback();
        }

        #endregion

    }
}