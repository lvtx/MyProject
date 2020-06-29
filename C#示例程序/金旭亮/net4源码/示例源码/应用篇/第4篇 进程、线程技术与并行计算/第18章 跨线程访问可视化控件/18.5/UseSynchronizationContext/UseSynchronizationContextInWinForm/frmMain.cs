using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace UseSynchronizationContextInWinForm
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            //获取同步上下文
            UIContext = SynchronizationContext.Current;

        }

        /// <summary>
        /// 按钮单击次数计数器
        /// </summary>
        private int counter = 0;

        /// <summary>
        /// 用于保存线程同步上下文
        /// </summary>
        private SynchronizationContext UIContext = null;

        /// <summary>
        /// 线程函数
        /// </summary>
        private void ThreadFunc(object ThreadArgument)
        {
            //将被“推送”到UI线程中执行的函数
            SendOrPostCallback func = delegate(object state)
            {
                lblInfo.Text = "按钮单击次数：" + state.ToString();
                
            };
            //将函数推送到UI线程中执行
            UIContext.Post(func, ThreadArgument);
        }
        private void btnUpdateLabel_Click(object sender, EventArgs e)
        {
            counter++;
            Thread th = new Thread(ThreadFunc);
            th.Start(counter);
        }
    }
}
