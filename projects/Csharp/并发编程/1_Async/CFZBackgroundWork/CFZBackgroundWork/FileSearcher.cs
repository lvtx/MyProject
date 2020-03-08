using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows;
using System.Threading;

namespace CFZBackgroundWork
{
    public class FileSearcher : BackgroundWorker
    {
        public FileSearcher() : base()
        {
            this.WorkerReportsProgress = true;//允许报告进度
            this.WorkerSupportsCancellation = true;//支持取消操作
        }
        /// <summary>
        /// 外界传入一个用于显示文件名的委托
        /// </summary>
        public Action<FileInfo[]> DisplayFileName = null;
        /// <summary>
        /// 外界传入一个用于显示文件数量的委托
        /// </summary>
        public Action DisplayFileCount = null;
        /// <summary>
        /// 外界传入一个用于显示文件大小的委托
        /// </summary>
        public Action<long> DisplayFileSize = null;
        /// <summary>
        /// ListBox中执行的异步操作
        /// </summary>
        AsyncOperation asyncDisplayFileName = AsyncOperationManager.CreateOperation(null);
        /// <summary>
        /// TextBlock显示文件个数的异步操作
        /// </summary>
        AsyncOperation asyncDisplayFileCount = AsyncOperationManager.CreateOperation(null);
        /// <summary>
        /// TextBlock显示文件尺寸的异步操作
        /// </summary>
        AsyncOperation asyncDisplayFileSize = AsyncOperationManager.CreateOperation(null);
        /// <summary>
        /// 提供要搜索文件的相关信息
        /// </summary>
        private string path;

        public string Path
        {
            get { return path; }
            set
            {
                if (Directory.Exists(value) == false)
                {
                    throw new DirectoryNotFoundException("文件夹不存在");
                }
                else
                    path = value;
            }
        }

        /// <summary>
        /// 一个保存有完整搜索结果的列表
        /// </summary>
        private ObservableCollection<FileInfo> foundFiles = new ObservableCollection<FileInfo>();
        public ObservableCollection<FileInfo> FoundFiles 
        {
            get
            {
                return foundFiles;
            }
        }
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            //在此完成异步搜索文件的工作
            DoSearchAsync(path, e);
            base.OnDoWork(e);
        }
        /// <summary>
        /// 完成递归搜索文件的功能
        /// </summary>
        /// <param name="searchInfoObj"></param>
        /// <param name="e"></param>
        private void DoSearchAsync(string path, DoWorkEventArgs e)
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            FileInfo[] files = null;
            DirectoryInfo[] subFolders = null;
            try
            {
                //注：有可能发生权限不足的异常，因为某些文件夹不允许检索文件和子文件夹

                //获取所有的子文件夹
                subFolders = folder.GetDirectories();
                //得到当前文件夹下的文件
                files = folder.GetFiles();
            }
            catch { }
            //如果用户取消了操作
            if (this.CancellationPending)
            {
                e.Cancel = true;//此结果将会被传送到RunWorkerCompleted事件中
                return;
            }
            if (files != null)
            {
                foreach (var file in files)
                {
                    foundFiles.Add(file);//将查询到的文件添加到集合中
                    //size += fil.Length;
                    //count++;
                }
                if (DisplayFileName != null)//外界挂接了要显示中间结果的函数
                {
                    SendOrPostCallback lstDisplayFileCallback = (result) => { DisplayFileName(result as FileInfo[]); };
                    asyncDisplayFileName.Post(lstDisplayFileCallback, files);//在UI线程中执行
                }
                if (DisplayFileCount != null)
                {
                    SendOrPostCallback DisplayFileCountCallback = (a) => { DisplayFileCount(); };
                    asyncDisplayFileName.Post(DisplayFileCountCallback, files);//在UI线程中执行
                }
                //获取当前文件夹下的子文件夹
                if (subFolders != null)//如果没有发生存取权限异常，则此变量不会为null
                {
                    foreach (var fol in subFolders)
                    {
                        DoSearchAsync(fol.FullName, e);
                    }
                }
            }

            //private ObservableCollection<FileInfo> files = new ObservableCollection<FileInfo>();
            //public ObservableCollection<FileInfo> AllFile
            //{
            //    get { return files; }
            //}
            //private int count;
            //public int Count
            //{
            //    get { return count; }
            //    set { count = value; }
            //}

            //private long size;
            //public long Size
            //{
            //    get { return size / 1024 / 1024 / 1024; }
            //}
            //public long ByteSize
            //{
            //    get { return size; }
            //}
        }
    }
}
