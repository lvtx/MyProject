using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuperMarket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AddItemToComboBox();
        }
        private void AddItemToComboBox()
        {
            var cbxItems = new string[] { "正常收费", "满300返100", "打8折" };
            foreach (var item in cbxItems)
            {
                cbxType.Items.Add(item);
            }
            cbxType.SelectedIndex = 0;
        }

        double total = 0.0d;
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            CashSuper csuper = CashFactory.createCashAccept(cbxType.SelectedItem.ToString());
            double totalPrices = 0d;
            totalPrices = csuper.acceptCash(Convert.ToDouble(txtPrice.Text)) * Convert.ToDouble(txtNum.Text);
            total = total + totalPrices;
            lbxList.Items.Add("单价" + txtPrice.Text + "数量：" 
                + txtNum.Text + " " + cbxType.SelectedItem + "合计：" + totalPrices.ToString());
            lblResult.Content = total.ToString();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
