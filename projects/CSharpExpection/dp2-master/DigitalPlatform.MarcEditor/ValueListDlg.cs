using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;

using DigitalPlatform.GUI;
using DigitalPlatform.Xml;

namespace DigitalPlatform.Marc
{
	/// <summary>
	/// ֵ�б�Ի���
	/// </summary>
	internal class ValueListDlg : System.Windows.Forms.Form
	{
        // ����������к�����
        SortColumns SortColumns = new SortColumns();

        public ApplicationInfo AppInfo = null;  // 2009/9/18

		private System.Windows.Forms.ColumnHeader columnHeader_value;
		private System.Windows.Forms.ColumnHeader columnHeader_description;
		private System.Windows.Forms.Button button_ok;
		private System.Windows.Forms.Button button_cancel;
        private DigitalPlatform.GUI.ListViewNF listView_valueList;

        string m_strInitialValue = ""; // ��ʼӦѡ���ֵ

		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ValueListDlg()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
		}

		/// <summary>
		/// ������������ʹ�õ���Դ��
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

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValueListDlg));
            this.listView_valueList = new DigitalPlatform.GUI.ListViewNF();
            this.columnHeader_value = new System.Windows.Forms.ColumnHeader();
            this.columnHeader_description = new System.Windows.Forms.ColumnHeader();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView_valueList
            // 
            this.listView_valueList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_valueList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_value,
            this.columnHeader_description});
            this.listView_valueList.FullRowSelect = true;
            this.listView_valueList.HideSelection = false;
            this.listView_valueList.Location = new System.Drawing.Point(12, 12);
            this.listView_valueList.MultiSelect = false;
            this.listView_valueList.Name = "listView_valueList";
            this.listView_valueList.Size = new System.Drawing.Size(295, 199);
            this.listView_valueList.TabIndex = 0;
            this.listView_valueList.UseCompatibleStateImageBehavior = false;
            this.listView_valueList.View = System.Windows.Forms.View.Details;
            this.listView_valueList.DoubleClick += new System.EventHandler(this.listView_valueList_DoubleClick);
            this.listView_valueList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_valueList_ColumnClick);
            // 
            // columnHeader_value
            // 
            this.columnHeader_value.Text = "ֵ";
            this.columnHeader_value.Width = 66;
            // 
            // columnHeader_description
            // 
            this.columnHeader_description.Text = "˵��";
            this.columnHeader_description.Width = 164;
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(101, 217);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(100, 28);
            this.button_ok.TabIndex = 1;
            this.button_ok.Text = "ȷ��";
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(207, 217);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(100, 28);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "ȡ��";
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // ValueListDlg
            // 
            this.AcceptButton = this.button_ok;
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 18);
            this.CancelButton = this.button_cancel;
            this.ClientSize = new System.Drawing.Size(319, 257);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.listView_valueList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ValueListDlg";
            this.ShowInTaskbar = false;
            this.Text = "ValueListDlg";
            this.Load += new System.EventHandler(this.ValueListDlg_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ValueListDlg_FormClosed);
            this.ResumeLayout(false);

		}
		#endregion

        public string SelectedValue
        {
            get
            {
                if (this.listView_valueList.SelectedItems.Count == 0)
                    return null;
                return this.listView_valueList.SelectedItems[0].Text;
            }
            set
            {
                if (this.listView_valueList.Items.Count == 0
                    || this.Visible == false)
                    m_strInitialValue = value;
                else
                    SelectItem(value);
            }
        }

        void SelectItem(string strValue)
        {
            for (int i = 0; i < this.listView_valueList.Items.Count; i++)
            {
                if (this.listView_valueList.Items[i].Text == strValue)
                {
                    this.listView_valueList.FocusedItem = this.listView_valueList.Items[i]; // 2009/9/18
                    this.listView_valueList.Items[i].Selected = true;
                    this.listView_valueList.EnsureVisible(i);
                    return;
                }
            }
        }

        private void button_ok_Click(object sender, System.EventArgs e)
		{
            if (this.listView_valueList.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ������");
                return;
            }
			// ��Ҫ��������
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void button_cancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}


		public int Initialize(List<XmlNode> valueListNodes,
			string strLang,
			out string strError)
		{
			strError = "";

            Debug.Assert(valueListNodes != null, "Initialize() �� valueListNodes��������Ϊnull��");
            
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("xml", Ns.xml);

            foreach (XmlNode valueListNode in valueListNodes)
            {
                XmlNodeList itemList = valueListNode.SelectNodes("Item");
                foreach (XmlNode itemNode in itemList)
                {
                    string strItemLable = "";

                    // ��һ��Ԫ�ص��¼��Ķ��<strElementName>Ԫ����, ��ȡ���Է��ϵ�XmlNode��InnerText
                    // parameters:
                    //      bReturnFirstNode    ����Ҳ���������Եģ��Ƿ񷵻ص�һ��<strElementName>
                    strItemLable = DomUtil.GetXmlLangedNodeText(
                strLang,
                itemNode,
                "Label",
                true);
                    if (string.IsNullOrEmpty(strItemLable) == true)
                        strItemLable = "????????";

#if NO
                    XmlNode itemLabelNode = itemNode.SelectSingleNode("Label[@xml:lang='" + strLang + "']", nsmgr);
                    if (itemLabelNode == null)
                    {
                        strItemLable = "????????";
                    }
                    else
                    {
                        strItemLable = DomUtil.GetNodeText(itemLabelNode);
                    }
#endif

                    XmlNode itemValueNode = itemNode.SelectSingleNode("Value");
                    string strItemValue = DomUtil.GetNodeText(itemValueNode);

                    ListViewItem item = new ListViewItem(strItemValue);
                    item.SubItems.Add(strItemLable);
                    this.listView_valueList.Items.Add(item);
                }
            }

			return 0;
		}

		private void listView_valueList_DoubleClick(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

        private void ValueListDlg_Load(object sender, EventArgs e)
        {
            if (this.AppInfo != null)
            {
                string strWidths = this.AppInfo.GetString(
                    "marceditor_valuelistdialog",
                    "list_column_width",
                    "");
                if (String.IsNullOrEmpty(strWidths) == false)
                {
                    ListViewUtil.SetColumnHeaderWidth(this.listView_valueList,
                        strWidths,
                        true);
                }
            }

            if (String.IsNullOrEmpty(this.m_strInitialValue) == false)
                this.SelectItem(this.m_strInitialValue);
        }

        private void listView_valueList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int nClickColumn = e.Column;
            this.SortColumns.SetFirstColumn(nClickColumn,
                this.listView_valueList.Columns);

            // ����
            this.listView_valueList.ListViewItemSorter = new SortColumnsComparer(this.SortColumns);
            this.listView_valueList.ListViewItemSorter = null;
        }

        private void ValueListDlg_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.AppInfo != null)
            {
                string strWidths = ListViewUtil.GetColumnWidthListString(this.listView_valueList);
                this.AppInfo.SetString(
                    "marceditor_valuelistdialog",
                    "list_column_width",
                    strWidths);
            }

        }
	}
}
