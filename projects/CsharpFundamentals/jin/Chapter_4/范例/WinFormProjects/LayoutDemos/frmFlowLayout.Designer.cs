namespace LayoutDemos
{
    partial class frmFlowLayout
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cboFlowDirection = new System.Windows.Forms.ComboBox();
            this.chkWrapContents = new System.Windows.Forms.CheckBox();
            this.chkAutoScroll = new System.Windows.Forms.CheckBox();
            this.btnAddButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 87);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(304, 162);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "排列方向：";
            // 
            // cboFlowDirection
            // 
            this.cboFlowDirection.FormattingEnabled = true;
            this.cboFlowDirection.Items.AddRange(new object[] {
            "BottomUp",
            "LeftToRight",
            "TopDown",
            "RightToLeft"});
            this.cboFlowDirection.Location = new System.Drawing.Point(80, 9);
            this.cboFlowDirection.Name = "cboFlowDirection";
            this.cboFlowDirection.Size = new System.Drawing.Size(80, 20);
            this.cboFlowDirection.TabIndex = 2;
            this.cboFlowDirection.SelectedIndexChanged += new System.EventHandler(this.cboFlowDirection_SelectedIndexChanged);
            // 
            // chkWrapContents
            // 
            this.chkWrapContents.AutoSize = true;
            this.chkWrapContents.Checked = true;
            this.chkWrapContents.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWrapContents.Location = new System.Drawing.Point(18, 43);
            this.chkWrapContents.Name = "chkWrapContents";
            this.chkWrapContents.Size = new System.Drawing.Size(72, 16);
            this.chkWrapContents.TabIndex = 3;
            this.chkWrapContents.Text = "自动换行";
            this.chkWrapContents.UseVisualStyleBackColor = true;
            this.chkWrapContents.CheckedChanged += new System.EventHandler(this.chkWrapContents_CheckedChanged);
            // 
            // chkAutoScroll
            // 
            this.chkAutoScroll.AutoSize = true;
            this.chkAutoScroll.Location = new System.Drawing.Point(96, 43);
            this.chkAutoScroll.Name = "chkAutoScroll";
            this.chkAutoScroll.Size = new System.Drawing.Size(72, 16);
            this.chkAutoScroll.TabIndex = 3;
            this.chkAutoScroll.Text = "自动滚动";
            this.chkAutoScroll.UseVisualStyleBackColor = true;
            this.chkAutoScroll.CheckedChanged += new System.EventHandler(this.chkAutoScroll_CheckedChanged);
            // 
            // btnAddButton
            // 
            this.btnAddButton.Location = new System.Drawing.Point(202, 9);
            this.btnAddButton.Name = "btnAddButton";
            this.btnAddButton.Size = new System.Drawing.Size(114, 46);
            this.btnAddButton.TabIndex = 4;
            this.btnAddButton.Text = "添加按钮";
            this.btnAddButton.UseVisualStyleBackColor = true;
            this.btnAddButton.Click += new System.EventHandler(this.btnAddButton_Click);
            // 
            // frmFlowLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 273);
            this.Controls.Add(this.btnAddButton);
            this.Controls.Add(this.chkAutoScroll);
            this.Controls.Add(this.chkWrapContents);
            this.Controls.Add(this.cboFlowDirection);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "frmFlowLayout";
            this.Text = "frmFlowLayout";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboFlowDirection;
        private System.Windows.Forms.CheckBox chkWrapContents;
        private System.Windows.Forms.CheckBox chkAutoScroll;
        private System.Windows.Forms.Button btnAddButton;
    }
}