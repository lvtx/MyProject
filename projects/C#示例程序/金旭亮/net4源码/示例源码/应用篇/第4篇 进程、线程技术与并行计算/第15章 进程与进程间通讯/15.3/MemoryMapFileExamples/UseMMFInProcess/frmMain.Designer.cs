namespace UseMMFInProcess
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnWrite1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnWrite2 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(121, 21);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "a";
            // 
            // btnWrite1
            // 
            this.btnWrite1.Location = new System.Drawing.Point(153, 10);
            this.btnWrite1.Name = "btnWrite1";
            this.btnWrite1.Size = new System.Drawing.Size(158, 23);
            this.btnWrite1.TabIndex = 1;
            this.btnWrite1.Text = "向文件前半段写入此字符";
            this.btnWrite1.UseVisualStyleBackColor = true;
            this.btnWrite1.Click += new System.EventHandler(this.btnWrite1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 50);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(121, 21);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "b";
            // 
            // btnWrite2
            // 
            this.btnWrite2.Location = new System.Drawing.Point(153, 48);
            this.btnWrite2.Name = "btnWrite2";
            this.btnWrite2.Size = new System.Drawing.Size(158, 23);
            this.btnWrite2.TabIndex = 1;
            this.btnWrite2.Text = "向文件后半段写入此字符";
            this.btnWrite2.UseVisualStyleBackColor = true;
            this.btnWrite2.Click += new System.EventHandler(this.btnWrite2_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(12, 94);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(299, 197);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 310);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(326, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblInfo
            // 
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(308, 17);
            this.lblInfo.Text = "展示如何在同一进程中同时访问内存映射文件的不同部分";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 332);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnWrite2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.btnWrite1);
            this.Controls.Add(this.textBox1);
            this.Name = "frmMain";
            this.Text = "内存映射文件示例";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnWrite1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnWrite2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblInfo;
    }
}

