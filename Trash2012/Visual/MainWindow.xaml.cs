﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Trash2012.Engine;
using Trash2012.Model;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Trash2012.Visual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Game Configuration
        
        //Configure the game here for more simplicity 
        private readonly Func<IMapTile[][]> _choosenMap = delegate() { return MapLoader.loadCustomMap(); };
        //Intro Animation timer interval
        private const bool PlayIntroAnimation = false;
        private readonly int[] _introInterval = {0, 0, 0, 0, 100}; 
        //Dashboard counter animation
        private const long DashboardAnimationTick = 800000;
        private readonly int[] _monthlyRevenueRange = {15000, 22000};
        private const int _initialMoneyAmout = 30000;
        //Travel
        private const int _garbageCollectionBenefit = 150;
        private const int _TravelCost = 200;
        //Garbage threshold
        private readonly int[] _garbageThresholdRange = { 500, 600 };

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

            KeyDown += MainWindow_KeyDown;

            BuyableItems = new List<ShopItem>(1) { PaperTruckBuyer };

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

            _garbageAmountThreshold = new Random().Next(_garbageThresholdRange[0], _garbageThresholdRange[1]);

            game.DateChangeEvents.Add(PaydayEvent);

            UpdateGameDashboard(game, delegate {}, false);
            UpdateBuyableItem(game, delegate {});
            UpdateTimeline(game, delegate { });
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
                this.InfoMessage.Text = "C'est la fin du mois, jour de paye ! Vous avez gagné: " + revenue + " € !";
            }
        }

        private int _garbageAmountThreshold;

        private void GameEndEvent(DateTime newDate)
        {
            if (_game.City.GarbageQuantity <= 100 )
            {
                string message = "Un sentiment de fraîcheur et propreté vous envahit..." +
                            "\nCette ville est enfin sauvé et goûte au bonheur !";
                MessageBox.Show(
                    message,
                    "Vous avez gagné !",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                gotoNewGameScreen();
            }
            if (_game.Company.Gold <= 0)
            {
                string message = "La compagnie fait faillite !" +
                            "\nSauvez-vous tant que vous le pouvez encore !";
                MessageBox.Show(
                    message,
                    "Vous avez perdu !",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                gotoNewGameScreen();
            }
            if (_game.City.GarbageQuantity > _garbageAmountThreshold)
            {
                string message = "Votre ville a croulé sous le poids de ses déchets...et de la fureur du maire !" +
                            "\nSauvez-vous tant que vous le pouvez encore !";
                MessageBox.Show(
                    message,
                    "Vous avez perdu !",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                gotoNewGameScreen();
            }
            if (newDate.Year != 2012)
            {
                string message = "Le temps a passé, passé, et vos stratégies n'y font rien !" +
                            "\nLe maire a décidé de vous remplacez, pensez à vous reconvertir !";
                MessageBox.Show(
                    message,
                    "Vous avez perdu !",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                gotoNewGameScreen();
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
                Canvas.SetZIndex(CamionIntro, 10);
				try
				{
                    SoundPlayer sp = new SoundPlayer();
                    sp.SoundLocation = "Resources/Music/Music.wav";
                    sp.PlayLooping();
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

        private void gotoNewGameScreen()
        {
            StartCanvas.Visibility = Visibility.Visible;
            StartGrid.Visibility = Visibility.Visible;
            bStart.Visibility = Visibility.Visible;  
        }

        private void bStart_Click(object sender, RoutedEventArgs e)
        {
            _game = new Game(_choosenMap(), _initialMoneyAmout);
            OnGameStart(_game);
            StartCanvas.Visibility = Visibility.Collapsed;
            StartGrid.Visibility = Visibility.Collapsed;    
        }

        private void CheckOtherBuyableItem(ShopItem item)
        {
            _currentlyPushed.Add(item);
            foreach (var buyableItem in BuyableItems.Where(
                                            buyableItem => !_currentlyPushed.Contains(buyableItem)))
            {
                buyableItem.IsEnabled = buyableItem.Price <= _game.Company.Gold;
            }
        }

        private void NextDayHandler(object sender, RoutedEventArgs e)
        {
            //reinitialise message box
            reinitializeMessage();

            bNextDay.IsEnabled = false;
            GameUpdate();
            UIUpdate();

            GameEndEvent(_game.CurrentDate);
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

            //3.bis CompanyUpdate
            CompanyUpdate();

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
                MyAssets.UpdateAssests(_game);
            }
        }

        private void TravelUpdate()
        {
            int totalCollectedGarbage = 0;
            int totalTravelledDistance = 0;
            int collectedGarbage = 0;
            int currentIndex = 0;
            Travel dailyTravel;
            foreach (ToggleButton tb in MyAssets.buttons)
            {
                currentIndex = MyAssets.MyListButton.Children.IndexOf(tb);
                dailyTravel = ((TruckButton)tb.Content).MyTruck.Travel;
                collectedGarbage = _game.ApplyTravel(dailyTravel, _game.Company.Trucks[currentIndex]);

                totalCollectedGarbage += collectedGarbage;
                totalTravelledDistance += dailyTravel.Count;
            }

            _game.Company.Gold += totalCollectedGarbage * _garbageCollectionBenefit - totalTravelledDistance * _TravelCost;
        }

        private void CityUpdate()
        {
            _game.ApplyDailyGarbage();
            _recentEvent = _game.ApplyRandomEvent();
            if (_recentEvent.HasValue)
            {
                _recentEvent.Value.Effect();
            }
        }

        private void CompanyUpdate()
        {
            foreach (var companyTruck in _game.Company.Trucks)
                companyTruck.Reset();
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

                bNextDay.IsEnabled = true;
            })))));
        }

        private void HandleRandomEvent(Game game, Game.GameEvent? gameEvent, Action doneCallback)
        {
            if(gameEvent.HasValue)
            {
                this.InfoMessage.Text = gameEvent.Value.Message;
                MessageBox.Show(
                    gameEvent.Value.Message,
                    "Un évènement inattendue !",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            UpdateGameDashboard(_game, doneCallback);
            MyAssets.UpdateAssests(_game);
        }

        private void UpdateMap(Game game, Action doneCallback)
        {
            var width = game.City.Width;
            var height = game.City.Height;
            var m = MyMap.TilesVisual;
            for (int i = height; i-- > 0; )
                for (int j = width; j-- > 0; )
                    m[i][j].Visited = false;
            AnimateTruck(game, 0, doneCallback);
        }

        private void AnimateTruck(Game game, int truckPosition, Action doneCallback)
        {
            if (truckPosition < game.Company.Trucks.Count)
            {
                var truck = game.Company.Trucks[truckPosition];
                if (truck.Travel.Count > 0)
                {
                    MyMap.Animate(
                        truck,
                        () => AnimateTruck(game, truckPosition + 1, doneCallback));
                }
                else
                    AnimateTruck(game, truckPosition + 1, doneCallback);
            }
            else
            {
                var width = game.City.Width;
                var height = game.City.Height;
                var m = MyMap.TilesVisual;
                for (var i = height; i-- > 0; )
                    for (var j = width; j-- > 0; )
                        if(!m[i][j].Visited)
                            m[i][j].Update();
                doneCallback();
            }
        }

        private void UpdateTimeline(Game game, Action doneCallback)
        {
            GameTimeline.CurrentDate = game.CurrentDate;
            doneCallback();
        }

        #region Dashboard

        private void UpdateGameDashboard(Game game, Action doneCallback, bool animated = true)
        {
            if(animated)
            {
                int
                    deltat = _game.Company.Trucks.Count - GameDashboard.TruckQuantity,
                    deltam = _game.Company.Gold - GameDashboard.MoneyQuantity,
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
            }
            else
            {
                GameDashboard.TruckQuantity = _game.Company.Trucks.Count;
                GameDashboard.MoneyQuantity = _game.Company.Gold;
                GameDashboard.PeopleQuantity = _game.City.PeopleNumber;
                GameDashboard.GarbageQuantity = _game.City.GarbageQuantity;
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
            var adjusted = GameDashboard.MoneyQuantity == _game.Company.Gold;
            var delta = _game.Company.Gold - GameDashboard.MoneyQuantity;
            int rates = 1;
            if (Math.Abs(delta) > 1000)
            {
                rates = 100;
            }
            else if (Math.Abs(delta) > 100)
            {
                rates = 10;
            }
            else
            {
                rates = 1;
            }
            if (!adjusted)
                GameDashboard.MoneyQuantity += Math.Sign(delta) * rates;
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
                buyableItem.IsEnabled = buyableItem.Price <= game.Company.Gold;
            }
            doneCallback();
        }

        #endregion

        void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!isZoom)
            {
                isZoom = true;
            }
            if (e.Key == System.Windows.Input.Key.Up)
            {
                if (MyMap.Position_Y < 0)
                {
                    this.MyMap.Position_Y += 1;
                }
            }
            if (e.Key == System.Windows.Input.Key.Down)
            {
                if (MyMap.Position_Y > MyMap.MaxTiles - MyMap.MyCity.Height)
                {
                    this.MyMap.Position_Y -= 1;
                }
            }
            if (e.Key == System.Windows.Input.Key.Left)
            {
                if (MyMap.Position_X < 0)
                {
                    this.MyMap.Position_X += 1;
                }
            }
            if (e.Key == System.Windows.Input.Key.Right)
            {
                if (MyMap.Position_X > MyMap.MaxTiles - MyMap.MyCity.Width)
                {
                    this.MyMap.Position_X -= 1;
                }
            }
            this.MyMap.UpdateCanvas();
        }

        void MainWindow_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(isZoom)
            {
                if (e.GetPosition(this.MyMap).Y < 10)
                {
                    if (MyMap.Position_Y < 0)
                    {
                        this.MyMap.Position_Y += 1;
                    }
                }
                if (e.GetPosition(this.MyMap).Y > this.MyMap.ActualHeight -10)
                {
                    if (MyMap.Position_Y > MyMap.MaxTiles - MyMap.MyCity.Height)
                    {
                        this.MyMap.Position_Y -= 1;
                    }
                }
                if (e.GetPosition(this.MyMap).X < 10)
                {
                    if (MyMap.Position_X < 0)
                    {
                        this.MyMap.Position_X += 1;
                    }
                }
                if (e.GetPosition(this.MyMap).X > this.MyMap.ActualWidth -10)
                {
                    if (MyMap.Position_X > MyMap.MaxTiles - MyMap.MyCity.Width)
                    {
                        this.MyMap.Position_X -= 1;
                    }
                }
                this.MyMap.UpdateCanvas();
            }
        } //FAIL

        bool isZoom = false;
        private void Zoom_Click(object sender, RoutedEventArgs e)
        {
            if (isZoom == false)
            {
                this.MyMap.UpdateCanvas();
                isZoom = true;
            }
            else
            {
                this.MyMap.UnUpdateCanvas();
                isZoom = false;
            }
        }

        #region MessageBoard

        public void errorMessage(string text)
        {
            InfoMessage.Foreground = new SolidColorBrush(Colors.Red);
            InfoMessage.Text = text;
        }

        public void reinitializeMessage()
        {
            InfoMessage.Foreground = new SolidColorBrush(Colors.Black);
            InfoMessage.Text = "";
        }

        #endregion


    }
}