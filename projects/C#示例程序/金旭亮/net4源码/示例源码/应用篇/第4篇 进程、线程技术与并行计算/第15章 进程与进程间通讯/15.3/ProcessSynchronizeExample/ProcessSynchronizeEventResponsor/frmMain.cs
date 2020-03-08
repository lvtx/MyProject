using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ProcessSynchronizeEventResponsor
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
        /// 用于判断是否只运行了一个实例
        /// </summary>
        private const string MyProcessName = "ProcessSynchronizeEventResponsor";

        /// <summary>
        /// 线程同步事件的名字
        /// </summary>
        private const string ProcessSynchronizeEventName = "ProcessSynchronizeEvent";

        private void GetEventHandle()
        {
            try
            {
                hEvent = EventWaitHandle.OpenExisting(ProcessSynchronizeEventName);
                //如果不可以打开EventWaitHandle，说明服务端进程还未运行,则关闭本程序
                if (hEvent != null)
                {
                    Thread th = new Thread(WaitForEvent);
                    th.IsBackground = true;
                    th.Start();
                    
                   lblInfo.Text = "成功获取EventWaitHandle对象，等待服务端进程单击按钮";
                }

            }
            catch (WaitHandleCannotBeOpenedException)
            {

                MessageBox.Show("请先运行程序ProcessSynchronizeEventSource的一个实例！");

                Close();
               
            }
        }

        /// <summary>
        /// 计数器
        /// </summary>
        private int ClickCounter = 0;

        /// <summary>
        /// 线程函数
        /// </summary>
        void WaitForEvent()
        {
            while (true)
            {
                hEvent.WaitOne();
                ClickCounter++;
                string Info = string.Format("自本程序运行以来，检测到服务端进程单击了{0}次按钮", ClickCounter);

                //跨线程更新可视化界面
                Action<string> ShowInfo = delegate(string info)
                {
                    lblInfo.Text = info;
                };

                this.Invoke(ShowInfo, Info);

               
                hEvent.Reset();
            }
        }

      

        /// <summary>
        /// 是否仅一个程序实例在运行？
        /// </summary>
        /// <returns></returns>
        private bool IsOnlyMe()
        {
            try
            {
                Mutex.OpenExisting(MyProcessName);
                return false;

            }
            catch (WaitHandleCannotBeOpenedException)
            {
               Mutex mtx= new Mutex(false, MyProcessName);

                return true;
            }
            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (!IsOnlyMe())
            {
                MessageBox.Show("本程序只能运行一个实例");
                Close();
            }
            else
            GetEventHandle();
        }

        
    }
}
