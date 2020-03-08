using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace UseBackgroundWorker
{
    public partial class frmShowPercentage : Form
    {
        public frmShowPercentage()
        {
            InitializeComponent();
        }
      
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //获取BackgroundWorker组件的引用
           BackgroundWorker bw=sender as BackgroundWorker;
           
            //开始计算1＋2＋……＋100
            int result = 0; //暂存结果
            for (int i = 0; i <=100; i++)
            {
                //如果用户取消了操作
                if (bw.CancellationPending)
                {
                    e.Cancel = true;//此结果将会被传送到RunWorkerCompleted事件中
                    return ; //提前结束工作任务
                }
                result += i; //累加
                
                //报告进度
                bw.ReportProgress(i,"已完成了"+(i).ToString()+"%");
                Thread.Sleep((int)e.Argument); //外部传入的参数，休眠特定的时间
            }

            e.Result = result; //向外界返回1＋2＋……＋100的值

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;//显示百分比
            lblInfo.Text = e.UserState.ToString(); //显示文字信息
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                toolStripStatusLabel1.Text = "用户取消了操作";
                btnStart.Enabled = true;
                return;
            }
            if (e.Error != null)
            {
                toolStripStatusLabel1.Text = e.Error.Message;
                return;
            }
            lblResult.Text = e.Result.ToString();
            btnStart.Enabled = true;
            toolStripStatusLabel1.Text = "计算完成";

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            lblResult.Text = "?";
            lblResult.Refresh();
            backgroundWorker1.RunWorkerAsync(200);
            btnStart.Enabled = false;
            toolStripStatusLabel1.Text = "正在工作……";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

  
}