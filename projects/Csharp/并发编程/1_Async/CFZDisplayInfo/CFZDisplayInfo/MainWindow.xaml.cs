using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CFZDisplayInfo
{
    public delegate void CalculateFolderSizeDelegate(string path);
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 取消线程运行
        /// </summary>
        private volatile bool IsCancel = false;
        /// <summary>
        /// 选择的文件夹路径
        /// </summary>
        string path;
        /// <summary>
        /// 选择一个文件夹
        /// </summary>
        private bool SelectFolder()
        {
            FolderBrowserDialog openFolderDialog = new FolderBrowserDialog();
            System.Windows.Forms.DialogResult ret = openFolderDialog.ShowDialog();
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                path = openFolderDialog.SelectedPath;
                return true;
            }
            else if (ret == System.Windows.Forms.DialogResult.Cancel)
            {
                System.Windows.MessageBox.Show("选择不能为空");
            }
            return false;
        }
        private void LstShowInfo()
        {
            //this.Dispatcher.Invoke(() => { lstShowFiles.ItemsSource = Files.AllFile; });
            //lstShowFiles.Items.Refresh();
            lstShowFiles.Items.Add(Files.AllFile);
            lstShowFiles.Items.Clear();
        }
        private void DisplaySearchInfo()
        {
            CalculateFolderSizeDelegate d = Files.GetAllFile;
            IAsyncResult ret = d.BeginInvoke(path, null, null);
            while (!ret.IsCompleted)
            {
                string info = Files.Count.ToString();
                Thread.Sleep(300);
                Action<string> ShowCount = (information) =>
                {
                    txtFilesCount.Text = information;
                };
                if (IsCancel == false)
                {
                    this.Dispatcher.Invoke(ShowCount, new object[] { info });
                }
                else
                {
                    info = "操作已取消";
                    Action<string> ShowStatus = (information) =>
                    {
                        txtStatus.Text = information;
                        txtFilesCount.Text = "0";
                    };
                    Files.Count = 0;
                    this.Dispatcher.Invoke(ShowStatus, DispatcherPriority.Normal, new object[] { info });
                    IsCancel = false;
                    return;
                }
            }
            this.Dispatcher.Invoke(() =>
            {
                txtFilesCount.Text = Files.Count.ToString();
                lstShowFiles.ItemsSource = Files.AllFile;
                txtFolderSize.Text = Files.Size.ToString();
                btnOpenFolder.IsEnabled = true;
            });

        }

        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            bool IsSelected = SelectFolder();
            if (IsSelected)
            {
                txtFolderName.Text = path;
                btnOpenFolder.IsEnabled = false;
                Thread thread = new Thread(DisplaySearchInfo);
                thread.IsBackground = true;
                thread.Start();
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            IsCancel = true;
            btnOpenFolder.IsEnabled = true;
        }
        //CalculateFolderSizeDelegate d = Files.GetAllFile;
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        //txtStatus.Text = "正在查询";
        //IAsyncResult result = d.BeginInvoke(path,
        //    (a) =>
        //    {
        //        lstShowFiles.Dispatcher.Invoke(() =>
        //        {
        //lstShowFiles.ItemsSource = Files.AllFile;
        //txtStatus.Text = txtFolderSize.Text = Files.Size.ToString() + "GB"; ;
        //txtFolderByteSize.Text = Files.ByteSize.ToString("N0") + "字节";
        //txtTime.Text = sw.ElapsedMilliseconds.ToString();
        //txtFilesCount.Text = lstShowFiles.Items.Count.ToString();
        //sw.Stop();
        //txtStatus.Text = "查询完成";
        //        });
        //    }
        //    , null);
    }
}
