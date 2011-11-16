using System.Windows.Controls;
using System.Windows.Media;

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

        public bool IsPressed
        {
            get { return BuyButton.IsPressed; }
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
