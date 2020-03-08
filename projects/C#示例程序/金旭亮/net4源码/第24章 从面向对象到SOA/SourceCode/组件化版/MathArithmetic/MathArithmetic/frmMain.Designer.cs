namespace MathArithmetic
{
    partial class frmMain
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
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUseStandardForm = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUseSimple = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.MenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(157, 6);
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEnd});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(284, 25);
            this.MenuStrip1.TabIndex = 8;
            this.MenuStrip1.Text = "MenuStrip1";
            // 
            // mnuEnd
            // 
            this.mnuEnd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuUseStandardForm,
            this.mnuUseSimple,
            this.ToolStripSeparator2,
            this.mnuSetup,
            this.ToolStripMenuItem1,
            this.mnuExit});
            this.mnuEnd.Name = "mnuEnd";
            this.mnuEnd.Size = new System.Drawing.Size(80, 21);
            this.mnuEnd.Text = "计算器功能";
            // 
            // mnuUseStandardForm
            // 
            this.mnuUseStandardForm.Checked = true;
            this.mnuUseStandardForm.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.mnuUseStandardForm.Name = "mnuUseStandardForm";
            this.mnuUseStandardForm.Size = new System.Drawing.Size(160, 22);
            this.mnuUseStandardForm.Text = "使用标准型界面";
            this.mnuUseStandardForm.Click += new System.EventHandler(this.mnuUseStandardForm_Click);
            // 
            // mnuUseSimple
            // 
            this.mnuUseSimple.Name = "mnuUseSimple";
            this.mnuUseSimple.Size = new System.Drawing.Size(160, 22);
            this.mnuUseSimple.Text = "使用精简型界面";
            this.mnuUseSimple.Click += new System.EventHandler(this.mnuUseSimple_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // mnuSetup
            // 
            this.mnuSetup.Name = "mnuSetup";
            this.mnuSetup.Size = new System.Drawing.Size(160, 22);
            this.mnuSetup.Text = "算法设置";
            this.mnuSetup.Click += new System.EventHandler(this.mnuSetup_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(160, 22);
            this.mnuExit.Text = "退出";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // Panel1
            // 
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(284, 262);
            this.Panel1.TabIndex = 9;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.MenuStrip1);
            this.Controls.Add(this.Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "我的四则运算计算器";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem1;
        internal System.Windows.Forms.MenuStrip MenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem mnuEnd;
        internal System.Windows.Forms.ToolStripMenuItem mnuUseStandardForm;
        internal System.Windows.Forms.ToolStripMenuItem mnuUseSimple;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.ToolStripMenuItem mnuSetup;
        internal System.Windows.Forms.ToolStripMenuItem mnuExit;
        internal System.Windows.Forms.Panel Panel1;
    }
}