namespace Pseudorandom
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
            this.Label1 = new System.Windows.Forms.Label();
            this.txtNumbers = new System.Windows.Forms.TextBox();
            this.RichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtSeed = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(16, 11);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(142, 15);
            this.Label1.TabIndex = 8;
            this.Label1.Text = "产生多少个随机数？";
            // 
            // txtNumbers
            // 
            this.txtNumbers.Location = new System.Drawing.Point(187, 11);
            this.txtNumbers.Margin = new System.Windows.Forms.Padding(4);
            this.txtNumbers.Name = "txtNumbers";
            this.txtNumbers.Size = new System.Drawing.Size(180, 25);
            this.txtNumbers.TabIndex = 7;
            this.txtNumbers.Text = "1000";
            // 
            // RichTextBox1
            // 
            this.RichTextBox1.Location = new System.Drawing.Point(37, 111);
            this.RichTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.RichTextBox1.Name = "RichTextBox1";
            this.RichTextBox1.Size = new System.Drawing.Size(489, 259);
            this.RichTextBox1.TabIndex = 5;
            this.RichTextBox1.Text = "";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(400, 11);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(128, 70);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "产生随机数";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(16, 61);
            this.Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(82, 15);
            this.Label2.TabIndex = 9;
            this.Label2.Text = "初始种子：";
            // 
            // txtSeed
            // 
            this.txtSeed.Location = new System.Drawing.Point(187, 61);
            this.txtSeed.Margin = new System.Windows.Forms.Padding(4);
            this.txtSeed.Name = "txtSeed";
            this.txtSeed.Size = new System.Drawing.Size(180, 25);
            this.txtSeed.TabIndex = 6;
            this.txtSeed.Text = "45";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 386);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.txtNumbers);
            this.Controls.Add(this.RichTextBox1);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.txtSeed);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.Text = "运用离散数学算法产生随机数";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox txtNumbers;
        internal System.Windows.Forms.RichTextBox RichTextBox1;
        internal System.Windows.Forms.Button btnGenerate;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox txtSeed;
    }
}

