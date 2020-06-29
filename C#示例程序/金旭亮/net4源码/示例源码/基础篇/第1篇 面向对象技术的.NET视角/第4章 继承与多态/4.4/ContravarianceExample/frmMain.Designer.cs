namespace CovarianceExample
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
            this.btnTestCovariance = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnTestCovariance
            // 
            this.btnTestCovariance.Location = new System.Drawing.Point(78, 32);
            this.btnTestCovariance.Name = "btnTestCovariance";
            this.btnTestCovariance.Size = new System.Drawing.Size(196, 48);
            this.btnTestCovariance.TabIndex = 0;
            this.btnTestCovariance.Text = "用鼠标单击或直接敲击键盘";
            this.btnTestCovariance.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(76, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "本示例展示Contravariance特性的应用";
            // 
            // lblInfo
            // 
            this.lblInfo.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInfo.ForeColor = System.Drawing.Color.Navy;
            this.lblInfo.Location = new System.Drawing.Point(78, 127);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(182, 24);
            this.lblInfo.TabIndex = 2;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 177);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTestCovariance);
            this.Name = "frmMain";
            this.Text = "使用同一个函数同时响应鼠标和键盘操作";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTestCovariance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblInfo;
    }
}

