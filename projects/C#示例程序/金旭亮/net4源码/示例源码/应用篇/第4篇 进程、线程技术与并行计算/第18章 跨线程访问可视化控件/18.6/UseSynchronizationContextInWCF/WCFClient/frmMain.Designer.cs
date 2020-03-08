namespace WCFClient
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
            this.btnVisitWCFService = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnVisitWCFService
            // 
            this.btnVisitWCFService.Location = new System.Drawing.Point(95, 63);
            this.btnVisitWCFService.Name = "btnVisitWCFService";
            this.btnVisitWCFService.Size = new System.Drawing.Size(147, 31);
            this.btnVisitWCFService.TabIndex = 1;
            this.btnVisitWCFService.Text = "访问WCF服务";
            this.btnVisitWCFService.UseVisualStyleBackColor = true;
            this.btnVisitWCFService.Click += new System.EventHandler(this.btnVisitWCFService_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInfo.Location = new System.Drawing.Point(27, 25);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(171, 16);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "点击按钮访问WCF服务";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 127);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnVisitWCFService);
            this.Name = "frmMain";
            this.Text = "WCF客户端";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnVisitWCFService;
        private System.Windows.Forms.Label lblInfo;
    }
}

