using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using System.Xml;

using DigitalPlatform.Xml;
using DigitalPlatform.GUI;

namespace DigitalPlatform.DTLP
{
    /// <summary>
    /// �༭һ�����ط����ĶԻ���
    /// </summary>
    public partial class ProjectDialog : Form
    {
        public bool CreateMode = false; // �Ƿ��ڴ���ģʽ��==false����ʾ���޸�ģʽ
        public DupCfgDialog DupCfgDialog = null;

        public XmlDocument dom = null;

        XmlNode m_nodeProject = null;

        bool m_bChanged = false;

        // ������˵�������Ƿ����˸ı䣿
        public bool Changed
        {
            get
            {
                return this.m_bChanged;
            }
            set
            {
                this.m_bChanged = value;

                if (value == true)
                    this.button_OK.Enabled = true;
                else
                    this.button_OK.Enabled = false;
            }
        }

        public string ProjectName
        {
            get
            {
                return this.textBox_projectName.Text;
            }
            set
            {
                this.textBox_projectName.Text = value;
            }
        }

        public string ProjectComment
        {
            get
            {
                return this.textBox_comment.Text;
            }
            set
            {
                this.textBox_comment.Text = value;
            }
        }

        string m_strCurDatabaseName = "";

        bool m_bAccessPointsChanged = false;

        // ������listview�е������Ƿ����˸ı䣿
        public bool AccessPointsChanged
        {
            get
            {
                return this.m_bAccessPointsChanged;
            }
            set
            {
                this.m_bAccessPointsChanged = value;

                if (value == true)
                    this.Changed = true;
            }
        }

        public ProjectDialog()
        {
            InitializeComponent();
        }

        private void ProjectDialog_Load(object sender, EventArgs e)
        {
            if (this.CreateMode == false)
            {
                // �Ȼ��<projectԪ��>
                if (String.IsNullOrEmpty(this.ProjectName) == false
                    && this.dom != null)
                {
                    this.m_nodeProject = this.dom.DocumentElement.SelectSingleNode("//project[@name='" + this.ProjectName + "']");
                    if (this.m_nodeProject == null)
                    {
                        MessageBox.Show(this, "DOM�в�������name����ֵΪ '" + this.ProjectName + "' ��<project>Ԫ��");
                    }
                }

                FillDatabaseList();
            }
            else
            {
                // ����<projectԪ��>
                if (this.dom != null)
                {
                    this.m_nodeProject = this.dom.CreateElement("project");
                    this.dom.DocumentElement.AppendChild(this.m_nodeProject);
                    DomUtil.SetAttr(this.m_nodeProject, "name", this.ProjectName);
                    DomUtil.SetAttr(this.m_nodeProject, "comment", this.ProjectComment);
                }
            }

            this.Changed = false;
        }

        private void ProjectDialog_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void ProjectDialog_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (this.AccessPointsChanged == true)
            {
                PutAccessPoints();
            }

            if (this.Changed == false)
            {
                Debug.Assert(false, "��OK��ť���԰��µ�ʱ��, this.Changed������Ϊfalse");
            }

            // ���أ���projectname�Ƿ������<project>Ԫ�ص�name����ֵ��ͬ
            XmlNodeList nodes = this.dom.DocumentElement.SelectNodes("//project[@name='" + this.textBox_projectName.Text + "']");
            int nCount = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];
                string strName = DomUtil.GetAttr(node, "name");

                if (node == this.m_nodeProject)
                    continue;

                nCount++;
            }

            if (nCount > 0)
            {
                MessageBox.Show(this, "���ֵ�ǰ������ '" +this.textBox_projectName+ "' ������ "+nCount.ToString()+" ��<project>Ԫ�ؾ�����ͬ��name����ֵ�������޸ĵ�ǰ���������֣��������(��)���ء�");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        ListViewItem FindDatabaseItem(string strName)
        {
            for (int i = 0; i < this.listView_databases.Items.Count; i++)
            {
                if (strName == this.listView_databases.Items[i].Text)
                    return this.listView_databases.Items[i];
            }

            return null;
        }

        private void button_newDatabase_Click(object sender, EventArgs e)
        {
            TargetDatabaseDialog dlg = new TargetDatabaseDialog();

            dlg.DupCfgDialog = this.DupCfgDialog;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            string strError = "";

            // ����
            ListViewItem exist = FindDatabaseItem(dlg.DatabaseName);
            if (exist != null)
            {
                exist.Selected = true;
                exist.EnsureVisible();

                strError = "�Ѿ�������Ϊ '" + dlg.DatabaseName + "' ��Ŀ�����������´������";
                goto ERROR1;
            }

            Debug.Assert(this.m_nodeProject != null, "");

            // �ҵ�<project>Ԫ��
            XmlNode nodeProject = this.m_nodeProject;
            if (nodeProject == null)
            {
                Debug.Assert(false, "");
                strError = "m_nodeProject��ԱΪ��";
                goto ERROR1;
            }

            // �����µ�<database>Ԫ��
            XmlNode nodeDatabase = this.dom.CreateElement("database");
            nodeProject.AppendChild(nodeDatabase);

            DomUtil.SetAttr(nodeDatabase, "name", dlg.DatabaseName);
            DomUtil.SetAttr(nodeDatabase, "threshold", dlg.Threshold);

            // ���ֶ�ListViewItem������
            ListViewItem item = new ListViewItem(dlg.DatabaseName, 0);
            item.SubItems.Add(dlg.Threshold);
            this.listView_databases.Items.Add(item);

            item.Selected = true;   // ѡ�иղ����listviewitem����

            this.Changed = true;
            return;
        ERROR1:
            MessageBox.Show(this, strError);

        }

        private void listView_databases_DoubleClick(object sender, EventArgs e)
        {
            button_modifyDatabase_Click(this, e);
        }

        private void button_modifyDatabase_Click(object sender, EventArgs e)
        {
            if (this.listView_databases.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�޸ĵ�Ŀ�����ݿ�����");
                return;
            }

            ListViewItem item = this.listView_databases.SelectedItems[0];

            TargetDatabaseDialog dlg = new TargetDatabaseDialog();

            dlg.DupCfgDialog = this.DupCfgDialog;
            dlg.DatabaseName = item.Text;
            dlg.Threshold = ListViewUtil.GetItemText(item, 1);

            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            string strError = "";

            // ����
            ListViewItem exist = FindDatabaseItem(dlg.DatabaseName);
            if (exist != null && exist != item)
            {
                exist.Selected = true;
                exist.EnsureVisible();

                strError = "�Ѿ�������Ϊ '" + dlg.DatabaseName + "' ��Ŀ�����������ղŶ�������޸ġ�";
                goto ERROR1;
            }

            Debug.Assert(this.m_nodeProject != null, "");

            // �ҵ���Ӧ��<database>Ԫ��
            XmlNode nodeDatabase = this.m_nodeProject.SelectSingleNode("database[@name='" + item.Text + "']");
            if (nodeDatabase == null)
            {
                strError = "��Ϊ '" + item.Text + "' ��<database>Ԫ�ز�������";
                goto ERROR1;
            }

            // ���ֶ�DOM���޸�
            DomUtil.SetAttr(nodeDatabase, "name", dlg.DatabaseName);
            DomUtil.SetAttr(nodeDatabase, "threshold", dlg.Threshold);

            // ���ֶ�ListViewItem���޸�
            item.Text = dlg.DatabaseName;
            ListViewUtil.ChangeItemText(item, 1, dlg.Threshold);

            this.Changed = true;
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        private void button_deleteDatabase_Click(object sender, EventArgs e)
        {
            if (this.listView_databases.SelectedIndices.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫɾ����Ŀ�����ݿ�����");
                return;
            }

            DialogResult result = MessageBox.Show(this,
                "ȷʵҪɾ����ѡ���� " + this.listView_databases.SelectedIndices.Count.ToString() + " ��Ŀ�����ݿ�����?",
                "ProjectDialog",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result == DialogResult.No)
                return;

            string strError = "";

            for (int i = this.listView_databases.SelectedIndices.Count - 1; i >= 0; i--)
            {
                int index = this.listView_databases.SelectedIndices[i];
                ListViewItem item = this.listView_databases.Items[index];

                Debug.Assert(this.m_nodeProject != null, "");

                // �ҵ���Ӧ��<database>Ԫ��
                XmlNode nodeDatabase = this.m_nodeProject.SelectSingleNode("database[@name='" + item.Text + "']");
                if (nodeDatabase == null)
                {
                    strError = "��Ϊ '" + item.Text + "' ��<database>Ԫ�ز�������";
                    goto ERROR1;
                }
                // ɾ��XML�ڵ�
                nodeDatabase.ParentNode.RemoveChild(nodeDatabase);

                // ���ֶ�ListViewItem���޸�
                this.listView_databases.Items.RemoveAt(index);
            }

            this.Changed = true;
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // �½�һ������������
        private void button_newAccessPoint_Click(object sender, EventArgs e)
        {
            AccessPointDialog dlg = new AccessPointDialog();

            // TODO: �Ƿ���Ҫ�ѵ�ǰ�Ѿ�ѡ��Ķ������ο�����?

            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            ListViewItem new_item = new ListViewItem(dlg.FromName, 0);
            new_item.SubItems.Add(dlg.Weight);
            new_item.SubItems.Add(dlg.SearchStyle);
            this.listView_accessPoints.Items.Add(new_item);

            // TODO: �Ƿ�����Ҫ���ɲ����ڵ�ǰ��ѡ�������ǰ��

            this.AccessPointsChanged = true;
        }

        // �޸�һ������������
        private void button_modifyAccessPoint_Click(object sender, EventArgs e)
        {
            if (this.listView_accessPoints.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�޸ĵļ���������");
                return;
            }

            ListViewItem item = this.listView_accessPoints.SelectedItems[0];

            AccessPointDialog dlg = new AccessPointDialog();

            dlg.FromName = item.Text;
            dlg.Weight = ListViewUtil.GetItemText(item, 1);
            dlg.SearchStyle = ListViewUtil.GetItemText(item, 2);

            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            item.Text = dlg.FromName;
            ListViewUtil.ChangeItemText(item, 1, dlg.Weight);
            ListViewUtil.ChangeItemText(item, 2, dlg.SearchStyle);

            this.AccessPointsChanged = true;
        }

        // ɾ��һ������������
        private void button_deleteAccessPoint_Click(object sender, EventArgs e)
        {
            if (this.listView_accessPoints.SelectedIndices.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫɾ���ļ���������");
                return;
            }

            DialogResult result = MessageBox.Show(this,
                "ȷʵҪɾ����ѡ���� " + this.listView_accessPoints.SelectedIndices.Count.ToString() + " ������������?",
                    "ProjectDialog",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
            if (result == DialogResult.No)
                return;

            for (int i = this.listView_accessPoints.SelectedIndices.Count - 1; i >= 0; i--)
            {
                int index = this.listView_accessPoints.SelectedIndices[i];

                this.listView_accessPoints.Items.RemoveAt(index);
            }

            this.AccessPointsChanged = true;
        }

        // ���(Ŀ��)���ݿ��б�
        // throw:
        //      Exception
        void FillDatabaseList()
        {
            this.listView_databases.Items.Clear();

            if (this.dom == null)
                return;

            if (String.IsNullOrEmpty(this.ProjectName) == true)
                return;

            Debug.Assert(this.m_nodeProject != null, "");

            XmlNodeList database_nodes = this.m_nodeProject.SelectNodes("database");
            for (int i = 0; i < database_nodes.Count; i++)
            {
                XmlNode node = database_nodes[i];

                string strName = DomUtil.GetAttr(node, "name");
                string strThreshold = DomUtil.GetAttr(node, "threshold");

                ListViewItem item = new ListViewItem(strName, 0);
                item.SubItems.Add(strThreshold);

                this.listView_databases.Items.Add(item);
            }
        }

        private void listView_databases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_databases.SelectedItems.Count == 0)
            {
                this.button_deleteDatabase.Enabled = false;
                this.button_modifyDatabase.Enabled = false;

                // ������listviewû��ѡ�����ʱ������Ҳ�޷���
                this.button_newAccessPoint.Enabled = false;
                this.button_modifyAccessPoint.Enabled = false;
                this.button_deleteAccessPoint.Enabled = false;
            }
            else
            {
                this.button_deleteDatabase.Enabled = true;
                this.button_modifyDatabase.Enabled = true;

                this.button_newAccessPoint.Enabled = true;

                // ����������ť����ȥ��
            }

            FillAccessPointList();
        }

        // ���ݵ�ǰѡ�е����ݿ��������������б�
        void FillAccessPointList()
        {
            // �����ǰ�Ƿ����޸ģ�
            if (this.AccessPointsChanged == true)
            {
                PutAccessPoints();
            }

            this.listView_accessPoints.Items.Clear();

            if (this.listView_databases.SelectedItems.Count == 0)
                return;

            string strDatabaseName = this.listView_databases.SelectedItems[0].Text;

            XmlNodeList nodes = this.dom.DocumentElement.SelectNodes("//project[@name='" + this.ProjectName + "']/database[@name='" + strDatabaseName + "']/accessPoint");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                string strName = DomUtil.GetAttr(node, "name");
                string strWeight = DomUtil.GetAttr(node, "weight");
                string strSearchStyle = DomUtil.GetAttr(node, "searchStyle");

                ListViewItem item = new ListViewItem(strName, 0);
                item.SubItems.Add(strWeight);
                item.SubItems.Add(strSearchStyle);

                this.listView_accessPoints.Items.Add(item);
            }

            this.m_strCurDatabaseName = strDatabaseName;

            listView_accessPoints_SelectedIndexChanged(this, null);
        }

        // ����������Ϣ���޸ģ����ֵ���Ӧ��<database>Ԫ����
        void PutAccessPoints()
        {
            if (this.AccessPointsChanged == false)
                return;

            if (this.m_strCurDatabaseName == "")
            {
                Debug.Assert(false, "");
                return;
            }

            // �ҵ���Ӧ��<database>Ԫ��
            XmlNode nodeDatabase = this.dom.DocumentElement.SelectSingleNode("//project[@name='" + this.ProjectName + "']/database[@name='" + this.m_strCurDatabaseName + "']");
            if (nodeDatabase == null)
            {
                Debug.Assert(false, "��Ϊ '" + this.m_strCurDatabaseName + "' ��<database>Ԫ�ز�������");
                return;
            }

            // ɾ��ԭ�е�����Ԫ��
            while (nodeDatabase.ChildNodes.Count != 0)
            {
                nodeDatabase.RemoveChild(nodeDatabase.ChildNodes[0]);
            }

            // ��listview�е�Ԫ�ؼ���
            for (int i = 0; i < this.listView_accessPoints.Items.Count; i++)
            {
                ListViewItem item = this.listView_accessPoints.Items[i];

                string strName = item.Text;
                string strWeight = ListViewUtil.GetItemText(item, 1);
                string strSearchStyle = ListViewUtil.GetItemText(item, 2);

                XmlNode new_node = this.dom.CreateElement("accessPoint");
                nodeDatabase.AppendChild(new_node);
                DomUtil.SetAttr(new_node, "name", strName);
                DomUtil.SetAttr(new_node, "weight", strWeight);
                DomUtil.SetAttr(new_node, "searchStyle", strSearchStyle);
            }

            this.AccessPointsChanged = false;
        }

        private void listView_accessPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_accessPoints.SelectedItems.Count == 0)
            {
                this.button_deleteAccessPoint.Enabled = false;
                this.button_modifyAccessPoint.Enabled = false;
            }
            else
            {
                this.button_deleteAccessPoint.Enabled = true;
                this.button_modifyAccessPoint.Enabled = true;
            }
        }

        private void listView_accessPoints_DoubleClick(object sender, EventArgs e)
        {
            button_modifyAccessPoint_Click(this, e);
        }

        private void textBox_projectName_TextChanged(object sender, EventArgs e)
        {
            if (this.m_nodeProject != null)
                DomUtil.SetAttr(this.m_nodeProject, "name", this.textBox_projectName.Text);

            this.Changed = true;
        }

        private void textBox_comment_TextChanged(object sender, EventArgs e)
        {
            if (this.m_nodeProject != null)
                DomUtil.SetAttr(this.m_nodeProject, "comment", this.textBox_comment.Text);

            this.Changed = true;

        }


    }
}