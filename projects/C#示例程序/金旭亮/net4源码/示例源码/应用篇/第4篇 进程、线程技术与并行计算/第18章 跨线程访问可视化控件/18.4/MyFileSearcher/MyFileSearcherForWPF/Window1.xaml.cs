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
using WinForms = System.Windows.Forms;

namespace MyFileSearcherForWPF
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            folderBrowserDialog1 = new WinForms.FolderBrowserDialog();
            folderBrowserDialog1.Description = "选择要搜索的文件夹";
        }
        /// <summary>
        /// 文件夹选择组件框
        /// </summary>
        private WinForms.FolderBrowserDialog folderBrowserDialog1 =null;

        private void btnChooseSearchDirectory_Click(object sender, RoutedEventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == WinForms.DialogResult.OK)
            {
                this.txtBeginDirectory.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnBeginSearch_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearchFileName.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入要查找的文件名：", "提示信息",MessageBoxButton.OK,MessageBoxImage.Information);
                txtSearchFileName.Focus();
                return;
            }
            if (this.txtBeginDirectory.Text.Trim().Length == 0)
            {
                MessageBox.Show("请选择查找起始目录：", "提示信息", MessageBoxButton.OK, MessageBoxImage.Information);
                btnChooseSearchDirectory.Focus();
                return;
            }
            SearchInfo info = new SearchInfo { BeginDirectory = this.txtBeginDirectory.Text.Trim(), SearchFile = txtSearchFileName.Text.Trim() };
            frmFileSearcher frm = new frmFileSearcher(info);
            frm.Show();
        }
    }
}
