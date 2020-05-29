using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DigitalPlatform.rms.Client
{
	/// <summary>
	/// ���ݿ��¼��������, �´���ͬ�������¼�����ʱ�������ͺ����ƵĶԻ���
	/// </summary>
	public class NewObjectDlg : System.Windows.Forms.Form
	{
		public int Type = -1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBox_objectType;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.TextBox textBox_objectName;
		private System.Windows.Forms.Button button_OK;
		private System.Windows.Forms.Button button_Cancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NewObjectDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_objectType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_objectName = new System.Windows.Forms.TextBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "����:";
            // 
            // comboBox_objectType
            // 
            this.comboBox_objectType.Items.AddRange(new object[] {
            "Ŀ¼",
            "�ļ�"});
            this.comboBox_objectType.Location = new System.Drawing.Point(72, 12);
            this.comboBox_objectType.Name = "comboBox_objectType";
            this.comboBox_objectType.Size = new System.Drawing.Size(176, 20);
            this.comboBox_objectType.TabIndex = 1;
            this.comboBox_objectType.Text = "�ļ�";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "����:";
            // 
            // textBox_objectName
            // 
            this.textBox_objectName.Location = new System.Drawing.Point(72, 43);
            this.textBox_objectName.Name = "textBox_objectName";
            this.textBox_objectName.Size = new System.Drawing.Size(176, 21);
            this.textBox_objectName.TabIndex = 3;
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(281, 12);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 4;
            this.button_OK.Text = "ȷ��";
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(281, 41);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 5;
            this.button_Cancel.Text = "ȡ��";
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // NewObjectDlg
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(368, 88);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.textBox_objectName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_objectType);
            this.Controls.Add(this.label1);
            this.Name = "NewObjectDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "�¶���";
            this.Load += new System.EventHandler(this.NewObjectDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void button_OK_Click(object sender, System.EventArgs e)
		{
			if (this.textBox_objectName.Text == "")
			{
				MessageBox.Show("��δָ����������");
				return;
			}

			if (comboBox_objectType.Text == "")
			{
				MessageBox.Show("��δָ����������");
				return;
			}

			this.Type = ConvertObjectTypeStringToInt(this.comboBox_objectType.Text);
			if (this.Type == -1)
			{
				MessageBox.Show("��������δ֪");
				return;
			}

		
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		public static int ConvertObjectTypeStringToInt(string strType)
		{
			switch (strType)
			{
				case "�ļ�":
					return ResTree.RESTYPE_FILE;
				case "Ŀ¼":
					return ResTree.RESTYPE_FOLDER;
			}

			return -1;	// δ֪������
		}

		public static string ConvertObjectTypeIntToString(int nType)
		{
			switch (nType)
			{
				case ResTree.RESTYPE_FILE:
					return "�ļ�";
				case ResTree.RESTYPE_FOLDER:
					return "Ŀ¼";
			}

			return "δ֪����:" + Convert.ToString(nType);	// δ֪������
		}

		private void button_Cancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void NewObjectDlg_Load(object sender, System.EventArgs e)
		{
			if (this.Type != -1)
				this.comboBox_objectType.Text = ConvertObjectTypeIntToString(this.Type);
		
		}
	}
}
