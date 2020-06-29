namespace MathArithmetic
{
    partial class frmSetup
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
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoExprTree = new System.Windows.Forms.RadioButton();
            this.rdoPrefix = new System.Windows.Forms.RadioButton();
            this.rdoInfix = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.rdoExprTree);
            this.GroupBox1.Controls.Add(this.rdoPrefix);
            this.GroupBox1.Controls.Add(this.rdoInfix);
            this.GroupBox1.Location = new System.Drawing.Point(12, 12);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(250, 110);
            this.GroupBox1.TabIndex = 2;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "选择算法";
            // 
            // rdoExprTree
            // 
            this.rdoExprTree.AutoSize = true;
            this.rdoExprTree.Location = new System.Drawing.Point(20, 74);
            this.rdoExprTree.Name = "rdoExprTree";
            this.rdoExprTree.Size = new System.Drawing.Size(95, 16);
            this.rdoExprTree.TabIndex = 0;
            this.rdoExprTree.Text = "表达式树算法";
            this.rdoExprTree.UseVisualStyleBackColor = true;
            // 
            // rdoPrefix
            // 
            this.rdoPrefix.AutoSize = true;
            this.rdoPrefix.Location = new System.Drawing.Point(20, 52);
            this.rdoPrefix.Name = "rdoPrefix";
            this.rdoPrefix.Size = new System.Drawing.Size(167, 16);
            this.rdoPrefix.TabIndex = 0;
            this.rdoPrefix.Text = "前序算法（使用一个堆栈）";
            this.rdoPrefix.UseVisualStyleBackColor = true;
            // 
            // rdoInfix
            // 
            this.rdoInfix.AutoSize = true;
            this.rdoInfix.Checked = true;
            this.rdoInfix.Location = new System.Drawing.Point(20, 30);
            this.rdoInfix.Name = "rdoInfix";
            this.rdoInfix.Size = new System.Drawing.Size(167, 16);
            this.rdoInfix.TabIndex = 0;
            this.rdoInfix.TabStop = true;
            this.rdoInfix.Text = "中序算法（使用两个堆栈）";
            this.rdoInfix.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(105, 140);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(71, 25);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(191, 140);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 180);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetup";
            this.Text = "计算器算法参数设置";
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.RadioButton rdoExprTree;
        internal System.Windows.Forms.RadioButton rdoPrefix;
        internal System.Windows.Forms.RadioButton rdoInfix;
        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.Button btnCancel;
    }
}