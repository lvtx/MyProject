namespace DynamicEventsInvoke
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
            this.RadioButton1 = new System.Windows.Forms.RadioButton();
            this.Button1 = new System.Windows.Forms.Button();
            this.RadioButton2 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // RadioButton1
            // 
            this.RadioButton1.Location = new System.Drawing.Point(170, 24);
            this.RadioButton1.Name = "RadioButton1";
            this.RadioButton1.Size = new System.Drawing.Size(112, 24);
            this.RadioButton1.TabIndex = 4;
            this.RadioButton1.Text = "事件处理程序一";
            this.RadioButton1.CheckedChanged += new System.EventHandler(this.RadioButton1_CheckedChanged);
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(21, 24);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(128, 40);
            this.Button1.TabIndex = 2;
            this.Button1.Text = "激发事件";
            // 
            // RadioButton2
            // 
            this.RadioButton2.Location = new System.Drawing.Point(170, 56);
            this.RadioButton2.Name = "RadioButton2";
            this.RadioButton2.Size = new System.Drawing.Size(112, 24);
            this.RadioButton2.TabIndex = 3;
            this.RadioButton2.Text = "事件处理程序二";
            this.RadioButton2.CheckedChanged += new System.EventHandler(this.RadioButton2_CheckedChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 114);
            this.Controls.Add(this.RadioButton1);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.RadioButton2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "动态事件挂接-CS";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.RadioButton RadioButton1;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.RadioButton RadioButton2;
    }
}

