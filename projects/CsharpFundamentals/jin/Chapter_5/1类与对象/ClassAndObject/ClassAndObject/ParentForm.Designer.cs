namespace ClassAndObject
{
    partial class ParentForm
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
            this.btnShowSubForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnShowSubForm
            // 
            this.btnShowSubForm.Location = new System.Drawing.Point(280, 159);
            this.btnShowSubForm.Name = "btnShowSubForm";
            this.btnShowSubForm.Size = new System.Drawing.Size(217, 90);
            this.btnShowSubForm.TabIndex = 0;
            this.btnShowSubForm.Text = "显示子窗口";
            this.btnShowSubForm.UseVisualStyleBackColor = true;
            this.btnShowSubForm.Click += new System.EventHandler(this.BtnShowSubForm_Click);
            // 
            // ParentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnShowSubForm);
            this.Name = "ParentForm";
            this.Text = "ParentForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnShowSubForm;
    }
}

