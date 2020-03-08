namespace VisitVisualControl3
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.txtUserInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnVisitLabel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblInfo);
            this.groupBox1.Location = new System.Drawing.Point(18, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(284, 77);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "跨线程访问的控件";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(57, 36);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(161, 12);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "此标签被另一个线程设置文本";
            // 
            // txtUserInput
            // 
            this.txtUserInput.Location = new System.Drawing.Point(31, 139);
            this.txtUserInput.Name = "txtUserInput";
            this.txtUserInput.Size = new System.Drawing.Size(205, 21);
            this.txtUserInput.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "说明：输入文字后单击“设置”按钮。";
            // 
            // btnVisitLabel
            // 
            this.btnVisitLabel.Location = new System.Drawing.Point(242, 139);
            this.btnVisitLabel.Name = "btnVisitLabel";
            this.btnVisitLabel.Size = new System.Drawing.Size(60, 21);
            this.btnVisitLabel.TabIndex = 3;
            this.btnVisitLabel.Text = "设置";
            this.btnVisitLabel.UseVisualStyleBackColor = true;
            this.btnVisitLabel.Click += new System.EventHandler(this.btnVisitLabel_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 193);
            this.Controls.Add(this.btnVisitLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUserInput);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "向跨线程访问控件的函数传送参数";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TextBox txtUserInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnVisitLabel;
    }
}

