namespace UseEmbodiedDLL
{
    partial class frmUseEmbodiedDLL
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
            this.btnInvokeStaticMethod = new System.Windows.Forms.Button();
            this.btnInvokeDynamicMethod = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInvokeStaticMethod
            // 
            this.btnInvokeStaticMethod.Location = new System.Drawing.Point(38, 24);
            this.btnInvokeStaticMethod.Name = "btnInvokeStaticMethod";
            this.btnInvokeStaticMethod.Size = new System.Drawing.Size(173, 41);
            this.btnInvokeStaticMethod.TabIndex = 0;
            this.btnInvokeStaticMethod.Text = "动态调用DLL中的静态函数";
            this.btnInvokeStaticMethod.UseVisualStyleBackColor = true;
            this.btnInvokeStaticMethod.Click += new System.EventHandler(this.btnInvokeStaticMethod_Click);
            // 
            // btnInvokeDynamicMethod
            // 
            this.btnInvokeDynamicMethod.Location = new System.Drawing.Point(38, 96);
            this.btnInvokeDynamicMethod.Name = "btnInvokeDynamicMethod";
            this.btnInvokeDynamicMethod.Size = new System.Drawing.Size(173, 41);
            this.btnInvokeDynamicMethod.TabIndex = 0;
            this.btnInvokeDynamicMethod.Text = "动态调用DLL中的实例函数";
            this.btnInvokeDynamicMethod.UseVisualStyleBackColor = true;
            this.btnInvokeDynamicMethod.Click += new System.EventHandler(this.btnInvokeDynamicMethod_Click);
            // 
            // frmUseEmbodiedDLL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 181);
            this.Controls.Add(this.btnInvokeDynamicMethod);
            this.Controls.Add(this.btnInvokeStaticMethod);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUseEmbodiedDLL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "使用嵌入式DLL";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnInvokeStaticMethod;
        private System.Windows.Forms.Button btnInvokeDynamicMethod;
    }
}

