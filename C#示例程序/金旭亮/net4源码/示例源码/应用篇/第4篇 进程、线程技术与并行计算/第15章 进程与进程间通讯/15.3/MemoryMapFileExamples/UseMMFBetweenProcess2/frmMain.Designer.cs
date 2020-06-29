namespace UseMMFBetweenProcess2
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
            this.txtImageInfo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLoadFromMMF = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSaveToMMF = new System.Windows.Forms.Button();
            this.btnLoadPic = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtImageInfo
            // 
            this.txtImageInfo.Location = new System.Drawing.Point(80, 7);
            this.txtImageInfo.Name = "txtImageInfo";
            this.txtImageInfo.Size = new System.Drawing.Size(391, 21);
            this.txtImageInfo.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "图片说明：";
            // 
            // btnLoadFromMMF
            // 
            this.btnLoadFromMMF.Location = new System.Drawing.Point(238, 282);
            this.btnLoadFromMMF.Name = "btnLoadFromMMF";
            this.btnLoadFromMMF.Size = new System.Drawing.Size(138, 34);
            this.btnLoadFromMMF.TabIndex = 8;
            this.btnLoadFromMMF.Text = "从内存映射文件中提取";
            this.btnLoadFromMMF.UseVisualStyleBackColor = true;
            this.btnLoadFromMMF.Click += new System.EventHandler(this.btnLoadFromMMF_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(382, 282);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(89, 34);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSaveToMMF
            // 
            this.btnSaveToMMF.Location = new System.Drawing.Point(101, 282);
            this.btnSaveToMMF.Name = "btnSaveToMMF";
            this.btnSaveToMMF.Size = new System.Drawing.Size(131, 34);
            this.btnSaveToMMF.TabIndex = 5;
            this.btnSaveToMMF.Text = "保存到内存映射文件";
            this.btnSaveToMMF.UseVisualStyleBackColor = true;
            this.btnSaveToMMF.Click += new System.EventHandler(this.btnSaveToMMF_Click);
            // 
            // btnLoadPic
            // 
            this.btnLoadPic.Location = new System.Drawing.Point(11, 282);
            this.btnLoadPic.Name = "btnLoadPic";
            this.btnLoadPic.Size = new System.Drawing.Size(75, 34);
            this.btnLoadPic.TabIndex = 6;
            this.btnLoadPic.Text = "装入图片";
            this.btnLoadPic.UseVisualStyleBackColor = true;
            this.btnLoadPic.Click += new System.EventHandler(this.btnLoadPic_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(11, 39);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(460, 237);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
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
            this.ClientSize = new System.Drawing.Size(483, 325);
            this.Controls.Add(this.txtImageInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLoadFromMMF);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSaveToMMF);
            this.Controls.Add(this.btnLoadPic);
            this.Controls.Add(this.pictureBox1);
            this.Name = "frmMain";
            this.Text = "在进程间共享内存映射文件（2）";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtImageInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoadFromMMF;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSaveToMMF;
        private System.Windows.Forms.Button btnLoadPic;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

