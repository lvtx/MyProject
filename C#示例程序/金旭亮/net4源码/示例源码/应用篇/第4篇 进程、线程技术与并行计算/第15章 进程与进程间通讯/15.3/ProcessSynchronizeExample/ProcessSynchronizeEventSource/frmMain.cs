using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ProcessSynchronizeEventSource
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
          
           
        }
        /// <summary>
        /// 用于实现跨进程同步的事件
        /// </summary>
        private EventWaitHandle hEvent = null;

       
        /// <summary>
        /// 线程同步事件的名字
        /// </summary>
        private const string ProcessSynchronizeEventName = "ProcessSynchronizeEvent";

        private void GetEventHandle()
        {
            try
            {
                hEvent = EventWaitHandle.OpenExisting(ProcessSynchronizeEventName);
                //如果可以打开EventWaitHandle，说明已有一个实例运行,则关闭本程序
                if (hEvent != null)
                {
                    MessageBox.Show("只能运行本程序的一个实例！");
                    hEvent = null;
                    Close();
                   
                }
               
            }
            catch (WaitHandleCannotBeOpenedException)
            {

                hEvent = new EventWaitHandle(false, EventResetMode.ManualReset, ProcessSynchronizeEventName);
                lblInfo.Text = "EventWaitHandle已创建！";
            }
        }

        private void btnClickMe_Click(object sender, EventArgs e)
        {
            hEvent.Set();
           
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            GetEventHandle();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
    }
}
