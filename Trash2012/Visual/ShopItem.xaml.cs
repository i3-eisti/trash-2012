using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Trash2012.Model;

namespace Trash2012.Visual
{
    /// <summary>
    /// Interaction logic for ShopItem.xaml
    /// </summary>
    public partial class ShopItem : UserControl
    {
        public ShopItem()
        {
            InitializeComponent();
        }

        public ImageSource Source
        {
            get { return Icon.Source; }
            set { Icon.Source = value; }
        }

        public string Text
        {
            get { return ItemLabel.Text; }
            set { ItemLabel.Text = value; }
        }

        public int Price
        {
            get
            {
                var strPrice = ItemPrice.Text;
                if(strPrice.Contains("€"))
                {
                    strPrice = strPrice.Substring(0, strPrice.Length - 1);
                }
                return (int)decimal.Parse(strPrice);
            }
            set { ItemPrice.Text = string.Format("{0:C}",value); }
        }

        public Truck GetArticle()
        {
            return new Truck(
                TrashType.Paper,
                25,
                1f);
        }

        public bool IsBuyed
        {
            get { return BuyButton.IsChecked ?? false; }
            set { BuyButton.IsChecked = value; }
        }

        new public bool IsEnabled
        {
            get { return BuyButton.IsEnabled; }
            set
            {
                var enabled = value;
                //If button is curently pressed, unpress it by simulating a cick on it
                if (!enabled && BuyButton.IsEnabled && IsBuyed)
                {
                    BuyButton.IsChecked = false;
                }
                BuyButton.IsEnabled = enabled;

                //update tooltip
                if (enabled)
                    BuyButton.ToolTip = null;
                else
                {
                    StackPanel container = new StackPanel();
                    TextBlock block = new TextBlock
                    {
                        Text = "Vous n'avez pas assez d'argent pour acheter cet article !",
                        FontWeight = FontWeights.Bold
                    };
                    container.Children.Add(block);

                    BuyButton.ToolTip = container;
                }
            }
        }

        public delegate void BuyActionHandler(ShopItem item);

        #region Button Behavior Handler

        private BuyActionHandler _handler;
        public BuyActionHandler BuyHandler
        {
            set { _handler = value; }
        }

        private void BuyButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_handler != null)
                _handler(this);
        }

        #endregion

    }
}
