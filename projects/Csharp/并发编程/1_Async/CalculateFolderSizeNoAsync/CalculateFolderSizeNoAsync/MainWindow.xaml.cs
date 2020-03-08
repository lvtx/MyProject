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

namespace CalculateFolderSizeNoAsync
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
        string path;
        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFolderDialog = new FolderBrowserDialog();
            if (openFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFolderName.Text = openFolderDialog.SelectedPath;
                path = openFolderDialog.SelectedPath;
            }
            CalculateFolderSizeDelegate d = Files.GetAllFile;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            txtStatus.Text = "正在查询";
            IAsyncResult result = d.BeginInvoke(path,
                (a) =>
                {
                    lstShowFiles.Dispatcher.Invoke(() =>
                    {
                        lstShowFiles.ItemsSource = Files.AllFile;
                        txtStatus.Text = txtFolderSize.Text = Files.Size.ToString() + "GB"; ;
                        txtFolderByteSize.Text = Files.ByteSize.ToString("N0") + "字节";
                        txtTime.Text = sw.ElapsedMilliseconds.ToString();
                        txtFilesCount.Text = lstShowFiles.Items.Count.ToString();
                        sw.Stop();
                        txtStatus.Text = "查询完成";
                    });
                }
                , null);
            #region "1.使用轮询"
            //while (result.IsCompleted == false)
            //{
            //    if (count < 7)
            //    {
            //        txtStatus.Dispatcher.Invoke(() =>
            //        {
            //            txtFolderSize.Text += ".";
            //        });
            //        count++;
            //    }
            //    else
            //    {
            //        txtStatus.Dispatcher.Invoke(() =>
            //        {
            //            txtFolderSize.Text += "正在查询";
            //        });
            //        count = 0;
            //    }
            //    Thread.Sleep(2000);
            //}
            #endregion

            #region "2.使用等待句柄"
            //while (!result.AsyncWaitHandle.WaitOne(2000))
            //{
            //    if (count < 7)
            //    {
            //        txtStatus.Dispatcher.Invoke(() =>
            //        {
            //            txtFolderSize.Text += ".";
            //        });
            //        count++;
            //    }
            //    else
            //    {
            //        txtStatus.Dispatcher.Invoke(() =>
            //        {
            //            txtFolderSize.Text += "正在查询";
            //        });
            //        count = 0;
            //    }
            //}
            #endregion
        }
    }
}
