namespace UseEventHandler2
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoanFromHuang = new System.Windows.Forms.Button();
            this.btnSum = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLoanMoney = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblLoanCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLoanFromHuang
            // 
            this.btnLoanFromHuang.Location = new System.Drawing.Point(48, 22);
            this.btnLoanFromHuang.Name = "btnLoanFromHuang";
            this.btnLoanFromHuang.Size = new System.Drawing.Size(145, 35);
            this.btnLoanFromHuang.TabIndex = 0;
            this.btnLoanFromHuang.Text = "向黄世仁借钱100元";
            this.btnLoanFromHuang.UseVisualStyleBackColor = true;
            this.btnLoanFromHuang.Click += new System.EventHandler(this.btnLoanFromHuang_Click);
            // 
            // btnSum
            // 
            this.btnSum.Enabled = false;
            this.btnSum.Location = new System.Drawing.Point(48, 75);
            this.btnSum.Name = "btnSum";
            this.btnSum.Size = new System.Drawing.Size(144, 40);
            this.btnSum.TabIndex = 1;
            this.btnSum.Text = "看看我现在欠了黄世仁多少钱！";
            this.btnSum.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(207, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "欠款总额：";
            // 
            // lblLoanMoney
            // 
            this.lblLoanMoney.AutoSize = true;
            this.lblLoanMoney.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLoanMoney.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblLoanMoney.Location = new System.Drawing.Point(278, 87);
            this.lblLoanMoney.Name = "lblLoanMoney";
            this.lblLoanMoney.Size = new System.Drawing.Size(30, 14);
            this.lblLoanMoney.TabIndex = 3;
            this.lblLoanMoney.Text = "0元";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(207, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "借款次数：";
            // 
            // lblLoanCount
            // 
            this.lblLoanCount.AutoSize = true;
            this.lblLoanCount.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLoanCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblLoanCount.Location = new System.Drawing.Point(278, 33);
            this.lblLoanCount.Name = "lblLoanCount";
            this.lblLoanCount.Size = new System.Drawing.Size(30, 14);
            this.lblLoanCount.TabIndex = 3;
            this.lblLoanCount.Text = "0次";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 189);
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

        private System.Windows.Forms.Button btnLoanFromHuang;
        private System.Windows.Forms.Button btnSum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLoanMoney;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblLoanCount;
    }
}

