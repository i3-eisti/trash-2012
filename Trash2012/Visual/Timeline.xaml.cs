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
        }

        /// <summary>
        /// Current game date
        /// TODO Bind it to a model component
        /// </summary>
        public DateTime CurrentDate { get; private set; }

        private void sldTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateCurrentDate();
            UpdateDateIcon();
        }

        private void UpdateCurrentDate()
        {
            CurrentDate = Trash2012.Model.Game.TRASH2012_BEGIN.AddDays((int)sldTime.Value);
        }

        private void UpdateDateIcon()
        {
            //Draw corresponding date icon
        }
    }
}
