using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace DataBindingAndAsyn
{    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker worker = new BackgroundWorker();
        public MainWindow()
        {
            InitializeComponent();
            InitializeWorker();
        }
        int reportCount = 0;
        private void InitializeBinding()
        {

        }
        private void InitializeWorker()
        {
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //开始计算1+2+……+100
            int result = 0;//保存结果
            int i = 0;
            for (i = 0; i <= 100; i++)
            {
                //如果用户取消操作
                if (worker.CancellationPending)
                {
                    //此结果将传送到RunWorkerCompleted事件中
                    e.Cancel = true;
                    return;//提前结束
                }
                result += i;//累加
                //报告进度
                worker.ReportProgress(i, "已完成了" + i.ToString() + "%");
                //外部传入的参数，休眠特定时间
                Thread.Sleep((int)e.Argument);
            }
            //向外界返回1+2+……+100的值
            e.Result = result;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                StatusLabel.Content = "用户取消了操作";
                btnStart.IsEnabled = true;
                return;
            }
            if (e.Error != null)
            {
                StatusLabel.Content = e.Error.Message;
                return;
            }
            lblResult.Content = string.Format("1 + 2 + 3 + 4 + … + 100 = " + e.Result.ToString());
            btnStart.IsEnabled = true;
            StatusLabel.Content = "计算完成";
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarOne.Value = e.ProgressPercentage; //显示百分比
            //lblShowProgress.Content = e.UserState.ToString(); //显示文字信息
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync(200);
            btnStart.IsEnabled = false;
            StatusLabel.Content = "正在工作……";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
