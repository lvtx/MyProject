namespace UseCustomEvent
{
    partial class frmUseMyCustomButton
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
            this.myCustomButton1 = new UseCustomEvent.MyCustomButton();
            this.myCustomButtonUseGenericDelegate1 = new UseCustomEvent.MyCustomButtonUseGenericDelegate();
            this.SuspendLayout();
            // 
            // myCustomButton1
            // 
            this.myCustomButton1.Location = new System.Drawing.Point(45, 12);
            this.myCustomButton1.Name = "myCustomButton1";
            this.myCustomButton1.Size = new System.Drawing.Size(190, 38);
            this.myCustomButton1.TabIndex = 0;
            this.myCustomButton1.Text = "使用自定义事件的标准方式";
            this.myCustomButton1.UseVisualStyleBackColor = true;
            // 
            // myCustomButtonUseGenericDelegate1
            // 
            this.myCustomButtonUseGenericDelegate1.Location = new System.Drawing.Point(45, 76);
            this.myCustomButtonUseGenericDelegate1.Name = "myCustomButtonUseGenericDelegate1";
            this.myCustomButtonUseGenericDelegate1.Size = new System.Drawing.Size(190, 44);
            this.myCustomButtonUseGenericDelegate1.TabIndex = 1;
            this.myCustomButtonUseGenericDelegate1.Text = "使用泛型委托自定义事件";
            this.myCustomButtonUseGenericDelegate1.UseVisualStyleBackColor = true;
            // 
            // frmUseMyCustomButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 152);
            this.Controls.Add(this.myCustomButtonUseGenericDelegate1);
            this.Controls.Add(this.myCustomButton1);
            this.Name = "frmUseMyCustomButton";
            this.Text = "拥有自定义事件的按钮控件";
            this.ResumeLayout(false);

        }

        #endregion

        private MyCustomButton myCustomButton1;
        private MyCustomButtonUseGenericDelegate myCustomButtonUseGenericDelegate1;
    }
}

