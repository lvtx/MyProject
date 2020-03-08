using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace MyFileSearcherForWPF
{
    /// <summary>
    /// 实现文件搜索功能
    /// </summary>
    public class FileSearcher : BackgroundWorker
    {
        public FileSearcher()
            : base()
        {
            this.WorkerReportsProgress = true;  //允许报告进度
            this.WorkerSupportsCancellation = true;　//支持用户取消操作
        }

        /// <summary>
        /// 外界传入的用于显示搜索信息的委托，有一个参数表示文件列表
        /// </summary>
        public Action<FileInfo[]> ShowSearchResult = null;

        /// <summary>
        /// 代表一个将在UI线程中执行的异步操作
        /// </summary>
        AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(null);

        /// <summary>
        /// 提供要搜索文件的相关信息
        /// </summary>
        public SearchInfo SearchInfoObj = null;

        /// <summary>
        /// 一个保存有完整搜索结果的列表
        /// </summary>
        public List<FileInfo> FoundFiles = new List<FileInfo>();

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            //在此完成异步搜索文件的工作
            DoSearchAsync(SearchInfoObj.SearchFile, SearchInfoObj.BeginDirectory, e);
            base.OnDoWork(e);
        }


        /// <summary>
        /// 完成递归搜索文件的功能
        /// </summary>
        /// <param name="SearchFile"></param>
        /// <param name="BeginDirectory"></param>
        /// <param name="e"></param>
        void DoSearchAsync(string SearchFile, string BeginDirectory, DoWorkEventArgs e)
        {
            if (Directory.Exists(BeginDirectory) == false)
                throw new DirectoryNotFoundException("文件夹不存在");
            DirectoryInfo RootDir = new DirectoryInfo(BeginDirectory);
            FileInfo[] files = null;
            DirectoryInfo[] ChildDirs = null;

            try
            {
                //注：有可能发生权限不足的异常，因为某些文件夹不允许检索文件和子文件夹

                //获取所有的子文件夹
                ChildDirs = RootDir.GetDirectories();
                //获取当前文件夹中的所有符合条件的文件
                files = RootDir.GetFiles(SearchInfoObj.SearchFile, SearchOption.TopDirectoryOnly);
            }
            catch //不处理可能发生的权限异常，继续搜索
            {
            }
            //如果用户取消了操作
            if (this.CancellationPending)
            {
                e.Cancel = true;//此结果将会被传送到RunWorkerCompleted事件中
                return;//提前结束工作任务
            }

            if (files != null && files.Length > 0 && ShowSearchResult != null)
            {
                FoundFiles.AddRange(files);  //将找到的文件加入到结果集合中

                if (ShowSearchResult != null)  //外界挂接了要显示中间结果的函数
                {
                    SendOrPostCallback UICallback = (result) => { ShowSearchResult(result as FileInfo[]); };
                    asyncOperation.Post(UICallback, files);  //在UI线程中执行它们
                }

                // ShowSearchResult(files);//调用外界传入的委托，向外界报告本次找到的文件

                //激发ProgressChanged事件，外界可显示工作处理情况
                this.ReportProgress(0, "在文件夹 “" + RootDir.Name + " ”中找到了需要的文件，正在处理中...");
            }



            if (ChildDirs != null)  //如果没有发生存取权限异常，则此变量不会为null
            {
                //对每个子文件夹执行同样的查找过程
                //这是通过递归调用实现的
                foreach (DirectoryInfo dir in ChildDirs)
                {
                    DoSearchAsync(SearchFile, dir.FullName, e);
                }
            }
        }

    }
}
