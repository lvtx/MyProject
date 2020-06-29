using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using System.Xml;
using DigitalPlatform.Xml;

namespace DigitalPlatform.CommonDialog
{
	/// <summary>
	/// ��������ֹ
	/// </summary>
	public class PropertyDlg111 : System.Windows.Forms.Form
	{
		public string CfgFileName = "";
		public string Lang = "zh";

		public string PropertyString = "";

		string[] aPropertyName = null;	// �����г��ֹ���������

		ArrayList aLangName = null;	// �����г��ֹ�����������

        ListViewItem tipsItem = null;

		private System.Windows.Forms.Label label_property;
		private System.Windows.Forms.TextBox textBox_property;
		private System.Windows.Forms.Button button_Cancel;
		private System.Windows.Forms.Button button_OK;
        private DigitalPlatform.GUI.ListViewNF listView_property;
		private System.Windows.Forms.ToolTip toolTip_comment;
		private System.Windows.Forms.Button button_checkAll;
		private System.Windows.Forms.Button button_uncheckAll;
        private SplitContainer splitContainer_main;
        private Panel panel_up;
        private Panel panel_down;
		private System.ComponentModel.IContainer components;

		public PropertyDlg111()
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyDlg111));
            this.label_property = new System.Windows.Forms.Label();
            this.textBox_property = new System.Windows.Forms.TextBox();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.listView_property = new DigitalPlatform.GUI.ListViewNF();
            this.toolTip_comment = new System.Windows.Forms.ToolTip(this.components);
            this.button_checkAll = new System.Windows.Forms.Button();
            this.button_uncheckAll = new System.Windows.Forms.Button();
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.panel_up = new System.Windows.Forms.Panel();
            this.panel_down = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            this.panel_up.SuspendLayout();
            this.panel_down.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_property
            // 
            this.label_property.AutoSize = true;
            this.label_property.Location = new System.Drawing.Point(-2, 4);
            this.label_property.Name = "label_property";
            this.label_property.Size = new System.Drawing.Size(41, 12);
            this.label_property.TabIndex = 1;
            this.label_property.Text = "ֵ(&V):";
            this.label_property.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_property
            // 
            this.textBox_property.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_property.Location = new System.Drawing.Point(0, 18);
            this.textBox_property.MaxLength = 0;
            this.textBox_property.Multiline = true;
            this.textBox_property.Name = "textBox_property";
            this.textBox_property.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_property.Size = new System.Drawing.Size(586, 81);
            this.textBox_property.TabIndex = 2;
            this.textBox_property.TextChanged += new System.EventHandler(this.textBox_property_TextChanged);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.AutoSize = true;
            this.button_Cancel.Location = new System.Drawing.Point(535, 373);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(60, 22);
            this.button_Cancel.TabIndex = 4;
            this.button_Cancel.Text = "ȡ��";
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.AutoSize = true;
            this.button_OK.Location = new System.Drawing.Point(471, 373);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(60, 22);
            this.button_OK.TabIndex = 3;
            this.button_OK.Text = "ȷ��";
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // listView_property
            // 
            this.listView_property.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_property.CheckBoxes = true;
            this.listView_property.FullRowSelect = true;
            this.listView_property.Location = new System.Drawing.Point(0, 0);
            this.listView_property.Name = "listView_property";
            this.listView_property.Size = new System.Drawing.Size(586, 207);
            this.listView_property.TabIndex = 0;
            this.listView_property.UseCompatibleStateImageBehavior = false;
            this.listView_property.View = System.Windows.Forms.View.Details;
            this.listView_property.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_property_ColumnClick);
            this.listView_property.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listView_property_ItemCheck);
            this.listView_property.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listView_property_MouseMove);
            this.listView_property.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView_property_MouseUp);
            // 
            // toolTip_comment
            // 
            this.toolTip_comment.AutomaticDelay = 1000;
            this.toolTip_comment.AutoPopDelay = 5000;
            this.toolTip_comment.InitialDelay = 1000;
            this.toolTip_comment.IsBalloon = true;
            this.toolTip_comment.ReshowDelay = 1000;
            this.toolTip_comment.ShowAlways = true;
            this.toolTip_comment.UseAnimation = false;
            this.toolTip_comment.UseFading = false;
            // 
            // button_checkAll
            // 
            this.button_checkAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_checkAll.AutoSize = true;
            this.button_checkAll.Location = new System.Drawing.Point(0, 212);
            this.button_checkAll.Name = "button_checkAll";
            this.button_checkAll.Size = new System.Drawing.Size(68, 22);
            this.button_checkAll.TabIndex = 5;
            this.button_checkAll.Text = "ȫѡ(&A)";
            this.button_checkAll.Click += new System.EventHandler(this.button_checkAll_Click);
            // 
            // button_uncheckAll
            // 
            this.button_uncheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_uncheckAll.AutoSize = true;
            this.button_uncheckAll.Location = new System.Drawing.Point(73, 212);
            this.button_uncheckAll.Name = "button_uncheckAll";
            this.button_uncheckAll.Size = new System.Drawing.Size(66, 22);
            this.button_uncheckAll.TabIndex = 6;
            this.button_uncheckAll.Text = "���(&C)";
            this.button_uncheckAll.Click += new System.EventHandler(this.button_uncheckAll_Click);
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer_main.Location = new System.Drawing.Point(9, 10);
            this.splitContainer_main.Name = "splitContainer_main";
            this.splitContainer_main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.panel_up);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.panel_down);
            this.splitContainer_main.Size = new System.Drawing.Size(586, 341);
            this.splitContainer_main.SplitterDistance = 234;
            this.splitContainer_main.SplitterWidth = 8;
            this.splitContainer_main.TabIndex = 7;
            // 
            // panel_up
            // 
            this.panel_up.Controls.Add(this.listView_property);
            this.panel_up.Controls.Add(this.button_uncheckAll);
            this.panel_up.Controls.Add(this.button_checkAll);
            this.panel_up.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_up.Location = new System.Drawing.Point(0, 0);
            this.panel_up.Name = "panel_up";
            this.panel_up.Size = new System.Drawing.Size(586, 234);
            this.panel_up.TabIndex = 0;
            // 
            // panel_down
            // 
            this.panel_down.Controls.Add(this.label_property);
            this.panel_down.Controls.Add(this.textBox_property);
            this.panel_down.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_down.Location = new System.Drawing.Point(0, 0);
            this.panel_down.Name = "panel_down";
            this.panel_down.Size = new System.Drawing.Size(586, 99);
            this.panel_down.TabIndex = 0;
            // 
            // PropertyDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(604, 404);
            this.Controls.Add(this.splitContainer_main);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PropertyDlg";
            this.ShowInTaskbar = false;
            this.Text = "PropertyDlg";
            this.Load += new System.EventHandler(this.PropertyDlg_Load);
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.panel_up.ResumeLayout(false);
            this.panel_up.PerformLayout();
            this.panel_down.ResumeLayout(false);
            this.panel_down.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void PropertyDlg_Load(object sender, System.EventArgs e)
		{
			SetListViewTitle(listView_property);
			LoadXml();

			textBox_property.Text = PropertyString;

			toolTip_comment.SetToolTip(this.listView_property, "tool tip text");

		}

		public void SetListViewTitle(ListView listView)
		{
			listView.Columns.Add("����ֵ", 200, HorizontalAlignment.Left);
			listView.Columns.Add("˵��", 900, HorizontalAlignment.Left);
		}


		void LoadXml()
		{

			listView_property.Items.Clear();

			string strErrorInfo = "";

			if (CfgFileName == "")
				return;

			XmlDocument dom = new XmlDocument();

			try 
			{
				dom.Load(CfgFileName);
			}
			catch (Exception ex) 
			{
				strErrorInfo = ex.Message;
				goto ERROR1;

			}

			XmlNodeList propertyList = dom.SelectNodes("root/property");

			int i,j;
			for(i=0;i<propertyList.Count;i++) 
			{
				// �ҵ���������
				string strName = DomUtil.GetAttr(propertyList[i], "name");

				if (strName == "")
					continue;

				// ���������ҵ�comment�ַ���
				XmlNode nodeComment = null;

				if (Lang == "") 
					nodeComment = propertyList[i].SelectSingleNode("comment");
				else 
				{
					nodeComment = propertyList[i].SelectSingleNode("comment[@lang='"+Lang+"']");
					if (nodeComment == null)	// ����ָ���������ң�����û���ҵ�
						nodeComment = propertyList[i].SelectSingleNode("comment");
				}

				string strComment = "";

				if (nodeComment != null) 
				{
					strComment = DomUtil.GetNodeText(nodeComment);
				}

				ListViewItem item =
					new ListViewItem(strName,
					0);

				item.SubItems.Add(strComment);

				listView_property.Items.Add(item);	

			}

			// �����ַ������飬������ʱ����
			aPropertyName = new string[listView_property.Items.Count];
			for(j=0;j<listView_property.Items.Count;j++) 
			{
				aPropertyName[j] = listView_property.Items[j].Text;
			}


			// ������������
			XmlNodeList commentList = dom.SelectNodes("//property/comment");

			aLangName = new ArrayList();

			for(i=0;i<propertyList.Count;i++) 
			{
				// �ҵ���������
				string strLang = DomUtil.GetAttr(commentList[i], "lang");

				if (strLang == "")
					continue;

				bool bFound = false;
				for(j=0;j<aLangName.Count;j++) 
				{
					if (strLang == (string)aLangName[j]) 
					{
						bFound = true;
						break;
					}
				}

				if (bFound == false)
					aLangName.Add(strLang);

			}


			return;
			ERROR1:
				MessageBox.Show(strErrorInfo);
			return;

		}

		// �Ƿ�Ϊ�Ѿ������������
		bool IsDefinedPropertyName(string strName)
		{
            if (this.aPropertyName == null)
                return false;

			for(int i=0;i<aPropertyName.Length;i++) 
			{
				if (String.Compare(strName,aPropertyName[i],true) == 0)
					return true;
			}

			return false;
		}

		// ���һ���б������ڵ�ǰû�ж����������
		ArrayList GetNoDefinedPropertyNames(string strList)
		{
			ArrayList aResult = new ArrayList();
			string[] aName = strList.Split(new Char [] {','});

			for(int i=0;i<aName.Length;i++) 
			{
				string strName = aName[i];

				strName = strName.Trim();
				if (strName == "")
					continue;

				if (IsDefinedPropertyName(strName) == true)
					continue;

				aResult.Add(strName);
			}

			return aResult;
		}

		// ���һ���б������ڵ�ǰ�����������
		ArrayList GetDefinedPropertyNames(string strList)
		{
			ArrayList aResult = new ArrayList();
			string[] aName = strList.Split(new Char [] {','});

			for(int i=0;i<aName.Length;i++) 
			{
				string strName = aName[i];
				strName = strName.Trim();
				if (strName == "")
					continue;

				if (IsDefinedPropertyName(strName) == false)
					continue;

				aResult.Add(strName);
			}

			return aResult;
		}

		private void listView_property_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			// �õ�checked����
			ArrayList checkedItems = GetCheckedItems(listView_property, e);


			// ���edit������û�ж���Ĳ���
			ArrayList aNotDefined = GetNoDefinedPropertyNames(textBox_property.Text);

			string strText = "";

			// checked���Ϊ�ַ���
			for(int i=0; i<checkedItems.Count;i++) 
			{
				if (strText != "")
					strText += ",";
				strText += ((ListViewItem)checkedItems[i]).Text;
			}

			// û�ж��岿�����Ϊ�ַ���
			for(int j=0; j<aNotDefined.Count;j++) 
			{
				if (strText != "")
					strText += ",";
				strText += (string)aNotDefined[j];
			}
	
			/*
			if (textBox_property.Text != strText)
				textBox_property.Text = strText;
			*/
			string[] aOld = textBox_property.Text.Split(new Char [] {','});
			string[] aNew = strText.Split(new Char [] {','});
			Array.Sort(aOld);
			Array.Sort(aNew);

			if (String.Compare(String.Join(",", aOld), String.Join(",", aNew),true) != 0) 
			{
				textBox_property.Text = strText;
			}

			ChangeColor();

		}


		void ChangeColor()
		{
			for(int i=0;i<this.listView_property.Items.Count;i++)
			{
				if (this.listView_property.Items[i].Checked == false)
				{
					this.listView_property.Items[i].ForeColor = SystemColors.WindowText;
					this.listView_property.Items[i].BackColor = SystemColors.Window;
				}
				else 
				{
					this.listView_property.Items[i].ForeColor = SystemColors.MenuText;
					this.listView_property.Items[i].BackColor = SystemColors.Menu;
				}

			}
		}

		private void textBox_property_TextChanged(object sender, System.EventArgs e)
		{
			bool bChanged = false;
			// ��ȡ�Ѷ���Ĳ���
			ArrayList aDefined = GetDefinedPropertyNames(textBox_property.Text);

			// check
			for(int i=0;i<aDefined.Count;i++) 
			{
				string strName = (string)aDefined[i];

				bool bRet = CheckItem(strName, true);
				if (bRet == true)
					bChanged = true;
			}

			// uncheck
			for(int j=0;j<listView_property.Items.Count;j++) 
			{
				string strItemName = listView_property.Items[j].Text;

				bool bFound = false;
				for(int k=0;k<aDefined.Count;k++) 
				{
					if (String.Compare(strItemName, (string)aDefined[k], true) == 0) 
					{
						bFound = true;	// ������Ҫon������
						break;
					}
				}

				// ������Ҫoff������
				if (bFound == false) 
				{
					if (listView_property.Items[j].Checked == false)
						continue;
					listView_property.Items[j].Checked = false;
					bChanged = true;
				}

			}

		}

		bool CheckItem(string strName, bool bChecked)
		{
			for(int i=0;i<listView_property.Items.Count;i++) 
			{
				if (String.Compare(strName, listView_property.Items[i].Text, true) ==0) 
				{
					if (listView_property.Items[i].Checked == bChecked)
						return false;	// û�иı�״̬
					else
					{
						listView_property.Items[i].Checked = bChecked;
						return true;	// �ı���״̬
					}

				}
			}

			return false;	// û���ҵ�����
		}

		// �õ�����checked Item
		// �ǵ�ǰ������checked item���Ͽ���e�����п������Ӻͼ�����item���ϲ�����
		// e����Ϊnull����ʾ����������������
		ArrayList GetCheckedItems(System.Windows.Forms.ListView listview,
			System.Windows.Forms.ItemCheckEventArgs e)
		{
			ArrayList selectedItems = new ArrayList();
			selectedItems.AddRange(listview.CheckedItems);

			if (e != null) 
			{
				if (e.NewValue == CheckState.Checked)
					selectedItems.Add(listview.Items[e.Index]);
				else 
				{
					selectedItems.Remove(listview.Items[e.Index]);
				}
			}

			return selectedItems;
		}

		private void button_OK_Click(object sender, System.EventArgs e)
		{
			PropertyString = textBox_property.Text;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void button_Cancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}


		private void listView_property_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ListViewItem selection = listView_property.GetItemAt(e.X, e.Y);

			// If the user selects an item in the ListView, display
			// the image in the PictureBox.
			if (selection != null) 
			{
                if (selection != tipsItem)
                {
                    toolTip_comment.SetToolTip(this.listView_property, selection.SubItems[1].Text);
                }
			}
			else 
			{
				toolTip_comment.SetToolTip(listView_property, "");
			}

            tipsItem = selection;
		
		}

		private void listView_property_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button != MouseButtons.Right)
				return;

			ContextMenu contextMenu = new ContextMenu();
			MenuItem menuItem = null;

			menuItem = new MenuItem("ѡ������(&L)");
			contextMenu.MenuItems.Add(menuItem);

			// �Ӳ˵�
			for(int i=0;i<aLangName.Count;i++) 
			{

				MenuItem menuItemSub = new MenuItem((string)aLangName[i]);
				menuItemSub.Click += new System.EventHandler(this.menu_selectLanguage_Click);

				menuItem.MenuItems.Add(menuItemSub);

				if ((string)aLangName[i] == Lang) 
				{
					menuItemSub.Enabled = false;
					menuItemSub.Checked = true;
				}

			}
		
			contextMenu.Show(listView_property, new Point(e.X, e.Y) );		
		}

		// ѡ��������
		private void menu_selectLanguage_Click(object sender, System.EventArgs e)
		{

			if (sender is MenuItem) 
			{
				string strSave = textBox_property.Text;

				MenuItem menuItem = (MenuItem)sender;
				// MessageBox.Show(menuItem.Text);
				Lang = menuItem.Text;
				LoadXml();

				textBox_property.Text = strSave;
				textBox_property_TextChanged(null, null);
			}

		}

		private void button_checkAll_Click(object sender, System.EventArgs e)
		{
			for(int i=0;i<this.listView_property.Items.Count;i++)
			{
				this.listView_property.Items[i].Checked = true;
			}
		
		}

		private void button_uncheckAll_Click(object sender, System.EventArgs e)
		{
			for(int i=0;i<this.listView_property.Items.Count;i++)
			{
				this.listView_property.Items[i].Checked = false;
			}
		}

        SortColumns SortColumns = new SortColumns();

        private void listView_property_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int nClickColumn = e.Column;
            this.SortColumns.SetFirstColumn(nClickColumn,
                this.listView_property.Columns);

            // ����
            this.listView_property.ListViewItemSorter = new SortColumnsComparer(this.SortColumns);

            this.listView_property.ListViewItemSorter = null;
        }

	}
}
