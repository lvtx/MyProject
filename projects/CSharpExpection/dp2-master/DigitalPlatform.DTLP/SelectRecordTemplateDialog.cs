using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DigitalPlatform.GUI;
using DigitalPlatform.Marc;

namespace DigitalPlatform.DTLP
{
    public partial class SelectRecordTemplateDialog : Form
    {
        bool m_bChanged = false;
        public bool LoadMode = true;    // �Ƿ�Ϊװ��״̬��==false��ʾ���桢�޸�״̬

        public string SelectedRecordMarc = "";  // װ��ģʽ�£�ѡ�еļ�¼��MARC���ڸ�ʽ�ַ������޸�ģʽ�£��Ի����ǰ��Ҫ���ã��ṩ�µ�MARC��¼����

        public string Content = ""; // �����ļ�����

        public SelectRecordTemplateDialog()
        {
            InitializeComponent();
        }

        private void SelectRecordTemplateDialog_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Content) == false)
                FillList(this.LoadMode); // �޸�ģʽʱ���������Զ�ѡ���������û�����ʶ�����˵�һ��
            

            if (this.LoadMode == true)
            {
                this.textBox_name.ReadOnly = true;
                this.listView1.MultiSelect = false;
                this.Text = "��ѡ���¼�¼ģ��";
            }
            else
            {
                this.textBox_name.ReadOnly = false;
                this.listView1.MultiSelect = true;
                if (this.Text == "��ѡ���¼�¼ģ��")
                    this.Text = "��ָ��Ҫ�޸ĵļ�¼ģ��";
            }
        }

        private void SelectRecordTemplateDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.LoadMode == false)
            {
                if (this.DialogResult != DialogResult.OK
                    && m_bChanged == true)
                {
                    DialogResult result = MessageBox.Show(this,
                        "ȷʵҪ������ǰ������ȫ���޸�ô?\r\n\r\n(��)�����޸� (��)���رմ���\r\n\r\n(ע: ģ����Ϊ�յ�������Կ��԰�\"ȷ��\"��ť�����������޸ġ�)",
                        "SelectRecordTemplateDialog",
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
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (this.LoadMode == true)
            {
                if (this.listView1.SelectedItems.Count == 0)
                {
                    MessageBox.Show(this, "��δѡ��Ҫװ�ص�ģ������");
                    return;
                }

                ListViewItem item = this.listView1.SelectedItems[0];
                this.SelectedRecordMarc = GetMarc((string)item.Tag);
            }
            else
            {
                // �۲�textbox_name�е����֡�
                // ������Ѵ��ڵ�ĳ������ͬ��������Ҫ���ṩ��MARC��¼�滻ģ���¼
                // ����������κ��Ѵ��ڵ���������ͬ������Ҫ��ĩβ����һ���µ�ģ������
                // return:
                //      0   not changed
                //      1   changed
                ChangeContent();

                if (this.Changed == true)
                {
                    // �ϳ�content
                    this.Content = "";
                    for (int i = 0; i < this.listView1.Items.Count; i++)
                    {
                        ListViewItem item = this.listView1.Items[i];
                        string strName = item.Text;
                        string strComment = ListViewUtil.GetItemText(item, 1);

                        if (String.IsNullOrEmpty(strComment) == false)
                            strName += "|" + strComment;

                        this.Content += strName + "\r\n";
                        this.Content += (string)item.Tag;
                    }

                    // �Ի��򷵻غ�this.Content���и��¹�������ģ���ļ�����
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // return:
        //      0   not changed
        //      1   changed
        int ChangeContent()
        {
            if (String.IsNullOrEmpty(this.SelectedRecordMarc) == true)
                return 0;

            if (String.IsNullOrEmpty(this.textBox_name.Text) == true)
                return 0;

            // �۲�textbox_name�е����֡�
            // ������Ѵ��ڵ�ĳ������ͬ��������Ҫ���ṩ��MARC��¼�滻ģ���¼
            // ����������κ��Ѵ��ڵ���������ͬ������Ҫ��ĩβ����һ���µ�ģ������
            for (int i = 0; i < this.listView1.Items.Count; i++)
            {
                ListViewItem item = this.listView1.Items[i];

                if (this.textBox_name.Text == item.Text)
                {
                    item.Tag = GetWorksheet(this.SelectedRecordMarc);
                    this.Changed = true;
                    return 1;
                }
            }

            // û���ҵ�������ĩβ������һ��
            ListViewItem newitem = new ListViewItem(this.textBox_name.Text, 0);
            newitem.Tag = GetWorksheet(this.SelectedRecordMarc);
            this.listView1.Items.Add(newitem);

            this.Changed = true;
            return 1;
        }

        public bool Changed
        {
            get
            {
                return this.m_bChanged;
            }
            set
            {
                this.m_bChanged = value;
            }
        }

        void FillList(bool bAutoSelect)
        {
            listView1.Items.Clear();
            // listView1_SelectedIndexChanged(null, null);

            List<TemplateItem> items = GetItems();


            for (int i = 0; i < items.Count; i++)
            {
                TemplateItem template_item = items[i];

                // ����name��comment'
                string strName = "";
                string strComment = "";
                int nRet = template_item.Title.IndexOf("|");
                if (nRet != -1)
                {
                    strName = template_item.Title.Substring(0, nRet);
                    strComment = template_item.Title.Substring(nRet + 1);
                }
                else
                {
                    strName = template_item.Title;
                }


                ListViewItem listview_item = new ListViewItem(strName, 0);

                listview_item.SubItems.Add(strComment);
                listview_item.Tag = template_item.Content;

                this.listView1.Items.Add(listview_item);
            }

            // ѡ���һ��
            if (bAutoSelect == true)
            {
                if (listView1.Items.Count != 0)
                    listView1.Items[0].Selected = true;
            }

        }

        // ������ȫ������
        List<TemplateItem> GetItems()
        {
            List<TemplateItem> items = new List<TemplateItem>();
            int nRet = 0;

            if (String.IsNullOrEmpty(this.Content) == true)
                return items;

            int nOffs = 0;
            string strLine = "";
            bool bEnd = false;
            TemplateItem item = null;
            for (int i=0; ;i++)
            {
                if (nOffs >= this.Content.Length)
                    break;

                nRet = this.Content.IndexOf("\r\n", nOffs);
                if (nRet == -1)
                {
                    strLine = this.Content.Substring(nOffs);
                    nOffs += strLine.Length;
                }
                else
                {
                    strLine = this.Content.Substring(nOffs, nRet - nOffs);
                    nOffs = nRet + 2;
                }

                if (i == 0)
                {
                    // ��ʼ��һ��
                    item = new TemplateItem();
                    item.Title = strLine;
                    // items.Add(strLine);
                }
                else if (bEnd == true)
                {
                    // ǰһ�������б�
                    items.Add(item);

                    // ��ʼ�µ�һ��
                    item = new TemplateItem();
                    item.Title = strLine;
                    // items.Add(strLine);
                    bEnd = false;
                }
                else
                {
                    item.Content += strLine + "\r\n";
                }

                if (strLine == "***")
                    bEnd = true;

            }

            if (item != null)
                items.Add(item);

            return items;
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = null;

            bool bSelected = listView1.SelectedItems.Count > 0;

            //
            menuItem = new MenuItem("�鿴(&C)");
            menuItem.Click += new System.EventHandler(this.menu_viewContent);
            if (bSelected == false)
            {
                menuItem.Enabled = false;
            }
            contextMenu.MenuItems.Add(menuItem);


            //
            menuItem = new MenuItem("�޸�(&M)");
            menuItem.Click += new System.EventHandler(this.menu_Modify);
            if (bSelected == false)
            {
                menuItem.Enabled = false;
            }
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);


            menuItem = new MenuItem("ɾ��(&D)");
            menuItem.Click += new System.EventHandler(this.menu_deleteRecord);
            if (bSelected == false)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);

            contextMenu.Show(listView1, new Point(e.X, e.Y));	
        }

        void menu_viewContent(object sender, System.EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�鿴��ģ���¼����...");
                return;
            }

            string strText = "";
            for (int i = 0; i < this.listView1.SelectedItems.Count; i++)
            {
                ListViewItem item = this.listView1.SelectedItems[i];
                string strTitle = item.Text;
                string strContent = (string)item.Tag;

                strText += "[" + strTitle + "\r\n";
                strText += strContent + "]";
                strText += "\r\n";
            }

            MessageBox.Show(this, strText);
        }


        // �޸����ֺ�ע��
        void menu_Modify(object sender, System.EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ�����޸ĵ�ģ���¼����...");
                return;
            }
            TemplateRecordDialog dlg = new TemplateRecordDialog();

            // string strOldName = ListViewUtil.GetItemText(listView1.SelectedItems[0], 0);

            dlg.TemplateName = ListViewUtil.GetItemText(listView1.SelectedItems[0], 0);
            dlg.TemplateComment = ListViewUtil.GetItemText(listView1.SelectedItems[0], 1);

            dlg.ShowDialog(this);
            if (dlg.DialogResult != DialogResult.OK)
                return;

            ListViewUtil.ChangeItemText(this.listView1.SelectedItems[0], 0, dlg.TemplateName);
            ListViewUtil.ChangeItemText(this.listView1.SelectedItems[0], 1, dlg.TemplateComment);

            this.Changed = true;
        }

        void menu_deleteRecord(object sender, System.EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ����ɾ����ģ���¼����...");
                return;
            }

            //string strError = "";
            //int nRet = 0;

            DialogResult result = MessageBox.Show(this,
                "ȷʵҪɾ����ѡ��� " + this.listView1.SelectedItems.Count.ToString() + " ��ģ���¼?",
                "SelectRecordTemplateDialog",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
                return;


            for (int i = this.listView1.SelectedIndices.Count - 1; i >= 0 ; i--)
            {
                int index = this.listView1.SelectedIndices[i];

                this.listView1.Items.RemoveAt(index);
            }

            this.Changed = true;

        }

        // �ѻ��ڸ�ʽת��Ϊ��������ʽ
        // ����������
        static string GetWorksheet(string strMarc)
        {
            if (String.IsNullOrEmpty(strMarc) == true)
                return "012345678901234567890123\r\n***\r\n";
            if (strMarc.Length < 24)
                strMarc = strMarc.PadRight(24, ' ');

            string strRecord = strMarc;
            
            // Ϊͷ����ĩβ����һ���ֶν�����
            if (strRecord.Length > 24)
                strRecord = strRecord.Insert(24, new string(MarcUtil.FLDEND, 1));

            // ���������һ���ַ�����FLDEND�������һ��
            if (strRecord[strRecord.Length - 1] != MarcUtil.FLDEND)
            {
                strRecord += new string(MarcUtil.FLDEND, 1);
            }

            // ���ֶν������滻Ϊ�س�����
            strRecord = strRecord.Replace(new string(MarcUtil.FLDEND, 1), "\r\n");

            // �����ֶη����滻Ϊ'@'
            strRecord = strRecord.Replace(new string(MarcUtil.SUBFLD, 1), "@");


            strRecord += "***\r\n";

            return strRecord;
        }

        // �ѹ�������ʽ������ת��Ϊ���ڸ�ʽ
        static string GetMarc(string strContent)
        {
            string strRecord = strContent.Replace("\r\n***\r\n",
                new string(MarcUtil.FLDEND, 1));

            strRecord = strRecord.Replace("\r\n", new string(MarcUtil.FLDEND, 1));

            if (strRecord.Length >= 25)
            {
            // ��Ѱ��һ���ֶν����������ܲ���24λ��
            int nRet = strRecord.IndexOf((char)MarcUtil.FLDEND);
            if (nRet != -1)
            {
                strRecord = strRecord.Remove(nRet, 1);

                if (nRet > 24)
                    strRecord = strRecord.Remove(24, nRet - 24);
                else if (nRet < 24)
                    strRecord = strRecord.Insert(nRet , new string(' ', 24 - nRet));

                if (strRecord[23] == '\\')
                    strRecord = strRecord.Remove(23, 1).Insert(23, " ");
            }

                // strRecord = strRecord.Remove(24, 1);	// ɾ��ͷ�������һ��FLDEND
            }

            // ���������һ���ַ�����FLDEND�������һ��
            if (strRecord[strRecord.Length - 1] != MarcUtil.FLDEND)
            {
                strRecord += new string(MarcUtil.FLDEND, 1);
            }

            return strRecord.Replace('@', MarcUtil.SUBFLD);
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            this.button_OK_Click(null, null);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                this.textBox_name.Text = "";
            else
                this.textBox_name.Text = this.listView1.SelectedItems[0].Text;
        }



    }

    // һ��ģ������
    class TemplateItem
    {
        public string Title = "";   // ����
        public string Content = ""; // ���ݡ�ԭʼ�Ĺ�������ʽ
    }
}