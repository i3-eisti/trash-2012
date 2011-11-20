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
        public int MoneyQuantity
        {
            get
            {
                var text = (string) quantityMoney.Content;
                if(text.Contains("€"))
                    return (int)decimal.Parse(text.Substring(0, text.Length - 1));
                else
                    return int.Parse(text);
            }
            set { quantityMoney.Content = string.Format("{0:C}", value); }
        }
        public int PeopleQuantity
        {
            get { return int.Parse((string)quantityInhabitant.Content); }
            set { quantityInhabitant.Content = string.Format("{0}", value); }
        }
        public int GarbageQuantity
        {
            get { return int.Parse((string)quantityGarbage.Content); }
            set { quantityGarbage.Content = string.Format("{0}", value); }
        }

        public Dashboard()
        {
            InitializeComponent();
        }
    }
}
