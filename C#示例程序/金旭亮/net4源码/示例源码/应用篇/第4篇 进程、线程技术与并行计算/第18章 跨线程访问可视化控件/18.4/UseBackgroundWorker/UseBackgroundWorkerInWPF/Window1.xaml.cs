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
using System.ComponentModel;
using System.Threading;

namespace RunBackgroundWorderInWFP
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

        BackgroundWorker bw;

        private void Init()
        {
            bw = new BackgroundWorker();
            bw.WorkerReportsProgress= true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                txtInfo.Text = e.Error.Message;
            else
                if (e.Cancelled)
                    txtInfo.Text = "操作已取消";
                else
                    txtInfo.Text = "操作已完成";
            
            btnStart.IsEnabled = true;
            btnCancel.IsEnabled = false;
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            txtInfo.Text = "已完成了"+e.ProgressPercentage+"%";
            pgbProcess.Value = e.ProgressPercentage;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; ++i)
            { 
                int percent = i ;
                if ((sender as BackgroundWorker).CancellationPending)
                {
                    bw.ReportProgress(percent, "用户取消了操作");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    bw.ReportProgress(percent);
                }

                Thread.Sleep(300);
            }
        }
        
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            bw.RunWorkerAsync();
            btnCancel.IsEnabled = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            bw.CancelAsync();
        }
    }
}
