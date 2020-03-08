using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SplashScreenForWinForm
{

    public partial class frmSplash : Form
    {
        public frmSplash()
        {
            InitializeComponent();

            //显示初始化进程，可以在此定义您所需要的显示方式
            ShowProgress = delegate(String ProgressInfo)
            { 
                lblInfo.Text = String.Format("正在初始化：{0}%", ProgressInfo);
                lblInfo.Refresh(); //强制标签立即重绘，以及时显示间隔很短传入的信息
                progressBar1.Value = Convert.ToInt32(ProgressInfo);
            };

            CloseSplash = this.Close;

        }

            //显示初始化进程,供外界调用
            public  Action<string> ShowProgress ;
            //关闭Splash窗体，供外界调用
            public  Action CloseSplash;

        private void frmSplash_Load(object sender, EventArgs e)
        {
            //通知主线程启动窗体已创建完毕,从而让主线程开始启动初始化过程
            Program.mre.Set();
        }

 
   }
}