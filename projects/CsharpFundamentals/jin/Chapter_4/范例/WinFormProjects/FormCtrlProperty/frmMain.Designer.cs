namespace FormCtrlProperty
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
            this.btnEnable = new System.Windows.Forms.Button();
            this.btnVisible = new System.Windows.Forms.Button();
            this.btnAnchor = new System.Windows.Forms.Button();
            this.btnDock = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEnable
            // 
            this.btnEnable.Location = new System.Drawing.Point(38, 24);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(206, 43);
            this.btnEnable.TabIndex = 0;
            this.btnEnable.Text = "控件的激活与禁用";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // btnVisible
            // 
            this.btnVisible.Location = new System.Drawing.Point(38, 73);
            this.btnVisible.Name = "btnVisible";
            this.btnVisible.Size = new System.Drawing.Size(206, 43);
            this.btnVisible.TabIndex = 0;
            this.btnVisible.Text = "控件的显示与隐藏";
            this.btnVisible.UseVisualStyleBackColor = true;
            this.btnVisible.Click += new System.EventHandler(this.btnVisible_Click);
            // 
            // btnAnchor
            // 
            this.btnAnchor.Location = new System.Drawing.Point(38, 122);
            this.btnAnchor.Name = "btnAnchor";
            this.btnAnchor.Size = new System.Drawing.Size(206, 43);
            this.btnAnchor.TabIndex = 0;
            this.btnAnchor.Text = "Anchor属性的功用";
            this.btnAnchor.UseVisualStyleBackColor = true;
            this.btnAnchor.Click += new System.EventHandler(this.btnAnchor_Click);
            // 
            // btnDock
            // 
            this.btnDock.Location = new System.Drawing.Point(38, 171);
            this.btnDock.Name = "btnDock";
            this.btnDock.Size = new System.Drawing.Size(206, 43);
            this.btnDock.TabIndex = 0;
            this.btnDock.Text = "Dock属性的妙用";
            this.btnDock.UseVisualStyleBackColor = true;
            this.btnDock.Click += new System.EventHandler(this.btnDock_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 236);
            this.Controls.Add(this.btnDock);
            this.Controls.Add(this.btnAnchor);
            this.Controls.Add(this.btnVisible);
            this.Controls.Add(this.btnEnable);
            this.Name = "frmMain";
            this.Text = "控件通用属性示例";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEnable;
        private System.Windows.Forms.Button btnVisible;
        private System.Windows.Forms.Button btnAnchor;
        private System.Windows.Forms.Button btnDock;
    }
}

