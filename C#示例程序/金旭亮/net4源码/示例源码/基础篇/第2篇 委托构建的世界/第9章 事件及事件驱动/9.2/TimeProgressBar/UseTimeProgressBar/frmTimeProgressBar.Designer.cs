namespace UseTimeProgressBar
{
    partial class frmTimeProgressBar
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
            this.btnSetup = new System.Windows.Forms.Button();
            this.timeProgressBar1 = new TimeProgressBar.TimeProgressBar();
            this.updnHour = new System.Windows.Forms.NumericUpDown();
            this.updnMinute = new System.Windows.Forms.NumericUpDown();
            this.updnSecond = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.updnHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updnMinute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updnSecond)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(221, 31);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(58, 32);
            this.btnSetup.TabIndex = 1;
            this.btnSetup.Text = "确定";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // timeProgressBar1
            // 
            this.timeProgressBar1.Location = new System.Drawing.Point(8, 100);
            this.timeProgressBar1.Name = "timeProgressBar1";
            this.timeProgressBar1.Size = new System.Drawing.Size(293, 22);
            this.timeProgressBar1.TabIndex = 0;
            this.timeProgressBar1.TimeIsUp += new TimeProgressBar.TimeIsUpDelegate(this.timeProgressBar1_TimeIsUp);
            // 
            // updnHour
            // 
            this.updnHour.Location = new System.Drawing.Point(16, 39);
            this.updnHour.Name = "updnHour";
            this.updnHour.Size = new System.Drawing.Size(42, 21);
            this.updnHour.TabIndex = 2;
            // 
            // updnMinute
            // 
            this.updnMinute.Location = new System.Drawing.Point(76, 39);
            this.updnMinute.Name = "updnMinute";
            this.updnMinute.Size = new System.Drawing.Size(42, 21);
            this.updnMinute.TabIndex = 2;
            // 
            // updnSecond
            // 
            this.updnSecond.Location = new System.Drawing.Point(139, 39);
            this.updnSecond.Name = "updnSecond";
            this.updnSecond.Size = new System.Drawing.Size(42, 21);
            this.updnSecond.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnSetup);
            this.groupBox1.Controls.Add(this.updnSecond);
            this.groupBox1.Controls.Add(this.updnHour);
            this.groupBox1.Controls.Add(this.updnMinute);
            this.groupBox1.Location = new System.Drawing.Point(8, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(293, 77);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置闹钟时间";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(137, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "秒：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(74, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "分：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "小时：";
            // 
            // frmTimeProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 133);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.timeProgressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTimeProgressBar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "拥有自定义事件的进度条控件";
            ((System.ComponentModel.ISupportInitialize)(this.updnHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updnMinute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updnSecond)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TimeProgressBar.TimeProgressBar timeProgressBar1;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.NumericUpDown updnHour;
        private System.Windows.Forms.NumericUpDown updnMinute;
        private System.Windows.Forms.NumericUpDown updnSecond;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

