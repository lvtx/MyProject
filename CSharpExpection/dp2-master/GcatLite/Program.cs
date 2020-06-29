using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Diagnostics;

using DigitalPlatform;

namespace GcatLite
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ��ֹ����һ����Instance����
            Process[] processes = Process.GetProcessesByName("GcatLite");

            if (processes.Length > 1)
            {
                for (int i = 0; i < processes.Length; i++)
                {
                    IntPtr handle = processes[i].MainWindowHandle;
                    if (handle != (IntPtr)API.INVALID_HANDLE_VALUE)
                    {
                        API.ShowWindow(handle,API.SW_RESTORE);  // ����С��������󻯵Ĵ��ڸ�ԭ
                        API.SetForegroundWindow(handle);    // �����ڷ���ǰ̨
                    }
                }
                return;
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}