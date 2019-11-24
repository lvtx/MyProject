namespace MaxMinValueForGP2
{
    partial class frmGPExample2
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
            this.lblMin = new System.Windows.Forms.Label();
            this.lblMax = new System.Windows.Forms.Label();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.lstData = new System.Windows.Forms.ListBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoComplex = new System.Windows.Forms.RadioButton();
            this.rdoChar = new System.Windows.Forms.RadioButton();
            this.rdoInteger = new System.Windows.Forms.RadioButton();
            this.btnFillArray = new System.Windows.Forms.Button();
            this.GroupBox2.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMin
            // 
            this.lblMin.AutoSize = true;
            this.lblMin.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMin.Location = new System.Drawing.Point(288, 175);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(65, 19);
            this.lblMin.TabIndex = 11;
            this.lblMin.Text = "最小值：";
            // 
            // lblMax
            // 
            this.lblMax.AutoSize = true;
            this.lblMax.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMax.Location = new System.Drawing.Point(288, 137);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(65, 19);
            this.lblMax.TabIndex = 12;
            this.lblMax.Text = "最大值：";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.lstData);
            this.GroupBox2.Location = new System.Drawing.Point(12, 12);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(248, 239);
            this.GroupBox2.TabIndex = 10;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "数据列表";
            // 
            // lstData
            // 
            this.lstData.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lstData.FormattingEnabled = true;
            this.lstData.ItemHeight = 19;
            this.lstData.Location = new System.Drawing.Point(16, 20);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(211, 194);
            this.lstData.TabIndex = 0;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(292, 214);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(151, 22);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.rdoComplex);
            this.GroupBox1.Controls.Add(this.rdoChar);
            this.GroupBox1.Controls.Add(this.rdoInteger);
            this.GroupBox1.Controls.Add(this.btnFillArray);
            this.GroupBox1.Location = new System.Drawing.Point(279, 32);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(181, 81);
            this.GroupBox1.TabIndex = 9;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "向列表中加入数据";
            // 
            // rdoComplex
            // 
            this.rdoComplex.AutoSize = true;
            this.rdoComplex.Location = new System.Drawing.Point(119, 20);
            this.rdoComplex.Name = "rdoComplex";
            this.rdoComplex.Size = new System.Drawing.Size(47, 16);
            this.rdoComplex.TabIndex = 1;
            this.rdoComplex.Text = "复数";
            this.rdoComplex.UseVisualStyleBackColor = true;
            // 
            // rdoChar
            // 
            this.rdoChar.AutoSize = true;
            this.rdoChar.Location = new System.Drawing.Point(66, 20);
            this.rdoChar.Name = "rdoChar";
            this.rdoChar.Size = new System.Drawing.Size(47, 16);
            this.rdoChar.TabIndex = 1;
            this.rdoChar.Text = "字符";
            this.rdoChar.UseVisualStyleBackColor = true;
            // 
            // rdoInteger
            // 
            this.rdoInteger.AutoSize = true;
            this.rdoInteger.Checked = true;
            this.rdoInteger.Location = new System.Drawing.Point(13, 20);
            this.rdoInteger.Name = "rdoInteger";
            this.rdoInteger.Size = new System.Drawing.Size(47, 16);
            this.rdoInteger.TabIndex = 1;
            this.rdoInteger.TabStop = true;
            this.rdoInteger.Text = "整数";
            this.rdoInteger.UseVisualStyleBackColor = true;
            // 
            // btnFillArray
            // 
            this.btnFillArray.Location = new System.Drawing.Point(13, 42);
            this.btnFillArray.Name = "btnFillArray";
            this.btnFillArray.Size = new System.Drawing.Size(153, 25);
            this.btnFillArray.TabIndex = 2;
            this.btnFillArray.Text = "埴充列表";
            this.btnFillArray.UseVisualStyleBackColor = true;
            this.btnFillArray.Click += new System.EventHandler(this.btnFillArray_Click);
            // 
            // frmGPExample2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 275);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.lblMax);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.GroupBox1);
            this.Name = "frmGPExample2";
            this.Text = "泛型引例：求数组的最大值和最小值";
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lblMin;
        internal System.Windows.Forms.Label lblMax;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.ListBox lstData;
        internal System.Windows.Forms.Button btnExit;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.RadioButton rdoComplex;
        internal System.Windows.Forms.RadioButton rdoChar;
        internal System.Windows.Forms.RadioButton rdoInteger;
        internal System.Windows.Forms.Button btnFillArray;
    }
}

