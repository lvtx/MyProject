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
using System.IO;

namespace MyFileSearcherForWPF
{
    /// <summary>
    /// frmFileSearcher.xaml 的交互逻辑
    /// </summary>
    public partial class frmFileSearcher : Window
    {

        #region "重载的构造函数"


        public frmFileSearcher(SearchInfo info)
        {
            InitializeComponent();

            SearchInfoObj = info;
            Searcher = new FileSearcher();
            Searcher.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(Searcher_RunWorkerCompleted);

        }

        void Searcher_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            btnCancel.IsEnabled = false;
            btnBeginSearch.IsEnabled = true;
            if (e.Cancelled)
                txtInfo2.Text = "搜索任务取消，已搜索到" + lstFiles.Items.Count.ToString() + "个文件。";
            else
                txtInfo2.Text = "搜索任务已完成，共搜索到" + lstFiles.Items.Count.ToString() + "个文件。";
        }
        #endregion

        #region "变量区"
        private SearchInfo _SearchInfoObj = null;
        public SearchInfo SearchInfoObj
        {
            get
            {
                return _SearchInfoObj;
            }
            set
            {
                _SearchInfoObj = value;
                txtInfo.Text = string.Format("正在搜索：{0}，起始目录：{1}", value.SearchFile, value.BeginDirectory);
            }
        }
        /// <summary>
        /// 文件搜索器
        /// </summary>
        private FileSearcher Searcher = null;


        #endregion

        /// <summary>
        /// 显示当前已找到的文件

        /// </summary>
        /// <param name="files"></param>
        private void ShowSearchedFiles(FileInfo[] files)
        {

            foreach (FileInfo file in files)
                lstFiles.Items.Add(file.FullName);

        }

        private void btnBeginSearch_Click(object sender, RoutedEventArgs e)
        {
            lstFiles.Items.Clear();
            txtInfo2.Text = "点击“取消”按钮停止查找";
            BeginSearch();
            btnCancel.IsEnabled = true;
            btnBeginSearch.IsEnabled = false;
        }

        /// <summary>
        /// 启动搜索
        /// </summary>
        public void BeginSearch()
        {
            //挂接显示函数，此函数将在搜索过程中不断地被fileSearcher组件所调用。
            Searcher.ShowSearchResult = this.ShowSearchedFiles;
            //向fileSearcher组件提供要搜索文件的信息
            Searcher.SearchInfoObj = this.SearchInfoObj;
            //启动搜索
            Searcher.RunWorkerAsync();  //启动搜索
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Searcher.CancelAsync();
            btnCancel.IsEnabled = false;
            btnBeginSearch.IsEnabled = true;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BeginSearch();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Searcher.IsBusy)
            {
                MessageBox.Show("搜索正在进行中，请先取消搜索任务后再关闭程序", "提示信息",
                    MessageBoxButton.OK, MessageBoxImage.Stop);
                e.Cancel = true;
            }
        }
    }
}
