namespace ContainerDemo
{
    partial class frmPanel
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
            this.pnlOuter = new System.Windows.Forms.Panel();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.pnlInner = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoOuter = new System.Windows.Forms.RadioButton();
            this.rdoInner = new System.Windows.Forms.RadioButton();
            this.btnShowOrHide = new System.Windows.Forms.Button();
            this.btnEnableOrDisable = new System.Windows.Forms.Button();
            this.pnlOuter.SuspendLayout();
            this.pnlInner.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOuter
            // 
            this.pnlOuter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlOuter.Controls.Add(this.label2);
            this.pnlOuter.Controls.Add(this.pnlInner);
            this.pnlOuter.Controls.Add(this.checkBox2);
            this.pnlOuter.Controls.Add(this.button1);
            this.pnlOuter.Controls.Add(this.checkBox1);
            this.pnlOuter.Controls.Add(this.monthCalendar1);
            this.pnlOuter.Location = new System.Drawing.Point(38, 81);
            this.pnlOuter.Name = "pnlOuter";
            this.pnlOuter.Size = new System.Drawing.Size(432, 249);
            this.pnlOuter.TabIndex = 0;
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(21, 43);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 0;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(262, 43);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(78, 16);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(253, 195);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(159, 28);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(262, 65);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(78, 16);
            this.checkBox2.TabIndex = 3;
            this.checkBox2.Text = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // pnlInner
            // 
            this.pnlInner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.pnlInner.Controls.Add(this.label1);
            this.pnlInner.Location = new System.Drawing.Point(253, 105);
            this.pnlInner.Name = "pnlInner";
            this.pnlInner.Size = new System.Drawing.Size(159, 70);
            this.pnlInner.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "内部面板";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "外部面板";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoInner);
            this.groupBox1.Controls.Add(this.rdoOuter);
            this.groupBox1.Location = new System.Drawing.Point(38, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(211, 44);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "要操作的面板";
            // 
            // rdoOuter
            // 
            this.rdoOuter.AutoSize = true;
            this.rdoOuter.Checked = true;
            this.rdoOuter.Location = new System.Drawing.Point(17, 21);
            this.rdoOuter.Name = "rdoOuter";
            this.rdoOuter.Size = new System.Drawing.Size(71, 16);
            this.rdoOuter.TabIndex = 0;
            this.rdoOuter.TabStop = true;
            this.rdoOuter.Text = "外部面板";
            this.rdoOuter.UseVisualStyleBackColor = true;
            this.rdoOuter.CheckedChanged += new System.EventHandler(this.rdoOuter_CheckedChanged);
            // 
            // rdoInner
            // 
            this.rdoInner.AutoSize = true;
            this.rdoInner.Location = new System.Drawing.Point(94, 20);
            this.rdoInner.Name = "rdoInner";
            this.rdoInner.Size = new System.Drawing.Size(71, 16);
            this.rdoInner.TabIndex = 0;
            this.rdoInner.Text = "内部面板";
            this.rdoInner.UseVisualStyleBackColor = true;
            this.rdoInner.CheckedChanged += new System.EventHandler(this.rdoInner_CheckedChanged);
            // 
            // btnShowOrHide
            // 
            this.btnShowOrHide.Location = new System.Drawing.Point(269, 22);
            this.btnShowOrHide.Name = "btnShowOrHide";
            this.btnShowOrHide.Size = new System.Drawing.Size(92, 34);
            this.btnShowOrHide.TabIndex = 2;
            this.btnShowOrHide.Text = "隐藏";
            this.btnShowOrHide.UseVisualStyleBackColor = true;
            this.btnShowOrHide.Click += new System.EventHandler(this.btnShowOrHide_Click);
            // 
            // btnEnableOrDisable
            // 
            this.btnEnableOrDisable.Location = new System.Drawing.Point(378, 22);
            this.btnEnableOrDisable.Name = "btnEnableOrDisable";
            this.btnEnableOrDisable.Size = new System.Drawing.Size(92, 34);
            this.btnEnableOrDisable.TabIndex = 2;
            this.btnEnableOrDisable.Text = "禁用";
            this.btnEnableOrDisable.UseVisualStyleBackColor = true;
            this.btnEnableOrDisable.Click += new System.EventHandler(this.btnEnableOrDisable_Click);
            // 
            // frmPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 368);
            this.Controls.Add(this.btnEnableOrDisable);
            this.Controls.Add(this.btnShowOrHide);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnlOuter);
            this.Name = "frmPanel";
            this.Text = "frmPanel";
            this.pnlOuter.ResumeLayout(false);
            this.pnlOuter.PerformLayout();
            this.pnlInner.ResumeLayout(false);
            this.pnlInner.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlOuter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlInner;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoInner;
        private System.Windows.Forms.RadioButton rdoOuter;
        private System.Windows.Forms.Button btnShowOrHide;
        private System.Windows.Forms.Button btnEnableOrDisable;
    }
}