namespace CalculateFolderSizeUseTPL
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.rtfInfo = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "请选择一个要计算容量的文件夹：";
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(25, 19);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(135, 32);
            this.btnSelectFolder.TabIndex = 0;
            this.btnSelectFolder.Text = "选择一个文件夹";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // rtfInfo
            // 
            this.rtfInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfInfo.Location = new System.Drawing.Point(21, 69);
            this.rtfInfo.Name = "rtfInfo";
            this.rtfInfo.Size = new System.Drawing.Size(388, 207);
            this.rtfInfo.TabIndex = 1;
            this.rtfInfo.Text = "";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 301);
            this.Controls.Add(this.rtfInfo);
            this.Controls.Add(this.btnSelectFolder);
            this.Name = "frmMain";
            this.Text = "使用TPL异步计算文件夹容量";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.RichTextBox rtfInfo;
    }
}

