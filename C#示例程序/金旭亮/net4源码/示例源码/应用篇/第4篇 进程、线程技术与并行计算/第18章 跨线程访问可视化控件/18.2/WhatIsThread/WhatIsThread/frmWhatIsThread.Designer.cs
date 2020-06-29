namespace WhatIsThread
{
    partial class frmWhatIsThread
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSingleThreadID = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartSingleThread = new System.Windows.Forms.Button();
            this.progressBarSingleThread = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblMultiThreadID2 = new System.Windows.Forms.Label();
            this.lblMultiThreadID1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnStartMultiThread2 = new System.Windows.Forms.Button();
            this.btnStartMultiThread1 = new System.Windows.Forms.Button();
            this.progressBarMultiThread2 = new System.Windows.Forms.ProgressBar();
            this.progressBarMultiThread1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMainThread = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblSingleThreadID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnStartSingleThread);
            this.groupBox1.Controls.Add(this.progressBarSingleThread);
            this.groupBox1.Location = new System.Drawing.Point(13, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(393, 72);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "单线程运行";
            // 
            // lblSingleThreadID
            // 
            this.lblSingleThreadID.AutoSize = true;
            this.lblSingleThreadID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblSingleThreadID.Location = new System.Drawing.Point(68, 22);
            this.lblSingleThreadID.Name = "lblSingleThreadID";
            this.lblSingleThreadID.Size = new System.Drawing.Size(11, 12);
            this.lblSingleThreadID.TabIndex = 3;
            this.lblSingleThreadID.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "线程ID：";
            // 
            // btnStartSingleThread
            // 
            this.btnStartSingleThread.Location = new System.Drawing.Point(307, 22);
            this.btnStartSingleThread.Name = "btnStartSingleThread";
            this.btnStartSingleThread.Size = new System.Drawing.Size(80, 31);
            this.btnStartSingleThread.TabIndex = 1;
            this.btnStartSingleThread.Text = "启动";
            this.btnStartSingleThread.UseVisualStyleBackColor = true;
            this.btnStartSingleThread.Click += new System.EventHandler(this.btnStartSingleThread_Click);
            // 
            // progressBarSingleThread
            // 
            this.progressBarSingleThread.Location = new System.Drawing.Point(98, 22);
            this.progressBarSingleThread.Name = "progressBarSingleThread";
            this.progressBarSingleThread.Size = new System.Drawing.Size(185, 31);
            this.progressBarSingleThread.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblMultiThreadID2);
            this.groupBox2.Controls.Add(this.lblMultiThreadID1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnStartMultiThread2);
            this.groupBox2.Controls.Add(this.btnStartMultiThread1);
            this.groupBox2.Controls.Add(this.progressBarMultiThread2);
            this.groupBox2.Controls.Add(this.progressBarMultiThread1);
            this.groupBox2.Location = new System.Drawing.Point(14, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(392, 120);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "多线程运行";
            // 
            // lblMultiThreadID2
            // 
            this.lblMultiThreadID2.AutoSize = true;
            this.lblMultiThreadID2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblMultiThreadID2.Location = new System.Drawing.Point(68, 71);
            this.lblMultiThreadID2.Name = "lblMultiThreadID2";
            this.lblMultiThreadID2.Size = new System.Drawing.Size(11, 12);
            this.lblMultiThreadID2.TabIndex = 3;
            this.lblMultiThreadID2.Text = "0";
            // 
            // lblMultiThreadID1
            // 
            this.lblMultiThreadID1.AutoSize = true;
            this.lblMultiThreadID1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblMultiThreadID1.Location = new System.Drawing.Point(68, 28);
            this.lblMultiThreadID1.Name = "lblMultiThreadID1";
            this.lblMultiThreadID1.Size = new System.Drawing.Size(11, 12);
            this.lblMultiThreadID1.TabIndex = 3;
            this.lblMultiThreadID1.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "线程ID：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "线程ID：";
            // 
            // btnStartMultiThread2
            // 
            this.btnStartMultiThread2.Location = new System.Drawing.Point(307, 66);
            this.btnStartMultiThread2.Name = "btnStartMultiThread2";
            this.btnStartMultiThread2.Size = new System.Drawing.Size(80, 31);
            this.btnStartMultiThread2.TabIndex = 1;
            this.btnStartMultiThread2.Text = "启动";
            this.btnStartMultiThread2.UseVisualStyleBackColor = true;
            this.btnStartMultiThread2.Click += new System.EventHandler(this.btnStartMultiThread2_Click);
            // 
            // btnStartMultiThread1
            // 
            this.btnStartMultiThread1.Location = new System.Drawing.Point(307, 20);
            this.btnStartMultiThread1.Name = "btnStartMultiThread1";
            this.btnStartMultiThread1.Size = new System.Drawing.Size(80, 31);
            this.btnStartMultiThread1.TabIndex = 1;
            this.btnStartMultiThread1.Text = "启动";
            this.btnStartMultiThread1.UseVisualStyleBackColor = true;
            this.btnStartMultiThread1.Click += new System.EventHandler(this.btnStartMultiThread1_Click);
            // 
            // progressBarMultiThread2
            // 
            this.progressBarMultiThread2.Location = new System.Drawing.Point(98, 66);
            this.progressBarMultiThread2.Name = "progressBarMultiThread2";
            this.progressBarMultiThread2.Size = new System.Drawing.Size(185, 26);
            this.progressBarMultiThread2.TabIndex = 0;
            // 
            // progressBarMultiThread1
            // 
            this.progressBarMultiThread1.Location = new System.Drawing.Point(98, 23);
            this.progressBarMultiThread1.Name = "progressBarMultiThread1";
            this.progressBarMultiThread1.Size = new System.Drawing.Size(185, 26);
            this.progressBarMultiThread1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "主线程ID：";
            // 
            // lblMainThread
            // 
            this.lblMainThread.AutoSize = true;
            this.lblMainThread.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblMainThread.Location = new System.Drawing.Point(81, 10);
            this.lblMainThread.Name = "lblMainThread";
            this.lblMainThread.Size = new System.Drawing.Size(11, 12);
            this.lblMainThread.TabIndex = 3;
            this.lblMainThread.Text = "0";
            // 
            // frmWhatIsThread
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 277);
            this.Controls.Add(this.lblMainThread);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWhatIsThread";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "什么是多线程？";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmWhatIsThread_FormClosing);
            this.Load += new System.EventHandler(this.frmWhatIsThread_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ProgressBar progressBarSingleThread;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progressBarMultiThread1;
        private System.Windows.Forms.Button btnStartSingleThread;
        private System.Windows.Forms.Button btnStartMultiThread1;
        private System.Windows.Forms.Label lblSingleThreadID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMultiThreadID1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMainThread;
        private System.Windows.Forms.Label lblMultiThreadID2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnStartMultiThread2;
        private System.Windows.Forms.ProgressBar progressBarMultiThread2;
    }
}

