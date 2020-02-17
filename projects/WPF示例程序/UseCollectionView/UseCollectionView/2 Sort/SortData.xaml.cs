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
using System.Windows.Shapes;
using System.ComponentModel;

namespace UseCollectionView
{
    /// <summary>
    /// SortData.xaml 的交互逻辑
    /// </summary>
    public partial class SortData : Window
    {
        public SortData()
        {
            InitializeComponent();

        }
        private ListCollectionView lstView = null;
        ListView lstFiles = null;
        private void btnChooseDirectory_Click(object sender, RoutedEventArgs e)
        {
            string dir = App.ChooseDirectory();
            if (string.IsNullOrEmpty(dir))
            {
                txtDir.Text = "当前没有选中文件夹";
                FilesArea.Visibility = Visibility.Collapsed;
                lstView = null;
                return;
            }
            else
                txtDir.Text = "当前文件夹：" + dir;

            //提取数据并绑定显示
            lstFiles = ListViewContainer.Content as ListView;
            lstFiles.ItemsSource = App.GetFiles(dir);

            lstView = CollectionViewSource.GetDefaultView(lstFiles.ItemsSource) as ListCollectionView;
            FilesArea.Visibility = Visibility.Visible;

            rdoFileName.IsChecked = true;


        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rdo = e.OriginalSource as RadioButton;
            if ((rdo == null)||(lstView==null))
                return;

            lstView.SortDescriptions.Clear();
            switch (rdo.Name)
            {
                case "rdoFileName":
                    lstView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    break;
                case "rdoFileSize":
                    lstView.SortDescriptions.Add(new SortDescription("Length", ListSortDirection.Ascending));
                    break;
                case "rdoFileCreateTime":
                    lstView.SortDescriptions.Add(new SortDescription("CreationTime", ListSortDirection.Ascending));
                    break;
                case "rdoCustomSort":
                    //自定义排序方式
                    lstView.CustomSort = new SortByFileNameLength();
                    break;

            }
            
         
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = e.OriginalSource as CheckBox;
            if (chk == null)
                return;
        }

    }
}
