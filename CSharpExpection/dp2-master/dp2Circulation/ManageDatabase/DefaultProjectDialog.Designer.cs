namespace dp2Circulation
{
    partial class DefaultProjectDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefaultProjectDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_defaultProjectName = new System.Windows.Forms.TextBox();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_findProjectName = new System.Windows.Forms.Button();
            this.comboBox_databaseName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "���ݿ�(&D):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "ȱʡ���ط���(&D):";
            // 
            // textBox_defaultProjectName
            // 
            this.textBox_defaultProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_defaultProjectName.Location = new System.Drawing.Point(12, 91);
            this.textBox_defaultProjectName.Name = "textBox_defaultProjectName";
            this.textBox_defaultProjectName.Size = new System.Drawing.Size(219, 25);
            this.textBox_defaultProjectName.TabIndex = 3;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(205, 174);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 28);
            this.button_Cancel.TabIndex = 6;
            this.button_Cancel.Text = "ȡ��";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.Location = new System.Drawing.Point(124, 174);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 28);
            this.button_OK.TabIndex = 5;
            this.button_OK.Text = "ȷ��";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_findProjectName
            // 
            this.button_findProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_findProjectName.Location = new System.Drawing.Point(237, 91);
            this.button_findProjectName.Name = "button_findProjectName";
            this.button_findProjectName.Size = new System.Drawing.Size(43, 26);
            this.button_findProjectName.TabIndex = 4;
            this.button_findProjectName.Text = "...";
            this.button_findProjectName.UseVisualStyleBackColor = true;
            this.button_findProjectName.Click += new System.EventHandler(this.button_findProjectName_Click);
            // 
            // comboBox_databaseName
            // 
            this.comboBox_databaseName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_databaseName.Enabled = false;
            this.comboBox_databaseName.FormattingEnabled = true;
            this.comboBox_databaseName.Location = new System.Drawing.Point(12, 28);
            this.comboBox_databaseName.Name = "comboBox_databaseName";
            this.comboBox_databaseName.Size = new System.Drawing.Size(219, 23);
            this.comboBox_databaseName.TabIndex = 1;
            this.comboBox_databaseName.DropDown += new System.EventHandler(this.comboBox_databaseName_DropDown);
            // 
            // DefaultProjectDialog
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(292, 214);
            this.Controls.Add(this.comboBox_databaseName);
            this.Controls.Add(this.button_findProjectName);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.textBox_defaultProjectName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DefaultProjectDialog";
            this.ShowInTaskbar = false;
            this.Text = "ȱʡ��ϵ����";
            this.Load += new System.EventHandler(this.DefaultProjectDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_defaultProjectName;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_findProjectName;
        private System.Windows.Forms.ComboBox comboBox_databaseName;
    }
}