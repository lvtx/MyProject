namespace ProcessInfo
{
    partial class frmProcess
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
            this.lstProcesses = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rtfBasicProcessInfo = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstModules = new System.Windows.Forms.ListBox();
            this.rtfProcessModule = new System.Windows.Forms.RichTextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.rtfThreadInfo = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lstThreads = new System.Windows.Forms.ListBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnCloseWindows = new System.Windows.Forms.Button();
            this.btnKillProcess = new System.Windows.Forms.Button();
            this.btnNewProcess = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstProcesses
            // 
            this.lstProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lstProcesses.FormattingEnabled = true;
            this.lstProcesses.ItemHeight = 12;
            this.lstProcesses.Location = new System.Drawing.Point(13, 38);
            this.lstProcesses.Name = "lstProcesses";
            this.lstProcesses.Size = new System.Drawing.Size(177, 316);
            this.lstProcesses.TabIndex = 0;
            this.lstProcesses.SelectedIndexChanged += new System.EventHandler(this.lstProcesses_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "正在运行的进程(ProcessName)：";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(209, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(355, 329);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rtfBasicProcessInfo);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(323, 304);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "选中进程基本信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // rtfBasicProcessInfo
            // 
            this.rtfBasicProcessInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfBasicProcessInfo.Location = new System.Drawing.Point(17, 15);
            this.rtfBasicProcessInfo.Name = "rtfBasicProcessInfo";
            this.rtfBasicProcessInfo.Size = new System.Drawing.Size(285, 264);
            this.rtfBasicProcessInfo.TabIndex = 0;
            this.rtfBasicProcessInfo.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.lstModules);
            this.tabPage2.Controls.Add(this.rtfProcessModule);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(323, 304);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "选中进程装载模块";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lstModules
            // 
            this.lstModules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstModules.FormattingEnabled = true;
            this.lstModules.ItemHeight = 12;
            this.lstModules.Location = new System.Drawing.Point(14, 37);
            this.lstModules.Name = "lstModules";
            this.lstModules.Size = new System.Drawing.Size(292, 148);
            this.lstModules.TabIndex = 1;
            this.lstModules.SelectedIndexChanged += new System.EventHandler(this.lstModules_SelectedIndexChanged);
            // 
            // rtfProcessModule
            // 
            this.rtfProcessModule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfProcessModule.Location = new System.Drawing.Point(16, 221);
            this.rtfProcessModule.Name = "rtfProcessModule";
            this.rtfProcessModule.Size = new System.Drawing.Size(291, 116);
            this.rtfProcessModule.TabIndex = 0;
            this.rtfProcessModule.Text = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.rtfThreadInfo);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.lstThreads);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(347, 304);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "选中进程启动的线程";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // rtfThreadInfo
            // 
            this.rtfThreadInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfThreadInfo.Location = new System.Drawing.Point(16, 189);
            this.rtfThreadInfo.Name = "rtfThreadInfo";
            this.rtfThreadInfo.Size = new System.Drawing.Size(313, 100);
            this.rtfThreadInfo.TabIndex = 2;
            this.rtfThreadInfo.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "线程清单(ProcessID)：";
            // 
            // lstThreads
            // 
            this.lstThreads.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstThreads.FormattingEnabled = true;
            this.lstThreads.ItemHeight = 12;
            this.lstThreads.Location = new System.Drawing.Point(13, 22);
            this.lstThreads.Name = "lstThreads";
            this.lstThreads.Size = new System.Drawing.Size(317, 136);
            this.lstThreads.TabIndex = 0;
            this.lstThreads.SelectedIndexChanged += new System.EventHandler(this.lstThreads_SelectedIndexChanged);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExit.Location = new System.Drawing.Point(455, 379);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(81, 25);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(13, 379);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(81, 25);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnCloseWindows
            // 
            this.btnCloseWindows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCloseWindows.Location = new System.Drawing.Point(226, 379);
            this.btnCloseWindows.Name = "btnCloseWindows";
            this.btnCloseWindows.Size = new System.Drawing.Size(96, 25);
            this.btnCloseWindows.TabIndex = 3;
            this.btnCloseWindows.Text = "请求进程退出";
            this.btnCloseWindows.UseVisualStyleBackColor = true;
            this.btnCloseWindows.Click += new System.EventHandler(this.btnCloseWindows_Click);
            // 
            // btnKillProcess
            // 
            this.btnKillProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnKillProcess.Location = new System.Drawing.Point(337, 379);
            this.btnKillProcess.Name = "btnKillProcess";
            this.btnKillProcess.Size = new System.Drawing.Size(112, 25);
            this.btnKillProcess.TabIndex = 3;
            this.btnKillProcess.Text = "强制关闭进程";
            this.btnKillProcess.UseVisualStyleBackColor = true;
            this.btnKillProcess.Click += new System.EventHandler(this.btnKillProcess_Click);
            // 
            // btnNewProcess
            // 
            this.btnNewProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNewProcess.Location = new System.Drawing.Point(110, 379);
            this.btnNewProcess.Name = "btnNewProcess";
            this.btnNewProcess.Size = new System.Drawing.Size(96, 25);
            this.btnNewProcess.TabIndex = 3;
            this.btnNewProcess.Text = "启动新进程";
            this.btnNewProcess.UseVisualStyleBackColor = true;
            this.btnNewProcess.Click += new System.EventHandler(this.btnNewProcess_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "模块清单（ModuleName）：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "选中线程信息：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "选中模块信息：";
            // 
            // frmProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 416);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnNewProcess);
            this.Controls.Add(this.btnKillProcess);
            this.Controls.Add(this.btnCloseWindows);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstProcesses);
            this.Name = "frmProcess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "进程信息查看器";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstProcesses;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox rtfBasicProcessInfo;
        private System.Windows.Forms.RichTextBox rtfProcessModule;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox rtfThreadInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstThreads;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ListBox lstModules;
        private System.Windows.Forms.Button btnCloseWindows;
        private System.Windows.Forms.Button btnKillProcess;
        private System.Windows.Forms.Button btnNewProcess;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

