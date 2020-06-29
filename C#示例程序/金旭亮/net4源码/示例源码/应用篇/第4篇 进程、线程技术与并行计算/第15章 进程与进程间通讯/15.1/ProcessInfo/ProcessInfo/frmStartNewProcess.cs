using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace ProcessInfo
{
    public partial class frmStartNewProcess : Form
    {
        public frmStartNewProcess()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = openFileDialog1.FileName;
                GetVerbs(txtFile.Text);
            }
        }

        //获取程序或文档支持的谓词
        private void GetVerbs(string FileName)
        {
            ProcessStartInfo si = new ProcessStartInfo(FileName);
            cboVerb.Items.Clear();
            foreach (string verb in si.Verbs)
            {
                cboVerb.Items.Add(verb);
            }

        }

        //根据用户输入的信息创建ProcessStartInfo对象
        private ProcessStartInfo GetStartInfo()
        {
            ProcessStartInfo info = new ProcessStartInfo(txtFile.Text);
            //获取用户手工输入的命令行参数
            if (txtArguments.Text != "")
                info.Arguments = txtArguments.Text;
            //获取用户选择的谓词
            if (cboVerb.SelectedIndex != -1)
                info.Verb = cboVerb.Items[cboVerb.SelectedIndex].ToString(); 
            //用户希望主窗体的状态：最大化，最小化，隐藏还是顺其自然？
            if (cboWindowStyle.SelectedIndex != -1)
                switch (cboWindowStyle.SelectedIndex)
                {
                    case 0:
                        info.WindowStyle = ProcessWindowStyle.Hidden ;
                        break;
                    case 1:
                        info.WindowStyle=ProcessWindowStyle.Maximized; 
                        break;
                    case 2:
                        info.WindowStyle=ProcessWindowStyle.Minimized ;
                        break ;
                    case 3:
                        info.WindowStyle = ProcessWindowStyle.Normal;
                        break;
                }
            return info;

        }
        public  void StartWithVerb(string fileName, string verb, string args)
        {
            if (((fileName != null) && (fileName.Length > 0)) &&
                ((verb != null) && (verb.Length > 0)))
            {
                if (File.Exists(fileName))
                {
                    ProcessStartInfo startInfo;
                    startInfo = new ProcessStartInfo(fileName);

                    startInfo.Verb = verb;
                    startInfo.Arguments = args;

                    Process newProcess = new Process();
                    newProcess.StartInfo = startInfo;

                    try
                    {
                        newProcess.Start();

                        Console.WriteLine(
                            "{0} for file {1} started successfully with verb \"{2}\"!",
                            newProcess.ProcessName, fileName, startInfo.Verb);
                    }
                    catch (System.ComponentModel.Win32Exception e)
                    {
                        Console.WriteLine("  Win32Exception caught!");
                        Console.WriteLine("  Win32 error = {0}",
                            e.Message);
                    }
                    catch (System.InvalidOperationException)
                    {
                        // Catch this exception if the process exits quickly, 
                        // and the properties are not accessible.
                        Console.WriteLine("File {0} started with verb {1}",
                            fileName, verb);
                    }
                }
                else
                {
                    Console.WriteLine("File not found:  {0}", fileName);
                }
            }
            else
            {
                Console.WriteLine("Invalid input for file name or verb.");
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            Process.Start(GetStartInfo());
        }
    }
}