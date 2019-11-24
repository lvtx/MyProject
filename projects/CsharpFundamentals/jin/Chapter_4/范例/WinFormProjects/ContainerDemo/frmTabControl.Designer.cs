namespace ContainerDemo
{
    partial class frmTabControl
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnAddTab = new System.Windows.Forms.Button();
            this.btnActiveLeft = new System.Windows.Forms.Button();
            this.btnActiveRight = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(22, 92);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(456, 208);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(448, 182);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnAddTab
            // 
            this.btnAddTab.Location = new System.Drawing.Point(24, 39);
            this.btnAddTab.Name = "btnAddTab";
            this.btnAddTab.Size = new System.Drawing.Size(108, 36);
            this.btnAddTab.TabIndex = 1;
            this.btnAddTab.Text = "添加卡片";
            this.btnAddTab.UseVisualStyleBackColor = true;
            this.btnAddTab.Click += new System.EventHandler(this.btnAddTab_Click);
            // 
            // btnActiveLeft
            // 
            this.btnActiveLeft.Location = new System.Drawing.Point(146, 41);
            this.btnActiveLeft.Name = "btnActiveLeft";
            this.btnActiveLeft.Size = new System.Drawing.Size(50, 33);
            this.btnActiveLeft.TabIndex = 2;
            this.btnActiveLeft.Text = "<";
            this.btnActiveLeft.UseVisualStyleBackColor = true;
            this.btnActiveLeft.Click += new System.EventHandler(this.btnActiveLeft_Click);
            // 
            // btnActiveRight
            // 
            this.btnActiveRight.Location = new System.Drawing.Point(202, 42);
            this.btnActiveRight.Name = "btnActiveRight";
            this.btnActiveRight.Size = new System.Drawing.Size(50, 33);
            this.btnActiveRight.TabIndex = 2;
            this.btnActiveRight.Text = ">";
            this.btnActiveRight.UseVisualStyleBackColor = true;
            this.btnActiveRight.Click += new System.EventHandler(this.btnActiveRight_Click);
            // 
            // frmTabControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 338);
            this.Controls.Add(this.btnActiveRight);
            this.Controls.Add(this.btnActiveLeft);
            this.Controls.Add(this.btnAddTab);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmTabControl";
            this.Text = "frmTabControl";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnAddTab;
        private System.Windows.Forms.Button btnActiveLeft;
        private System.Windows.Forms.Button btnActiveRight;
    }
}