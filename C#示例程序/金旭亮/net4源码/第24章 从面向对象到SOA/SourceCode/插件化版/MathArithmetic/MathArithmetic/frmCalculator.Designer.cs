namespace MathArithmetic
{
    partial class frmCalculator
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
            this.components = new System.ComponentModel.Container();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtExpr = new System.Windows.Forms.TextBox();
            this.btn7 = new System.Windows.Forms.Button();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnDiv = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnDot = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnMulti = new System.Windows.Forms.Button();
            this.btn8 = new System.Windows.Forms.Button();
            this.btnEqual = new System.Windows.Forms.Button();
            this.btn4 = new System.Windows.Forms.Button();
            this.btn3 = new System.Windows.Forms.Button();
            this.btn9 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSub = new System.Windows.Forms.Button();
            this.btn1 = new System.Windows.Forms.Button();
            this.btn6 = new System.Windows.Forms.Button();
            this.btn5 = new System.Windows.Forms.Button();
            this.btn0 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.StatusStrip1.SuspendLayout();
            this.Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel1});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 242);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(220, 22);
            this.StatusStrip1.SizingGrip = false;
            this.StatusStrip1.TabIndex = 3;
            this.StatusStrip1.Text = "StatusStrip1";
            // 
            // ToolStripStatusLabel1
            // 
            this.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1";
            this.ToolStripStatusLabel1.Size = new System.Drawing.Size(116, 17);
            this.ToolStripStatusLabel1.Text = "单击按键输入表达式";
            // 
            // txtExpr
            // 
            this.txtExpr.BackColor = System.Drawing.Color.White;
            this.txtExpr.Location = new System.Drawing.Point(22, 51);
            this.txtExpr.Name = "txtExpr";
            this.txtExpr.ReadOnly = true;
            this.txtExpr.Size = new System.Drawing.Size(174, 21);
            this.txtExpr.TabIndex = 1;
            this.txtExpr.Text = "0.";
            this.txtExpr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btn7
            // 
            this.btn7.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn7.Location = new System.Drawing.Point(22, 78);
            this.btn7.Name = "btn7";
            this.btn7.Size = new System.Drawing.Size(30, 31);
            this.btn7.TabIndex = 0;
            this.btn7.Text = "7";
            this.btn7.UseVisualStyleBackColor = true;
            this.btn7.Click += new System.EventHandler(this.btn7_Click);
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.StatusStrip1);
            this.Panel1.Controls.Add(this.txtExpr);
            this.Panel1.Controls.Add(this.btn7);
            this.Panel1.Controls.Add(this.btnLeft);
            this.Panel1.Controls.Add(this.btnDiv);
            this.Panel1.Controls.Add(this.btnRight);
            this.Panel1.Controls.Add(this.btnDot);
            this.Panel1.Controls.Add(this.btnClear);
            this.Panel1.Controls.Add(this.btnMulti);
            this.Panel1.Controls.Add(this.btn8);
            this.Panel1.Controls.Add(this.btnEqual);
            this.Panel1.Controls.Add(this.btn4);
            this.Panel1.Controls.Add(this.btn3);
            this.Panel1.Controls.Add(this.btn9);
            this.Panel1.Controls.Add(this.btn2);
            this.Panel1.Controls.Add(this.btnAdd);
            this.Panel1.Controls.Add(this.btnSub);
            this.Panel1.Controls.Add(this.btn1);
            this.Panel1.Controls.Add(this.btn6);
            this.Panel1.Controls.Add(this.btn5);
            this.Panel1.Controls.Add(this.btn0);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(220, 264);
            this.Panel1.TabIndex = 4;
            // 
            // btnLeft
            // 
            this.btnLeft.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLeft.Location = new System.Drawing.Point(166, 115);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(30, 31);
            this.btnLeft.TabIndex = 0;
            this.btnLeft.Text = "(";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnDiv
            // 
            this.btnDiv.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDiv.Location = new System.Drawing.Point(130, 189);
            this.btnDiv.Name = "btnDiv";
            this.btnDiv.Size = new System.Drawing.Size(30, 31);
            this.btnDiv.TabIndex = 0;
            this.btnDiv.Text = "/";
            this.btnDiv.UseVisualStyleBackColor = true;
            this.btnDiv.Click += new System.EventHandler(this.btnDiv_Click);
            // 
            // btnRight
            // 
            this.btnRight.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRight.Location = new System.Drawing.Point(166, 152);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(30, 31);
            this.btnRight.TabIndex = 0;
            this.btnRight.Text = ")";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnDot
            // 
            this.btnDot.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDot.Location = new System.Drawing.Point(94, 189);
            this.btnDot.Name = "btnDot";
            this.btnDot.Size = new System.Drawing.Size(30, 31);
            this.btnDot.TabIndex = 0;
            this.btnDot.Text = ".";
            this.btnDot.UseVisualStyleBackColor = true;
            this.btnDot.Click += new System.EventHandler(this.btnDot_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(166, 78);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(30, 31);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "C";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnMulti
            // 
            this.btnMulti.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMulti.Location = new System.Drawing.Point(130, 152);
            this.btnMulti.Name = "btnMulti";
            this.btnMulti.Size = new System.Drawing.Size(30, 31);
            this.btnMulti.TabIndex = 0;
            this.btnMulti.Text = "*";
            this.btnMulti.UseVisualStyleBackColor = true;
            this.btnMulti.Click += new System.EventHandler(this.btnMulti_Click);
            // 
            // btn8
            // 
            this.btn8.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn8.Location = new System.Drawing.Point(58, 78);
            this.btn8.Name = "btn8";
            this.btn8.Size = new System.Drawing.Size(30, 31);
            this.btn8.TabIndex = 0;
            this.btn8.Text = "8";
            this.btn8.UseVisualStyleBackColor = true;
            this.btn8.Click += new System.EventHandler(this.btn8_Click);
            // 
            // btnEqual
            // 
            this.btnEqual.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEqual.Location = new System.Drawing.Point(166, 189);
            this.btnEqual.Name = "btnEqual";
            this.btnEqual.Size = new System.Drawing.Size(30, 31);
            this.btnEqual.TabIndex = 0;
            this.btnEqual.Text = "=";
            this.btnEqual.UseVisualStyleBackColor = true;
            this.btnEqual.Click += new System.EventHandler(this.btnEqual_Click);
            // 
            // btn4
            // 
            this.btn4.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn4.Location = new System.Drawing.Point(22, 115);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(30, 31);
            this.btn4.TabIndex = 0;
            this.btn4.Text = "4";
            this.btn4.UseVisualStyleBackColor = true;
            this.btn4.Click += new System.EventHandler(this.btn4_Click);
            // 
            // btn3
            // 
            this.btn3.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3.Location = new System.Drawing.Point(94, 152);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(30, 31);
            this.btn3.TabIndex = 0;
            this.btn3.Text = "3";
            this.btn3.UseVisualStyleBackColor = true;
            this.btn3.Click += new System.EventHandler(this.btn3_Click);
            // 
            // btn9
            // 
            this.btn9.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn9.Location = new System.Drawing.Point(94, 78);
            this.btn9.Name = "btn9";
            this.btn9.Size = new System.Drawing.Size(30, 31);
            this.btn9.TabIndex = 0;
            this.btn9.Text = "9";
            this.btn9.UseVisualStyleBackColor = true;
            this.btn9.Click += new System.EventHandler(this.btn9_Click);
            // 
            // btn2
            // 
            this.btn2.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2.Location = new System.Drawing.Point(58, 152);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(30, 31);
            this.btn2.TabIndex = 0;
            this.btn2.Text = "2";
            this.btn2.UseVisualStyleBackColor = true;
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(130, 78);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(30, 31);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSub
            // 
            this.btnSub.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSub.Location = new System.Drawing.Point(130, 115);
            this.btnSub.Name = "btnSub";
            this.btnSub.Size = new System.Drawing.Size(30, 31);
            this.btnSub.TabIndex = 0;
            this.btnSub.Text = "-";
            this.btnSub.UseVisualStyleBackColor = true;
            this.btnSub.Click += new System.EventHandler(this.btnSub_Click);
            // 
            // btn1
            // 
            this.btn1.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn1.Location = new System.Drawing.Point(22, 152);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(30, 31);
            this.btn1.TabIndex = 0;
            this.btn1.Text = "1";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // btn6
            // 
            this.btn6.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn6.Location = new System.Drawing.Point(94, 115);
            this.btn6.Name = "btn6";
            this.btn6.Size = new System.Drawing.Size(30, 31);
            this.btn6.TabIndex = 0;
            this.btn6.Text = "6";
            this.btn6.UseVisualStyleBackColor = true;
            this.btn6.Click += new System.EventHandler(this.btn6_Click);
            // 
            // btn5
            // 
            this.btn5.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn5.Location = new System.Drawing.Point(58, 115);
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(30, 31);
            this.btn5.TabIndex = 0;
            this.btn5.Text = "5";
            this.btn5.UseVisualStyleBackColor = true;
            this.btn5.Click += new System.EventHandler(this.btn5_Click);
            // 
            // btn0
            // 
            this.btn0.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn0.Location = new System.Drawing.Point(22, 189);
            this.btn0.Name = "btn0";
            this.btn0.Size = new System.Drawing.Size(66, 31);
            this.btn0.TabIndex = 0;
            this.btn0.Text = "0";
            this.btn0.UseVisualStyleBackColor = true;
            this.btn0.Click += new System.EventHandler(this.btn0_Click);
            // 
            // frmCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 264);
            this.Controls.Add(this.Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCalculator";
            this.Text = "frmCalculator";
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.StatusStrip StatusStrip1;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel1;
        internal System.Windows.Forms.TextBox txtExpr;
        internal System.Windows.Forms.Button btn7;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.Button btnLeft;
        internal System.Windows.Forms.Button btnDiv;
        internal System.Windows.Forms.Button btnRight;
        internal System.Windows.Forms.Button btnDot;
        internal System.Windows.Forms.Button btnClear;
        internal System.Windows.Forms.Button btnMulti;
        internal System.Windows.Forms.Button btn8;
        internal System.Windows.Forms.Button btnEqual;
        internal System.Windows.Forms.Button btn4;
        internal System.Windows.Forms.Button btn3;
        internal System.Windows.Forms.Button btn9;
        internal System.Windows.Forms.Button btn2;
        internal System.Windows.Forms.Button btnAdd;
        internal System.Windows.Forms.Button btnSub;
        internal System.Windows.Forms.Button btn1;
        internal System.Windows.Forms.Button btn6;
        internal System.Windows.Forms.Button btn5;
        internal System.Windows.Forms.Button btn0;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

