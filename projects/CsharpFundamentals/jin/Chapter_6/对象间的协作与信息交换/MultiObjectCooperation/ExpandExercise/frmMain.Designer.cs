namespace ExpandExercise
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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.btnNewSubForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(116, 124);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 91);
            this.button1.TabIndex = 0;
            this.button1.Text = "选一种颜色";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnNewSubForm
            // 
            this.btnNewSubForm.Location = new System.Drawing.Point(116, 30);
            this.btnNewSubForm.Name = "btnNewSubForm";
            this.btnNewSubForm.Size = new System.Drawing.Size(141, 73);
            this.btnNewSubForm.TabIndex = 1;
            this.btnNewSubForm.Text = "新建子窗体";
            this.btnNewSubForm.UseVisualStyleBackColor = true;
            this.btnNewSubForm.Click += new System.EventHandler(this.btnNewSubForm_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 282);
            this.Controls.Add(this.btnNewSubForm);
            this.Controls.Add(this.button1);
            this.Name = "frmMain";
            this.Text = "主窗体";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnNewSubForm;
    }
}

