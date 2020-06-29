namespace Wizard
{
    partial class frmStep2
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
            this.chkCPP = new System.Windows.Forms.CheckBox();
            this.cboEduBackground = new System.Windows.Forms.ComboBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.chkJava = new System.Windows.Forms.CheckBox();
            this.chkVB = new System.Windows.Forms.CheckBox();
            this.chkCS = new System.Windows.Forms.CheckBox();
            this.Panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel2
            // 
            this.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.Panel2.Controls.Add(this.chkCPP);
            this.Panel2.Controls.Add(this.cboEduBackground);
            this.Panel2.Controls.Add(this.Label4);
            this.Panel2.Controls.Add(this.Label1);
            this.Panel2.Controls.Add(this.chkJava);
            this.Panel2.Controls.Add(this.chkVB);
            this.Panel2.Controls.Add(this.chkCS);
            this.Panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel2.Location = new System.Drawing.Point(0, 0);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(428, 123);
            this.Panel2.TabIndex = 7;
            // 
            // chkCPP
            // 
            this.chkCPP.Location = new System.Drawing.Point(130, 63);
            this.chkCPP.Name = "chkCPP";
            this.chkCPP.Size = new System.Drawing.Size(45, 17);
            this.chkCPP.TabIndex = 3;
            this.chkCPP.Text = "C++";
            // 
            // cboEduBackground
            // 
            this.cboEduBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEduBackground.Items.AddRange(new object[] {
            "本科",
            "硕士",
            "博士"});
            this.cboEduBackground.Location = new System.Drawing.Point(131, 22);
            this.cboEduBackground.Name = "cboEduBackground";
            this.cboEduBackground.Size = new System.Drawing.Size(179, 20);
            this.cboEduBackground.TabIndex = 2;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label4.Location = new System.Drawing.Point(44, 24);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(70, 14);
            this.Label4.TabIndex = 1;
            this.Label4.Text = "学   历：";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label1.Location = new System.Drawing.Point(44, 64);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(77, 14);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "编程语言：";
            // 
            // chkJava
            // 
            this.chkJava.Location = new System.Drawing.Point(182, 63);
            this.chkJava.Name = "chkJava";
            this.chkJava.Size = new System.Drawing.Size(53, 17);
            this.chkJava.TabIndex = 3;
            this.chkJava.Text = "Java";
            // 
            // chkVB
            // 
            this.chkVB.Location = new System.Drawing.Point(248, 62);
            this.chkVB.Name = "chkVB";
            this.chkVB.Size = new System.Drawing.Size(38, 17);
            this.chkVB.TabIndex = 3;
            this.chkVB.Text = "VB";
            // 
            // chkCS
            // 
            this.chkCS.Location = new System.Drawing.Point(301, 64);
            this.chkCS.Name = "chkCS";
            this.chkCS.Size = new System.Drawing.Size(38, 17);
            this.chkCS.TabIndex = 3;
            this.chkCS.Text = "C#";
            // 
            // frmStep2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(428, 169);
            this.Controls.Add(this.Panel2);
            this.Name = "frmStep2";
            this.Text = "第二步";
            this.Load += new System.EventHandler(this.frmStep2_Load);
            this.Controls.SetChildIndex(this.Panel2, 0);
            this.Panel2.ResumeLayout(false);
            this.Panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel Panel2;
        internal System.Windows.Forms.CheckBox chkCPP;
        internal System.Windows.Forms.ComboBox cboEduBackground;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.CheckBox chkJava;
        internal System.Windows.Forms.CheckBox chkVB;
        internal System.Windows.Forms.CheckBox chkCS;
    }
}
