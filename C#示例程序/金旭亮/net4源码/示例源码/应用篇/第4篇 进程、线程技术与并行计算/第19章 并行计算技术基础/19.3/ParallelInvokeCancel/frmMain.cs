using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace ParallelInvoke
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 线程安全的设置进度条方法
        /// </summary>
        /// <param name="index">第几个进度条控件</param>
        /// <param name="value">设置的值</param>
        public void SetProgressBarValue(int index, int value)
        {

            ProgressBar pgb = null;
            switch (index)
            {
                case 1:
                    pgb = progressBar1;
                    break;
                case 2:
                    pgb = progressBar2;
                    break;
                case 3:
                    pgb = progressBar3;
                    break;
            }
            if (this.InvokeRequired)
            {
                Action<int> del = delegate(int argu)
                {
                    pgb.Value = argu;
                };
                this.BeginInvoke(del, value);
            }
            else
                pgb.Value = value;
        }

        /// <summary>
        /// 用于同步3个线程
        /// </summary>
        private CountdownEvent threadCounter = new CountdownEvent(3);

        /// <summary>
        /// 用于取消并行操作
        /// </summary>
        private CancellationTokenSource cts = null;
        /// <summary>
        /// 线程安全的激活或屏蔽按钮方法
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="Value"></param>
        private void EnableOrDisableButton(Button btn, bool Value)
        {
            Action del = delegate()
            {
                if (btn.Enabled != Value)
                    btn.Enabled = Value;
            };

            if (btn.InvokeRequired)
            {
                btn.Invoke(del);
            }
            else
                del();
        }

        /// <summary>
        /// 将要并行执行的任务函数
        /// </summary>
        /// <param name="index"></param>
        private void DoWork(int index)
        {
            Random ran = new Random();
            for (int i = 0; i < 100; i++)
            {
                //检测外界是否要求取消工作，如果是，抛出OperationCanceledException异常
                cts.Token.ThrowIfCancellationRequested();

                //如果取消下面的注释，则Parallel.Invoke()将抛出AggregateException
                //if (cts.Token.IsCancellationRequested)
                //    throw new OperationCanceledException();

                Thread.Sleep(ran.Next(0, 1000)); //随机休眠
                SetProgressBarValue(index, i);  //设置进度条值

            }
            threadCounter.Signal();//线程计数减一
            threadCounter.Wait();   //等待其他线程的结束

           
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            threadCounter.Reset();  //复原线程同步对象

            cts = new CancellationTokenSource();  

            ParallelOptions opt = new ParallelOptions();
            opt.CancellationToken = cts.Token;
            //创建一个线程函数，在此函数中启动并行任务
            ThreadStart del = delegate()
            {
                try
                {
                    Parallel.Invoke(
                    opt,    //关联上一个取消令牌
                    () => DoWork(1),    //并行任务1
                    () => DoWork(2),    //并行任务2
                    () => DoWork(3)     //并行任务3
                        );
                }

                   
               
               
                catch (Exception ex)
                {
                    string info = string.Format("{0}:{1}", ex.GetType(), ex.Message);
                    MessageBox.Show(info);
                }
                finally
                {
                    //所有线程都结束了，激活“启动”按钮，屏蔽“取消”按钮
                    EnableOrDisableButton(btnStart, true);
                    EnableOrDisableButton(btnCancel, false);
                }
                
            };

            Thread th = new Thread(del);

            th.IsBackground = true;  //当用户关闭窗体时，强制结束未完成的线程
            th.Start(); //在另一个线程中启动并行任务

            //激活"取消"按钮，屏蔽“启动”按钮
            EnableOrDisableButton(btnStart, false);
            EnableOrDisableButton(btnCancel, true);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(cts!=null)
            cts.Cancel();  //发出取消并行任务的命令

        }
    }
}
