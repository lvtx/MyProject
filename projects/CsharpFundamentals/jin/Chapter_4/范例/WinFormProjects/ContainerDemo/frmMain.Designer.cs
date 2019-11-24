namespace ContainerDemo
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
            this.btnPanel = new System.Windows.Forms.Button();
            this.btnTabControl = new System.Windows.Forms.Button();
            this.btnGroupBox = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPanel
            // 
            this.btnPanel.Location = new System.Drawing.Point(52, 36);
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Size = new System.Drawing.Size(205, 50);
            this.btnPanel.TabIndex = 0;
            this.btnPanel.Text = "使用面板";
            this.btnPanel.UseVisualStyleBackColor = true;
            this.btnPanel.Click += new System.EventHandler(this.btnPanel_Click);
            // 
            // btnTabControl
            // 
            this.btnTabControl.Location = new System.Drawing.Point(52, 167);
            this.btnTabControl.Name = "btnTabControl";
            this.btnTabControl.Size = new System.Drawing.Size(205, 50);
            this.btnTabControl.TabIndex = 0;
            this.btnTabControl.Text = "使用选项卡";
            this.btnTabControl.UseVisualStyleBackColor = true;
            this.btnTabControl.Click += new System.EventHandler(this.btnTabControl_Click);
            // 
            // btnGroupBox
            // 
            this.btnGroupBox.Location = new System.Drawing.Point(52, 101);
            this.btnGroupBox.Name = "btnGroupBox";
            this.btnGroupBox.Size = new System.Drawing.Size(205, 50);
            this.btnGroupBox.TabIndex = 0;
            this.btnGroupBox.Text = "使用组合框";
            this.btnGroupBox.UseVisualStyleBackColor = true;
            this.btnGroupBox.Click += new System.EventHandler(this.btnGroupBox_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 270);
            this.Controls.Add(this.btnGroupBox);
            this.Controls.Add(this.btnTabControl);
            this.Controls.Add(this.btnPanel);
            this.Name = "frmMain";
            this.Text = "容器控件使用示例";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPanel;
        private System.Windows.Forms.Button btnTabControl;
        private System.Windows.Forms.Button btnGroupBox;
    }
}

