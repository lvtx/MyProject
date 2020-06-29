namespace NonCompileSystemUpdateUseMEF
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
            this.btnOpenForm = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.rdoNone = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoComponent1 = new System.Windows.Forms.RadioButton();
            this.rdoComponent2 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOpenForm
            // 
            this.btnOpenForm.Location = new System.Drawing.Point(301, 69);
            this.btnOpenForm.Name = "btnOpenForm";
            this.btnOpenForm.Size = new System.Drawing.Size(157, 48);
            this.btnOpenForm.TabIndex = 5;
            this.btnOpenForm.Text = "比比谁最牛！";
            this.btnOpenForm.Click += new System.EventHandler(this.btnOpenForm_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label1.Location = new System.Drawing.Point(75, 19);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(286, 19);
            this.Label1.TabIndex = 4;
            this.Label1.Text = "利用MEF元数据实现动态装配组件";
            // 
            // rdoNone
            // 
            this.rdoNone.AutoSize = true;
            this.rdoNone.Checked = true;
            this.rdoNone.Location = new System.Drawing.Point(17, 29);
            this.rdoNone.Name = "rdoNone";
            this.rdoNone.Size = new System.Drawing.Size(35, 16);
            this.rdoNone.TabIndex = 6;
            this.rdoNone.TabStop = true;
            this.rdoNone.Text = "无";
            this.rdoNone.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoComponent2);
            this.groupBox1.Controls.Add(this.rdoComponent1);
            this.groupBox1.Controls.Add(this.rdoNone);
            this.groupBox1.Location = new System.Drawing.Point(37, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 60);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择要实例化的组件";
            // 
            // rdoComponent1
            // 
            this.rdoComponent1.AutoSize = true;
            this.rdoComponent1.Location = new System.Drawing.Point(83, 29);
            this.rdoComponent1.Name = "rdoComponent1";
            this.rdoComponent1.Size = new System.Drawing.Size(59, 16);
            this.rdoComponent1.TabIndex = 6;
            this.rdoComponent1.TabStop = true;
            this.rdoComponent1.Text = "组件一";
            this.rdoComponent1.UseVisualStyleBackColor = true;
            // 
            // rdoComponent2
            // 
            this.rdoComponent2.AutoSize = true;
            this.rdoComponent2.Location = new System.Drawing.Point(168, 29);
            this.rdoComponent2.Name = "rdoComponent2";
            this.rdoComponent2.Size = new System.Drawing.Size(59, 16);
            this.rdoComponent2.TabIndex = 6;
            this.rdoComponent2.TabStop = true;
            this.rdoComponent2.Text = "组件二";
            this.rdoComponent2.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 149);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOpenForm);
            this.Controls.Add(this.Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "MEF元数据示例";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnOpenForm;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.RadioButton rdoNone;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoComponent2;
        private System.Windows.Forms.RadioButton rdoComponent1;
    }
}

