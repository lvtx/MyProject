using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MyFileSearcher
{
    public partial class frmFileSearcher : Form
    {
        #region "重载的构造函数"

        public frmFileSearcher()
        {
            InitializeComponent();
        }
        public frmFileSearcher(SearchInfo info)
        {
            InitializeComponent();

            SearchInfoObj = info;
        }
        #endregion

        #region "变量区"
        private SearchInfo _SearchInfoObj = null;
        public SearchInfo SearchInfoObj
        {
            get
            {
                return _SearchInfoObj;
            }
            set
            {
                _SearchInfoObj = value;
                lblInfo.Text = string.Format("正在搜索：{0}，起始目录：{1}", value.SearchFile, value.BeginDirectory);
            }
        }
        /// <summary>
        /// 文件搜索器
        /// </summary>
        private FileSearcher searcher = new FileSearcher();


        #endregion

       
      
        /// <summary>
        /// 显示当前已找到的文件
           /// </summary>
        /// <param name="files"></param>
        private void ShowSearchedFiles(FileInfo[] files)
        {
             foreach (FileInfo file in files)
                lstFiles.Items.Add(file.FullName);
        }
        
     

        private void btnBeginSearch_Click(object sender, EventArgs e)
        {
            lstFiles.Items.Clear();
            BeginSearch();
            btnCancel.Enabled= true;
            btnBeginSearch.Enabled = false;
        }

        /// <summary>
        /// 启动搜索
        /// </summary>
        public void BeginSearch()
        {
            //挂接显示函数，此函数将在搜索过程中不断地被fileSearcher组件所调用。
            fileSearcher1.ShowSearchResult = this.ShowSearchedFiles;
            //向fileSearcher组件提供要搜索文件的信息
            fileSearcher1.SearchInfoObj = this.SearchInfoObj;
            //启动搜索
            fileSearcher1.RunWorkerAsync();  //启动搜索
        }

        private void fileSearcher1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
         lblInfo2.Text = e.UserState.ToString();
         lblInfo2.Refresh();
        }

        private void fileSearcher1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
                lblInfo2.Text = string.Format("用户取消了操作，到目前为止，找到了{0}个文件",fileSearcher1.FoundFiles.Count);
            else
                lblInfo2.Text = string.Format("搜索完成，共找到{0}个文件。",fileSearcher1.FoundFiles.Count);

            btnBeginSearch.Enabled = true;
            btnCancel.Enabled = false;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            fileSearcher1.CancelAsync();
            btnCancel.Enabled = false;
            btnBeginSearch.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
           
            Close();
        }

        private void frmFileSearcher_Load(object sender, EventArgs e)
        {
            BeginSearch();
        }

        private void frmFileSearcher_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileSearcher1.IsBusy)
            {
                MessageBox.Show("搜索正在进行中，请先取消搜索任务后再关闭程序","提示信息",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                e.Cancel = true;
            }
        }

     

    }
}
