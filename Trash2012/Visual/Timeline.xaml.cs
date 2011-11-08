using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

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
            CurrentDate = Trash2012.Model.Game.TRASH2012_BEGIN;
            UpdateDateIcon();
        }

        /// <summary>
        /// Current game date
        /// TODO Bind it to a model component
        /// </summary>
        public DateTime CurrentDate { get; private set; }

        private void SldTimeValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateCurrentDate();
            UpdateDateIcon();
        }

        private void UpdateCurrentDate()
        {
            CurrentDate = Model.Game.TRASH2012_BEGIN.AddDays((int)sldTime.Value);
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
