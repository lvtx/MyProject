namespace UseWhereFunc
{
    partial class frmWhere
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnFind = new System.Windows.Forms.Button();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFindWhat = new System.Windows.Forms.TextBox();
            this.lblFolder = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "选择文件夹";
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(29, 35);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(227, 33);
            this.btnFind.TabIndex = 0;
            this.btnFind.Text = "开始查找";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // lstFiles
            // 
            this.lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.ItemHeight = 12;
            this.lstFiles.Location = new System.Drawing.Point(29, 101);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(230, 148);
            this.lstFiles.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "文件名中包含的字符：";
            // 
            // txtFindWhat
            // 
            this.txtFindWhat.Location = new System.Drawing.Point(151, 8);
            this.txtFindWhat.Name = "txtFindWhat";
            this.txtFindWhat.Size = new System.Drawing.Size(105, 21);
            this.txtFindWhat.TabIndex = 3;
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblFolder.Location = new System.Drawing.Point(27, 83);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(77, 12);
            this.lblFolder.TabIndex = 4;
            this.lblFolder.Text = "未选择文件夹";
            // 
            // frmWhere
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.lblFolder);
            this.Controls.Add(this.txtFindWhat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.btnFind);
            this.Name = "frmWhere";
            this.Text = "筛选数据——Where";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFindWhat;
        private System.Windows.Forms.Label lblFolder;
    }
}