using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace ProcessInfo
{
    public partial class frmProcess : Form
    {
        public frmProcess()
        {
            InitializeComponent();
            GetAllProcesses();
        }
        //进程集合
        private Process[] Processes;
        //模块集合
        private ProcessModuleCollection Modules;
        //线程集合
        private ProcessThreadCollection Threads;


        //获取所有的进程清单
        private void GetAllProcesses()
        {
            Processes=Process.GetProcesses();
            lstProcesses.Items.Clear();
            foreach (Process p in Processes)
            {
                lstProcesses.Items.Add(p.ProcessName);
            }
            ClearAllInformation();

        }

        //获取进程的基本信息
        private string GetProcessBasicInfo(Process p)
        {
            string info = "";
            try
            {
                info += "------------------进程标识信息---------------\n";
                info += "进程的唯一标识符(Id):\t" + p.Id + "\n";
                info += "关联进程的本机句柄(Handle):\t" + p.Handle + "\n";
                info += "打开的句柄数(HandleCount):\t" + p.HandleCount + "\n";
                info += "关联进程的基本优先级(BasePriority):\t" + p.BasePriority + "\n";
                info += "\n------------------进程运行信息---------------\n";
                info += "进程启动的时间(StartTime):\t" + p.StartTime + "\n";


                info += "进程正在其上运行的计算机名称(MachineName):\t" + p.MachineName + "\n";
                info += "进程的主窗口标题(MainWindowTitle):\t" + p.MainWindowTitle + "\n";
                info += "进程主窗口的窗口句柄(MainWindowHandle):\t" + p.MainWindowHandle + "\n";
                info += "进程的用户界面当前是否响应(Responding):\t" + p.Responding + "\n";
                info += "进程的终端服务会话标识符(SessionId):\t" + p.SessionId + "\n";
                info += "进程终止时是否应激发 Exited 事件(EnableRaisingEvents):\t" + p.EnableRaisingEvents + "\n";

                info += "\n---------------进程运行时操作系统提供的服务---------------\n";

                info += "可安排此进程中的线程在其上运行的处理器(ProcessorAffinity):\t" + p.ProcessorAffinity + "\n";

                info += "进程允许的最大工作集大小(MaxWorkingSet):\t" + p.MaxWorkingSet + "\n";
                info += "进程允许的最小工作集大小(MinWorkingSet):\t" + p.MinWorkingSet + "\n";
                info += "分配给此进程的未分页的系统内存大小(NonpagedSystemMemorySize):\t" + p.NonpagedSystemMemorySize64 + "\n";
                info += "分页的内存大小(PagedMemorySize):\t" + p.PagedMemorySize64 + "\n";
                info += "分页的系统内存大小(PagedSystemMemorySize):\t" + p.PagedSystemMemorySize64 + "\n";
                info += "峰值分页内存大小(PeakPagedMemorySize):\t" + p.PeakPagedMemorySize64 + "\n";
                info += "峰值虚拟内存大小(PeakVirtualMemorySize):\t" + p.PeakVirtualMemorySize64 + "\n";
                info += "进程的峰值工作集大小(PeakWorkingSet):\t" + p.PeakWorkingSet64 + "\n";
                info += "专用内存大小(PrivateMemorySize):\t" + p.PrivateMemorySize64+ "\n";
                info += "进程的虚拟内存大小(VirtualMemorySize):\t" + p.VirtualMemorySize64 + "\n";
                info += "物理内存使用情况(WorkingSet):\t" + p.WorkingSet64 + "\n";
                info += "进程的特权处理器时间(PrivilegedProcessorTime):\t" + p.PrivilegedProcessorTime + "\n";
                info += "进程的总的处理器时间(TotalProcessorTime):\t" + p.TotalProcessorTime + "\n";
                info += "进程的用户处理器时间(UserProcessorTime):\t" + p.UserProcessorTime + "\n";

            }
            catch (Win32Exception e)
            {
                MessageBox.Show(e.Message);
            }
            catch (InvalidOperationException e)
            { 
                MessageBox.Show(e.Message);
            }
            ClearAllInformation();

            return info;


        }
        //当重新初始化时，需要重置所有的控件
        private void ClearAllInformation()
        {
            rtfProcessModule.Text = "";
            rtfBasicProcessInfo.Text = "";
            rtfThreadInfo.Text = "";
            lstModules.Items.Clear();
            
            lstThreads.Items.Clear();

        }

        //获取指定进程装载的所有模块（ .dll 或 .exe 文件）
        private void GetAllModules(Process p)
        {
            try
            {
                Modules = p.Modules;
                lstModules.Items.Clear();
                
                foreach (ProcessModule pm in Modules)
                {
                    lstModules.Items.Add(pm.ModuleName);
                }
            }
            catch (Win32Exception e)
            {
                MessageBox.Show(e.Message);
            }
            catch (InvalidOperationException e)
            {
                MessageBox.Show(e.Message);
            }

        }
        //获取指定模块的信息
        private string GetModuleInfo(ProcessModule pm)
        {
            string info = "";
            try
            {
                info += "模块的完整路径（FileName）:" + pm.FileName + "\n";
                info +="加载模块所需内存量（ModuleMemorySize）:" + pm.ModuleMemorySize + "\n";
                info += "加载模块的内存起始地址（BaseAddress）:" + pm.BaseAddress + "\n";
                info +="系统加载和运行模块时运行的函数的内存地址（EntryPointAddress）:" + pm.EntryPointAddress + "\n";
            }
            catch(Exception e)
            {
                MessageBox.Show(e.GetType().Name + ":" + e.Message);
            }
            return info;
        }

        //获取指定进程的线程信息
        private void GetAllThreads(Process p)
        {
            Threads = p.Threads;
            lstThreads.Items.Clear();
            foreach (ProcessThread pt in Threads)
            {
                lstThreads.Items.Add("线程" + pt.Id);
            }
        }

        //获取指定线程的信息
        private string GetThreadInfo(ProcessThread pt)
        {
            string info = "";
            try
            {
                info += "线程的基本优先级(BasePriority):\t" + pt.BasePriority + "\n";
                info += "线程的当前优先级(CurrentPriority):\t" + pt.CurrentPriority + "\n";
                info += "是否让操作系统自动调整线程优先级（PriorityBoostEnabled）:\t" + pt.PriorityBoostEnabled + "\n";
                info += "线程的优先级别（PriorityLevel）:\t" + pt.PriorityLevel + "\n";
                info += "在操作系统内核中运行代码所用的时间（PrivilegedProcessorTime）:\t" + pt.PrivilegedProcessorTime + "\n";
                info += "操作系统调用的、启动此线程的函数的内存地址（StartAddress）:\t" + pt.StartAddress + "\n";
                info += "操作系统启动该线程的时间（StartTime）:\t" + pt.StartTime + "\n";
                info += "此线程的当前状态（ThreadState）:\t" + pt.ThreadState + "\n";
                info += "此线程使用处理器的时间总量（TotalProcessorTime）:\t" + pt.TotalProcessorTime + "\n";
                info += "关联的线程在应用程序内(不是在操作系统内核)运行代码所用的时间（UserProcessorTime）:\t" + pt.UserProcessorTime + "\n";
                info += "线程等待的原因（WaitReason）:\t" + pt.WaitReason + "\n";


            }

            catch (Exception e)
            {
                MessageBox.Show(e.GetType().Name + ":" + e.Message);
            }
            return info;
        }


        //请求关闭进程
        private void CloseProcess()
        {
            if (lstProcesses.SelectedIndex == -1)
            {
                MessageBox.Show("请选择一个进程");
                return;
            }
            //获取当前进程
            Process curProc = Processes[lstProcesses.SelectedIndex];
            try
            {
                string processName = curProc.ProcessName;
                //关闭主窗体
                bool ret=curProc.CloseMainWindow();
                //释放进程资源
                curProc.Close();
                if(ret)
                    MessageBox.Show(String.Format("已成功向进程{0}发送了关闭请求",processName));
                //重新刷新进程列表
                GetAllProcesses();
            }

            catch (Exception e)
            {
                MessageBox.Show(e.GetType().Name + ":" + e.Message);
            }
        }


        //强制关闭进程
        private void KillProcess()
        {
            if (lstProcesses.SelectedIndex == -1)
            {
                MessageBox.Show("请选择一个进程");
                return;
            }
            Process curProc = Processes[lstProcesses.SelectedIndex];
            try
            {
                string processName = curProc.ProcessName;
                //强制关闭
                curProc.Kill();
                //释放进程资源
                curProc.Close();
                MessageBox.Show(String.Format("进程{0}已关闭", processName));
                //重新刷新进程列表
                GetAllProcesses();
            }

            catch (Exception e)
            {
                MessageBox.Show(e.GetType().Name + ":" + e.Message);
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetAllProcesses();
        }

        private void lstProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            rtfBasicProcessInfo.Text = GetProcessBasicInfo(Processes[lstProcesses.SelectedIndex]);
            GetAllModules(Processes[lstProcesses.SelectedIndex]);
            GetAllThreads(Processes[lstProcesses.SelectedIndex]);

            rtfProcessModule.Text = "";
            rtfThreadInfo.Text = "";
        }

        private void lstModules_SelectedIndexChanged(object sender, EventArgs e)
        {
           rtfProcessModule.Text= GetModuleInfo(Modules[lstModules.SelectedIndex]);
        }

        private void lstThreads_SelectedIndexChanged(object sender, EventArgs e)
        {
            rtfThreadInfo.Text = GetThreadInfo(Threads[lstThreads.SelectedIndex]);
        }

        private void btnCloseWindows_Click(object sender, EventArgs e)
        {
            CloseProcess();
            ClearAllInformation();
        }

        private void btnKillProcess_Click(object sender, EventArgs e)
        {
            KillProcess();
            ClearAllInformation();

        }

        private void btnNewProcess_Click(object sender, EventArgs e)
        {
            if (new frmStartNewProcess().ShowDialog() == DialogResult.OK)
                GetAllProcesses();//启动了新进程，则刷新进程信息

        }

     

      
    }
}