using System;
using System.Windows;
using Trash2012.Engine;
using Trash2012.Model;
using Trash2012.Properties;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Controls;

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

        public MainWindow()
        {
            InitializeComponent();
            intro_timer = new DispatcherTimer();
            intro_timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            intro_timer.Tick += new EventHandler(intro_timer_Tick);
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

        private Game _game;
        private bool displayAnnounce = true;

        #endregion

        DispatcherTimer intro_timer;
        private void Trash2012_Loaded(object sender, RoutedEventArgs e)
        {
            this.Intro.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                Properties.Resources.intro.GetHbitmap(),
                                IntPtr.Zero,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());
            this.CamionIntro.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                Properties.Resources.CamionIntro.GetHbitmap(),
                                IntPtr.Zero,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());

            intro_timer.Start();
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
                this.StartImage.Opacity -= 0.05;
                if(this.StartImage.Opacity < 0)
                {
                    this.StartImage.Visibility = Visibility.Collapsed;
                    step1 = false;
                    step2 = true;
                    CamionIntro.Visibility = System.Windows.Visibility.Visible;
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
                        CamionIntro.Visibility = System.Windows.Visibility.Collapsed;
                        Intro.Visibility = Visibility.Collapsed;
                        StartImage.Visibility = Visibility.Visible;
                        StartImage.Opacity = 100;
                        bStart.Visibility = Visibility.Visible;
                        intro_timer.Stop();
                    }
                }

            }
            intro_counter++;
        }


        private void TruckShopItemHandler(ShopItem item)
        {
            Console.WriteLine(string.Format(
                "Bought a '{0}' truck which costs {1:C}!",
                item.Text,
                item.Price
            ));
        }

        private void NextRoundButton_Click(object sender, RoutedEventArgs e)
        {
            MyMap.Animate();
        }

        private void bStart_Click(object sender, RoutedEventArgs e)
        {
            //VisualStateManager.GoToState(this, "GameState", true);s
            _game = new Game(MapLoader.loadDefaultMap());
            OnGameStart(_game);
            StartCanvas.Visibility = System.Windows.Visibility.Collapsed;
            if (!displayAnnounce) return;
            MessageBox.Show(
                this,
                "Une ville est en proie à la saleté et aux déchets qui s'accumulent !\nAidez la en faisant les choix judicieux pour nettoyer au mieux cette ville !",
                "Nouvelle Partie",
                MessageBoxButton.OK,
                MessageBoxImage.Information
                );
            displayAnnounce = false;
        }
    }
}