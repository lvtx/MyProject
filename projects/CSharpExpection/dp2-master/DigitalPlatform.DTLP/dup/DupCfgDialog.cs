using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;

using DigitalPlatform.Xml;
using DigitalPlatform.GUI;

namespace DigitalPlatform.DTLP
{
    /// <summary>
    /// �������öԻ���
    /// </summary>
    public partial class DupCfgDialog : Form
    {
        public DtlpChannelArray DtlpChannels = null;
        public DtlpChannel DtlpChannel = null;

        public string CfgFilename = ""; // XML�����ļ���

        XmlDocument dom = null; // �����ļ�DOM

        bool m_bChanged = false;

        public DupCfgDialog()
        {
            InitializeComponent();
        }

        private void DupCfgDialog_Load(object sender, EventArgs e)
        {
            FillProjectNameList();

            FillDefaultList();
        }

        private void DupCfgDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Changed == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ���޸ĵ�������δ���档����ʱ�رմ��ڣ���Щ�޸Ľ���ʧ��\r\n\r\nȷʵҪ�رմ���? ",
                    "DupCfgDialog",
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

        private void DupCfgDialog_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (this.Changed == true
                && this.dom != null
                && String.IsNullOrEmpty(this.CfgFilename) == false)
            {
                this.dom.Save(this.CfgFilename);
                this.Changed = false;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_newProject_Click(object sender, EventArgs e)
        {
            // ���Ƴ�һ���µ�DOM
            XmlDocument new_dom = new XmlDocument();
            new_dom.LoadXml(this.dom.OuterXml);

            ProjectDialog dlg = new ProjectDialog();

            dlg.CreateMode = true;
            dlg.DupCfgDialog = this;
            dlg.ProjectName = "�µĲ��ط���";
            dlg.ProjectComment = "";
            dlg.dom = new_dom;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            this.dom = new_dom;

            // ˢ���б�
            FillProjectNameList();

            // ѡ���ղ��������
            SelectProjectItem(dlg.ProjectName);

            this.Changed = true;

            FillDefaultList();  // �����ļ��Ͽ��ܷ����ı�
        }

        // �ڷ������б��У�ѡ��һ���ض������ֵ���
        void SelectProjectItem(string strProjectName)
        {
            for (int i = 0; i < this.listView_projects.Items.Count; i++)
            {
                ListViewItem item = this.listView_projects.Items[i];
                if (item.Text == strProjectName)
                    item.Selected = true;
                else
                    item.Selected = false;
            }
        }

        // �޸Ĳ��ط���
        private void button_modifyProject_Click(object sender, EventArgs e)
        {
            if (this.listView_projects.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�޸ĵĲ��ط�������");
                return;
            }

            ListViewItem item = this.listView_projects.SelectedItems[0];

            string strProjectName = item.Text;
            string strProjectComment = ListViewUtil.GetItemText(item, 1);

            // ���Ƴ�һ���µ�DOM
            XmlDocument new_dom = new XmlDocument();
            new_dom.LoadXml(this.dom.OuterXml);

            ProjectDialog dlg = new ProjectDialog();

            dlg.CreateMode = false;
            dlg.DupCfgDialog = this;
            dlg.ProjectName = strProjectName;
            dlg.ProjectComment = strProjectComment;
            dlg.dom = new_dom;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            this.dom = new_dom;

            item.Text = dlg.ProjectName;
            ListViewUtil.ChangeItemText(item,
                1, dlg.ProjectComment);

            this.Changed = true;

            FillDefaultList(); // �����ļ��Ͽ��ܷ����ı�

            if (strProjectName != dlg.ProjectName)
            {
                // �����������ı�󣬶��ֵ��·���ȱʡ��ϵ�б���
                ChangeDefaultProjectName(strProjectName,
                    dlg.ProjectName);
            }
        }

        // ɾ�����ط���
        private void button_deleteProject_Click(object sender, EventArgs e)
        {
            string strError = "";

            if (this.listView_projects.SelectedIndices.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫɾ���Ĳ��ط�������");
                return;
            }

            DialogResult result = MessageBox.Show(this,
                "ȷʵҪɾ����ѡ���� " + this.listView_projects.SelectedIndices.Count.ToString() + " �����ط���?",
                "DupCfgDialog",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result == DialogResult.No)
                return;

            for (int i = this.listView_projects.SelectedIndices.Count - 1; i >= 0; i--)
            {
                int index = this.listView_projects.SelectedIndices[i];

                ListViewItem item = this.listView_projects.Items[index];

                string strProjectName = item.Text;

                XmlNode nodeProject = this.dom.DocumentElement.SelectSingleNode("//project[@name='" + strProjectName + "']");
                if (nodeProject == null)
                {
                    strError = "������name����ֵΪ '" + strProjectName + "' ��<project>Ԫ��";
                    goto ERROR1;
                }

                nodeProject.ParentNode.RemoveChild(nodeProject);

                this.listView_projects.Items.RemoveAt(index);

                // ������ɾ�������ֵ��·���ȱʡ��ϵ�б��У�Ҳɾ����Ӧ����
                ChangeDefaultProjectName(strProjectName,
                        null);
            }

            this.Changed = true;

            FillDefaultList(); // �����ļ��Ͽ��ܷ����ı�

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // ��ʼ��
        public int Initial(string strCfgFilename,
            out string strError)
        {
            strError = "";

            this.dom = new XmlDocument();
            try
            {
                this.dom.Load(strCfgFilename);
            }
            catch (FileNotFoundException /*ex*/)
            {
                this.dom.LoadXml("<root />");
            }
            catch (Exception ex)
            {
                strError = "װ�������ļ� '" 
                    +strCfgFilename
                    +"' ��XMLDOMʱ��������: "
                    +ex.Message;
                return -1;
            }

            this.CfgFilename = strCfgFilename;

            return 0;
        }

        void FillProjectNameList()
        {
            this.listView_projects.Items.Clear();

            if (this.dom == null)
                return;

            XmlNodeList nodes = this.dom.DocumentElement.SelectNodes("//project");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                string strName = DomUtil.GetAttr(node, "name");
                string strComment = DomUtil.GetAttr(node, "comment");

                ListViewItem item = new ListViewItem(strName, 0);
                item.SubItems.Add(strComment);
                this.listView_projects.Items.Add(item);
            }
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

        private void listView_projects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_projects.SelectedItems.Count == 0)
            {
                this.button_deleteProject.Enabled = false;
                this.button_modifyProject.Enabled = false;
            }
            else
            {
                this.button_deleteProject.Enabled = true;
                this.button_modifyProject.Enabled = true;
            }
        }

        // ˫������ͬ�ڱ༭�޸�
        private void listView_projects_DoubleClick(object sender, EventArgs e)
        {
            button_modifyProject_Click(this, e);
        }

        // �޸����ݿ��ȱʡ���ط�����Ӧ��ϵ
        private void button_modifyDefaut_Click(object sender, EventArgs e)
        {
            if (this.listView_defaults.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�޸ĵ�ȱʡ��ϵ����");
                return;
            }

            ListViewItem item = this.listView_defaults.SelectedItems[0];

            DefaultProjectDialog dlg = new DefaultProjectDialog();
            dlg.dom = this.dom;
            dlg.DatabaseName = item.Text;
            dlg.DefaultProjectName = ListViewUtil.GetItemText(item, 1);

            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            // ���ֵ�DOM��
            XmlNode nodeDefault = this.dom.DocumentElement.SelectSingleNode("//defaultProject/sourceDatabase[@name='" + item.Text + "']");
            if (nodeDefault == null)
            {
                nodeDefault = this.dom.CreateElement("sourceDatabase");

                XmlNode nodeRoot = this.dom.DocumentElement.SelectSingleNode("//defaultProject");
                if (nodeRoot == null)
                {
                    nodeRoot = this.dom.CreateElement("defaultProject");
                    this.dom.DocumentElement.AppendChild(nodeRoot);
                }

                nodeRoot.AppendChild(nodeDefault);
            }

            DomUtil.SetAttr(nodeDefault, "name", item.Text);
            DomUtil.SetAttr(nodeDefault, "defaultProject", dlg.DefaultProjectName);


            // ���ֵ�listview��
            Debug.Assert(dlg.DatabaseName == item.Text, "");
            ListViewUtil.ChangeItemText(item, 1, dlg.DefaultProjectName);

            this.Changed = true;
        }

        private void button_newDefault_Click(object sender, EventArgs e)
        {
            string strError = "";

            DefaultProjectDialog dlg = new DefaultProjectDialog();

            dlg.Text = "����ȱʡ��ϵ����";
            dlg.DupCfgDialog = this;
            dlg.dom = this.dom;
            dlg.DatabaseName = "";  // ����������
            dlg.DefaultProjectName = "";

            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            // ���ֵ�DOM��
            XmlNode nodeDefault = this.dom.DocumentElement.SelectSingleNode("//defaultProject/sourceDatabase[@name='" + dlg.DatabaseName + "']");
            if (nodeDefault != null)
            {
                // ����
                strError = "����·��Ϊ '" + dlg.DatabaseName + "' ��ȱʡ��ϵ�����Ѿ����ڣ������ٴ��������ɱ༭�Ѿ����ڵĸ����";
                goto ERROR1;
            }

            {
                nodeDefault = this.dom.CreateElement("sourceDatabase");

                XmlNode nodeRoot = this.dom.DocumentElement.SelectSingleNode("//defaultProject");
                if (nodeRoot == null)
                {
                    nodeRoot = this.dom.CreateElement("defaultProject");
                    this.dom.DocumentElement.AppendChild(nodeRoot);
                }

                nodeRoot.AppendChild(nodeDefault);
            }
            DomUtil.SetAttr(nodeDefault, "name", dlg.DatabaseName);
            DomUtil.SetAttr(nodeDefault, "defaultProject", dlg.DefaultProjectName);

            // ���ֵ�listview��
            ListViewItem item = new ListViewItem(dlg.DatabaseName, 0);
            item.SubItems.Add(dlg.DefaultProjectName);
            this.listView_defaults.Items.Add(item);

            // �������ݿ������Ƿ����Ѿ��õ������ݿ��������У�����ǣ�Ϊʵ����ɫ��������ǣ�Ϊ������ɫ
            List<string> database_names = GetAllDatabaseNames();
            if (database_names.IndexOf(dlg.DatabaseName) == -1)
            {
                item.ForeColor = SystemColors.GrayText;
                item.Tag = null;
            }
            else
            {
                item.Tag = 1;
            }

            this.Changed = true;
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        private void button_deleteDefault_Click(object sender, EventArgs e)
        {
            // string strError = "";

            if (this.listView_defaults.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫɾ����ȱʡ��ϵ����");
                return;
            }

            ListViewItem item = this.listView_defaults.SelectedItems[0];
            string strText = item.Text + " -- " + ListViewUtil.GetItemText(item, 1);
            if (item.Tag == null)
            {
                DialogResult result = MessageBox.Show(this,
                    "ȷʵҪɾ��ȱʡ��ϵ���� "+strText+" ?",
                    "DupCfgDialog",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                    return;

                // ������������ɾ��
                this.listView_defaults.Items.Remove(item);
            }
            else
            {
                DialogResult result = MessageBox.Show(this,
                    "ȷʵҪ���ȱʡ��ϵ���� " + strText + " ?",
                    "DupCfgDialog",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                    return;

                // ʵ�ڵ����ֻ��Ĩ��������������
                ListViewUtil.ChangeItemText(item, 1, "");
            }

            // ���ֵ�DOM��
            XmlNode nodeDefault = this.dom.DocumentElement.SelectSingleNode("//defaultProject/sourceDatabase[@name='" + item.Text + "']");
            /*
            if (nodeDefault == null)
            {
                strError = "����·��Ϊ '" + item.Text + "' ��ȱʡ��ϵ�����Ȼ��DOM�в�����";
                goto ERROR1;
            }
             * */
            // 2009/3/13 changed
            if (nodeDefault != null)
            {
                nodeDefault.ParentNode.RemoveChild(nodeDefault);
                this.Changed = true;
            }
            return;
            /*
        ERROR1:
            MessageBox.Show(this, strError);
             * */
        }

        private void listView_defaults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_defaults.SelectedItems.Count == 0)
            {
                this.button_modifyDefaut.Enabled = false;
                this.button_deleteDefault.Enabled = false;
            }
            else
            {
                this.button_modifyDefaut.Enabled = true;
                this.button_deleteDefault.Enabled = true;
            }
        }


        void FillDefaultList()
        {
            this.listView_defaults.Items.Clear();

            // ���ȫ��<sourceDatabase>Ԫ��name�����еķ���·��
            List<string> startpaths = new List<string>();
            XmlNodeList nodes = this.dom.DocumentElement.SelectNodes("//defaultProject/sourceDatabase");
            for (int i = 0; i < nodes.Count; i++)
            {
                string strStartPath = DomUtil.GetAttr(nodes[i], "name");
                if (String.IsNullOrEmpty(strStartPath) == true)
                    continue;
                startpaths.Add(strStartPath);
            }


            // �Ȱ��ղ��ط����������õ����ķ���·��(���ݿ�)�г�����
            List<string> database_names = GetAllDatabaseNames();
            for (int i = 0; i < database_names.Count; i++)
            {
                string strDatabaseName = database_names[i];

                string strDefaultProject = "";
                XmlNode nodeDefault = this.dom.DocumentElement.SelectSingleNode("//defaultProject/sourceDatabase[@name='" + strDatabaseName + "']");
                if (nodeDefault != null)
                    strDefaultProject = DomUtil.GetAttr(nodeDefault, "defaultProject");

                ListViewItem item = new ListViewItem(strDatabaseName, 0);
                item.SubItems.Add(strDefaultProject);
                this.listView_defaults.Items.Add(item);
                item.Tag = 1;   // ��ʾΪʵ��

                // ��startpaths�������Ѿ��ù���startpath
                startpaths.Remove(strDatabaseName);
            }

            // �ٰ��ղ��ط���������û���õ����ķ���·���г�����
            for (int i = 0; i < startpaths.Count; i++)
            {
                string strDatabaseName = startpaths[i];

                string strDefaultProject = "";
                XmlNode nodeDefault = this.dom.DocumentElement.SelectSingleNode("//defaultProject/sourceDatabase[@name='" + strDatabaseName + "']");
                if (nodeDefault != null)
                    strDefaultProject = DomUtil.GetAttr(nodeDefault, "defaultProject");

                ListViewItem item = new ListViewItem(strDatabaseName, 0);
                item.SubItems.Add(strDefaultProject);
                this.listView_defaults.Items.Add(item);
                item.Tag = null;    // ��ʾΪ����

                item.ForeColor = SystemColors.GrayText; // ��ɫ���飬��ʾ������ݿ���û���ڲ��ط��������г��ֹ�
            }
        }

        // ���ȫ�������ݿ���
        List<string> GetAllDatabaseNames()
        {
            List<string> results = new List<string>();
            XmlNodeList nodes = this.dom.DocumentElement.SelectNodes("//database");
            for (int i = 0; i < nodes.Count; i++)
            {
                results.Add(DomUtil.GetAttr(nodes[i], "name"));
            }

            results.Sort();

            // ȥ��
            Global.RemoveDup(ref results);

            return results;
        }

        // �޸�ȱʡ��ϵ
        private void listView_defaults_DoubleClick(object sender, EventArgs e)
        {
            button_modifyDefaut_Click(this, e);
        }

        // �����������ı�󣬶��ֵ��·���ȱʡ��ϵ�б���
        void ChangeDefaultProjectName(string strOldProjectName,
            string strNewProjectName)
        {
            if (strOldProjectName == strNewProjectName)
            {
                Debug.Assert(false, "");
                return;
            }

            bool bChanged = false;
            int nCount = 0;
            for (int i = 0; i < listView_defaults.Items.Count; i++)
            {
                ListViewItem item = this.listView_defaults.Items[i];

                // �����Ӿ��޸�
                string strProjectName = ListViewUtil.GetItemText(item, 1);
                if (strProjectName == strOldProjectName)
                {
                    ListViewUtil.ChangeItemText(item, 1, strNewProjectName);
                    bChanged = true;
                    nCount++;
                }

            }
            // ����DOM�޸�
            XmlNodeList nodes = this.dom.DocumentElement.SelectNodes("//defaultProject/sourceDatabase[@defaultProject='"+strOldProjectName+"']");
            Debug.Assert(nCount == nodes.Count, "������ĿӦ�����");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                if (String.IsNullOrEmpty(strNewProjectName) == true)
                {
                    // ɾ��
                    node.ParentNode.RemoveChild(node);
                }
                else
                {
                    DomUtil.SetAttr(node, "defaultProject", strNewProjectName);
                }
                bChanged = true;
            }

            if (bChanged == true)
                this.Changed = true;
        }

        #region ��dt1000 gcs.ini������������

        // ��dt1000 gcs.ini�ļ��еĲ�������������dup.xml�ļ���
        /*
[/�ҵĵ���/ͼ���Ŀ]
key1=010$a,50,
key2=200$a,50,
key3=905$d,20,
key4=906$a,50,
key5=986$a,50,
HoldValue=130
targetDB1=/�ҵĵ���/ͼ���Ŀ,
targetDB2=/�ҵĵ���/ͼ���ܿ�,
         * */
        public static int UpgradeGcsIniDupCfg(string strGcsIniFilename,
            string strDupXmlFilename,
            out string strError)
        {
            strError = "";

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<root />");

            // ���ȫ��sectionֵ��
            List<string> sections = null;

            int nRet = GetIniSections(strGcsIniFilename,
                out sections,
                out strError);
            if (nRet == -1)
                return -1;
            if (nRet == 0)
                return -1;

            // Ȼ��ÿ��section������û��һ����Ϊ"key1"��entry��
            // ����У��������ǹ��ĵ�����section
            List<string> dbpaths = new List<string>();
            for (int i = 0; i < sections.Count; i++)
            {
                string strSection = sections[i];

                StringBuilder s = new StringBuilder(255, 255);

                nRet = API.GetPrivateProfileString(strSection,
                    "key1",
                    "!!!null",
                    s,
                    255,
                    strGcsIniFilename);
                string strValue = s.ToString();
                if (nRet <= 0
                    || strValue == "!!!null")
                {
                }
                else
                {
                    if (IsValidDbPath(strSection) == true)
                        dbpaths.Add(strSection);
                }
            }

            // �������ͷ������ݿ�Ĺ�ϵ����
            List<DefaultProjectRelation> relations = new List<DefaultProjectRelation>();

            string strToday = DateTime.Now.ToShortDateString();

            // ��ÿ�����ݿ�
            for (int i = 0; i < dbpaths.Count; i++)
            {
                string strDbPath = dbpaths[i];

                if (String.IsNullOrEmpty(strDbPath) == true)
                    continue;

                string strThreshold = "";

                StringBuilder s = new StringBuilder(255, 255);
                nRet = API.GetPrivateProfileString(strDbPath,
                    "HoldValue",
                    "!!!null",
                    s,
                    255,
                    strGcsIniFilename);
                string strValue = s.ToString();
                if (strValue != "!!!null")
                {
                    strThreshold = strValue;
                }

                if (String.IsNullOrEmpty(strThreshold) == true)
                    continue;

                // ����һ��<project>Ԫ��
                XmlNode nodeProject = dom.CreateElement("project");
                dom.DocumentElement.AppendChild(nodeProject);
                string strProjectName = "���ط���" + (i + 1).ToString();    //"���ط���" + (i + 1).ToString() + " " + NewStylePath(strDbPath)
                DomUtil.SetAttr(nodeProject, "name", strProjectName);
                DomUtil.SetAttr(nodeProject, "comment", "�����Ϊ " + NewStylePath(strDbPath) + "��" + strToday + " ��dt1000��������");

                DefaultProjectRelation relation = new DefaultProjectRelation();
                relation.StartDbPath = NewStylePath(strDbPath);
                relation.ProjectName = strProjectName;
                relations.Add(relation);

                // ��������<database>Ԫ��
                for (int j = 0; ; j++)
                {
                    string strEntry = "targetDB" + (j + 1).ToString();

                    StringBuilder s1 = new StringBuilder(255, 255);
                    nRet = API.GetPrivateProfileString(strDbPath,
                        strEntry,
                        "!!!null",
                        s1,
                        255,
                        strGcsIniFilename);
                    string strLine = s1.ToString();
                    if (nRet <= 0
                        || strLine == "!!!null")
                        break;

                    string strDatabaseName = "";
                    string strType = "";

                    string[] parts = strLine.Split(new char[] { ',' });
                    if (parts.Length > 0)
                        strDatabaseName = parts[0].Trim();

                    if (parts.Length > 1)
                        strType = parts[1].Trim();

                    XmlNode nodeDatabase = dom.CreateElement("database");
                    nodeProject.AppendChild(nodeDatabase);

                    DomUtil.SetAttr(nodeDatabase, "name", NewStylePath(strDatabaseName));
                    DomUtil.SetAttr(nodeDatabase, "threshold", strThreshold);

                    ////
                    // ��������<accessPoint>Ԫ��
                    for (int k = 0; ; k++)
                    {
                        string strEntry2 = "key" + (k + 1).ToString();

                        StringBuilder s2 = new StringBuilder(255, 255);
                        nRet = API.GetPrivateProfileString(strDbPath,
                            strEntry2,
                            "!!!null",
                            s2,
                            255,
                            strGcsIniFilename);
                        string strLine1 = s2.ToString();
                        if (nRet <= 0
                            || strLine1 == "!!!null")
                            break;

                        string strFromName = "";
                        string strWeight = "";
                        string strSearchStyle = "";

                        string[] parts_of_line = strLine1.Split(new char[] { ',' });
                        if (parts_of_line.Length > 0)
                            strFromName = parts_of_line[0].Trim();

                        if (parts.Length > 1)
                            strWeight = parts_of_line[1].Trim();

                        if (parts_of_line.Length > 2)
                            strSearchStyle = parts_of_line[2].Trim();

                        if (strSearchStyle == "q")
                            strSearchStyle = "Left";
                        else if (strSearchStyle == "")
                            strSearchStyle = "Exact";

                        XmlNode nodeAccessPoint = dom.CreateElement("accessPoint");
                        nodeDatabase.AppendChild(nodeAccessPoint);

                        DomUtil.SetAttr(nodeAccessPoint, "name", strFromName);
                        DomUtil.SetAttr(nodeAccessPoint, "weight", strWeight);
                        DomUtil.SetAttr(nodeAccessPoint, "searchStyle", strSearchStyle);
                    } // end of k


                } // end of j

            } // end of i

            // ȱʡ���ط�������
            // �ڸ�������һ��<defaultProject>����Ԫ��
            XmlNode nodeContainer = dom.CreateElement("defaultProject");
            dom.DocumentElement.AppendChild(nodeContainer);

            for (int i = 0; i < relations.Count; i++)
            {
                DefaultProjectRelation relation = relations[i];

                XmlNode nodeSourceDatabase = dom.CreateElement("sourceDatabase");
                nodeContainer.AppendChild(nodeSourceDatabase);

                DomUtil.SetAttr(nodeSourceDatabase, "name", relation.StartDbPath);
                DomUtil.SetAttr(nodeSourceDatabase, "defaultProject", relation.ProjectName);
            }


            dom.Save(strDupXmlFilename);

            return 0;
        }

        // ���ɷ���·����Ϊ�·���·����ȥ����һ���ַ���'/'
        public static string NewStylePath(string strDbPath)
        {
            if (String.IsNullOrEmpty(strDbPath) == true)
                return strDbPath;

            // ȥ����һ��'/'�ַ�
            if (strDbPath[0] == '/')
                return strDbPath.Substring(1);

            return strDbPath;
        }

        // �����ǲ��ǺϷ������ݿ�·����
        // �����һ��Ϊ���ҵĵ��ԡ������ǺϷ���·��(����Ȼ��homeģʽ�µ�·�����������Ѳ���֧��)
        static bool IsValidDbPath(string strDbPath)
        {
            strDbPath = NewStylePath(strDbPath);

            string[] parts = strDbPath.Split(new char[] { '/' });

            string strServerName = "";
            string strDbName = "";

            if (parts.Length > 0)
                strServerName = parts[0].Trim();

            if (parts.Length > 1)
                strDbName = parts[1].Trim();

            if (strServerName == "�ҵĵ���")
                return false;

            return true;
        }



        // ���һ��.ini�ļ��е�����sectionֵ
        // return:
        //      -1  error
        //      0   �ļ�������
        //      1   �ļ�����
        public static int GetIniSections(string strIniFilename,
            out List<string> sections,
            out string strError)
        {
            strError = "";
            sections = new List<string>();


            try
            {
                using (StreamReader sr = new StreamReader(strIniFilename, Encoding.GetEncoding(936)))
                {
                    for (int i = 0; ; i++)
                    {
                        string strLine = sr.ReadLine();
                        if (strLine == null)
                            break;
                        strLine = strLine.Trim();
                        if (String.IsNullOrEmpty(strLine) == true)
                            continue;

                        if (strLine[0] == '[' && strLine[strLine.Length - 1] == ']')
                        {
                            strLine = strLine.Substring(1, strLine.Length - 2); // ȥ����Χ��[]
                            sections.Add(strLine);
                        }
                    }
                }
                return 1;
            }
            catch (FileNotFoundException)
            {
                strError = "�ļ� " + strIniFilename + " ������";
                return 0;
            }
            catch (Exception ex)
            {
                strError = "װ���ļ� " + strIniFilename + " ʱ��������: " + ex.Message;
                return -1;
            }
        }

        private void button_upgradeFromGcsIni_Click(object sender, EventArgs e)
        {
            // ѯ��gcs.iniԭʼ�ļ�ȫ·��
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Title = "��ָ��Ҫ�ο���gcs.ini�ļ�";
            dlg.FileName = "";
            dlg.Filter = "gcs.ini file (gcs.ini)|gcs.ini|All files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string strOutputFilename = this.CfgFilename + ".tmp";

            string strError = "";
            int nRet = UpgradeGcsIniDupCfg(dlg.FileName,
                strOutputFilename,
                out strError);
            if (nRet == -1)
                MessageBox.Show(this, strError);

            // ���渲��
            DialogResult result = MessageBox.Show(this,
                "�Ƿ�Ҫ�ô��ļ� " + dlg.FileName + " �л�ȡ���������ݸ��ǵ�ǰ�����е�������������?\r\n\r\n(Yes)�� -- ���ǣ�(No)�� -- �����ǣ�����notepad�й۲��ȡ�����ݣ�(Cancel)����",
                "DupCfgDialog",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Cancel)
            {
                return;
            }

            if (result == DialogResult.No)
            {
                System.Diagnostics.Process.Start("notepad", strOutputFilename);
                return;
            }

            // ȷ���ڴ��޸Ķ��ֵ������ļ�
            if (this.Changed == true
                && this.dom != null
                && String.IsNullOrEmpty(this.CfgFilename) == false)
            {
                this.dom.Save(this.CfgFilename);
                this.Changed = false;
            }

            // ����һ�������ļ�
            try
            {
                File.Copy(this.CfgFilename, this.CfgFilename + ".bak", true);
            }
            catch
            {
            }

            File.Copy(strOutputFilename, this.CfgFilename, true);

            nRet = Initial(this.CfgFilename,
                out strError);
            if (nRet == -1)
                goto ERROR1;

            // �л������� page
            this.tabControl_main.SelectedTab = this.tabPage_projects;

            FillProjectNameList();
            FillDefaultList();
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        #endregion

        private void button_viewDupXml_Click(object sender, EventArgs e)
        {
            if (File.Exists(this.CfgFilename) == true)
                System.Diagnostics.Process.Start("notepad", this.CfgFilename);
            else 
                MessageBox.Show(this, "�����ļ� " + this.CfgFilename + " �в�����...");

        }


    }

    class DefaultProjectRelation
    {
        public string ProjectName = "";
        public string StartDbPath = "";
    }
}