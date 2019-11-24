namespace LayoutDemos
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
            this.btnSplitContainer = new System.Windows.Forms.Button();
            this.btnFlowLayout = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSplitContainer
            // 
            this.btnSplitContainer.Location = new System.Drawing.Point(55, 24);
            this.btnSplitContainer.Name = "btnSplitContainer";
            this.btnSplitContainer.Size = new System.Drawing.Size(145, 39);
            this.btnSplitContainer.TabIndex = 0;
            this.btnSplitContainer.Text = "使用SplitContainer";
            this.btnSplitContainer.UseVisualStyleBackColor = true;
            this.btnSplitContainer.Click += new System.EventHandler(this.btnSplitContainer_Click);
            // 
            // btnFlowLayout
            // 
            this.btnFlowLayout.Location = new System.Drawing.Point(55, 87);
            this.btnFlowLayout.Name = "btnFlowLayout";
            this.btnFlowLayout.Size = new System.Drawing.Size(145, 39);
            this.btnFlowLayout.TabIndex = 0;
            this.btnFlowLayout.Text = "使用FlowLayout";
            this.btnFlowLayout.UseVisualStyleBackColor = true;
            this.btnFlowLayout.Click += new System.EventHandler(this.btnFlowLayout_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(55, 153);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 39);
            this.button1.TabIndex = 0;
            this.button1.Text = "使用TableLayout";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnFlowLayout);
            this.Controls.Add(this.btnSplitContainer);
            this.Name = "frmMain";
            this.Text = "使用容器控件布局窗体";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSplitContainer;
        private System.Windows.Forms.Button btnFlowLayout;
        private System.Windows.Forms.Button button1;
    }
}

