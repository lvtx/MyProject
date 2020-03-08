namespace ClipboardInfo
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
            this.btnGetClipboardInfo = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnGetClipboardInfo
            // 
            this.btnGetClipboardInfo.Location = new System.Drawing.Point(17, 14);
            this.btnGetClipboardInfo.Name = "btnGetClipboardInfo";
            this.btnGetClipboardInfo.Size = new System.Drawing.Size(121, 37);
            this.btnGetClipboardInfo.TabIndex = 0;
            this.btnGetClipboardInfo.Text = "剪贴板上有什么？";
            this.btnGetClipboardInfo.UseVisualStyleBackColor = true;
            this.btnGetClipboardInfo.Click += new System.EventHandler(this.btnGetClipboardInfo_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(17, 75);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(288, 168);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 266);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnGetClipboardInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "剪贴板信息";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGetClipboardInfo;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

