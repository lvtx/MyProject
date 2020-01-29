﻿namespace MyFileSearcher
{
    partial class frmFileSearcher
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.btnBeginSearch = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblInfo2 = new System.Windows.Forms.Label();
            this.fileSearcher1 = new MyFileSearcher.FileSearcher();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(25, 29);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(91, 15);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "正在搜索...";
            // 
            // lstFiles
            // 
            this.lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.ItemHeight = 15;
            this.lstFiles.Location = new System.Drawing.Point(37, 69);
            this.lstFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(487, 94);
            this.lstFiles.TabIndex = 1;
            // 
            // btnBeginSearch
            // 
            this.btnBeginSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBeginSearch.Enabled = false;
            this.btnBeginSearch.Location = new System.Drawing.Point(40, 185);
            this.btnBeginSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBeginSearch.Name = "btnBeginSearch";
            this.btnBeginSearch.Size = new System.Drawing.Size(136, 41);
            this.btnBeginSearch.TabIndex = 2;
            this.btnBeginSearch.Text = "重新开始搜索";
            this.btnBeginSearch.UseVisualStyleBackColor = true;
            this.btnBeginSearch.Click += new System.EventHandler(this.btnBeginSearch_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(223, 185);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(136, 40);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(385, 188);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(140, 39);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblInfo2
            // 
            this.lblInfo2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInfo2.AutoSize = true;
            this.lblInfo2.Location = new System.Drawing.Point(37, 250);
            this.lblInfo2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(187, 15);
            this.lblInfo2.TabIndex = 5;
            this.lblInfo2.Text = "点击“取消”按钮停止查找";
            // 
            // fileSearcher1
            // 
            this.fileSearcher1.WorkerReportsProgress = true;
            this.fileSearcher1.WorkerSupportsCancellation = true;
            this.fileSearcher1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.fileSearcher1_ProgressChanged);
            this.fileSearcher1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.fileSearcher1_RunWorkerCompleted);
            // 
            // frmFileSearcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 299);
            this.Controls.Add(this.lblInfo2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBeginSearch);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.lblInfo);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmFileSearcher";
            this.Text = "文件搜索";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFileSearcher_FormClosing);
            this.Load += new System.EventHandler(this.frmFileSearcher_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Button btnBeginSearch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private FileSearcher fileSearcher1;
        private System.Windows.Forms.Label lblInfo2;
    }
}