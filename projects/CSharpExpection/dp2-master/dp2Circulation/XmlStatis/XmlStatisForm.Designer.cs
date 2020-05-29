namespace dp2Circulation
{
    partial class XmlStatisForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XmlStatisForm));
            this.button_projectManage = new System.Windows.Forms.Button();
            this.button_next = new System.Windows.Forms.Button();
            this.tabControl_main = new System.Windows.Forms.TabControl();
            this.tabPage_source = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.button_findInputXmlFilename = new System.Windows.Forms.Button();
            this.textBox_inputXmlFilename = new System.Windows.Forms.TextBox();
            this.tabPage_selectProject = new System.Windows.Forms.TabPage();
            this.button_getProjectName = new System.Windows.Forms.Button();
            this.textBox_projectName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage_runStatis = new System.Windows.Forms.TabPage();
            this.progressBar_records = new System.Windows.Forms.ProgressBar();
            this.webBrowser1_running = new System.Windows.Forms.WebBrowser();
            this.tabPage_print = new System.Windows.Forms.TabPage();
            this.button_print = new System.Windows.Forms.Button();
            this.tableLayoutPanel_runStatis = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl_main.SuspendLayout();
            this.tabPage_source.SuspendLayout();
            this.tabPage_selectProject.SuspendLayout();
            this.tabPage_runStatis.SuspendLayout();
            this.tabPage_print.SuspendLayout();
            this.tableLayoutPanel_runStatis.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_projectManage
            // 
            this.button_projectManage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_projectManage.Location = new System.Drawing.Point(0, 230);
            this.button_projectManage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_projectManage.Name = "button_projectManage";
            this.button_projectManage.Size = new System.Drawing.Size(94, 22);
            this.button_projectManage.TabIndex = 8;
            this.button_projectManage.Text = "��������(&M)...";
            this.button_projectManage.UseVisualStyleBackColor = true;
            this.button_projectManage.Click += new System.EventHandler(this.button_projectManage_Click);
            // 
            // button_next
            // 
            this.button_next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_next.Location = new System.Drawing.Point(290, 230);
            this.button_next.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_next.Name = "button_next";
            this.button_next.Size = new System.Drawing.Size(76, 22);
            this.button_next.TabIndex = 7;
            this.button_next.Text = "��һ��(&N)";
            this.button_next.UseVisualStyleBackColor = true;
            this.button_next.Click += new System.EventHandler(this.button_next_Click);
            // 
            // tabControl_main
            // 
            this.tabControl_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_main.Controls.Add(this.tabPage_source);
            this.tabControl_main.Controls.Add(this.tabPage_selectProject);
            this.tabControl_main.Controls.Add(this.tabPage_runStatis);
            this.tabControl_main.Controls.Add(this.tabPage_print);
            this.tabControl_main.Location = new System.Drawing.Point(0, 10);
            this.tabControl_main.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabControl_main.Name = "tabControl_main";
            this.tabControl_main.SelectedIndex = 0;
            this.tabControl_main.Size = new System.Drawing.Size(366, 215);
            this.tabControl_main.TabIndex = 6;
            this.tabControl_main.SelectedIndexChanged += new System.EventHandler(this.tabControl_main_SelectedIndexChanged);
            // 
            // tabPage_source
            // 
            this.tabPage_source.Controls.Add(this.label6);
            this.tabPage_source.Controls.Add(this.button_findInputXmlFilename);
            this.tabPage_source.Controls.Add(this.textBox_inputXmlFilename);
            this.tabPage_source.Location = new System.Drawing.Point(4, 22);
            this.tabPage_source.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_source.Name = "tabPage_source";
            this.tabPage_source.Size = new System.Drawing.Size(358, 189);
            this.tabPage_source.TabIndex = 4;
            this.tabPage_source.Text = "������Դ";
            this.tabPage_source.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 16);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "XML�ļ�(&F):";
            // 
            // button_findInputXmlFilename
            // 
            this.button_findInputXmlFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_findInputXmlFilename.Location = new System.Drawing.Point(320, 30);
            this.button_findInputXmlFilename.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_findInputXmlFilename.Name = "button_findInputXmlFilename";
            this.button_findInputXmlFilename.Size = new System.Drawing.Size(32, 22);
            this.button_findInputXmlFilename.TabIndex = 2;
            this.button_findInputXmlFilename.Text = "...";
            this.button_findInputXmlFilename.UseVisualStyleBackColor = true;
            this.button_findInputXmlFilename.Click += new System.EventHandler(this.button_findInputXmlFilename_Click);
            // 
            // textBox_inputXmlFilename
            // 
            this.textBox_inputXmlFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_inputXmlFilename.Location = new System.Drawing.Point(11, 30);
            this.textBox_inputXmlFilename.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_inputXmlFilename.Name = "textBox_inputXmlFilename";
            this.textBox_inputXmlFilename.Size = new System.Drawing.Size(305, 21);
            this.textBox_inputXmlFilename.TabIndex = 1;
            // 
            // tabPage_selectProject
            // 
            this.tabPage_selectProject.Controls.Add(this.button_getProjectName);
            this.tabPage_selectProject.Controls.Add(this.textBox_projectName);
            this.tabPage_selectProject.Controls.Add(this.label3);
            this.tabPage_selectProject.Location = new System.Drawing.Point(4, 22);
            this.tabPage_selectProject.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_selectProject.Name = "tabPage_selectProject";
            this.tabPage_selectProject.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_selectProject.Size = new System.Drawing.Size(358, 189);
            this.tabPage_selectProject.TabIndex = 1;
            this.tabPage_selectProject.Text = " ѡ������ ";
            this.tabPage_selectProject.UseVisualStyleBackColor = true;
            // 
            // button_getProjectName
            // 
            this.button_getProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getProjectName.Location = new System.Drawing.Point(319, 9);
            this.button_getProjectName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_getProjectName.Name = "button_getProjectName";
            this.button_getProjectName.Size = new System.Drawing.Size(32, 22);
            this.button_getProjectName.TabIndex = 2;
            this.button_getProjectName.Text = "...";
            this.button_getProjectName.UseVisualStyleBackColor = true;
            this.button_getProjectName.Click += new System.EventHandler(this.button_getProjectName_Click);
            // 
            // textBox_projectName
            // 
            this.textBox_projectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_projectName.Location = new System.Drawing.Point(81, 9);
            this.textBox_projectName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_projectName.Name = "textBox_projectName";
            this.textBox_projectName.Size = new System.Drawing.Size(234, 21);
            this.textBox_projectName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "������(&P):";
            // 
            // tabPage_runStatis
            // 
            this.tabPage_runStatis.Controls.Add(this.tableLayoutPanel_runStatis);
            this.tabPage_runStatis.Location = new System.Drawing.Point(4, 22);
            this.tabPage_runStatis.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_runStatis.Name = "tabPage_runStatis";
            this.tabPage_runStatis.Size = new System.Drawing.Size(358, 189);
            this.tabPage_runStatis.TabIndex = 3;
            this.tabPage_runStatis.Text = " ִ��ͳ�� ";
            this.tabPage_runStatis.UseVisualStyleBackColor = true;
            // 
            // progressBar_records
            // 
            this.progressBar_records.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar_records.Location = new System.Drawing.Point(2, 168);
            this.progressBar_records.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.progressBar_records.Name = "progressBar_records";
            this.progressBar_records.Size = new System.Drawing.Size(354, 11);
            this.progressBar_records.TabIndex = 1;
            // 
            // webBrowser1_running
            // 
            this.webBrowser1_running.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1_running.Location = new System.Drawing.Point(2, 10);
            this.webBrowser1_running.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.webBrowser1_running.MinimumSize = new System.Drawing.Size(15, 16);
            this.webBrowser1_running.Name = "webBrowser1_running";
            this.webBrowser1_running.Size = new System.Drawing.Size(354, 154);
            this.webBrowser1_running.TabIndex = 0;
            // 
            // tabPage_print
            // 
            this.tabPage_print.Controls.Add(this.button_print);
            this.tabPage_print.Location = new System.Drawing.Point(4, 22);
            this.tabPage_print.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage_print.Name = "tabPage_print";
            this.tabPage_print.Size = new System.Drawing.Size(358, 189);
            this.tabPage_print.TabIndex = 2;
            this.tabPage_print.Text = " ��ӡ��� ";
            this.tabPage_print.UseVisualStyleBackColor = true;
            // 
            // button_print
            // 
            this.button_print.Location = new System.Drawing.Point(3, 14);
            this.button_print.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_print.Name = "button_print";
            this.button_print.Size = new System.Drawing.Size(160, 22);
            this.button_print.TabIndex = 0;
            this.button_print.Text = "��ӡͳ�ƽ��(&P)";
            this.button_print.UseVisualStyleBackColor = true;
            this.button_print.Click += new System.EventHandler(this.button_print_Click);
            // 
            // tableLayoutPanel_runStatis
            // 
            this.tableLayoutPanel_runStatis.ColumnCount = 1;
            this.tableLayoutPanel_runStatis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_runStatis.Controls.Add(this.webBrowser1_running, 0, 0);
            this.tableLayoutPanel_runStatis.Controls.Add(this.progressBar_records, 0, 1);
            this.tableLayoutPanel_runStatis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_runStatis.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_runStatis.Name = "tableLayoutPanel_runStatis";
            this.tableLayoutPanel_runStatis.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.tableLayoutPanel_runStatis.RowCount = 2;
            this.tableLayoutPanel_runStatis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_runStatis.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_runStatis.Size = new System.Drawing.Size(358, 189);
            this.tableLayoutPanel_runStatis.TabIndex = 2;
            // 
            // XmlStatisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 262);
            this.Controls.Add(this.button_projectManage);
            this.Controls.Add(this.button_next);
            this.Controls.Add(this.tabControl_main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "XmlStatisForm";
            this.ShowInTaskbar = false;
            this.Text = "XMLͳ�ƴ�";
            this.Activated += new System.EventHandler(this.XmlStatisForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.XmlStatisForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.XmlStatisForm_FormClosed);
            this.Load += new System.EventHandler(this.XmlStatisForm_Load);
            this.tabControl_main.ResumeLayout(false);
            this.tabPage_source.ResumeLayout(false);
            this.tabPage_source.PerformLayout();
            this.tabPage_selectProject.ResumeLayout(false);
            this.tabPage_selectProject.PerformLayout();
            this.tabPage_runStatis.ResumeLayout(false);
            this.tabPage_print.ResumeLayout(false);
            this.tableLayoutPanel_runStatis.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_projectManage;
        private System.Windows.Forms.Button button_next;
        private System.Windows.Forms.TabControl tabControl_main;
        private System.Windows.Forms.TabPage tabPage_source;
        private System.Windows.Forms.Button button_findInputXmlFilename;
        private System.Windows.Forms.TextBox textBox_inputXmlFilename;
        private System.Windows.Forms.TabPage tabPage_selectProject;
        private System.Windows.Forms.Button button_getProjectName;
        private System.Windows.Forms.TextBox textBox_projectName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage_runStatis;
        private System.Windows.Forms.ProgressBar progressBar_records;
        private System.Windows.Forms.WebBrowser webBrowser1_running;
        private System.Windows.Forms.TabPage tabPage_print;
        private System.Windows.Forms.Button button_print;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_runStatis;
    }
}