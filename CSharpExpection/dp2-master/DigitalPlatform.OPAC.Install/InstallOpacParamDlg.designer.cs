namespace DigitalPlatform.OPAC
{
    partial class InstallOpacParamDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallOpacParamDlg));
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_resetManageUserPassword = new System.Windows.Forms.Button();
            this.button_createManageUser = new System.Windows.Forms.Button();
            this.button_detectManageUser = new System.Windows.Forms.Button();
            this.textBox_confirmManagePassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_managePassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_manageUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_dp2LibraryUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.AutoSize = true;
            this.button_Cancel.Location = new System.Drawing.Point(335, 465);
            this.button_Cancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(65, 33);
            this.button_Cancel.TabIndex = 19;
            this.button_Cancel.Text = "ȡ��";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.AutoSize = true;
            this.button_OK.Location = new System.Drawing.Point(264, 465);
            this.button_OK.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(65, 33);
            this.button_OK.TabIndex = 18;
            this.button_OK.Text = "ȷ��";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_resetManageUserPassword);
            this.groupBox1.Controls.Add(this.button_createManageUser);
            this.groupBox1.Controls.Add(this.button_detectManageUser);
            this.groupBox1.Controls.Add(this.textBox_confirmManagePassword);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox_managePassword);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_manageUserName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 86);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox1.Size = new System.Drawing.Size(388, 241);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " �����ʻ�(�������dp2Library) ";
            // 
            // button_resetManageUserPassword
            // 
            this.button_resetManageUserPassword.AutoSize = true;
            this.button_resetManageUserPassword.Enabled = false;
            this.button_resetManageUserPassword.Location = new System.Drawing.Point(160, 180);
            this.button_resetManageUserPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_resetManageUserPassword.Name = "button_resetManageUserPassword";
            this.button_resetManageUserPassword.Size = new System.Drawing.Size(100, 33);
            this.button_resetManageUserPassword.TabIndex = 8;
            this.button_resetManageUserPassword.Text = "��������(&R)";
            this.button_resetManageUserPassword.UseVisualStyleBackColor = true;
            this.button_resetManageUserPassword.Click += new System.EventHandler(this.button_resetManageUserPassword_Click);
            // 
            // button_createManageUser
            // 
            this.button_createManageUser.AutoSize = true;
            this.button_createManageUser.Enabled = false;
            this.button_createManageUser.Location = new System.Drawing.Point(89, 180);
            this.button_createManageUser.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_createManageUser.Name = "button_createManageUser";
            this.button_createManageUser.Size = new System.Drawing.Size(65, 33);
            this.button_createManageUser.TabIndex = 7;
            this.button_createManageUser.Text = "����(&C)";
            this.button_createManageUser.UseVisualStyleBackColor = true;
            this.button_createManageUser.Click += new System.EventHandler(this.button_createManageUser_Click);
            // 
            // button_detectManageUser
            // 
            this.button_detectManageUser.AutoSize = true;
            this.button_detectManageUser.Enabled = false;
            this.button_detectManageUser.Location = new System.Drawing.Point(19, 180);
            this.button_detectManageUser.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_detectManageUser.Name = "button_detectManageUser";
            this.button_detectManageUser.Size = new System.Drawing.Size(65, 33);
            this.button_detectManageUser.TabIndex = 6;
            this.button_detectManageUser.Text = "���(&D)";
            this.button_detectManageUser.UseVisualStyleBackColor = true;
            this.button_detectManageUser.Click += new System.EventHandler(this.button_detectManageUser_Click);
            // 
            // textBox_confirmManagePassword
            // 
            this.textBox_confirmManagePassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_confirmManagePassword.Location = new System.Drawing.Point(131, 133);
            this.textBox_confirmManagePassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_confirmManagePassword.Name = "textBox_confirmManagePassword";
            this.textBox_confirmManagePassword.PasswordChar = '*';
            this.textBox_confirmManagePassword.Size = new System.Drawing.Size(180, 23);
            this.textBox_confirmManagePassword.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 137);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "�ٴ���������:";
            // 
            // textBox_managePassword
            // 
            this.textBox_managePassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_managePassword.Location = new System.Drawing.Point(131, 99);
            this.textBox_managePassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_managePassword.Name = "textBox_managePassword";
            this.textBox_managePassword.PasswordChar = '*';
            this.textBox_managePassword.Size = new System.Drawing.Size(180, 23);
            this.textBox_managePassword.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 102);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "����(&P):";
            // 
            // textBox_manageUserName
            // 
            this.textBox_manageUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_manageUserName.Location = new System.Drawing.Point(131, 47);
            this.textBox_manageUserName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_manageUserName.Name = "textBox_manageUserName";
            this.textBox_manageUserName.Size = new System.Drawing.Size(180, 23);
            this.textBox_manageUserName.TabIndex = 1;
            this.textBox_manageUserName.TextChanged += new System.EventHandler(this.textBox_manageUserName_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 50);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "�û���(&U):";
            // 
            // textBox_dp2LibraryUrl
            // 
            this.textBox_dp2LibraryUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dp2LibraryUrl.Location = new System.Drawing.Point(12, 35);
            this.textBox_dp2LibraryUrl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_dp2LibraryUrl.Name = "textBox_dp2LibraryUrl";
            this.textBox_dp2LibraryUrl.Size = new System.Drawing.Size(389, 23);
            this.textBox_dp2LibraryUrl.TabIndex = 12;
            this.textBox_dp2LibraryUrl.Text = "http://localhost:8001/dp2Library";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "dp2Library (ͼ���Ӧ��) ������ URL (&U):";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.Info;
            this.label5.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label5.Location = new System.Drawing.Point(12, 347);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(387, 115);
            this.label5.TabIndex = 20;
            this.label5.Text = "ע���������ʻ�����ָ��ôһ���˻���dp2OPAC�ڵ�һЩ(����ȫ��)���������������dp2Library���С��������ʻ�����ֻ��Ҫ��ϵͳ����Ա���ռ��ɡ�";
            // 
            // InstallOpacParamDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 513);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox_dp2LibraryUrl);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "InstallOpacParamDlg";
            this.Text = "��ָ����װ����";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_resetManageUserPassword;
        private System.Windows.Forms.Button button_createManageUser;
        private System.Windows.Forms.Button button_detectManageUser;
        private System.Windows.Forms.TextBox textBox_confirmManagePassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_managePassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_manageUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_dp2LibraryUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
    }
}