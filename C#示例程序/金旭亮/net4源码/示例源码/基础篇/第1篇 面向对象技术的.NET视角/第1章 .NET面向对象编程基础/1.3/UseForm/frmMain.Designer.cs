namespace UseForm
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
            this.btnShowOtherForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnShowOtherForm
            // 
            this.btnShowOtherForm.Location = new System.Drawing.Point(76, 25);
            this.btnShowOtherForm.Name = "btnShowOtherForm";
            this.btnShowOtherForm.Size = new System.Drawing.Size(121, 36);
            this.btnShowOtherForm.TabIndex = 0;
            this.btnShowOtherForm.Text = "显示从窗体";
            this.btnShowOtherForm.UseVisualStyleBackColor = true;
            this.btnShowOtherForm.Click += new System.EventHandler(this.btnShowOtherForm_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 103);
            this.Controls.Add(this.btnShowOtherForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "主窗体";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnShowOtherForm;
    }
}

