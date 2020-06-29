namespace Reader
{
    partial class frmReader
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
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.txtReader = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.Filter = "*.rtf";
            this.fileSystemWatcher1.NotifyFilter = System.IO.NotifyFilters.Size;
            this.fileSystemWatcher1.SynchronizingObject = this;
            this.fileSystemWatcher1.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Changed);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "文本文件（*.txt）|*.txt|所有文件（*.*）|*.*";
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnChooseFile.Location = new System.Drawing.Point(15, 219);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(114, 35);
            this.btnChooseFile.TabIndex = 1;
            this.btnChooseFile.Text = "选择要监控的文件";
            this.btnChooseFile.UseVisualStyleBackColor = true;
            this.btnChooseFile.Click += new System.EventHandler(this.btnChooseFile_Click);
            // 
            // txtReader
            // 
            this.txtReader.Location = new System.Drawing.Point(14, 20);
            this.txtReader.Multiline = true;
            this.txtReader.Name = "txtReader";
            this.txtReader.ReadOnly = true;
            this.txtReader.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReader.Size = new System.Drawing.Size(434, 186);
            this.txtReader.TabIndex = 2;
            this.txtReader.Text = "请打开一个将被FileWriter修改的文件";
            // 
            // frmReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 266);
            this.Controls.Add(this.txtReader);
            this.Controls.Add(this.btnChooseFile);
            this.Name = "frmReader";
            this.Text = "文件监视器";
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.Button btnChooseFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtReader;
    }
}

