namespace ReceiveInfoFromCenter
{
    partial class frmSub
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnIncreaseCounter = new System.Windows.Forms.Button();
            this.btnNewForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.BackColor = System.Drawing.SystemColors.Control;
            this.lblInfo.Font = new System.Drawing.Font("SimSun", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInfo.Location = new System.Drawing.Point(172, 43);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(37, 38);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "0";
            // 
            // btnIncreaseCounter
            // 
            this.btnIncreaseCounter.Location = new System.Drawing.Point(98, 133);
            this.btnIncreaseCounter.Name = "btnIncreaseCounter";
            this.btnIncreaseCounter.Size = new System.Drawing.Size(179, 79);
            this.btnIncreaseCounter.TabIndex = 2;
            this.btnIncreaseCounter.Text = "单击增加主窗体计数";
            this.btnIncreaseCounter.UseVisualStyleBackColor = true;
            this.btnIncreaseCounter.Click += new System.EventHandler(this.btnIncreaseCounter_Click);
            // 
            // btnNewForm
            // 
            this.btnNewForm.Location = new System.Drawing.Point(55, 261);
            this.btnNewForm.Margin = new System.Windows.Forms.Padding(4);
            this.btnNewForm.Name = "btnNewForm";
            this.btnNewForm.Size = new System.Drawing.Size(253, 62);
            this.btnNewForm.TabIndex = 5;
            this.btnNewForm.Text = "点击我新建一个主窗体对象";
            this.btnNewForm.UseVisualStyleBackColor = true;
            this.btnNewForm.Click += new System.EventHandler(this.btnNewForm_Click);
            // 
            // frmSub
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 389);
            this.Controls.Add(this.btnNewForm);
            this.Controls.Add(this.btnIncreaseCounter);
            this.Controls.Add(this.lblInfo);
            this.Name = "frmSub";
            this.Text = "子窗体";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnIncreaseCounter;
        private System.Windows.Forms.Button btnNewForm;
    }
}