namespace FormCtrlProperties
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnDock = new System.Windows.Forms.Button();
            this.btnAnchor = new System.Windows.Forms.Button();
            this.btnVisible = new System.Windows.Forms.Button();
            this.btnEnable = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDock
            // 
            this.btnDock.Location = new System.Drawing.Point(38, 228);
            this.btnDock.Margin = new System.Windows.Forms.Padding(4);
            this.btnDock.Name = "btnDock";
            this.btnDock.Size = new System.Drawing.Size(275, 54);
            this.btnDock.TabIndex = 1;
            this.btnDock.Text = "Dock属性的妙用";
            this.btnDock.UseVisualStyleBackColor = true;
            this.btnDock.Click += new System.EventHandler(this.BtnDock_Click);
            // 
            // btnAnchor
            // 
            this.btnAnchor.Location = new System.Drawing.Point(38, 166);
            this.btnAnchor.Margin = new System.Windows.Forms.Padding(4);
            this.btnAnchor.Name = "btnAnchor";
            this.btnAnchor.Size = new System.Drawing.Size(275, 54);
            this.btnAnchor.TabIndex = 2;
            this.btnAnchor.Text = "Anchor属性的功用";
            this.btnAnchor.UseVisualStyleBackColor = true;
            this.btnAnchor.Click += new System.EventHandler(this.BtnAnchor_Click);
            // 
            // btnVisible
            // 
            this.btnVisible.Location = new System.Drawing.Point(38, 105);
            this.btnVisible.Margin = new System.Windows.Forms.Padding(4);
            this.btnVisible.Name = "btnVisible";
            this.btnVisible.Size = new System.Drawing.Size(275, 54);
            this.btnVisible.TabIndex = 3;
            this.btnVisible.Text = "控件的显示与隐藏";
            this.btnVisible.UseVisualStyleBackColor = true;
            this.btnVisible.Click += new System.EventHandler(this.BtnVisible_Click);
            // 
            // btnEnable
            // 
            this.btnEnable.Location = new System.Drawing.Point(38, 44);
            this.btnEnable.Margin = new System.Windows.Forms.Padding(4);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(275, 54);
            this.btnEnable.TabIndex = 4;
            this.btnEnable.Text = "控件的激活与禁用";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.BtnEnable_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 327);
            this.Controls.Add(this.btnDock);
            this.Controls.Add(this.btnAnchor);
            this.Controls.Add(this.btnVisible);
            this.Controls.Add(this.btnEnable);
            this.Name = "FrmMain";
            this.Text = "FrmMain";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDock;
        private System.Windows.Forms.Button btnAnchor;
        private System.Windows.Forms.Button btnVisible;
        private System.Windows.Forms.Button btnEnable;
    }
}