using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ButtonCounterForMultiFormUseStaticMethod
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new frmMain();
            Application.Run(MainForm);
        }

        static int Counter = 0; //单击计数器
        static frmMain MainForm = null;   //引用主窗体对象
        //统计单击次数，并在主窗体上显示统计结果
        public static void NotifyClicked()
        {
            //计数值累加
            Counter++;
            //通知主窗体更新显示
            if (MainForm != null)
            {
                MainForm.ShowCounter(Counter);
            }

        }
    }
}
