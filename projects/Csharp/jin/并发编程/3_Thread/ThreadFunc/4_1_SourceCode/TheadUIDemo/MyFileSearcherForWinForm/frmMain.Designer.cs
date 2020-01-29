namespace MyFileSearcher
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBeginSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearchFileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnChooseSearchDirectory = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txtBeginDirectory = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnBeginSearch
            // 
            this.btnBeginSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBeginSearch.Location = new System.Drawing.Point(305, 153);
            this.btnBeginSearch.Name = "btnBeginSearch";
            this.btnBeginSearch.Size = new System.Drawing.Size(100, 33);
            this.btnBeginSearch.TabIndex = 0;
            this.btnBeginSearch.Text = "开始搜索";
            this.btnBeginSearch.UseVisualStyleBackColor = true;
            this.btnBeginSearch.Click += new System.EventHandler(this.btnBeginSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(389, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "要查找的文件名（“*”代表任意多个字符，“?”代表任意单个字符）：";
            // 
            // txtSearchFileName
            // 
            this.txtSearchFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchFileName.Location = new System.Drawing.Point(18, 52);
            this.txtSearchFileName.Name = "txtSearchFileName";
            this.txtSearchFileName.Size = new System.Drawing.Size(387, 21);
            this.txtSearchFileName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "搜索起始目录：";
            // 
            // btnChooseSearchDirectory
            // 
            this.btnChooseSearchDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChooseSearchDirectory.Location = new System.Drawing.Point(188, 153);
            this.btnChooseSearchDirectory.Name = "btnChooseSearchDirectory";
            this.btnChooseSearchDirectory.Size = new System.Drawing.Size(111, 33);
            this.btnChooseSearchDirectory.TabIndex = 5;
            this.btnChooseSearchDirectory.Text = "选择搜索起始目录";
            this.btnChooseSearchDirectory.UseVisualStyleBackColor = true;
            this.btnChooseSearchDirectory.Click += new System.EventHandler(this.btnChooseSearchDirectory_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "选择起始目录";
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // txtBeginDirectory
            // 
            this.txtBeginDirectory.Location = new System.Drawing.Point(19, 109);
            this.txtBeginDirectory.Name = "txtBeginDirectory";
            this.txtBeginDirectory.Size = new System.Drawing.Size(386, 21);
            this.txtBeginDirectory.TabIndex = 6;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 198);
            this.Controls.Add(this.txtBeginDirectory);
            this.Controls.Add(this.btnChooseSearchDirectory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSearchFileName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBeginSearch);
            this.Name = "frmMain";
            this.Text = "文件搜索器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBeginSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearchFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChooseSearchDirectory;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtBeginDirectory;
    }
}

