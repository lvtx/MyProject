namespace ShowAssembyInfo
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoadAssembly = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cboModules = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lstTypes = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rtfConstructor = new System.Windows.Forms.RichTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rtfMethod = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.rtfField = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.rtfProperties = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtAssemblyLocation = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadAssembly
            // 
            this.btnLoadAssembly.Location = new System.Drawing.Point(13, 15);
            this.btnLoadAssembly.Name = "btnLoadAssembly";
            this.btnLoadAssembly.Size = new System.Drawing.Size(136, 29);
            this.btnLoadAssembly.TabIndex = 0;
            this.btnLoadAssembly.Text = "加载程序集";
            this.btnLoadAssembly.UseVisualStyleBackColor = true;
            this.btnLoadAssembly.Click += new System.EventHandler(this.btnLoadAssembly_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "程序集文件（*.exe，*.dll）|*.exe;*.dll";
            // 
            // cboModules
            // 
            this.cboModules.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModules.FormattingEnabled = true;
            this.cboModules.Location = new System.Drawing.Point(13, 72);
            this.cboModules.Name = "cboModules";
            this.cboModules.Size = new System.Drawing.Size(136, 20);
            this.cboModules.TabIndex = 2;
            this.cboModules.SelectedIndexChanged += new System.EventHandler(this.cboModules_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "选择模块：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "模块中的定义所有类型：";
            // 
            // lstTypes
            // 
            this.lstTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lstTypes.FormattingEnabled = true;
            this.lstTypes.ItemHeight = 12;
            this.lstTypes.Location = new System.Drawing.Point(14, 126);
            this.lstTypes.Name = "lstTypes";
            this.lstTypes.Size = new System.Drawing.Size(135, 220);
            this.lstTypes.TabIndex = 4;
            this.lstTypes.SelectedIndexChanged += new System.EventHandler(this.lstTypes_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(164, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "程序集：";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(165, 51);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(481, 293);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rtfConstructor);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(473, 268);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "构造函数";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // rtfConstructor
            // 
            this.rtfConstructor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfConstructor.Location = new System.Drawing.Point(26, 32);
            this.rtfConstructor.Name = "rtfConstructor";
            this.rtfConstructor.Size = new System.Drawing.Size(411, 206);
            this.rtfConstructor.TabIndex = 2;
            this.rtfConstructor.Text = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 12);
            this.label10.TabIndex = 1;
            this.label10.Text = "构造函数清单：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.rtfMethod);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(473, 268);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "方法";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // rtfMethod
            // 
            this.rtfMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfMethod.Location = new System.Drawing.Point(24, 44);
            this.rtfMethod.Name = "rtfMethod";
            this.rtfMethod.Size = new System.Drawing.Size(425, 200);
            this.rtfMethod.TabIndex = 3;
            this.rtfMethod.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "方法信息：";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.rtfField);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(473, 268);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "字段";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // rtfField
            // 
            this.rtfField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfField.Location = new System.Drawing.Point(31, 54);
            this.rtfField.Name = "rtfField";
            this.rtfField.Size = new System.Drawing.Size(412, 192);
            this.rtfField.TabIndex = 10;
            this.rtfField.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "字段信息：";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.rtfProperties);
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Location = new System.Drawing.Point(4, 21);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(473, 268);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "属性";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // rtfProperties
            // 
            this.rtfProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfProperties.Location = new System.Drawing.Point(33, 46);
            this.rtfProperties.Name = "rtfProperties";
            this.rtfProperties.Size = new System.Drawing.Size(408, 187);
            this.rtfProperties.TabIndex = 10;
            this.rtfProperties.Text = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "属性信息：";
            // 
            // txtAssemblyLocation
            // 
            this.txtAssemblyLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAssemblyLocation.Location = new System.Drawing.Point(218, 12);
            this.txtAssemblyLocation.Name = "txtAssemblyLocation";
            this.txtAssemblyLocation.ReadOnly = true;
            this.txtAssemblyLocation.Size = new System.Drawing.Size(424, 21);
            this.txtAssemblyLocation.TabIndex = 6;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 368);
            this.Controls.Add(this.txtAssemblyLocation);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lstTypes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboModules);
            this.Controls.Add(this.btnLoadAssembly);
            this.Name = "frmMain";
            this.Text = "程序集信息查看器";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadAssembly;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox cboModules;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstTypes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.RichTextBox rtfConstructor;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RichTextBox rtfMethod;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox rtfField;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox rtfProperties;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtAssemblyLocation;
    }
}

