namespace CalculateVarianceOfPopulation
{
    partial class frmMain
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
            this.btnUseSequence = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUseTPL = new System.Windows.Forms.Button();
            this.btnUseThread = new System.Windows.Forms.Button();
            this.btnGenerateData = new System.Windows.Forms.Button();
            this.rtfInfo = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUseSequence
            // 
            this.btnUseSequence.Location = new System.Drawing.Point(21, 20);
            this.btnUseSequence.Name = "btnUseSequence";
            this.btnUseSequence.Size = new System.Drawing.Size(92, 31);
            this.btnUseSequence.TabIndex = 0;
            this.btnUseSequence.Text = "串行算法";
            this.btnUseSequence.UseVisualStyleBackColor = true;
            this.btnUseSequence.Click += new System.EventHandler(this.btnUseSequence_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnUseTPL);
            this.groupBox1.Controls.Add(this.btnUseThread);
            this.groupBox1.Controls.Add(this.btnUseSequence);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(109, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(343, 62);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "计算统计数据的总体方差";
            // 
            // btnUseTPL
            // 
            this.btnUseTPL.Location = new System.Drawing.Point(236, 20);
            this.btnUseTPL.Name = "btnUseTPL";
            this.btnUseTPL.Size = new System.Drawing.Size(92, 31);
            this.btnUseTPL.TabIndex = 0;
            this.btnUseTPL.Text = "使用并行库";
            this.btnUseTPL.UseVisualStyleBackColor = true;
            this.btnUseTPL.Click += new System.EventHandler(this.btnUseTPL_Click);
            // 
            // btnUseThread
            // 
            this.btnUseThread.Location = new System.Drawing.Point(129, 20);
            this.btnUseThread.Name = "btnUseThread";
            this.btnUseThread.Size = new System.Drawing.Size(92, 31);
            this.btnUseThread.TabIndex = 0;
            this.btnUseThread.Text = "使用线程";
            this.btnUseThread.UseVisualStyleBackColor = true;
            this.btnUseThread.Click += new System.EventHandler(this.btnUseThread_Click);
            // 
            // btnGenerateData
            // 
            this.btnGenerateData.Location = new System.Drawing.Point(9, 32);
            this.btnGenerateData.Name = "btnGenerateData";
            this.btnGenerateData.Size = new System.Drawing.Size(94, 29);
            this.btnGenerateData.TabIndex = 2;
            this.btnGenerateData.Text = "生成统计数据";
            this.btnGenerateData.UseVisualStyleBackColor = true;
            this.btnGenerateData.Click += new System.EventHandler(this.btnGenerateData_Click);
            // 
            // rtfInfo
            // 
            this.rtfInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfInfo.Location = new System.Drawing.Point(9, 92);
            this.rtfInfo.Name = "rtfInfo";
            this.rtfInfo.Size = new System.Drawing.Size(444, 263);
            this.rtfInfo.TabIndex = 3;
            this.rtfInfo.Text = "";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 387);
            this.Controls.Add(this.rtfInfo);
            this.Controls.Add(this.btnGenerateData);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmMain";
            this.Text = "并行计算示例：线程 vs 任务";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUseSequence;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnUseTPL;
        private System.Windows.Forms.Button btnUseThread;
        private System.Windows.Forms.Button btnGenerateData;
        private System.Windows.Forms.RichTextBox rtfInfo;
    }
}

