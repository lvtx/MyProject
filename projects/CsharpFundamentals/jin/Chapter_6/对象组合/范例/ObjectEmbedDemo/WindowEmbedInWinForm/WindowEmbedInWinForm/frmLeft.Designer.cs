namespace WindowEmbedInWinForm
{
    partial class frmLeft
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
            this.UIControlPanel = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.UIControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // UIControlPanel
            // 
            this.UIControlPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.UIControlPanel.Controls.Add(this.lblInfo);
            this.UIControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UIControlPanel.Location = new System.Drawing.Point(0, 0);
            this.UIControlPanel.Name = "UIControlPanel";
            this.UIControlPanel.Size = new System.Drawing.Size(284, 262);
            this.UIControlPanel.TabIndex = 0;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(12, 9);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(63, 18);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "frmLeft";
            // 
            // frmLeft
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.UIControlPanel);
            this.Name = "frmLeft";
            this.Text = "frmLeft";
            this.UIControlPanel.ResumeLayout(false);
            this.UIControlPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lblInfo;
        public System.Windows.Forms.Panel UIControlPanel;
    }
}