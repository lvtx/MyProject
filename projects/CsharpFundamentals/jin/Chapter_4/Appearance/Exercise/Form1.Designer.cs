namespace Exercise
{
    partial class Form1
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
            this.radioSumRecursive = new System.Windows.Forms.RadioButton();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.radioSumRecursion = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // radioSumRecursive
            // 
            this.radioSumRecursive.AutoSize = true;
            this.radioSumRecursive.Location = new System.Drawing.Point(144, 32);
            this.radioSumRecursive.Name = "radioSumRecursive";
            this.radioSumRecursive.Size = new System.Drawing.Size(166, 19);
            this.radioSumRecursive.TabIndex = 0;
            this.radioSumRecursive.TabStop = true;
            this.radioSumRecursive.Text = "求1到100的和(递推)";
            this.radioSumRecursive.UseVisualStyleBackColor = true;
            this.radioSumRecursive.CheckedChanged += new System.EventHandler(this.RadioSumRecursive_CheckedChanged);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(144, 104);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(152, 37);
            this.btnCopy.TabIndex = 1;
            this.btnCopy.Text = "复制文本";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(480, 104);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(169, 37);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "清除文本";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // radioSumRecursion
            // 
            this.radioSumRecursion.AutoSize = true;
            this.radioSumRecursion.Location = new System.Drawing.Point(144, 69);
            this.radioSumRecursion.Name = "radioSumRecursion";
            this.radioSumRecursion.Size = new System.Drawing.Size(166, 19);
            this.radioSumRecursion.TabIndex = 4;
            this.radioSumRecursion.TabStop = true;
            this.radioSumRecursion.Text = "求1到100的和(递进)";
            this.radioSumRecursion.UseVisualStyleBackColor = true;
            this.radioSumRecursion.CheckedChanged += new System.EventHandler(this.RadioSumRecursion_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(144, 165);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(505, 264);
            this.textBox1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.radioSumRecursion);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.radioSumRecursive);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioSumRecursive;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.RadioButton radioSumRecursion;
        private System.Windows.Forms.TextBox textBox1;
    }
}

