namespace UseMathWebService
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
            this.txtExpr = new System.Windows.Forms.TextBox();
            this.btnCalculator = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtExpr
            // 
            this.txtExpr.Location = new System.Drawing.Point(28, 30);
            this.txtExpr.Name = "txtExpr";
            this.txtExpr.Size = new System.Drawing.Size(358, 21);
            this.txtExpr.TabIndex = 0;
            // 
            // btnCalculator
            // 
            this.btnCalculator.Location = new System.Drawing.Point(408, 24);
            this.btnCalculator.Name = "btnCalculator";
            this.btnCalculator.Size = new System.Drawing.Size(65, 30);
            this.btnCalculator.TabIndex = 1;
            this.btnCalculator.Text = "计算";
            this.btnCalculator.UseVisualStyleBackColor = true;
            this.btnCalculator.Click += new System.EventHandler(this.btnCalculator_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(30, 91);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(344, 23);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "输入要计算的表达式后点击“计算”按钮";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 137);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnCalculator);
            this.Controls.Add(this.txtExpr);
            this.Name = "frmMain";
            this.Text = "调用四则运算服务";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtExpr;
        private System.Windows.Forms.Button btnCalculator;
        private System.Windows.Forms.Label lblInfo;
    }
}

