using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Xml;
using System.Diagnostics;
using DigitalPlatform.Xml;
//using DigitalPlatform.Text;
using DigitalPlatform.Text.SectionPropertyString;

namespace DigitalPlatform.CommonDialog
{
    public partial class CategoryPropertyDlg : Form
    {
        static Color CheckedBackColor = Color.FromArgb(180, 255, 180);
        static Color UncheckedBackColor = SystemColors.Window;
        string m_strCfgFileName = "";
        string m_strLang = "zh";

        XmlDocument CfgDom = null;

        DigitalPlatform.Text.SectionPropertyString.PropertyCollection m_pc = new DigitalPlatform.Text.SectionPropertyString.PropertyCollection();

        int DisableEvent = 0;

        public CategoryPropertyDlg()
        {
            InitializeComponent();
        }

        public string Lang
        {
            get
            {
                return m_strLang;
            }
            set
            {
                if (this.m_strLang == value)
                    return;

                this.m_strLang = value;

                if (this.Visible == false)
                    return;

                if (this.CfgDom == null)
                    return;

                // ˢ����ʾ
                RefreshDisplay();

            }
        }

        public string CfgFileName
        {
            get
            {
                return m_strCfgFileName;
            }
            set
            {
                m_strCfgFileName = value;

                if (this.Visible == false)
                    return;

                string strError = "";
                // װ�������ļ�
                // return:
                //      -1  error
                //      0   CfgFileName��δָ��
                //      1   �ɹ�
                int nRet = LoadCfgXml(out strError);
                if (nRet == -1)
                    throw new Exception(strError);
                if (nRet == 0)
                    return;


                this.RefreshDisplay();
            }
        }

        public void RefreshDisplay()
        {
            // ˢ����ʾ
            string strError;
            int nRet = FillCategoryList(this.Lang,
                out strError);
            if (nRet == -1)
                goto ERROR1;

            RefreshCategoryComboText();

            nRet = FillPropertyList(this.comboBox_category.Text,
                this.Lang,
                out strError);
            if (nRet == -1)
                goto ERROR1;

//             SetCheckList(this.m_pc);


            return;

        ERROR1:
            throw new Exception(strError);
        }

        // ˢ��combo edit��ð�ź���������йص�����
        private void RefreshCategoryComboText()
        {
            if (this.CfgDom == null)
                return;

            if (String.IsNullOrEmpty(this.comboBox_category.Text) == true)
                return;

            int nRet = this.comboBox_category.Text.IndexOf(":");
            if (nRet == -1)
                return;

            string strName = this.comboBox_category.Text.Substring(0, nRet).Trim();
            if (String.IsNullOrEmpty(strName) == true)
                return;

            XmlNode node = CfgDom.DocumentElement.SelectSingleNode("//category[@name='"+strName+"']");
            if (node == null)
                return;

            string strComment = GetComment(node, this.Lang);
            if (String.IsNullOrEmpty(strComment) == true)
                return;

            this.comboBox_category.Text = strName + ": " + strComment;
        }

        public string PropertyString
        {
            get
            {
                if (this.m_pc == null)
                    return "";

                return this.m_pc.ToString();
            }
            set
            {
                this.m_pc = new DigitalPlatform.Text.SectionPropertyString.PropertyCollection(
                    "this", value, DelimiterFormat.Mix);

                if (this.Visible == false)
                    return;

                // ���ֵ�list��
                SetCheckList(this.m_pc);

            }
        }

        void SetCheckList(DigitalPlatform.Text.SectionPropertyString.PropertyCollection pc)
        {
            PrepareTag();

            string strCurCategory = this.SelectedCategory;
            if (String.IsNullOrEmpty(strCurCategory))
                strCurCategory = "*";

            for (int i = 0; i < m_pc.Count; i++)
            {
                Section section = m_pc[i];

                // ��������
                string strSectionName = section.Name;
                string strValues = section.ToString();

                if (String.IsNullOrEmpty(strSectionName) == true)
                    strSectionName = "this";

                // �����Ƿ��ڿ���ʾ��Χ
                if (strCurCategory == "*"
                    || String.Compare(strCurCategory, strSectionName, true) == 0)
                {
                }
                else
                    continue;

                for (int j = 0; j < section.Count; j++)
                {
                    Item propertyItem = section[j];

                    ListViewItem item = LocateListItem(strSectionName, propertyItem.Value);
                    if (item == null)
                    {
                        // δ����ı���ֵ
                    }
                    else
                    {
                        if (item.Checked != true)
                        {
                            item.BackColor = CheckedBackColor;
                            item.Checked = true;
                            item.Tag = null;    // ��ǣ���ʾ���δ�������
                        }
                        else
                            item.Tag = null;    // ���ö�
                    }

                }
            }

            // uncheck����û��on��
            for (int i = 0; i < this.listView_property.Items.Count; i++)
            {
                ListViewItem item = this.listView_property.Items[i];
                if (item.Tag == null)
                {

                }
                else {
                    if ((bool)item.Tag == false && item.Checked != false)
                    {
                        item.BackColor = UncheckedBackColor;
                        item.Checked = false;
                    }

                    if ((bool)item.Tag == true && item.Checked != true)
                    {
                        item.BackColor = CheckedBackColor;
                        item.Checked = true;
                    }

                }

           }
        }

        void PrepareTag()
        {
            for (int i = 0; i < this.listView_property.Items.Count; i++)
            {
                ListViewItem item = this.listView_property.Items[i];
                item.Tag = false;
            }
        }


        // ����Ŀ¼����ֵ��λlistview item
        ListViewItem LocateListItem(string strCategory, string strValue)
        {
            for (int i = 0; i < this.listView_property.Items.Count; i++)
            {
                ListViewItem item = this.listView_property.Items[i];
                if (String.Compare(strCategory, item.SubItems[0].Text, true) != 0)
                    continue;
                if (String.Compare(strValue, item.SubItems[1].Text, true) == 0)
                    return item;
            }

            return null;
        }

        public string SelectedCategory
        {
            get
            {
                if (String.IsNullOrEmpty(this.comboBox_category.Text) == true)
                    return "";

                int nRet = this.comboBox_category.Text.IndexOf(":");
                if (nRet == -1)
                    return this.comboBox_category.Text;

                string strName = this.comboBox_category.Text.Substring(0, nRet).Trim();
                if (String.IsNullOrEmpty(strName) == true)
                    return "";

                return strName;
            }
            set
            {
                this.comboBox_category.Text = value;

                // ����ð�ŵ�ȫ����
                for (int i = 0; i < this.comboBox_category.Items.Count; i++)
                {
                    string strLine = (string)this.comboBox_category.Items[i];
                    string strCurName = "";
                    int nRet = strLine.IndexOf(":");
                    if (nRet == -1)
                        strCurName = strLine.Trim();
                    else
                        strCurName = strLine.Substring(0, nRet).Trim();

                    if (String.IsNullOrEmpty(strCurName) == true)
                        continue;

                    if (value == strCurName)
                    {
                        this.comboBox_category.Text = strLine;
                        break;
                    }
                }
            }
        }

        private void CategoryPropertyDlg_Load(object sender, EventArgs e)
        {
            string strError = "";

            // װ�������ļ�
        // return:
        //      -1  error
        //      0   CfgFileName��δָ��
        //      1   �ɹ�
            int nRet = LoadCfgXml(out strError);
            if (nRet == 0)
                return;
            if (nRet == -1)
                goto ERROR1;

            nRet = FillCategoryList(this.Lang,
                out strError);
            if (nRet == -1)
                goto ERROR1;

            SelectFirstCategory();

            nRet = FillPropertyList(this.comboBox_category.Text,
                this.Lang,
                out strError);

            if (nRet == -1)
                goto ERROR1;

            // SetCheckList(this.m_pc);

            return;
        ERROR1:
            MessageBox.Show(this, strError);
            return;
        }



        private void button_checkAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.listView_property.Items.Count; i++)
            {
                this.listView_property.Items[i].Checked = true;
            }
        }

        private void button_uncheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.listView_property.Items.Count; i++)
            {
                this.listView_property.Items[i].Checked = false;
            }
        }

        int FillCategoryList(string strLang,
            out string strError)
        {
            strError = "";

            Debug.Assert(this.CfgDom != null, "�����ļ�CfgDom��δ��ʼ��...");


            string strXPath = "";

            this.comboBox_category.Items.Clear();



            strXPath = "//category";
            XmlNodeList nodes = this.CfgDom.DocumentElement.SelectNodes(strXPath);

            bool bFoundWildchar = false;
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                string strName = DomUtil.GetAttr(node, "name");
                string strComment = GetComment(node, strLang);

                if (strName == "*")
                    bFoundWildchar = true;

                string strText = strName;
                if (String.IsNullOrEmpty(strComment) == false)
                    strText += ": " + strComment;

                this.comboBox_category.Items.Add(strText);
            }

            if (bFoundWildchar == false)
            {
                // ��ʾȫ����Ŀ��ȱʡ����
                this.comboBox_category.Items.Insert(0, "*: ȫ������");
            }


            return 0;
        }

        // ѡ��Combo�б��е�һ������
        int SelectFirstCategory()
        {
            if (this.comboBox_category.Items.Count == 0)
                return 0;
            this.comboBox_category.Text = (string)this.comboBox_category.Items[0];
            return 1;
        }

        // ���listview
        // parameters:
        //      strCategory ��Ŀ���ơ����=="*"����ʾȫ����Ŀ
        int FillPropertyList(string strCategory,
            string strLang,
            out string strError)
        {
            strError = "";
            string strXPath = "";

            Debug.Assert(this.CfgDom != null, "�����ļ�CfgDom��δ��ʼ��...");

            this.listView_property.Items.Clear();

            // ȥ��ð���Ժ�Ĳ���
            if (String.IsNullOrEmpty(strCategory) == false)
            {
                int nRet = strCategory.IndexOf(":");
                if (nRet != -1)
                    strCategory = strCategory.Substring(0, nRet).Trim();
            }

            if (strCategory == "*" || String.IsNullOrEmpty(strCategory))
                strXPath = "//category/property";
            else
                strXPath = "//category[@name='" + strCategory + "']/property";
            XmlNodeList nodes = this.CfgDom.DocumentElement.SelectNodes(strXPath);

            if (nodes.Count == 0)
                return 0;

            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                string strCategoryName = DomUtil.GetAttr(node.ParentNode, "name");
                string strValue = DomUtil.GetAttr(node, "name");

                string strComment = GetComment(node, strLang);

                ListViewItem item = new ListViewItem(strCategoryName, 0);

                item.SubItems.Add(strValue);
                item.SubItems.Add(strComment);

                ItemState state = this.m_pc.GetItemState(strCategoryName,
                    strValue);
                if (state == ItemState.On)
                    item.Checked = true;

                item.Tag = false;   // Ϊ�´���׼��

                DisableEvent ++;
                this.listView_property.Items.Add(item);
                DisableEvent --;
            }

            return 0;
        }

        // ���һ���ڵ�(�й����Ե�)��ע��ֵ
        private string GetComment(XmlNode node, string strLang)
        {
            XmlNode nodeComment = node.SelectSingleNode("comment[@lang='"+strLang+"']");
            if (nodeComment == null)
            {
                nodeComment = node.SelectSingleNode("comment");
            }

            if (nodeComment == null)
                return "";

            return DomUtil.GetNodeText(nodeComment);
        }

        // װ�������ļ�
        // return:
        //      -1  error
        //      0   CfgFileName��δָ��
        //      1   �ɹ�
        int LoadCfgXml(out string strError)
        {
            strError = "";

            if (CfgFileName == "")
                return 0;

            this.CfgDom = new XmlDocument();

            try
            {
                this.CfgDom.Load(CfgFileName);
            }
            catch (Exception ex)
            {
                strError = "�����ļ� '" + this.CfgFileName + "' װ����XmlDocumentʱ��������: " + ex.Message;
                return -1;
            }

            return 1;
        }

        private void comboBox_category_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strError = "";

            int nRet = FillPropertyList(this.comboBox_category.Text,
                this.Lang,
                out strError);

            if (nRet == -1)
                goto ERROR1;

            SetTextBoxString();

            return;
        ERROR1:
            MessageBox.Show(this, strError);
            return;
        }

        void SetTextBoxString()
        {
            this.DisableEvent++;

            // ������ʾ��textbox�ַ���
            string strCurCategory = this.SelectedCategory;
            if (String.IsNullOrEmpty(strCurCategory))
                strCurCategory = "*";

            string strCurResult = "";

            if (strCurCategory == "*")
            {
                this.label_property.Text = "������Ŀֵ֮(&V):";

            }
            else
            {
                this.label_property.Text = "��Ŀ '" + strCurCategory + "' ֵ֮(&V):";

                // strCurResult = this.m_pc[this.SelectedCategory].Value;

            }

            // ��Ե�ǰ��ѡ����Ŀ���ַ������
            strCurResult = this.m_pc.ToString(DelimiterFormat.CrLf,
                strCurCategory);

            if (this.textBox_property.Text != strCurResult) // if �ɱ�����Ϣ������ѭ��
                this.textBox_property.Text = strCurResult;  // ��ǰѡ����Ŀ���ַ���

            this.DisableEvent --;
        }

        private void textBox_property_TextChanged(object sender, EventArgs e)
        {
            if (this.DisableEvent > 0)
                return;

            REDO:
            string strCurCategory = this.SelectedCategory;
            if (String.IsNullOrEmpty(strCurCategory))
                strCurCategory = "*";

            if (strCurCategory == "*")
            {
                this.m_pc = new DigitalPlatform.Text.SectionPropertyString.PropertyCollection(
                    "this", this.textBox_property.Text, DelimiterFormat.Mix);
            }
            else
            {
                // �۲��Ƿ����������ϵ�ð��
                int nRet = this.textBox_property.Text.IndexOf(":");
                if (nRet != -1)
                {
                    nRet = this.textBox_property.Text.IndexOf(":", nRet + 1);
                    if (nRet != -1)
                    {
                        // ���л�Ϊ*��Ŀ��Ȼ�����ִ��changetext����
                        string strSaveText = this.textBox_property.Text;
                        this.SelectedCategory = "*";
                        this.textBox_property.Text = strSaveText;
                        goto REDO;
                    }
                }
                m_pc.NewSection("this", 
                    strCurCategory, 
                    this.textBox_property.Text);
            }

            SetCheckList(this.m_pc);
        }

        private void listView_property_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked == true)
            {
                e.Item.BackColor = CheckedBackColor;
            }
            else
            {
                e.Item.BackColor = UncheckedBackColor;
            }

            if (DisableEvent > 0)
                return;
            // �޸��ַ���
            ModiPropertyString(e.Item.Text, e.Item.SubItems[1].Text, e.Item.Checked);
        }

        // �޸������ַ���
        private void ModiPropertyString(string strCategory, string strValue, bool bAdd)
        {
            if (this.m_pc == null)
                return;
            // bool bFoundCategory = false;
            bool bFoundValue = false;

            for (int i = 0; i < this.m_pc.Count; i++)
            {
                Section section = this.m_pc[i];

                if (section.Count == 0)
                    continue;

                if (String.Compare(section.Name, strCategory, true) == 0)
                {
                    // bFoundCategory = true;

                    for (int j = 0; j < section.Count; j++)
                    {
                        Item item = section[j];

                        string strCurValue = item.Value;

                        if (String.Compare(strCurValue, strValue, true) == 0)
                        {
                            bFoundValue = true;

                            if (bAdd == true)   // Ҫ���룬�����Ѿ������� :-(
                                return;
                            if (bAdd == false)
                            {
                                // ȥ��λ��j�ļ���
                                section.RemoveAt(j);
                                break;
                            }
                        }

                    }

                }
            }

            if (bFoundValue == false && bAdd == true)
            {
                Item item = m_pc.NewItem(strCategory, strValue);
                Debug.Assert(item != null, "NewItem()ʧ��");
            }

            /*
            string strCurCategory = this.SelectedCategory;
            if (String.IsNullOrEmpty(strCurCategory))
                strCurCategory = "*";

            // ��Ե�ǰ��ѡ����Ŀ���ַ������
            string strCurResult = this.m_pc.ToString(DelimiterFormat.CrLf,
                strCurCategory);

            if (this.textBox_property.Text != strCurResult) // if �ɱ�����Ϣ������ѭ��
                this.textBox_property.Text = strCurResult;  // ��ǰѡ����Ŀ���ַ���
             */
            SetTextBoxString();

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

 
}