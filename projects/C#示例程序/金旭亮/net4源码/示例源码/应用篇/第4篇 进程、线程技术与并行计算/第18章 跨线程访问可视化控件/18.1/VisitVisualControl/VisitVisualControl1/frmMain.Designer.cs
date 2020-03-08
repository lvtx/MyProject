namespace VisitVisualControl1
{
    partial class frmVisitControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnVisitLabel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "此标签被另一个线程设置文本";
            // 
            // btnVisitLabel
            // 
            this.btnVisitLabel.Location = new System.Drawing.Point(114, 81);
            this.btnVisitLabel.Name = "btnVisitLabel";
            this.btnVisitLabel.Size = new System.Drawing.Size(109, 32);
            this.btnVisitLabel.TabIndex = 1;
            this.btnVisitLabel.Text = "多线程访问标签";
            this.btnVisitLabel.UseVisualStyleBackColor = true;
            this.btnVisitLabel.Click += new System.EventHandler(this.btnVisitLabel_Click);
            // 
            // frmVisitControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 147);
            this.Controls.Add(this.btnVisitLabel);
            this.Controls.Add(this.label1);
            this.Name = "frmVisitControl";
            this.Text = "多线程访问可视化控件（引发异常版本）";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnVisitLabel;
    }
}

