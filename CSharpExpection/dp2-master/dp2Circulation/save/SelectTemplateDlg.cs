using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;

using DigitalPlatform;
using DigitalPlatform.Xml;
using DigitalPlatform.GUI;

namespace dp2Circulation
{
	/// <summary>
    /// !!! �����Ѿ���ֹ���� DigitalPlatform.CirculationClient.SelectRecordTemplateDlg ����
    /// ѡ���¼�¼ģ�����ĶԻ���
    /// ���Ի������һ��xmlģ���ļ����г����� &lt;template&gt; Ԫ�ص� name����ֵ�����û�ѡ��
	/// ���Ի���ѡ���Ԫ���Լ�����ȫ��Ԫ�ش���һ���µ�xml�ĵ�(�ַ�����ʽ).
	/// ���Ի��򲻸���ӷ������ļ���
	/// </summary>
	internal class SelectTemplateDlg : System.Windows.Forms.Form
	{
        const int WM_AUTO_CLOSE = API.WM_USER + 200;
        public bool AutoClose = false;  // �Ի���ڴ򿪺������ر�?

        // 2008/6/24 new add
        public bool SaveMode = false;   // �Ƿ�Ϊ����ģʽ��

		//public ApplicationInfo AppInfo = null;	// ����
		//public string ApCfgTitle = "";	// ��ap�б��洰�����״̬�ı����ַ���


		// public string InputXml = "";
		// public string OutputXml = "";

		public string SelectedRecordXml = "";

		public bool CheckNameExist = true;

		XmlDocument dom = null;
		bool m_bChanged = false;	// DOM�����Ƿ��б仯

        private DigitalPlatform.GUI.ListViewNF listView1;
		private System.Windows.Forms.ColumnHeader columnHeader_name;
		private System.Windows.Forms.ColumnHeader columnHeader_comment;
		private System.Windows.Forms.CheckBox checkBox_notAsk;
		private System.Windows.Forms.Button button_OK;
		private System.Windows.Forms.Button button_Cancel;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.TextBox textBox_name;

		private System.ComponentModel.Container components = null;

		public SelectTemplateDlg()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectTemplateDlg));
            this.listView1 = new DigitalPlatform.GUI.ListViewNF();
            this.columnHeader_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_comment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBox_notAsk = new System.Windows.Forms.CheckBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_name,
            this.columnHeader_comment});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(9, 9);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(429, 249);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.Enter += new System.EventHandler(this.listView1_Enter);
            this.listView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseUp);
            // 
            // columnHeader_name
            // 
            this.columnHeader_name.Text = "ģ����";
            this.columnHeader_name.Width = 200;
            // 
            // columnHeader_comment
            // 
            this.columnHeader_comment.Text = "˵��";
            this.columnHeader_comment.Width = 300;
            // 
            // checkBox_notAsk
            // 
            this.checkBox_notAsk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_notAsk.AutoSize = true;
            this.checkBox_notAsk.Enabled = false;
            this.checkBox_notAsk.Location = new System.Drawing.Point(9, 295);
            this.checkBox_notAsk.Name = "checkBox_notAsk";
            this.checkBox_notAsk.Size = new System.Drawing.Size(144, 16);
            this.checkBox_notAsk.TabIndex = 1;
            this.checkBox_notAsk.Text = "�´β��ٳ��ִ˶Ի���";
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.Location = new System.Drawing.Point(362, 263);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(76, 22);
            this.button_OK.TabIndex = 2;
            this.button_OK.Text = "ȷ��";
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(362, 290);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(76, 21);
            this.button_Cancel.TabIndex = 3;
            this.button_Cancel.Text = "ȡ��";
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 265);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "ģ����(&N):";
            // 
            // textBox_name
            // 
            this.textBox_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_name.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBox_name.Location = new System.Drawing.Point(85, 263);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(273, 21);
            this.textBox_name.TabIndex = 5;
            this.textBox_name.TextChanged += new System.EventHandler(this.textBox_name_TextChanged);
            this.textBox_name.Enter += new System.EventHandler(this.textBox_name_Enter);
            // 
            // SelectTemplateDlg
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(446, 321);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.checkBox_notAsk);
            this.Controls.Add(this.listView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectTemplateDlg";
            this.ShowInTaskbar = false;
            this.Text = "��ѡ���¼�¼ģ��";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.SelectRecordTemplateDlg_Closing);
            this.Closed += new System.EventHandler(this.SelectRecordTemplateDlg_Closed);
            this.Load += new System.EventHandler(this.SelectRecordTemplateDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void SelectRecordTemplateDlg_Load(object sender, System.EventArgs e)
		{
            /*
			if (AppInfo != null) 
			{
				if (ApCfgTitle != "" && ApCfgTitle != null) 
				{
					AppInfo.LoadFormStates(this,
						ApCfgTitle);
				}
				else 
				{
					Debug.Assert(true, "��Ҫ��ap����ͻָ��������״̬������������ApCfgTitle��Ա");
				}

			}*/

			if (dom != null)
			{
				FillList(true);
			}
			else 
			{
				Debug.Assert(true, "��һ������������Initial()");
			}

#if NO
            if (this.SaveMode == false)
                this.checkBox_notAsk.Enabled = true;
#endif

            if (this.AutoClose == true)
                API.PostMessage(this.Handle, WM_AUTO_CLOSE, 0, 0);
		}

        private void SelectRecordTemplateDlg_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK
                && m_bChanged == true)
            {
                DialogResult result = MessageBox.Show(this,
                    "ȷʵҪ������ǰ������ȫ���޸�ô?\r\n\r\n(��)�����޸� (��)���رմ���\r\n\r\n(ע: ģ����Ϊ�յ�������Կ��԰�\"ȷ��\"��ť�����������޸ġ�)",
                    "SelectRecordTemplateDlg",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

		private void SelectRecordTemplateDlg_Closed(object sender, System.EventArgs e)
		{
            /*
			if (AppInfo != null) 
			{
				if (ApCfgTitle != "" && ApCfgTitle != null) 
				{
					AppInfo.SaveFormStates(this,
						ApCfgTitle);
				}
				else 
				{
					Debug.Assert(true, "��Ҫ��ap����ͻָ��������״̬������������ApCfgTitle��Ա");
				}

			}
             * */
		}

        // 2015/5/11
        /// <summary>
        /// �Ƿ����� ���´β��ٳ��� ...�� checkbox��ȱʡΪ false
        /// ����һ��������״̬���� SaveMode �޹�
        /// </summary>
        public bool EnableNotAsk
        {
            get
            {
                return this.checkBox_notAsk.Enabled;
            }
            set
            {
                this.checkBox_notAsk.Enabled = value;
            }
        }

		public int Initial(
            bool bSaveMode,
            string strInputXml,
			out string strError)
		{
			strError = "";

            this.SaveMode = bSaveMode;

            if (string.IsNullOrEmpty(strInputXml) == true)
                strInputXml = "<root />";

			dom = new XmlDocument();

			try 
			{
				dom.LoadXml(strInputXml);
			}
			catch (Exception ex)
			{
                // Debug.Assert(false, "");    // debug
				strError = ex.Message;
				return -1;
			}

			return 0;
		}

        /// <summary>
        /// ȱʡ���ڹ���
        /// </summary>
        /// <param name="m">��Ϣ</param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_AUTO_CLOSE:
                    this.button_OK_Click(this, null);
                    return;
            }
            base.DefWndProc(ref m);
        }

		void FillList(bool bAutoSelect)
		{
			listView1.Items.Clear();

			XmlNodeList nodes = dom.DocumentElement.SelectNodes("template");

			for(int i=0;i<nodes.Count; i++) 
			{
				string strName = DomUtil.GetAttr(nodes[i], "name");
				string strComment =  DomUtil.GetAttr(nodes[i], "comment");

				ListViewItem item = new ListViewItem(strName, 0);

				listView1.Items.Add(item);

				item.SubItems.Add(strComment);
			}

			// ѡ���һ��
			if (bAutoSelect == true) 
			{
                if (String.IsNullOrEmpty(this.SelectedName) == false)
                {
                    if (listView1.Items.Count != 0)
                    {
                        ListViewItem item = ListViewUtil.FindItem(this.listView1, this.SelectedName, 0);
                        if (item != null)
                        {
                            ListViewUtil.SelectLine(item, true);
                        }
                    }
                }
                else
                {
                    if (listView1.Items.Count != 0)
                        listView1.Items[0].Selected = true;
                }
			}

            listView1_SelectedIndexChanged(null, null);
		}

		private void button_OK_Click(object sender, System.EventArgs e)
		{
			// ���m_bChanged == true������հ���OK�˳�
			if (m_bChanged == false && textBox_name.Text == "")
			{
                MessageBox.Show(this, "��δָ��ģ����");
				return ;
			}

            /*
            if (checkBox_delete.Checked == true)
            {
                DialogResult result = MessageBox.Show(this,
                    "ȷʵҪɾ������ģ���¼?\r\n\r\n" + textBox_name.Text,
                    "SelectRecordTemplateDlg",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, 
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                    return;
                goto END1;
            }
            */


            string strName = textBox_name.Text;

			XmlNode node = dom.DocumentElement.SelectSingleNode("template[@name='" + strName + "']");

			if (
                this.Changed == false
                && CheckNameExist == true 
				&& node == null) 
			{
                MessageBox.Show(this, "ģ���� '" + strName + "��ģ���ļ��в�����...");
				// MessageBox.Show(this, "SelectSingleNode()ʧ��...");
				return;
			}

			if (node != null) 
			{
				if (node.ChildNodes.Count == 0) 
				{
					MessageBox.Show(this, "<template name='"+strName+"'>Ԫ���±�����һ�����ӽڵ㣬����ڵ㽫�䵱���ڵ�...");
					return;
				}

				SelectedRecordXml = node.ChildNodes[0].OuterXml;
			}
			else
			{
				SelectedRecordXml = "";
			}

			//END1:
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void button_Cancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void listView1_DoubleClick(object sender, System.EventArgs e)
		{
			button_OK_Click(null, null);
		}

		private void listView1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (listView1.SelectedItems.Count == 0)
			{
				textBox_name.Text = "";
			}
			else 
			{
				/*
				if () // ��ѡ
				{
					textBox_name.Text = "";

					for(int i=0;i<listView1.SelectedItems.Count;i++)
					{
						if (textBox_name.Text != "")
							textBox_name.Text += ",";

						textBox_name.Text += listView1.SelectedItems[i].Text;
					}

				}
				else
				*/
					textBox_name.Text = listView1.SelectedItems[0].Text;
			}
		
		}

		private void textBox_name_TextChanged(object sender, System.EventArgs e)
		{
			/*
			if (textBox_name.Text != "")
			{
				button_OK.Enabled = true;
			}
			else 
			{
				button_OK.Enabled = false;
			}
			*/
		}

		// �滻����׷��һ����¼
		public int ReplaceRecord(string strName,
			string strContent,
			out string strError)
		{
			strError = "";
//			strOutputXml = "";

			if (dom == null)
			{
				strError = "domΪnull";
				return -1;
			}

			XmlNode node = dom.DocumentElement.SelectSingleNode("template[@name='" + strName + "']");

			if (node == null) 
			{
				
				node = dom.CreateElement("template");
				DomUtil.SetAttr(node, "name", strName);
				// �½���һ����¼
				node = dom.DocumentElement.AppendChild(node);
			}


			// Ҫ��ֹstrContent��ȫXML�ļ�����
			XmlDocument temp = new XmlDocument();
			try 
			{
				temp.LoadXml(strContent);
			}
			catch (Exception ex)
			{
				strError = ex.Message;
				return -1;
			}

			node.InnerXml = temp.DocumentElement.OuterXml;	// ���Ĵ�XML

			m_bChanged = true;

			// strOutputXml = dom.DocumentElement.OuterXml;	// DomUtil.GetXml(dom);

			return 0;
		}

		/*
		// ɾ�����ɼ�¼
		public int DeleteRecords(string strNameList,
			out string strOutputXml,
			out string strError)
		{
			strError = "";
			strOutputXml = "";

			if (dom == null)
			{
				strError = "domΪnull";
				return -1;
			}

			string[] aName = strNameList.Split(new Char [] {','});


			for(int i=0;i<aName.Length;i++)
			{
				string strName = aName[i].Trim();
				if (strName == "")
					continue;

				XmlNode node = dom.DocumentElement.SelectSingleNode("template[@name='" + strName + "']");

				if (node == null) 
					continue;

				node.ParentNode.RemoveChild(node);
			}

			strOutputXml = dom.DocumentElement.OuterXml;	// DomUtil.GetXml(dom);

			return 0;
		}
		*/

		private void listView1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button != MouseButtons.Right)
				return;

			ContextMenu contextMenu = new ContextMenu();
			MenuItem menuItem = null;

			bool bSelected = listView1.SelectedItems.Count > 0;

			//
			menuItem = new MenuItem("�޸�(&M)");
			menuItem.Click += new System.EventHandler(this.menu_modify);
			if (bSelected == false || this.SaveMode == false) 
			{
				menuItem.Enabled = false;
			}
			contextMenu.MenuItems.Add(menuItem);

			// ---
			menuItem = new MenuItem("-");
			contextMenu.MenuItems.Add(menuItem);


			menuItem = new MenuItem("ɾ��(&D)");
			menuItem.Click += new System.EventHandler(this.menu_deleteRecord);
			if (bSelected == false || this.SaveMode == false)
				menuItem.Enabled = false;
			contextMenu.MenuItems.Add(menuItem);

			contextMenu.Show(listView1, new Point(e.X, e.Y) );		
			
		}

		// �޸����ֺ�ע��
		void menu_modify(object sender, System.EventArgs e)
		{
            string strError = "";

            if (listView1.SelectedItems.Count == 0)
			{
                strError = "��δѡ�����޸ĵ�ģ���¼����...";
                goto ERROR1;
			}

            ListViewItem item = listView1.SelectedItems[0];

			TemplateRecordDlg dlg = new TemplateRecordDlg();
            MainForm.SetControlFont(dlg, this.Font, false);

            string strOldName = ListViewUtil.GetItemText(item, 0);

            dlg.TemplateName = ListViewUtil.GetItemText(item, 0);
            dlg.Comment = ListViewUtil.GetItemText(item, 1);

        REDO_INPUT:
            dlg.ShowDialog(this);
            if (dlg.DialogResult != DialogResult.OK)
                return;

            // ���� 2014/6/21
            ListViewItem dup = ListViewUtil.FindItem(this.listView1, dlg.TemplateName, 0);
            if (dup != null && dup != item)
            {
                strError = "ģ���� '"+dlg.TemplateName+"' �Ѿ���ʹ���ˣ��������ظ����֡�����������ģ����";
                MessageBox.Show(this, strError);
                goto REDO_INPUT;
            }

			int nRet = ChangeRecordProperty(strOldName,
                dlg.TemplateName,
				dlg.Comment,
				out strError);
            if (nRet == -1)
                goto ERROR1;

			FillList(false);
            return;
        ERROR1:
            MessageBox.Show(this, strError);
		}
	
		void menu_deleteRecord(object sender, System.EventArgs e)
		{
			if (listView1.SelectedItems.Count == 0)
			{
                MessageBox.Show(this, "��δѡ����ɾ����ģ���¼����...");
				return;
			}

			string strError = "";
			int nRet = 0;

			DialogResult result = MessageBox.Show(this,
                "ȷʵҪɾ����ѡ���ģ���¼?",
				"SelectRecordTemplateDlg",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question, 
				MessageBoxDefaultButton.Button2);
			if (result != DialogResult.Yes)
				return;

			
			foreach(ListViewItem item in listView1.SelectedItems)
			{
				string strOldName = ListViewUtil.GetItemText(item, 0);
	
				nRet = ChangeRecordProperty(strOldName, 
					null,
					null,
					out strError);
				if (nRet == -1) 
				{
					MessageBox.Show(this, strError);
					return;
				}
			}

			FillList(false);
		}

		// �޸�DOM�еļ�¼���ԣ�����ɾ��DOM�еļ�¼
		// parameters:
		//		strNewName	���==null����ʾɾ���˼�¼
		int ChangeRecordProperty(string strOldName,
			string strNewName,
			string strNewComment,
			out string strError)
		{

			strError = "";

			if (dom == null)
			{
				strError = "domΪnull";
				return -1;
			}

			XmlNode node = dom.DocumentElement.SelectSingleNode("template[@name='" + strOldName + "']");

			if (node == null) 
			{
                strError = "ģ���¼ '" + strOldName + "' û���ҵ�...";
				return -1;
			}

			if (strNewName == null || strNewName == "")
			{
				node.ParentNode.RemoveChild(node);
			}
			else 
			{
				DomUtil.SetAttr(node, "name", strNewName);
				DomUtil.SetAttr(node, "comment", strNewComment);
			}

			m_bChanged = true;

			return 0;
		}


        /// <summary>
        /// �����Ƿ������޸�
        /// </summary>
        public bool Changed
		{
			get 
			{
				return m_bChanged;
			}
		}
	
		public string OutputXml
		{
			get 
			{
				return dom.DocumentElement.OuterXml;	// DomUtil.GetXml(dom);
			}
		}

        public string SelectedName
        {
            get
            {
                return this.textBox_name.Text;
            }
            set
            {
                this.textBox_name.Text = value;
            }
        }

        public bool NotAsk
        {
            get
            {
                return this.checkBox_notAsk.Checked;
            }
            set
            {
                this.checkBox_notAsk.Checked = value;
            }
        }

        private void listView1_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = this.button_OK;
        }

        private void textBox_name_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = this.button_OK;
        }
	}
}
