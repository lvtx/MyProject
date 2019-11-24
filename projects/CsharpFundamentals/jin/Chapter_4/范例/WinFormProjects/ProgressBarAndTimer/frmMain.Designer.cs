namespace ProgressBarAndTimer
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ManualProgressBar = new System.Windows.Forms.ProgressBar();
            this.AutoProgressBar = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnTimer = new System.Windows.Forms.Button();
            this.btnDecrease = new System.Windows.Forms.Button();
            this.btnIncrease = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ManualProgressBar
            // 
            this.ManualProgressBar.Location = new System.Drawing.Point(67, 21);
            this.ManualProgressBar.Name = "ManualProgressBar";
            this.ManualProgressBar.Size = new System.Drawing.Size(320, 36);
            this.ManualProgressBar.TabIndex = 0;
            // 
            // AutoProgressBar
            // 
            this.AutoProgressBar.Location = new System.Drawing.Point(98, 87);
            this.AutoProgressBar.Name = "AutoProgressBar";
            this.AutoProgressBar.Size = new System.Drawing.Size(331, 27);
            this.AutoProgressBar.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnTimer
            // 
            this.btnTimer.Image = global::ProgressBarAndTimer.Properties.Resources.EnableClock;
            this.btnTimer.Location = new System.Drawing.Point(24, 87);
            this.btnTimer.Name = "btnTimer";
            this.btnTimer.Size = new System.Drawing.Size(68, 74);
            this.btnTimer.TabIndex = 3;
            this.btnTimer.UseVisualStyleBackColor = true;
            this.btnTimer.Click += new System.EventHandler(this.btnTimer_Click);
            // 
            // btnDecrease
            // 
            this.btnDecrease.Image = global::ProgressBarAndTimer.Properties.Resources.appbar_minus_rest;
            this.btnDecrease.Location = new System.Drawing.Point(393, 21);
            this.btnDecrease.Name = "btnDecrease";
            this.btnDecrease.Size = new System.Drawing.Size(36, 36);
            this.btnDecrease.TabIndex = 1;
            this.btnDecrease.UseVisualStyleBackColor = true;
            this.btnDecrease.Click += new System.EventHandler(this.btnDecrease_Click);
            // 
            // btnIncrease
            // 
            this.btnIncrease.Image = global::ProgressBarAndTimer.Properties.Resources.appbar_add_rest;
            this.btnIncrease.Location = new System.Drawing.Point(26, 21);
            this.btnIncrease.Name = "btnIncrease";
            this.btnIncrease.Size = new System.Drawing.Size(35, 36);
            this.btnIncrease.TabIndex = 1;
            this.btnIncrease.UseVisualStyleBackColor = true;
            this.btnIncrease.Click += new System.EventHandler(this.btnIncrease_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInfo.ForeColor = System.Drawing.Color.Navy;
            this.lblInfo.Location = new System.Drawing.Point(220, 142);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(51, 19);
            this.lblInfo.TabIndex = 4;
            this.lblInfo.Text = "label1";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 183);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnTimer);
            this.Controls.Add(this.AutoProgressBar);
            this.Controls.Add(this.btnDecrease);
            this.Controls.Add(this.btnIncrease);
            this.Controls.Add(this.ManualProgressBar);
            this.Name = "frmMain";
            this.Text = "进度条与小闹钟";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar ManualProgressBar;
        private System.Windows.Forms.Button btnIncrease;
        private System.Windows.Forms.Button btnDecrease;
        private System.Windows.Forms.ProgressBar AutoProgressBar;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnTimer;
        private System.Windows.Forms.Label lblInfo;
    }
}

