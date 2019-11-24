namespace WindowEmbedInWinForm
{
    partial class frmBottom
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
            this.label1 = new System.Windows.Forms.Label();
            this.UIControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // UIControlPanel
            // 
            this.UIControlPanel.BackColor = System.Drawing.Color.Yellow;
            this.UIControlPanel.Controls.Add(this.label1);
            this.UIControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UIControlPanel.Location = new System.Drawing.Point(0, 0);
            this.UIControlPanel.Name = "UIControlPanel";
            this.UIControlPanel.Size = new System.Drawing.Size(800, 450);
            this.UIControlPanel.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 38);
            this.label1.TabIndex = 1;
            this.label1.Text = "frmBottom";
            // 
            // frmBottom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.UIControlPanel);
            this.Name = "frmBottom";
            this.Text = "frmBottom";
            this.UIControlPanel.ResumeLayout(false);
            this.UIControlPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Panel UIControlPanel;
    }
}