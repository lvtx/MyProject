namespace DynamicInvokeMethodForCS
{
    partial class frmDynamicInvokeMethod
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
            this.txtNumber1 = new System.Windows.Forms.TextBox();
            this.txtNumber2 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoAdd = new System.Windows.Forms.RadioButton();
            this.rdoSubtract = new System.Windows.Forms.RadioButton();
            this.rdoMultiply = new System.Windows.Forms.RadioButton();
            this.rdoDivid = new System.Windows.Forms.RadioButton();
            this.lblInfo = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtNumber1
            // 
            this.txtNumber1.Location = new System.Drawing.Point(119, 20);
            this.txtNumber1.Name = "txtNumber1";
            this.txtNumber1.Size = new System.Drawing.Size(127, 21);
            this.txtNumber1.TabIndex = 0;
            this.txtNumber1.Text = "0";
            this.txtNumber1.TextChanged += new System.EventHandler(this.txtNumber1_TextChanged);
            // 
            // txtNumber2
            // 
            this.txtNumber2.Location = new System.Drawing.Point(119, 63);
            this.txtNumber2.Name = "txtNumber2";
            this.txtNumber2.Size = new System.Drawing.Size(127, 21);
            this.txtNumber2.TabIndex = 0;
            this.txtNumber2.Text = "0";
            this.txtNumber2.TextChanged += new System.EventHandler(this.txtNumber2_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtNumber1);
            this.groupBox1.Controls.Add(this.txtNumber2);
            this.groupBox1.Location = new System.Drawing.Point(40, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 108);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作数";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "操作数一：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "操作数二：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoDivid);
            this.groupBox2.Controls.Add(this.rdoMultiply);
            this.groupBox2.Controls.Add(this.rdoSubtract);
            this.groupBox2.Controls.Add(this.rdoAdd);
            this.groupBox2.Location = new System.Drawing.Point(41, 135);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 61);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择运算方法";
            // 
            // rdoAdd
            // 
            this.rdoAdd.AutoSize = true;
            this.rdoAdd.Checked = true;
            this.rdoAdd.Location = new System.Drawing.Point(20, 24);
            this.rdoAdd.Name = "rdoAdd";
            this.rdoAdd.Size = new System.Drawing.Size(35, 16);
            this.rdoAdd.TabIndex = 0;
            this.rdoAdd.TabStop = true;
            this.rdoAdd.Text = "加";
            this.rdoAdd.UseVisualStyleBackColor = true;
            this.rdoAdd.CheckedChanged += new System.EventHandler(this.rdoAdd_CheckedChanged);
            // 
            // rdoSubtract
            // 
            this.rdoSubtract.AutoSize = true;
            this.rdoSubtract.Location = new System.Drawing.Point(81, 24);
            this.rdoSubtract.Name = "rdoSubtract";
            this.rdoSubtract.Size = new System.Drawing.Size(35, 16);
            this.rdoSubtract.TabIndex = 0;
            this.rdoSubtract.Text = "减";
            this.rdoSubtract.UseVisualStyleBackColor = true;
            this.rdoSubtract.CheckedChanged += new System.EventHandler(this.rdoSubtract_CheckedChanged);
            // 
            // rdoMultiply
            // 
            this.rdoMultiply.AutoSize = true;
            this.rdoMultiply.Location = new System.Drawing.Point(140, 24);
            this.rdoMultiply.Name = "rdoMultiply";
            this.rdoMultiply.Size = new System.Drawing.Size(35, 16);
            this.rdoMultiply.TabIndex = 0;
            this.rdoMultiply.Text = "乘";
            this.rdoMultiply.UseVisualStyleBackColor = true;
            this.rdoMultiply.CheckedChanged += new System.EventHandler(this.rdoMultiply_CheckedChanged);
            // 
            // rdoDivid
            // 
            this.rdoDivid.AutoSize = true;
            this.rdoDivid.Location = new System.Drawing.Point(210, 24);
            this.rdoDivid.Name = "rdoDivid";
            this.rdoDivid.Size = new System.Drawing.Size(35, 16);
            this.rdoDivid.TabIndex = 0;
            this.rdoDivid.Text = "除";
            this.rdoDivid.UseVisualStyleBackColor = true;
            this.rdoDivid.CheckedChanged += new System.EventHandler(this.rdoDivid_CheckedChanged);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(48, 217);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(79, 21);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "结果：0";
            // 
            // frmDynamicInvokeMethod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 264);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDynamicInvokeMethod";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[委托示例]动态调用方法";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNumber1;
        private System.Windows.Forms.TextBox txtNumber2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoAdd;
        private System.Windows.Forms.RadioButton rdoDivid;
        private System.Windows.Forms.RadioButton rdoMultiply;
        private System.Windows.Forms.RadioButton rdoSubtract;
        private System.Windows.Forms.Label lblInfo;
    }
}

