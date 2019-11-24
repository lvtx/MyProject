namespace FormCtrlProperty
{
    partial class frmEnable
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
            this.button1 = new System.Windows.Forms.Button();
            this.rdoEnable = new System.Windows.Forms.RadioButton();
            this.rdoDisable = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(49, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(266, 82);
            this.button1.TabIndex = 0;
            this.button1.Text = "我是一个大按钮";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // rdoEnable
            // 
            this.rdoEnable.AutoSize = true;
            this.rdoEnable.Checked = true;
            this.rdoEnable.Location = new System.Drawing.Point(58, 150);
            this.rdoEnable.Name = "rdoEnable";
            this.rdoEnable.Size = new System.Drawing.Size(47, 16);
            this.rdoEnable.TabIndex = 1;
            this.rdoEnable.TabStop = true;
            this.rdoEnable.Text = "激活";
            this.rdoEnable.UseVisualStyleBackColor = true;
            this.rdoEnable.CheckedChanged += new System.EventHandler(this.rdoEnable_CheckedChanged);
            // 
            // rdoDisable
            // 
            this.rdoDisable.AutoSize = true;
            this.rdoDisable.Location = new System.Drawing.Point(256, 150);
            this.rdoDisable.Name = "rdoDisable";
            this.rdoDisable.Size = new System.Drawing.Size(47, 16);
            this.rdoDisable.TabIndex = 2;
            this.rdoDisable.Text = "禁用";
            this.rdoDisable.UseVisualStyleBackColor = true;
            this.rdoDisable.CheckedChanged += new System.EventHandler(this.rdoDisable_CheckedChanged);
            // 
            // frmEnable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 218);
            this.Controls.Add(this.rdoDisable);
            this.Controls.Add(this.rdoEnable);
            this.Controls.Add(this.button1);
            this.Name = "frmEnable";
            this.Text = "frmEnable";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton rdoEnable;
        private System.Windows.Forms.RadioButton rdoDisable;
    }
}