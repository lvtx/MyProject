namespace UseClipboard
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnLoadPic = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtImageInfo = new System.Windows.Forms.TextBox();
            this.btnCopyToClipboard = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPasteFromClipboard = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(16, 41);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(395, 237);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnLoadPic
            // 
            this.btnLoadPic.Location = new System.Drawing.Point(16, 284);
            this.btnLoadPic.Name = "btnLoadPic";
            this.btnLoadPic.Size = new System.Drawing.Size(75, 34);
            this.btnLoadPic.TabIndex = 1;
            this.btnLoadPic.Text = "装入图片";
            this.btnLoadPic.UseVisualStyleBackColor = true;
            this.btnLoadPic.Click += new System.EventHandler(this.btnLoadPic_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "图片说明：";
            // 
            // txtImageInfo
            // 
            this.txtImageInfo.Location = new System.Drawing.Point(85, 9);
            this.txtImageInfo.Name = "txtImageInfo";
            this.txtImageInfo.Size = new System.Drawing.Size(326, 21);
            this.txtImageInfo.TabIndex = 3;
            
            // 
            // btnCopyToClipboard
            // 
            this.btnCopyToClipboard.Location = new System.Drawing.Point(106, 284);
            this.btnCopyToClipboard.Name = "btnCopyToClipboard";
            this.btnCopyToClipboard.Size = new System.Drawing.Size(89, 34);
            this.btnCopyToClipboard.TabIndex = 1;
            this.btnCopyToClipboard.Text = "复制到剪贴板";
            this.btnCopyToClipboard.UseVisualStyleBackColor = true;
            this.btnCopyToClipboard.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(322, 284);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(89, 34);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPasteFromClipboard
            // 
            this.btnPasteFromClipboard.Location = new System.Drawing.Point(212, 284);
            this.btnPasteFromClipboard.Name = "btnPasteFromClipboard";
            this.btnPasteFromClipboard.Size = new System.Drawing.Size(89, 34);
            this.btnPasteFromClipboard.TabIndex = 1;
            this.btnPasteFromClipboard.Text = "从剪贴板粘贴";
            this.btnPasteFromClipboard.UseVisualStyleBackColor = true;
            this.btnPasteFromClipboard.Click += new System.EventHandler(this.btnPasteFromClipboard_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "所有图片（*.jpg,*.gif,*.bmp,*.png）|*.jpg;*.bmp;*.gif;*.png|所有文件（*.*）|*.*";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 330);
            this.Controls.Add(this.txtImageInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPasteFromClipboard);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnCopyToClipboard);
            this.Controls.Add(this.btnLoadPic);
            this.Controls.Add(this.pictureBox1);
            this.Name = "frmMain";
            this.Text = "使用剪贴板传送信息";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnLoadPic;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtImageInfo;
        private System.Windows.Forms.Button btnCopyToClipboard;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPasteFromClipboard;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

