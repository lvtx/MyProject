using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace SplashScreenForWinForm
{

    //public delegate void ShowProgressDelegate(string ProgressInfo);
    //public delegate void CloseFormDelegate();
    //public delegate void SystemInitializeDelegate();

    static class Program
    {
       
        //启动屏幕对象引用
        static frmSplash splash ;
        //显示启动屏幕
        static void ShowSplash()
        {
            splash.ShowDialog();
            //系统初始化完成，显示主窗体
            Application.Run(new frmMain());
        }

        //线程同步对象
        public static ManualResetEvent mre = new ManualResetEvent(false);

        //系统初始化过程
        static void SystemInitialize()
        {
            mre.WaitOne();
            for (int i = 0; i < 100; i++)
            {
                splash.Invoke(splash.ShowProgress, new object[] { Convert.ToString(i) });

                Thread.Sleep(200);
            }
            //初始化完成，关闭启动屏幕
            splash.BeginInvoke(splash.CloseSplash);
           
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //创建启动屏幕
            splash = new frmSplash();

            //在辅助线程中显示启动窗体
            Thread thBegin=new Thread(ShowSplash);
            thBegin.Start();
            //在主线程中进行系统初始化工作
            SystemInitialize();
            
            
        }
     
  
    }
}