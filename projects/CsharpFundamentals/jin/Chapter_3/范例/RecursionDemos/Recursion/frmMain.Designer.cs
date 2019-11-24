namespace Recursion
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
            this.RichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.updnTimes = new System.Windows.Forms.NumericUpDown();
            this.btnExecute = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.updnTimes)).BeginInit();
            this.SuspendLayout();
            // 
            // RichTextBox1
            // 
            this.RichTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RichTextBox1.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RichTextBox1.Location = new System.Drawing.Point(10, 66);
            this.RichTextBox1.Name = "RichTextBox1";
            this.RichTextBox1.Size = new System.Drawing.Size(427, 335);
            this.RichTextBox1.TabIndex = 7;
            this.RichTextBox1.Text = "";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(130, 26);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(65, 12);
            this.Label1.TabIndex = 6;
            this.Label1.Text = "递归深度：";
            // 
            // updnTimes
            // 
            this.updnTimes.Location = new System.Drawing.Point(210, 26);
            this.updnTimes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updnTimes.Name = "updnTimes";
            this.updnTimes.Size = new System.Drawing.Size(48, 21);
            this.updnTimes.TabIndex = 5;
            this.updnTimes.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(10, 18);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(96, 32);
            this.btnExecute.TabIndex = 4;
            this.btnExecute.Text = "执行";
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 419);
            this.Controls.Add(this.RichTextBox1);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.updnTimes);
            this.Controls.Add(this.btnExecute);
            this.Name = "frmMain";
            this.Text = "递归的使用";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.updnTimes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.RichTextBox RichTextBox1;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.NumericUpDown updnTimes;
        internal System.Windows.Forms.Button btnExecute;
    }
}

