namespace UseReaderWriterLock
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblReader1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblReader2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblReader3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnWriter = new System.Windows.Forms.Button();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblReader1);
            this.groupBox1.Location = new System.Drawing.Point(15, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(103, 49);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "读线程1";
            // 
            // lblReader1
            // 
            this.lblReader1.AutoSize = true;
            this.lblReader1.Location = new System.Drawing.Point(22, 24);
            this.lblReader1.Name = "lblReader1";
            this.lblReader1.Size = new System.Drawing.Size(41, 12);
            this.lblReader1.TabIndex = 0;
            this.lblReader1.Text = "label1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblReader2);
            this.groupBox2.Location = new System.Drawing.Point(143, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(103, 49);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "读线程2";
            // 
            // lblReader2
            // 
            this.lblReader2.AutoSize = true;
            this.lblReader2.Location = new System.Drawing.Point(18, 24);
            this.lblReader2.Name = "lblReader2";
            this.lblReader2.Size = new System.Drawing.Size(41, 12);
            this.lblReader2.TabIndex = 0;
            this.lblReader2.Text = "label2";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblReader3);
            this.groupBox3.Location = new System.Drawing.Point(266, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(103, 49);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "读线程3";
            // 
            // lblReader3
            // 
            this.lblReader3.AutoSize = true;
            this.lblReader3.Location = new System.Drawing.Point(20, 24);
            this.lblReader3.Name = "lblReader3";
            this.lblReader3.Size = new System.Drawing.Size(41, 12);
            this.lblReader3.TabIndex = 0;
            this.lblReader3.Text = "label3";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnWriter);
            this.groupBox4.Controls.Add(this.txtNumber);
            this.groupBox4.Location = new System.Drawing.Point(15, 93);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(354, 90);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "写线程";
            // 
            // btnWriter
            // 
            this.btnWriter.Location = new System.Drawing.Point(215, 38);
            this.btnWriter.Name = "btnWriter";
            this.btnWriter.Size = new System.Drawing.Size(123, 23);
            this.btnWriter.TabIndex = 1;
            this.btnWriter.Text = "写入共享资源";
            this.btnWriter.UseVisualStyleBackColor = true;
            this.btnWriter.Click += new System.EventHandler(this.btnWriter_Click);
            // 
            // txtNumber
            // 
            this.txtNumber.Location = new System.Drawing.Point(24, 41);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(176, 21);
            this.txtNumber.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 207);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "线程同步示例-单写多读";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnWriter;
        private System.Windows.Forms.TextBox txtNumber;
        private System.Windows.Forms.Label lblReader1;
        private System.Windows.Forms.Label lblReader2;
        private System.Windows.Forms.Label lblReader3;
    }
}

