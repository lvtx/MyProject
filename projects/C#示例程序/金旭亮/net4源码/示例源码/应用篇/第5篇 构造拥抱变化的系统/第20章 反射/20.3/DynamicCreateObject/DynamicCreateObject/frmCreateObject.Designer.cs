namespace DynamicCreateObject
{
    partial class frmCreateObject
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
            this.ComboBox1 = new System.Windows.Forms.ComboBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.btnEquals = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ComboBox1
            // 
            this.ComboBox1.FormattingEnabled = true;
            this.ComboBox1.Items.AddRange(new object[] {
            "+",
            "-",
            "*",
            "/"});
            this.ComboBox1.Location = new System.Drawing.Point(102, 14);
            this.ComboBox1.Name = "ComboBox1";
            this.ComboBox1.Size = new System.Drawing.Size(45, 20);
            this.ComboBox1.TabIndex = 103;
            this.ComboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.Location = new System.Drawing.Point(251, 12);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(21, 24);
            this.lblResult.TabIndex = 102;
            this.lblResult.Text = "?";
            // 
            // TextBox2
            // 
            this.TextBox2.Location = new System.Drawing.Point(153, 14);
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.Size = new System.Drawing.Size(58, 21);
            this.TextBox2.TabIndex = 101;
            this.TextBox2.Text = "2";
            // 
            // TextBox1
            // 
            this.TextBox1.Location = new System.Drawing.Point(27, 12);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(69, 21);
            this.TextBox1.TabIndex = 104;
            this.TextBox1.Text = "100";
            // 
            // btnEquals
            // 
            this.btnEquals.Location = new System.Drawing.Point(227, 14);
            this.btnEquals.Name = "btnEquals";
            this.btnEquals.Size = new System.Drawing.Size(18, 21);
            this.btnEquals.TabIndex = 105;
            this.btnEquals.Text = "=";
            this.btnEquals.UseVisualStyleBackColor = true;
            this.btnEquals.Click += new System.EventHandler(this.btnEquals_Click);
            // 
            // frmCreateObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 54);
            this.Controls.Add(this.btnEquals);
            this.Controls.Add(this.ComboBox1);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.TextBox2);
            this.Controls.Add(this.TextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCreateObject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "反射示例之动态创建对象";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ComboBox ComboBox1;
        internal System.Windows.Forms.Label lblResult;
        internal System.Windows.Forms.TextBox TextBox2;
        internal System.Windows.Forms.TextBox TextBox1;
        private System.Windows.Forms.Button btnEquals;
    }
}

