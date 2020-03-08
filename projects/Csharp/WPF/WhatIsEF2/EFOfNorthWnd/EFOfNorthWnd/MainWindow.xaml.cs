using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;

namespace EFOfNorthWnd
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //GetProducts();
            //ShowProductsUseAsync();
            //UseView();
            UseStoreProcedures();
            //将查询出的产品绑定到datagrid上
            dgrdShowProduct.ItemsSource = Products;
            //将查询出视图上的产品绑定到datagrid上
            //dgrdShowProduct.ItemsSource = Current_Products;

        }
        /// <summary>
        /// 产品集合
        /// </summary>
        ObservableCollection<Product> Products = new ObservableCollection<Product>();
        /// <summary>
        /// 视图中产品的集合
        /// </summary>
        ObservableCollection<Current_Product_List> Current_Products = new ObservableCollection<Current_Product_List>();
        /// <summary>
        /// 从数据库中查询产品
        /// </summary>
        private void GetProducts()
        {
            using (var context = new NorthwindEntities())
            {
                //对查询出的数据按产品名称进行升序排序
                var ps = from p in context.Products
                         orderby p.ProductName ascending
                         select p;
                //查询出的产品添加到产品集合中
                foreach (var p in ps)
                {
                    Products.Add(p);
                }
            }
        }
        /// <summary>
        /// 使用异步的方法查询数据
        /// </summary>
        private async void ShowProductsUseAsync()
        {
            using (var context = new NorthwindEntities())
            {
                await context.Products.ForEachAsync((product) =>
                {
                    this.Dispatcher.Invoke(() => { dgrdShowProduct.Items.Add(product); });
                });
            }
        }
        /// <summary>
        /// 获取视图中的数据
        /// </summary>
        private void UseView()
        {
            using (var context = new NorthwindEntities())
            {
                //对查询出的数据按产品名称进行升序排序
                var ps = from p in context.Current_Product_Lists
                         orderby p.ProductName ascending
                         select p;
                //查询出的产品添加到产品集合中
                foreach (var p in ps)
                {
                    Current_Products.Add(p);
                }
            }
        }
        /// <summary>
        /// 使用存储过程
        /// </summary>
        private void UseStoreProcedures()
        {
            using (var context = new NorthwindEntities())
            {
                var product = context.GetProduct(1).First();
                if (product != null)
                {
                    Products.Add(product);
                }
            }
        }
    }
}
