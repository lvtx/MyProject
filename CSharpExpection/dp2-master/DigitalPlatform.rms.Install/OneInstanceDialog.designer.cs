﻿namespace DigitalPlatform.rms
{
    partial class OneInstanceDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OneInstanceDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_instanceName = new System.Windows.Forms.TextBox();
            this.textBox_dataDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_bindings = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_editBindings = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.textBox_sqlDef = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button_editSqlDef = new System.Windows.Forms.Button();
            this.button_editRootUserInfo = new System.Windows.Forms.Button();
            this.textBox_rootUserInfo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button_certificate = new System.Windows.Forms.Button();
            this.comboBox_sqlServerType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "实例名(&N):";
            // 
            // textBox_instanceName
            // 
            this.textBox_instanceName.Location = new System.Drawing.Point(100, 12);
            this.textBox_instanceName.Name = "textBox_instanceName";
            this.textBox_instanceName.Size = new System.Drawing.Size(157, 21);
            this.textBox_instanceName.TabIndex = 1;
            this.textBox_instanceName.TextChanged += new System.EventHandler(this.textBox_instanceName_TextChanged);
            this.textBox_instanceName.Leave += new System.EventHandler(this.textBox_instanceName_Leave);
            // 
            // textBox_dataDir
            // 
            this.textBox_dataDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_dataDir.Location = new System.Drawing.Point(100, 39);
            this.textBox_dataDir.Name = "textBox_dataDir";
            this.textBox_dataDir.Size = new System.Drawing.Size(281, 21);
            this.textBox_dataDir.TabIndex = 3;
            this.textBox_dataDir.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_dataDir_KeyPress);
            this.textBox_dataDir.Leave += new System.EventHandler(this.textBox_dataDir_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "数据目录(&D):";
            // 
            // textBox_bindings
            // 
            this.textBox_bindings.AcceptsReturn = true;
            this.textBox_bindings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_bindings.Location = new System.Drawing.Point(100, 186);
            this.textBox_bindings.Multiline = true;
            this.textBox_bindings.Name = "textBox_bindings";
            this.textBox_bindings.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_bindings.Size = new System.Drawing.Size(281, 89);
            this.textBox_bindings.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "协议绑定(&B):";
            // 
            // button_editBindings
            // 
            this.button_editBindings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_editBindings.Location = new System.Drawing.Point(387, 186);
            this.button_editBindings.Name = "button_editBindings";
            this.button_editBindings.Size = new System.Drawing.Size(45, 23);
            this.button_editBindings.TabIndex = 13;
            this.button_editBindings.Text = "...";
            this.button_editBindings.UseVisualStyleBackColor = true;
            this.button_editBindings.Click += new System.EventHandler(this.button_editBindings_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(358, 294);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 16;
            this.button_Cancel.Text = "取消";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.Location = new System.Drawing.Point(277, 294);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 15;
            this.button_OK.Text = "确定";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // textBox_sqlDef
            // 
            this.textBox_sqlDef.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_sqlDef.Location = new System.Drawing.Point(100, 105);
            this.textBox_sqlDef.Name = "textBox_sqlDef";
            this.textBox_sqlDef.ReadOnly = true;
            this.textBox_sqlDef.Size = new System.Drawing.Size(281, 21);
            this.textBox_sqlDef.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "SQL服务器(&S):";
            // 
            // button_editSqlDef
            // 
            this.button_editSqlDef.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_editSqlDef.Location = new System.Drawing.Point(387, 103);
            this.button_editSqlDef.Name = "button_editSqlDef";
            this.button_editSqlDef.Size = new System.Drawing.Size(45, 23);
            this.button_editSqlDef.TabIndex = 7;
            this.button_editSqlDef.Text = "...";
            this.button_editSqlDef.UseVisualStyleBackColor = true;
            this.button_editSqlDef.Click += new System.EventHandler(this.button_editSqlDef_Click);
            // 
            // button_editRootUserInfo
            // 
            this.button_editRootUserInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_editRootUserInfo.Location = new System.Drawing.Point(387, 142);
            this.button_editRootUserInfo.Name = "button_editRootUserInfo";
            this.button_editRootUserInfo.Size = new System.Drawing.Size(45, 23);
            this.button_editRootUserInfo.TabIndex = 10;
            this.button_editRootUserInfo.Text = "...";
            this.button_editRootUserInfo.UseVisualStyleBackColor = true;
            this.button_editRootUserInfo.Click += new System.EventHandler(this.button_editRootUserInfo_Click);
            // 
            // textBox_rootUserInfo
            // 
            this.textBox_rootUserInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_rootUserInfo.Location = new System.Drawing.Point(100, 144);
            this.textBox_rootUserInfo.Name = "textBox_rootUserInfo";
            this.textBox_rootUserInfo.ReadOnly = true;
            this.textBox_rootUserInfo.Size = new System.Drawing.Size(281, 21);
            this.textBox_rootUserInfo.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "root账户(&R):";
            // 
            // button_certificate
            // 
            this.button_certificate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_certificate.Location = new System.Drawing.Point(12, 294);
            this.button_certificate.Name = "button_certificate";
            this.button_certificate.Size = new System.Drawing.Size(85, 23);
            this.button_certificate.TabIndex = 14;
            this.button_certificate.Text = "证书(&C)...";
            this.button_certificate.UseVisualStyleBackColor = true;
            this.button_certificate.Click += new System.EventHandler(this.button_certificate_Click);
            // 
            // comboBox_sqlServerType
            // 
            this.comboBox_sqlServerType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_sqlServerType.FormattingEnabled = true;
            this.comboBox_sqlServerType.Items.AddRange(new object[] {
            "SQLite",
            "MS SQL Server",
            "MySQL Server",
            "Oracle"});
            this.comboBox_sqlServerType.Location = new System.Drawing.Point(100, 79);
            this.comboBox_sqlServerType.Name = "comboBox_sqlServerType";
            this.comboBox_sqlServerType.Size = new System.Drawing.Size(281, 20);
            this.comboBox_sqlServerType.TabIndex = 5;
            this.comboBox_sqlServerType.TextChanged += new System.EventHandler(this.comboBox_sqlServerType_TextChanged);
            // 
            // OneInstanceDialog
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(445, 329);
            this.Controls.Add(this.comboBox_sqlServerType);
            this.Controls.Add(this.button_certificate);
            this.Controls.Add(this.button_editRootUserInfo);
            this.Controls.Add(this.textBox_rootUserInfo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_editSqlDef);
            this.Controls.Add(this.textBox_sqlDef);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.button_editBindings);
            this.Controls.Add(this.textBox_bindings);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_dataDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_instanceName);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OneInstanceDialog";
            this.ShowInTaskbar = false;
            this.Text = "一个实例";
            this.Load += new System.EventHandler(this.OneInstanceDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_instanceName;
        private System.Windows.Forms.TextBox textBox_dataDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_bindings;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_editBindings;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.TextBox textBox_sqlDef;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_editSqlDef;
        private System.Windows.Forms.Button button_editRootUserInfo;
        private System.Windows.Forms.TextBox textBox_rootUserInfo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_certificate;
        private System.Windows.Forms.ComboBox comboBox_sqlServerType;
    }
}