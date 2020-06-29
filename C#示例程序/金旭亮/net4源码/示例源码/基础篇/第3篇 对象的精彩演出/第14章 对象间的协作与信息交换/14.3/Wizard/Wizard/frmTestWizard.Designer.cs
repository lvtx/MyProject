namespace Wizard
{
    partial class frmTestWizard
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
            this.lblName = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.btnShowWizard = new System.Windows.Forms.Button();
            this.lblIsMale = new System.Windows.Forms.Label();
            this.lblEduBackground = new System.Windows.Forms.Label();
            this.lblProgramLanguage = new System.Windows.Forms.Label();
            this.btnShowInfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblName.Location = new System.Drawing.Point(120, 30);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(33, 12);
            this.lblName.TabIndex = 13;
            this.lblName.Text = "Name";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label1.Location = new System.Drawing.Point(32, 108);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(70, 14);
            this.Label1.TabIndex = 11;
            this.Label1.Text = "学   历：";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label2.Location = new System.Drawing.Point(34, 147);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(77, 14);
            this.Label2.TabIndex = 10;
            this.Label2.Text = "编程语言：";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label4.Location = new System.Drawing.Point(34, 31);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(70, 14);
            this.Label4.TabIndex = 9;
            this.Label4.Text = "姓   名：";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label5.Location = new System.Drawing.Point(33, 69);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(70, 14);
            this.Label5.TabIndex = 8;
            this.Label5.Text = "性   别：";
            // 
            // btnShowWizard
            // 
            this.btnShowWizard.Location = new System.Drawing.Point(27, 202);
            this.btnShowWizard.Name = "btnShowWizard";
            this.btnShowWizard.Size = new System.Drawing.Size(97, 30);
            this.btnShowWizard.TabIndex = 6;
            this.btnShowWizard.Text = "显示向导";
            this.btnShowWizard.Click += new System.EventHandler(this.btnShowWizard_Click);
            // 
            // lblIsMale
            // 
            this.lblIsMale.AutoSize = true;
            this.lblIsMale.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblIsMale.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblIsMale.Location = new System.Drawing.Point(117, 67);
            this.lblIsMale.Name = "lblIsMale";
            this.lblIsMale.Size = new System.Drawing.Size(47, 12);
            this.lblIsMale.TabIndex = 14;
            this.lblIsMale.Text = "IsMale";
            // 
            // lblEduBackground
            // 
            this.lblEduBackground.AutoSize = true;
            this.lblEduBackground.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblEduBackground.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblEduBackground.Location = new System.Drawing.Point(118, 105);
            this.lblEduBackground.Name = "lblEduBackground";
            this.lblEduBackground.Size = new System.Drawing.Size(96, 12);
            this.lblEduBackground.TabIndex = 15;
            this.lblEduBackground.Text = "EduBackground";
            // 
            // lblProgramLanguage
            // 
            this.lblProgramLanguage.AutoSize = true;
            this.lblProgramLanguage.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblProgramLanguage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblProgramLanguage.Location = new System.Drawing.Point(118, 146);
            this.lblProgramLanguage.Name = "lblProgramLanguage";
            this.lblProgramLanguage.Size = new System.Drawing.Size(117, 12);
            this.lblProgramLanguage.TabIndex = 12;
            this.lblProgramLanguage.Text = "ProgrameLanguage";
            // 
            // btnShowInfo
            // 
            this.btnShowInfo.Location = new System.Drawing.Point(144, 203);
            this.btnShowInfo.Name = "btnShowInfo";
            this.btnShowInfo.Size = new System.Drawing.Size(113, 30);
            this.btnShowInfo.TabIndex = 7;
            this.btnShowInfo.Text = "显示收集的信息";
            this.btnShowInfo.Click += new System.EventHandler(this.btnShowInfo_Click);
            // 
            // frmTestWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.btnShowWizard);
            this.Controls.Add(this.lblIsMale);
            this.Controls.Add(this.lblEduBackground);
            this.Controls.Add(this.lblProgramLanguage);
            this.Controls.Add(this.btnShowInfo);
            this.Name = "frmTestWizard";
            this.Text = "测试向导";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lblName;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Button btnShowWizard;
        internal System.Windows.Forms.Label lblIsMale;
        internal System.Windows.Forms.Label lblEduBackground;
        internal System.Windows.Forms.Label lblProgramLanguage;
        internal System.Windows.Forms.Button btnShowInfo;
    }
}