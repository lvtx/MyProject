namespace UseEventHandler2
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
            this.lblLoanCount = new System.Windows.Forms.Label();
            this.lblLoanMoney = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSum = new System.Windows.Forms.Button();
            this.btnLoanFromHuang = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLoanCount
            // 
            this.lblLoanCount.AutoSize = true;
            this.lblLoanCount.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLoanCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblLoanCount.Location = new System.Drawing.Point(360, 54);
            this.lblLoanCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoanCount.Name = "lblLoanCount";
            this.lblLoanCount.Size = new System.Drawing.Size(37, 18);
            this.lblLoanCount.TabIndex = 8;
            this.lblLoanCount.Text = "0次";
            // 
            // lblLoanMoney
            // 
            this.lblLoanMoney.AutoSize = true;
            this.lblLoanMoney.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLoanMoney.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblLoanMoney.Location = new System.Drawing.Point(360, 122);
            this.lblLoanMoney.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoanMoney.Name = "lblLoanMoney";
            this.lblLoanMoney.Size = new System.Drawing.Size(37, 18);
            this.lblLoanMoney.TabIndex = 9;
            this.lblLoanMoney.Text = "0元";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(265, 54);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "借款次数：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(265, 124);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "欠款总额：";
            // 
            // btnSum
            // 
            this.btnSum.Enabled = false;
            this.btnSum.Location = new System.Drawing.Point(53, 107);
            this.btnSum.Margin = new System.Windows.Forms.Padding(4);
            this.btnSum.Name = "btnSum";
            this.btnSum.Size = new System.Drawing.Size(192, 50);
            this.btnSum.TabIndex = 5;
            this.btnSum.Text = "看看我现在欠了黄世仁多少钱！";
            this.btnSum.UseVisualStyleBackColor = true;

            // 
            // btnLoanFromHuang
            // 
            this.btnLoanFromHuang.Location = new System.Drawing.Point(53, 41);
            this.btnLoanFromHuang.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoanFromHuang.Name = "btnLoanFromHuang";
            this.btnLoanFromHuang.Size = new System.Drawing.Size(193, 44);
            this.btnLoanFromHuang.TabIndex = 4;
            this.btnLoanFromHuang.Text = "向黄世仁借钱100元";
            this.btnLoanFromHuang.UseVisualStyleBackColor = true;
            this.btnLoanFromHuang.Click += new System.EventHandler(this.btnLoanFromHuang_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 229);
            this.Controls.Add(this.lblLoanCount);
            this.Controls.Add(this.lblLoanMoney);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSum);
            this.Controls.Add(this.btnLoanFromHuang);
            this.Name = "frmMain";
            this.Text = "杨白劳的帐本";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLoanCount;
        private System.Windows.Forms.Label lblLoanMoney;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSum;
        private System.Windows.Forms.Button btnLoanFromHuang;
    }
}

