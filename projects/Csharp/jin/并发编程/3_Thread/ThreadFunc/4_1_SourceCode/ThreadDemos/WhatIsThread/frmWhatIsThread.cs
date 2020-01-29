using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WhatIsThread
{
    //让进度条显示进度的委托，用于跨线程调用
    public delegate void ShowProgressDelegate(ProgressBar prog, int value);
    //激活某个控件的委托，，用于跨线程调用
    public delegate void EnableControlDelegate (Control ctl);

 

    public partial class frmWhatIsThread : Form
    {
        //当启动线程时，向线程函数传送的信息
        class ThreadArgu
        {
            public ProgressBar prog; //用于显示工作进度的进度条对象
            public Button btnStart;  //用于启动多线程工作的按钮对象
        }


        public frmWhatIsThread()
        {
            InitializeComponent();
        }

        
        //一个执行比较长时间的函数(单线程)
        private void DoLongTaskInSingleThread()
        {
            for (int i = 0; i < 100; i++)
            {
                progressBarSingleThread.Value= i;
                Thread.Sleep(100);//暂停，以模拟长的工作任务
            }
        }

         //一个执行比较长时间的函数(多线程)
        private void DoLongTaskInMultiThread(Object argu)
        {
            for (int i = 0; i < 100; i++)
            {
                //通过委托跨线程设置进度条的进度值
                (argu  as ThreadArgu).prog.Invoke(new ShowProgressDelegate(SetProgressBarValue), new object[] { (argu  as ThreadArgu).prog,i });
                Thread.Sleep(100);//暂停，以模拟长的工作任务
            }
            //工作结束时，回调，以激活对应的按钮
            (argu as ThreadArgu).btnStart.Invoke(new EnableControlDelegate(EnabledControl), new object[] {  (argu as ThreadArgu).btnStart});
           
        }
      

        //设置进度条的进度值，用于跨线程委托调用
        private void SetProgressBarValue(ProgressBar prog,int value)
        {
            prog.Value = value;
        }

        //使某个控件激活，用于跨线程委托调用
        private void EnabledControl(Control ctl)
        {
            ctl.Enabled = true;
        }

        private void btnStartSingleThread_Click(object sender, EventArgs e)
        {
            lblSingleThreadID.Text  = Thread.CurrentThread.ManagedThreadId.ToString();
            lblSingleThreadID.Refresh(); //不加此句，则标签无法立即显示出信息
            btnStartSingleThread.Enabled = false;
            DoLongTaskInSingleThread();
            btnStartSingleThread.Enabled = true ;
        }
    
        //代表两个线程对象
       Thread  th1, th2;
        private void btnStartMultiThread1_Click(object sender, EventArgs e)
        {
            //灰掉自身，在线程完成前不允许用户再次点击按钮启动新的线程
            btnStartMultiThread1.Enabled = false;
            //创建线程对象
            th1 = new Thread(new ParameterizedThreadStart(DoLongTaskInMultiThread));
            
            //显示线程ID
            lblMultiThreadID1.Text = th1.ManagedThreadId.ToString();
            //封装参数
            ThreadArgu argu = new ThreadArgu();
            argu.prog = progressBarMultiThread1;
            argu.btnStart = btnStartMultiThread1;
            //启动线程
            th1.Start((Object)argu);
            
        }

        private void frmWhatIsThread_Load(object sender, EventArgs e)
        {
            lblMainThread.Text = Thread.CurrentThread.ManagedThreadId.ToString();
        }

        private void btnStartMultiThread2_Click(object sender, EventArgs e)
        {
            btnStartMultiThread2.Enabled = false;

            th2 = new Thread(new ParameterizedThreadStart(DoLongTaskInMultiThread));
           
            lblMultiThreadID2.Text = th2.ManagedThreadId.ToString();
            ThreadArgu argu = new ThreadArgu();
            argu.prog = progressBarMultiThread2;
            argu.btnStart = btnStartMultiThread2;
           
            th2.Start((Object)argu);
           

        }

        private void frmWhatIsThread_FormClosing(object sender, FormClosingEventArgs e)
        {
            //若关闭程序时线程还未终止，则强行中止它
            //也可以通过将线程设置为后台线程来自动完成这一工作
            if (th1 != null)
            {
                if (th1.ThreadState != ThreadState.Stopped)
                    th1.Abort();
            }
            if (th2 != null)
            {
                if (th2.ThreadState != ThreadState.Stopped)
                    th2.Abort();
            }
                
        }
    }

    
}