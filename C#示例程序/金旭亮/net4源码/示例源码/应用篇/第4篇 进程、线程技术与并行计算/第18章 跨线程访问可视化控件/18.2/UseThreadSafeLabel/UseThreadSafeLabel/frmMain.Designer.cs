namespace UseThreadSafeLabel
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
            this.threadSafeLabel1 = new ThreadSafeControl.ThreadSafeLabel();
            this.txtUserInput = new System.Windows.Forms.TextBox();
            this.btnSetLabelText = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // threadSafeLabel1
            // 
            this.threadSafeLabel1.AutoSize = true;
            this.threadSafeLabel1.Location = new System.Drawing.Point(44, 28);
            this.threadSafeLabel1.Name = "threadSafeLabel1";
            this.threadSafeLabel1.Size = new System.Drawing.Size(161, 12);
            this.threadSafeLabel1.TabIndex = 0;
            this.threadSafeLabel1.Text = "此标签可被安全地跨线程使用";
            // 
            // txtUserInput
            // 
            this.txtUserInput.Location = new System.Drawing.Point(34, 113);
            this.txtUserInput.Name = "txtUserInput";
            this.txtUserInput.Size = new System.Drawing.Size(212, 21);
            this.txtUserInput.TabIndex = 1;
            // 
            // btnSetLabelText
            // 
            this.btnSetLabelText.Location = new System.Drawing.Point(267, 114);
            this.btnSetLabelText.Name = "btnSetLabelText";
            this.btnSetLabelText.Size = new System.Drawing.Size(59, 20);
            this.btnSetLabelText.TabIndex = 2;
            this.btnSetLabelText.Text = "设置";
            this.btnSetLabelText.UseVisualStyleBackColor = true;
            this.btnSetLabelText.Click += new System.EventHandler(this.btnSetLabelText_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "说明：输入字串后单击“设置”按钮";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.threadSafeLabel1);
            this.groupBox1.Location = new System.Drawing.Point(28, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 56);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "跨线程访问的控件";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 163);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSetLabelText);
            this.Controls.Add(this.txtUserInput);
            this.Name = "Form1";
            this.Text = "使用线程安全的标签";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ThreadSafeControl.ThreadSafeLabel threadSafeLabel1;
        private System.Windows.Forms.TextBox txtUserInput;
        private System.Windows.Forms.Button btnSetLabelText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

