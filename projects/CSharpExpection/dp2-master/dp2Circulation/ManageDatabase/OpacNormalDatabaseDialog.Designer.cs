namespace dp2Circulation{
    partial class OpacNormalDatabaseDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpacNormalDatabaseDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_databaseName = new System.Windows.Forms.TextBox();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_findDatabaseName = new System.Windows.Forms.Button();
            this.textBox_databaseAlias = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "���ݿ���(&D):";
            // 
            // textBox_databaseName
            // 
            this.textBox_databaseName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_databaseName.Location = new System.Drawing.Point(9, 25);
            this.textBox_databaseName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_databaseName.Name = "textBox_databaseName";
            this.textBox_databaseName.Size = new System.Drawing.Size(275, 21);
            this.textBox_databaseName.TabIndex = 1;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(272, 107);
            this.button_Cancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(56, 22);
            this.button_Cancel.TabIndex = 8;
            this.button_Cancel.Text = "ȡ��";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.Location = new System.Drawing.Point(211, 107);
            this.button_OK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(56, 22);
            this.button_OK.TabIndex = 7;
            this.button_OK.Text = "ȷ��";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_findDatabaseName
            // 
            this.button_findDatabaseName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_findDatabaseName.Location = new System.Drawing.Point(287, 22);
            this.button_findDatabaseName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_findDatabaseName.Name = "button_findDatabaseName";
            this.button_findDatabaseName.Size = new System.Drawing.Size(40, 22);
            this.button_findDatabaseName.TabIndex = 9;
            this.button_findDatabaseName.Text = "...";
            this.button_findDatabaseName.UseVisualStyleBackColor = true;
            this.button_findDatabaseName.Click += new System.EventHandler(this.button_findDatabaseName_Click);
            // 
            // textBox_databaseAlias
            // 
            this.textBox_databaseAlias.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_databaseAlias.Location = new System.Drawing.Point(9, 68);
            this.textBox_databaseAlias.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_databaseAlias.Name = "textBox_databaseAlias";
            this.textBox_databaseAlias.Size = new System.Drawing.Size(275, 21);
            this.textBox_databaseAlias.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 54);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "����(&A):";
            // 
            // OpacNormalDatabaseDialog
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(337, 139);
            this.Controls.Add(this.textBox_databaseAlias);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_findDatabaseName);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.textBox_databaseName);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "OpacNormalDatabaseDialog";
            this.ShowInTaskbar = false;
            this.Text = "OpacNormalDatabaseDialog";
            this.Load += new System.EventHandler(this.OpacNormalDatabaseDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_databaseName;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_findDatabaseName;
        private System.Windows.Forms.TextBox textBox_databaseAlias;
        private System.Windows.Forms.Label label2;
    }
}