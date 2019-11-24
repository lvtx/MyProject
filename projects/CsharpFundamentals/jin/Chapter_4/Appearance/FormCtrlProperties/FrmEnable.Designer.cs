namespace FormCtrlProperties
{
    partial class FrmEnable
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
            this.rdoDisable = new System.Windows.Forms.RadioButton();
            this.rdoEnable = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rdoDisable
            // 
            this.rdoDisable.AutoSize = true;
            this.rdoDisable.Location = new System.Drawing.Point(341, 206);
            this.rdoDisable.Margin = new System.Windows.Forms.Padding(4);
            this.rdoDisable.Name = "rdoDisable";
            this.rdoDisable.Size = new System.Drawing.Size(58, 19);
            this.rdoDisable.TabIndex = 5;
            this.rdoDisable.Text = "禁用";
            this.rdoDisable.UseVisualStyleBackColor = true;
            this.rdoDisable.CheckedChanged += new System.EventHandler(this.RdoDisable_CheckedChanged);
            // 
            // rdoEnable
            // 
            this.rdoEnable.AutoSize = true;
            this.rdoEnable.Checked = true;
            this.rdoEnable.Location = new System.Drawing.Point(77, 206);
            this.rdoEnable.Margin = new System.Windows.Forms.Padding(4);
            this.rdoEnable.Name = "rdoEnable";
            this.rdoEnable.Size = new System.Drawing.Size(58, 19);
            this.rdoEnable.TabIndex = 4;
            this.rdoEnable.TabStop = true;
            this.rdoEnable.Text = "激活";
            this.rdoEnable.UseVisualStyleBackColor = true;
            this.rdoEnable.CheckedChanged += new System.EventHandler(this.RdoEnable_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(65, 69);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(355, 102);
            this.button1.TabIndex = 3;
            this.button1.Text = "我是一个大按钮";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // FrmEnable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 294);
            this.Controls.Add(this.rdoDisable);
            this.Controls.Add(this.rdoEnable);
            this.Controls.Add(this.button1);
            this.Name = "FrmEnable";
            this.Text = "FrmEnable";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoDisable;
        private System.Windows.Forms.RadioButton rdoEnable;
        private System.Windows.Forms.Button button1;
    }
}