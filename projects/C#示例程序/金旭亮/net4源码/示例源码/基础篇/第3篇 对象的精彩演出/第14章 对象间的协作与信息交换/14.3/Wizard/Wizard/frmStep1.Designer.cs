namespace Wizard
{
    partial class frmStep1
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
            this.Panel2 = new System.Windows.Forms.Panel();
            this.rdoMale = new System.Windows.Forms.RadioButton();
            this.txtName = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.rdoFemale = new System.Windows.Forms.RadioButton();
            this.Panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel2
            // 
            this.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.Panel2.Controls.Add(this.rdoMale);
            this.Panel2.Controls.Add(this.txtName);
            this.Panel2.Controls.Add(this.Label4);
            this.Panel2.Controls.Add(this.Label5);
            this.Panel2.Controls.Add(this.rdoFemale);
            this.Panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel2.Location = new System.Drawing.Point(0, 0);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(428, 135);
            this.Panel2.TabIndex = 6;
            // 
            // rdoMale
            // 
            this.rdoMale.Checked = true;
            this.rdoMale.Location = new System.Drawing.Point(146, 69);
            this.rdoMale.Name = "rdoMale";
            this.rdoMale.Size = new System.Drawing.Size(35, 19);
            this.rdoMale.TabIndex = 2;
            this.rdoMale.TabStop = true;
            this.rdoMale.Text = "男";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(134, 27);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(182, 21);
            this.txtName.TabIndex = 1;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label4.Location = new System.Drawing.Point(82, 34);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(49, 14);
            this.Label4.TabIndex = 0;
            this.Label4.Text = "姓名：";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label5.Location = new System.Drawing.Point(81, 72);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(49, 14);
            this.Label5.TabIndex = 0;
            this.Label5.Text = "性别：";
            // 
            // rdoFemale
            // 
            this.rdoFemale.Location = new System.Drawing.Point(205, 70);
            this.rdoFemale.Name = "rdoFemale";
            this.rdoFemale.Size = new System.Drawing.Size(35, 19);
            this.rdoFemale.TabIndex = 2;
            this.rdoFemale.Text = "女";
            // 
            // frmStep1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(428, 182);
            this.Controls.Add(this.Panel2);
            this.Name = "frmStep1";
            this.Text = "第一步";
            this.Controls.SetChildIndex(this.Panel2, 0);
            this.Panel2.ResumeLayout(false);
            this.Panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel Panel2;
        internal System.Windows.Forms.RadioButton rdoMale;
        internal System.Windows.Forms.TextBox txtName;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.RadioButton rdoFemale;
    }
}
