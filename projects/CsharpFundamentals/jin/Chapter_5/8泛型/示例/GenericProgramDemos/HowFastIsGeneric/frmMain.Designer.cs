namespace UseGenericExampleForCS
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
            this.Label1 = new System.Windows.Forms.Label();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.rdoList = new System.Windows.Forms.RadioButton();
            this.rdoArrayList = new System.Windows.Forms.RadioButton();
            this.btnSumElement = new System.Windows.Forms.Button();
            this.btnAddElement = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(16, 99);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(65, 12);
            this.Label1.TabIndex = 9;
            this.Label1.Text = "说明信息：";
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(14, 127);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInfo.Size = new System.Drawing.Size(303, 153);
            this.txtInfo.TabIndex = 8;
            // 
            // rdoList
            // 
            this.rdoList.AutoSize = true;
            this.rdoList.Location = new System.Drawing.Point(12, 34);
            this.rdoList.Name = "rdoList";
            this.rdoList.Size = new System.Drawing.Size(143, 16);
            this.rdoList.TabIndex = 7;
            this.rdoList.Text = "使用List（泛型用法）";
            this.rdoList.UseVisualStyleBackColor = true;
            // 
            // rdoArrayList
            // 
            this.rdoArrayList.AutoSize = true;
            this.rdoArrayList.Checked = true;
            this.rdoArrayList.Location = new System.Drawing.Point(12, 12);
            this.rdoArrayList.Name = "rdoArrayList";
            this.rdoArrayList.Size = new System.Drawing.Size(185, 16);
            this.rdoArrayList.TabIndex = 6;
            this.rdoArrayList.TabStop = true;
            this.rdoArrayList.Text = "使用ArrayList（非泛型用法）";
            this.rdoArrayList.UseVisualStyleBackColor = true;
            // 
            // btnSumElement
            // 
            this.btnSumElement.Location = new System.Drawing.Point(174, 60);
            this.btnSumElement.Name = "btnSumElement";
            this.btnSumElement.Size = new System.Drawing.Size(144, 29);
            this.btnSumElement.TabIndex = 4;
            this.btnSumElement.Text = "对集合中的整数求和";
            this.btnSumElement.UseVisualStyleBackColor = true;
            this.btnSumElement.Click += new System.EventHandler(this.btnSumElement_Click);
            // 
            // btnAddElement
            // 
            this.btnAddElement.Location = new System.Drawing.Point(13, 60);
            this.btnAddElement.Name = "btnAddElement";
            this.btnAddElement.Size = new System.Drawing.Size(144, 29);
            this.btnAddElement.TabIndex = 5;
            this.btnAddElement.Text = "加入大量整数到集合中";
            this.btnAddElement.UseVisualStyleBackColor = true;
            this.btnAddElement.Click += new System.EventHandler(this.btnAddElement_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 306);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.rdoList);
            this.Controls.Add(this.rdoArrayList);
            this.Controls.Add(this.btnSumElement);
            this.Controls.Add(this.btnAddElement);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "泛型示例";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox txtInfo;
        internal System.Windows.Forms.RadioButton rdoList;
        internal System.Windows.Forms.RadioButton rdoArrayList;
        internal System.Windows.Forms.Button btnSumElement;
        internal System.Windows.Forms.Button btnAddElement;
    }
}

