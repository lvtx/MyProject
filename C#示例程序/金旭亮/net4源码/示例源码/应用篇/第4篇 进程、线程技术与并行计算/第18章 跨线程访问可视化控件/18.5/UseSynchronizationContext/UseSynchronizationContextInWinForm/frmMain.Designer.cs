namespace UseSynchronizationContextInWinForm
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
            this.btnUpdateLabel = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnUpdateLabel
            // 
            this.btnUpdateLabel.Location = new System.Drawing.Point(78, 82);
            this.btnUpdateLabel.Name = "btnUpdateLabel";
            this.btnUpdateLabel.Size = new System.Drawing.Size(170, 30);
            this.btnUpdateLabel.TabIndex = 0;
            this.btnUpdateLabel.Text = "启动新线程更新标签内容";
            this.btnUpdateLabel.UseVisualStyleBackColor = true;
            this.btnUpdateLabel.Click += new System.EventHandler(this.btnUpdateLabel_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInfo.ForeColor = System.Drawing.Color.Navy;
            this.lblInfo.Location = new System.Drawing.Point(29, 36);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(136, 16);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "按钮单击次数：0";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 154);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnUpdateLabel);
            this.Name = "frmMain";
            this.Text = "使用线程同步上下文对象跨线程访问UI控件";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdateLabel;
        private System.Windows.Forms.Label lblInfo;
    }
}

