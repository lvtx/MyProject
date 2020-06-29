namespace MathArithmetic
{
    partial class frmExpr
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
            this.Panel1 = new System.Windows.Forms.Panel();
            this.lblPostfix = new System.Windows.Forms.Label();
            this.lblPrefix = new System.Windows.Forms.Label();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtExpr = new System.Windows.Forms.TextBox();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.Panel1.SuspendLayout();
            this.StatusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.lblPostfix);
            this.Panel1.Controls.Add(this.lblPrefix);
            this.Panel1.Controls.Add(this.StatusStrip1);
            this.Panel1.Controls.Add(this.btnClear);
            this.Panel1.Controls.Add(this.txtExpr);
            this.Panel1.Controls.Add(this.btnCalculate);
            this.Panel1.Controls.Add(this.lblInfo);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(378, 171);
            this.Panel1.TabIndex = 7;
            // 
            // lblPostfix
            // 
            this.lblPostfix.AutoSize = true;
            this.lblPostfix.Location = new System.Drawing.Point(78, 122);
            this.lblPostfix.Name = "lblPostfix";
            this.lblPostfix.Size = new System.Drawing.Size(77, 12);
            this.lblPostfix.TabIndex = 6;
            this.lblPostfix.Text = "后序表达式：";
            // 
            // lblPrefix
            // 
            this.lblPrefix.AutoSize = true;
            this.lblPrefix.Location = new System.Drawing.Point(78, 100);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new System.Drawing.Size(77, 12);
            this.lblPrefix.TabIndex = 6;
            this.lblPrefix.Text = "前序表达式：";
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel1});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 149);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(378, 22);
            this.StatusStrip1.SizingGrip = false;
            this.StatusStrip1.TabIndex = 5;
            this.StatusStrip1.Text = "StatusStrip1";
            // 
            // ToolStripStatusLabel1
            // 
            this.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1";
            this.ToolStripStatusLabel1.Size = new System.Drawing.Size(128, 17);
            this.ToolStripStatusLabel1.Text = "在文本框中输入表达式";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(312, 32);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(54, 20);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtExpr
            // 
            this.txtExpr.Location = new System.Drawing.Point(16, 33);
            this.txtExpr.Name = "txtExpr";
            this.txtExpr.Size = new System.Drawing.Size(282, 21);
            this.txtExpr.TabIndex = 0;
            this.txtExpr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtExpr_KeyDown);
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(16, 71);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(56, 63);
            this.btnCalculate.TabIndex = 1;
            this.btnCalculate.Text = "计算";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInfo.Location = new System.Drawing.Point(78, 75);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(65, 12);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "计算结果：";
            // 
            // frmExpr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 171);
            this.Controls.Add(this.Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExpr";
            this.Text = "frmExpr";
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.Label lblPostfix;
        internal System.Windows.Forms.Label lblPrefix;
        internal System.Windows.Forms.StatusStrip StatusStrip1;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel1;
        internal System.Windows.Forms.Button btnClear;
        internal System.Windows.Forms.TextBox txtExpr;
        internal System.Windows.Forms.Button btnCalculate;
        internal System.Windows.Forms.Label lblInfo;
    }
}