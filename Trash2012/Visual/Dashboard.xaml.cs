using System.Windows.Controls;

namespace Trash2012.Visual
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        public int TruckQuantity
        {
            get { return int.Parse((string)quantityTruck.Content); }
            set { quantityTruck.Content = string.Format("{0}", value); }
        }
        public int InhabitantQuantity
        {
            get { return int.Parse((string)quantityInhabitant.Content); }
            set { quantityInhabitant.Content = string.Format("{0}", value); }
        }
        public int MoneyQuantity
        {
            get { return int.Parse((string)quantityMoney.Content); }
            set { quantityMoney.Content = string.Format("{0:C}", value); }
        }

        public Dashboard()
        {
            InitializeComponent();
        }
    }
}
