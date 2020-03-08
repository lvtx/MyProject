using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;

namespace CalculateFolderSizeUseTPL
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        //计算指定文件夹的总容量
        private long CalculateFolderSize(string FolderName)
        {
            if (Directory.Exists(FolderName) == false)
            {
                throw new DirectoryNotFoundException("文件夹不存在");
            }

            DirectoryInfo RootDir = new DirectoryInfo(FolderName);
            //获取所有的子文件夹
            DirectoryInfo[] ChildDirs = RootDir.GetDirectories();
            //获取当前文件夹中的所有文件
            FileInfo[] files = RootDir.GetFiles();
            long totalSize = 0;
            //累加每个文件的大小
            foreach (FileInfo file in files)
            {
                totalSize += file.Length;
            }
            //对每个文件夹执行同样的计算过程：累加其下每个文件的大小
            //这是通过递归调用实现的
            foreach (DirectoryInfo dir in ChildDirs)
            {
                totalSize += CalculateFolderSize(dir.FullName);
            }
            //返回文件夹的总容量
            return totalSize;

        }

        /// <summary>
        /// 跨线程显示信息
        /// </summary>
        /// <param name="information"></param>
        private void ShowInfo(string information)
        {
            Action<string> del = (info) =>
                {
                    rtfInfo.AppendText(info + "\n");
                };
            if (this.InvokeRequired)
                this.BeginInvoke(del, information);
            else
                del(information);
        }

        /// <summary>
        /// 将同步方法CalculateFolderSize包装为任务对象
        /// </summary>
        /// <param name="FolderName"></param>
        /// <returns></returns>
        private Task<long> GetCalculateTask(string FolderName)
        {
            var tcs = new TaskCompletionSource<long>();
            try
            {
                tcs.SetResult(CalculateFolderSize(FolderName));
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return tcs.Task;
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string FolderName = folderBrowserDialog1.SelectedPath;
                Task.Factory.StartNew(() =>
                {
                    ShowInfo("正在计算文件夹的容量：" + FolderName);
                    Task<long> task = GetCalculateTask(FolderName);
                    try
                    {
                        ShowInfo(FolderName + "的大小为：" + task.Result.ToString() + "字节。");
                    }
                    catch (Exception ex)
                    {
                        if (ex is AggregateException)
                        {
                            var ae = ex as AggregateException;
                            ae.Flatten();
                            foreach (Exception taskerror in ae.InnerExceptions)
                                ShowInfo(taskerror.Message);
                        }
                        else
                            ShowInfo(ex.Message);
                    }
                });



            }
        }
    }
}
