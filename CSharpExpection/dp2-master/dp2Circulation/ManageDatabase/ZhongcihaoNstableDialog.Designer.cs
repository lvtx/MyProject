namespace dp2Circulation
{
    partial class ZhongcihaoNstableDialog
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
System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZhongcihaoNstableDialog));
this.textBox_xml = new DigitalPlatform.GUI.NoHasSelTextBox();
this.label1 = new System.Windows.Forms.Label();
this.button_Cancel = new System.Windows.Forms.Button();
this.button_OK = new System.Windows.Forms.Button();
this.SuspendLayout();
// 
// textBox_xml
// 
this.textBox_xml.AcceptsReturn = true;
this.textBox_xml.AcceptsTab = true;
this.textBox_xml.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
this.textBox_xml.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
this.textBox_xml.HideSelection = false;
this.textBox_xml.Location = new System.Drawing.Point(12, 27);
this.textBox_xml.MaxLength = 0;
this.textBox_xml.Multiline = true;
this.textBox_xml.Name = "textBox_xml";
this.textBox_xml.ScrollBars = System.Windows.Forms.ScrollBars.Both;
this.textBox_xml.Size = new System.Drawing.Size(376, 194);
this.textBox_xml.TabIndex = 3;
// 
// label1
// 
this.label1.AutoSize = true;
this.label1.Location = new System.Drawing.Point(9, 9);
this.label1.Name = "label1";
this.label1.Size = new System.Drawing.Size(138, 15);
this.label1.TabIndex = 4;
this.label1.Text = "�ڵ��XML����(&X):";
// 
// button_Cancel
// 
this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
this.button_Cancel.Location = new System.Drawing.Point(313, 227);
this.button_Cancel.Name = "button_Cancel";
this.button_Cancel.Size = new System.Drawing.Size(75, 28);
this.button_Cancel.TabIndex = 23;
this.button_Cancel.Text = "ȡ��";
this.button_Cancel.UseVisualStyleBackColor = true;
this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
// 
// button_OK
// 
this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
this.button_OK.Location = new System.Drawing.Point(232, 227);
this.button_OK.Name = "button_OK";
this.button_OK.Size = new System.Drawing.Size(75, 28);
this.button_OK.TabIndex = 22;
this.button_OK.Text = "ȷ��";
this.button_OK.UseVisualStyleBackColor = true;
this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
// 
// ZhongcihaoNstableDialog
// 
this.AcceptButton = this.button_OK;
this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
this.CancelButton = this.button_Cancel;
this.ClientSize = new System.Drawing.Size(400, 267);
this.Controls.Add(this.button_Cancel);
this.Controls.Add(this.button_OK);
this.Controls.Add(this.label1);
this.Controls.Add(this.textBox_xml);
this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
this.Name = "ZhongcihaoNstableDialog";
this.ShowInTaskbar = false;
this.Text = "���ֱ�ڵ�";
this.ResumeLayout(false);
this.PerformLayout();

        }

        #endregion

        private DigitalPlatform.GUI.NoHasSelTextBox textBox_xml;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_OK;
    }
}