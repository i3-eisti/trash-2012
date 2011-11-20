using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;

namespace Trash2012.Visual
{
    public partial class Timeline : UserControl
    {

        public Timeline()
        {
            InitializeComponent();
            UserInitialization();
        }

        private void UserInitialization()
        {
            CurrentDate = Model.Game.Trash2012Begin;
            UpdateDateIcon();
        }

        /// <summary>
        /// Current game date
        /// </summary>
        public DateTime CurrentDate
        {
            get { return _date; }
            set
            {
                if(value.Year != 2012)
                    throw new ArgumentException(
                        "This game takes place in 2012, not " + value.Year + " !");

                _date = value;
                sldTime.Value = value.DayOfYear;
            }
        }

        private DateTime _date;

        private void SldTimeValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateDateIcon();
        }

        #region Icon Handler

        private static readonly PointF MONTH_LABEL_LOCATION = new PointF(15f, 5f);
        private static readonly float MONTH_LABEL_FONTSIZE = 8f;

        private static readonly PointF DAY_LABEL_LOCATION = new PointF(15f, 19f);
        private static readonly float DAY_LABEL_FONTSIZE = 18f;

        private void UpdateDateIcon()
        {
            String lblMonth = MonthLabel(CurrentDate.Month);
            String lblDay = CurrentDate.Day.ToString();

            //get a new image
            Bitmap calIcon = new Bitmap(30,30);
            //get a painter
            Graphics painter = Graphics.FromImage(calIcon);

            //paint background
            painter.DrawImage(Properties.Resources.CalendarBackground, 0, 0, 30, 30);

            //month style
            Brush brush = Brushes.DarkBlue;
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            Font mthFont = new Font(
                System.Drawing.FontFamily.GenericMonospace,
                MONTH_LABEL_FONTSIZE,
                System.Drawing.FontStyle.Bold,
                GraphicsUnit.Pixel
            );
            painter.DrawString(
                lblMonth,
                mthFont,
                brush,
                MONTH_LABEL_LOCATION,
                format
            );


            Font dayFont = new Font(
                System.Drawing.FontFamily.GenericMonospace,
                DAY_LABEL_FONTSIZE,
                System.Drawing.FontStyle.Bold,
                GraphicsUnit.Pixel
            );
            painter.DrawString(
                lblDay,
                dayFont,
                brush,
                DAY_LABEL_LOCATION,
                format
            );

            //convert it to a stream
            MemoryStream ms = new MemoryStream();
            calIcon.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage imgSource = new BitmapImage();
            imgSource.BeginInit();
            imgSource.StreamSource = ms;
            imgSource.EndInit();

            imgCurrentDate.Source = imgSource;
        }

        private static String MonthLabel(int monthCode)
        {
            switch (monthCode)
            {
                case 1: return "Jan";
                case 2: return "Fev";
                case 3: return "Mar";
                case 4: return "Avr";
                case 5: return "Mai";
                case 6: return "Jun";
                case 7: return "Jui";
                case 8: return "Aou";
                case 9: return "Sep";
                case 10: return "Oct";
                case 11: return "Nov";
                case 12: return "Dec";
                default:
                    throw new ArgumentException("Unknown month code: " + monthCode);
            }
        }

        #endregion
    }
}
