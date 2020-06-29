using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Web;

using DigitalPlatform;
using DigitalPlatform.GUI;
using DigitalPlatform.CirculationClient;
using DigitalPlatform.Xml;
using DigitalPlatform.IO;
using DigitalPlatform.Text;

using DigitalPlatform.CirculationClient.localhost;

namespace dp2Circulation
{
    public partial class ManagerForm : Form
    {
        const int TYPE_NSTABLE = 0;
        const int TYPE_GROUP = 1;
        const int TYPE_DATABASE = 2;
        const int TYPE_ERROR = 3;


        int m_nRightsTableXmlVersion = 0;
        int m_nRightsTableHtmlVersion = 0;

        // ��ʾ��ǰȫ�����ݿ���Ϣ��XML�ַ���
        public string AllDatabaseInfoXml = "";

        const int WM_INITIAL = API.WM_USER + 201;

        public LibraryChannel Channel = new LibraryChannel();
        public string Lang = "zh";

        public MainForm MainForm = null;
        DigitalPlatform.Stop stop = null;

        string [] type_names = new string[] {
            "biblio","��Ŀ",
            "entity","ʵ��",
            "order","����",
            "issue","��",
            "reader","����",
            "message","��Ϣ",
            "arrived","ԤԼ����",
            "amerce","ΥԼ��",
            "publisher","������",
            "zhongcihao","�ִκ�",
        };

        // �������ͺ������õ������ַ���
        string GetTypeString(string strName)
        {
            for (int i = 0; i < type_names.Length / 2; i++)
            {
                if (type_names[i * 2 + 1] == strName)
                    return type_names[i * 2];
            }

            return null;    // not found
        }

        // ���������ַ����õ����ͺ�����
        public string GetTypeName(string strTypeString)
        {
            for (int i = 0; i < type_names.Length / 2; i++)
            {
                if (type_names[i * 2] == strTypeString)
                    return type_names[i * 2+1];
            }

            return null;    // not found
        }

        public ManagerForm()
        {
            InitializeComponent();
        }

        private void ManagerForm_Load(object sender, EventArgs e)
        {
            this.Channel.Url = this.MainForm.LibraryServerUrl;

            this.Channel.BeforeLogin -= new BeforeLoginEventHandle(Channel_BeforeLogin);
            this.Channel.BeforeLogin += new BeforeLoginEventHandle(Channel_BeforeLogin);

            stop = new DigitalPlatform.Stop();
            stop.Register(MainForm.stopManager, true);	// ����������

            API.PostMessage(this.Handle, WM_INITIAL, 0, 0);

            this.listView_opacDatabases.SmallImageList = this.imageList_opacDatabaseType;
            this.listView_opacDatabases.LargeImageList = this.imageList_opacDatabaseType;

            this.listView_databases.SmallImageList = this.imageList_opacDatabaseType;
            this.listView_databases.LargeImageList = this.imageList_opacDatabaseType;

            this.treeView_opacBrowseFormats.ImageList = this.imageList_opacBrowseFormatType;

            this.treeView_zhongcihao.ImageList = this.imageList_zhongcihao;
        }

        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_INITIAL:
                    {
                        string strError = "";
                        int nRet = ListAllDatabases(out strError);
                        if (nRet == -1)
                        {
                            MessageBox.Show(this, strError);
                        }

                        nRet = ListAllOpacDatabases(out strError);
                        if (nRet == -1)
                        {
                            MessageBox.Show(this, strError);
                        }

                        nRet = this.ListAllOpacBrowseFormats(out strError);
                        if (nRet == -1)
                        {
                            MessageBox.Show(this, strError);
                        }

                        nRet = this.ListRightsTables(out strError);
                        if (nRet == -1)
                        {
                            MessageBox.Show(this, strError);
                        }

                        // ��listview���г����йݲص�
                        nRet = this.ListAllLocations(out strError);
                        if (nRet == -1)
                        {
                            MessageBox.Show(this, strError);
                        }

                        // �г��ִκŶ���
                        nRet = this.ListZhongcihao(out strError);
                        if (nRet == -1)
                        {
                            MessageBox.Show(this, strError);
                        } 

                        // �г��ű�
                        nRet = this.ListScript(out strError);
                        if (nRet == -1)
                        {
                            MessageBox.Show(this, strError);
                        }
                    }
                    return;
            }
            base.DefWndProc(ref m);
        }

        public bool Changed
        {
            get
            {
                for (int i = 0; i < this.listView_opacDatabases.Items.Count; i++)
                {
                    ListViewItem item = this.listView_opacDatabases.Items[i];
                    if (item.ImageIndex == 2)
                        return true;    // ����δ�ύ�ġ���ǰ�������OPAC���ݿⶨ������
                }

                // TODO: ��δ�ύ��tree���� i j ����ѭ��
                for (int i = 0; i < this.treeView_opacBrowseFormats.Nodes.Count; i++)
                {
                    TreeNode node = this.treeView_opacBrowseFormats.Nodes[i];
                    if (node.ImageIndex == 2)
                        return true;

                    for (int j = 0; j < node.Nodes.Count; j++)
                    {
                        TreeNode sub_node = node.Nodes[j];
                        if (sub_node.ImageIndex == 2)
                            return true;
                    }
                }

                return false;
            }
        }

        private void ManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (stop != null)
            {
                if (stop.State == 0)    // 0 ��ʾ���ڴ���
                {
                    MessageBox.Show(this, "���ڹرմ���ǰֹͣ���ڽ��еĳ�ʱ������");
                    e.Cancel = true;
                    return;
                }

            }

            if (this.Changed == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�������ж��屻�޸ĺ���δ���档����ʱ�رմ��ڣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪ�رմ���? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (this.LoanPolicyDefChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�����ڶ�����ͨȨ�޶��屻�޸ĺ���δ���档����ʱ�رմ��ڣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪ�رմ���? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    this.tabControl_main.SelectedTab = this.tabPage_loanPolicy;
                    return;
                }
            }

            if (this.LocationTypesDefChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�������йݲصص㶨�屻�޸ĺ���δ���档����ʱ�رմ��ڣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪ�رմ���? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    this.tabControl_main.SelectedTab = this.tabPage_locations;
                    return;
                }
            }

            if (this.ScriptChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�������нű����屻�޸ĺ���δ���档����ʱ�رմ��ڣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪ�رմ���? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    this.tabControl_main.SelectedTab = this.tabPage_script;
                    return;
                }
            }
        }

        private void ManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (stop != null)
            {
                stop.Unregister(); // �������
                stop = null;
            }

        }

        void Channel_BeforeLogin(object sender, BeforeLoginEventArgs e)
        {
            this.MainForm.Channel_BeforeLogin(this, e);
        }

        void DoStop(object sender, StopEventArgs e)
        {
            if (this.Channel != null)
                this.Channel.Abort();
        }

        /*
        private void button_clearAllDbs_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = ClearAllDbs(out strError);
            if (nRet == -1)
                MessageBox.Show(this, strError);
            else
                MessageBox.Show(this, "OK");
        }*/

        // 
        int ClearAllDbs(
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("��������������ݿ������� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.ClearAllDbs(
                    stop,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

            return 1;
        ERROR1:
            return -1;
        }

        void EnableControls(bool bEnable)
        {
            // this.button_clearAllDbs.Enabled = bEnable;
            this.toolStrip_databases.Enabled = bEnable;
        }

        private void ManagerForm_Activated(object sender, EventArgs e)
        {
            this.MainForm.stopManager.Active(this.stop);

            this.MainForm.MenuItem_recoverUrgentLog.Enabled = false;
            this.MainForm.MenuItem_font.Enabled = false;
        }

        // �ӷ�����������µĹ���ȫ�����ݿ�� XML ���塣ע�⣬��ˢ�½��档
        int RefreshAllDatabaseXml(out string strError)
        {
            strError = "";

            string strOutputInfo = "";
            int nRet = GetAllDatabaseInfo(out strOutputInfo,
                    out strError);
            if (nRet == -1)
                return -1;

            this.AllDatabaseInfoXml = strOutputInfo;

            return 0;
        }

        // ��listview���г��������ݿ�
        int ListAllDatabases(out string strError)
        {
            strError = "";

            this.listView_databases.Items.Clear();

            string strOutputInfo = "";
            int nRet = GetAllDatabaseInfo(out strOutputInfo,
                    out strError);
            if (nRet == -1)
                return -1;

            this.AllDatabaseInfoXml = strOutputInfo;

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(strOutputInfo);
            }
            catch (Exception ex)
            {
                strError = "XMLװ��DOMʱ����: " + ex.Message;
                return -1;
            }

            XmlNodeList nodes = dom.DocumentElement.SelectNodes("database");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                string strName = DomUtil.GetAttr(node, "name");
                string strType = DomUtil.GetAttr(node, "type");

                // 2008/7/2 new add
                // �յ����ֽ�������
                if (String.IsNullOrEmpty(strName) == true)
                    continue;

                string strTypeName = GetTypeName(strType);
                if (strTypeName == null)
                    strTypeName = strType;

                ListViewItem item = new ListViewItem(strName, 0);
                item.SubItems.Add(strTypeName);
                item.Tag = node.OuterXml;   // ����XML����Ƭ��

                this.listView_databases.Items.Add(item);
            }

            return 0;
        }

        // ȷ��һ�����ݿ��ǲ�����Ŀ������?
        bool IsDatabaseBiblioType(string strDatabaseName)
        {
            for (int i = 0; i < this.listView_databases.Items.Count; i++)
            {
                ListViewItem item = this.listView_databases.Items[i];
                string strName = item.Text;
                if (strName == strDatabaseName)
                {
                    string strTypeName = ListViewUtil.GetItemText(item, 1);
                    string strTypeString = GetTypeString(strTypeName);

                    if (strTypeString == "biblio")
                        return true;
                }
            }

            return false;
        }

        int GetAllDatabaseInfo(out string strOutputInfo,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڻ�ȡȫ�����ݿ��� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.ManageDatabase(
                    stop,
                    "getinfo",
                    "",
                    "",
                    out strOutputInfo,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        private void listView_databases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_databases.SelectedItems.Count > 0)
            {
                this.toolStripButton_modifyDatabase.Enabled = true;
                this.toolStripButton_deleteDatabase.Enabled = true;
                this.toolStripButton_initializeDatabase.Enabled = true;
            }
            else
            {
                this.toolStripButton_modifyDatabase.Enabled = false;
                this.toolStripButton_deleteDatabase.Enabled = false;
                this.toolStripButton_initializeDatabase.Enabled = false;
            }
        }

        // ������Ŀ��
        private void ToolStripMenuItem_createBiblioDatabase_Click(object sender, EventArgs e)
        {

            BiblioDatabaseDialog dlg = new BiblioDatabaseDialog();

            dlg.Text = "��������Ŀ��";
            dlg.ManagerForm = this;
            dlg.CreateMode = true;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);


            if (dlg.DialogResult != DialogResult.OK)
                return;

            // ˢ�¿����б�
            string strError = "";
            int nRet = ListAllDatabases(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
            }

            // ѡ���մ��������ݿ�
            SelectDatabaseLine(dlg.BiblioDatabaseName);

            // ���»�ø��ֿ������б�
            this.MainForm.StartPrepareNames(false);
        }

        void SelectDatabaseLine(string strDatabaseName)
        {
            for (int i = 0; i < this.listView_databases.Items.Count; i++)
            {
                ListViewItem item = this.listView_databases.Items[i];

                if (item.Text == strDatabaseName)
                    item.Selected = true;
                else
                    item.Selected = false;
            }
        }

        // �������ݿ�
        public int CreateDatabase(
            string strDatabaseInfo,
            out string strOutputInfo,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڴ������ݿ� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.ManageDatabase(
                    stop,
                    "create",
                    "",
                    strDatabaseInfo,
                    out strOutputInfo,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // ɾ�����ݿ�
        public int DeleteDatabase(
            string strDatabaseNames,
            out string strOutputInfo,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("����ɾ�����ݿ� "+strDatabaseNames+"...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.ManageDatabase(
                    stop,
                    "delete",
                    strDatabaseNames,
                    "",
                    out strOutputInfo,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // ��ʼ�����ݿ�
        public int InitializeDatabase(
            string strDatabaseNames,
            out string strOutputInfo,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڳ�ʼ�����ݿ� " + strDatabaseNames + "...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.ManageDatabase(
                    stop,
                    "initialize",
                    strDatabaseNames,
                    "",
                    out strOutputInfo,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // �޸����ݿ�
        public int ChangeDatabase(
            string strDatabaseNames,
            string strDatabaseInfo,
            out string strOutputInfo,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("�����޸����ݿ� " + strDatabaseNames + "...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.ManageDatabase(
                    stop,
                    "change",
                    strDatabaseNames,
                    strDatabaseInfo,
                    out strOutputInfo,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // ɾ�����ݿ�
        private void toolStripButton_deleteDatabase_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            if (this.listView_databases.SelectedIndices.Count == 0)
            {
                strError = "��δѡ��Ҫɾ�������ݿ�����";
                goto ERROR1;
            }

            string strDbNameList = "";
            for (int i = 0; i < this.listView_databases.SelectedItems.Count; i++)
            {
                if (i > 0)
                    strDbNameList += ",";
                strDbNameList += this.listView_databases.SelectedItems[i].Text;
            }

            // �Ի��򾯸�
            DialogResult result = MessageBox.Show(this,
                "ȷʵҪɾ�����ݿ� "+strDbNameList+"?\r\n\r\n���棺���ݿ�һ����ɾ�������ڵ����ݼ�¼��ȫ����ʧ������Ҳ�޷���ԭ",
                "ManagerForm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
                return;

            // Ϊȷ����ݶ���¼
            // return:
            //      -1  ����
            //      0   ������¼
            //      1   ��¼�ɹ�
            nRet = ConfirmLogin(out strError);
            if (nRet == -1)
                goto ERROR1;
            if (nRet == 0)
            {
                strError = "ɾ�����ݿ����������";
                goto ERROR1;
            }

            /*
            // Ϊ����AllDatabaseInfoXml
            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(this.AllDatabaseInfoXml);
            }
            catch (Exception ex)
            {
                strError = "AllDatabaseInfoXmlװ��XMLDOMʱ����: " + ex.Message;
                goto ERROR1;
            }
             * */

            EnableControls(false);

            try
            {

                for (int i = this.listView_databases.SelectedIndices.Count - 1;
                    i >= 0;
                    i--)
                {
                    int index = this.listView_databases.SelectedIndices[i];

                    string strDatabaseName = this.listView_databases.Items[index].Text;

                    string strOutputInfo = "";
                    nRet = DeleteDatabase(strDatabaseName,
                        out strOutputInfo,
                        out strError);
                    if (nRet == -1)
                        goto ERROR1;

                    this.listView_databases.Items.RemoveAt(index);

                    /*
                    // ɾ��DOM�ж���
                    XmlNode nodeDatabase = dom.DocumentElement.SelectSingleNode("database[@name='" + strDatabaseName + "']");
                    if (nodeDatabase == null)
                    {
                        strError = "AllDatabaseInfoXml�о�Ȼû���ҵ���Ϊ '"+strDatabaseName+"' �����ݿⶨ��";
                        goto ERROR1;
                    }
                    dom.DocumentElement.RemoveChild(nodeDatabase);
                     * */

                }

                /*
                // ˢ�¶���
                this.AllDatabaseInfoXml = dom.OuterXml;
                 * */
                nRet = RefreshAllDatabaseXml(out strError);
                if (nRet == -1)
                    goto ERROR1;

                RefreshOpacDatabaseList();
                RefreshOpacBrowseFormatTree();

                // ���»�ø��ֿ������б�
                this.MainForm.StartPrepareNames(false);
            }
            finally
            {
                EnableControls(true);
            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // �޸����ݿ�����
        private void toolStripButton_modifyDatabase_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            if (this.listView_databases.SelectedItems.Count == 0)
            {
                strError = "��δѡ��Ҫ�޸ĵ����ݿ�";
                goto ERROR1;
            }
            ListViewItem item = this.listView_databases.SelectedItems[0];
            string strTypeName = ListViewUtil.GetItemText(item, 1);
            string strName = item.Text;

            string strType = GetTypeString(strTypeName);
            if (strType == null)
                strType = strTypeName;

            if (strType == "biblio")
            {
                BiblioDatabaseDialog dlg = new BiblioDatabaseDialog();

                dlg.Text = "�޸���Ŀ������";
                dlg.ManagerForm = this;
                dlg.CreateMode = false;
                dlg.StartPosition = FormStartPosition.CenterScreen;

                nRet = dlg.Initial((string)item.Tag,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;

                dlg.ShowDialog(this);

                if (dlg.DialogResult != DialogResult.OK)
                    return;

                // ˢ�¿����б�
                nRet = ListAllDatabases(out strError);
                if (nRet == -1)
                {
                    MessageBox.Show(this, strError);
                }

                // ѡ�����޸ĵ����ݿ�
                SelectDatabaseLine(dlg.BiblioDatabaseName);

                RefreshOpacDatabaseList();
                RefreshOpacBrowseFormatTree();

                // ���»�ø��ֿ������б�
                this.MainForm.StartPrepareNames(false);
            }
            else if (strType == "reader")
            {
                ReaderDatabaseDialog dlg = new ReaderDatabaseDialog();

                dlg.Text = "�޸Ķ��߿�����";
                dlg.ManagerForm = this;
                dlg.CreateMode = false;
                dlg.StartPosition = FormStartPosition.CenterScreen;

                nRet = dlg.Initial((string)item.Tag,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;

                dlg.ShowDialog(this);

                if (dlg.DialogResult != DialogResult.OK)
                    return;

                // ˢ�¿����б�
                nRet = ListAllDatabases(out strError);
                if (nRet == -1)
                {
                    MessageBox.Show(this, strError);
                }

                // ѡ�����޸ĵ����ݿ�
                SelectDatabaseLine(dlg.ReaderDatabaseName);

                // ���»�ø��ֿ������б�
                this.MainForm.StartPrepareNames(false);

                RefreshOpacDatabaseList();
                RefreshOpacBrowseFormatTree();
            }
            else if (strType == "publisher"
                || strType == "amerce"
                || strType == "arrived"
                || strType == "zhongcihao"
                || strType == "message")
            {
                SimpleDatabaseDialog dlg = new SimpleDatabaseDialog();

                /*
                string strTypeName = GetTypeName(strType);
                if (strTypeName == null)
                    strTypeName = strType;
                 * */

                dlg.Text = "�޸�" + strTypeName + "������";
                dlg.ManagerForm = this;
                dlg.CreateMode = false;
                dlg.StartPosition = FormStartPosition.CenterScreen;

                nRet = dlg.Initial(
                    strType,
                    (string)item.Tag,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;

                dlg.ShowDialog(this);

                if (dlg.DialogResult != DialogResult.OK)
                    return;

                // ˢ�¿����б�
                nRet = ListAllDatabases(out strError);
                if (nRet == -1)
                {
                    MessageBox.Show(this, strError);
                }

                // ѡ�����޸ĵ����ݿ�
                SelectDatabaseLine(dlg.DatabaseName);


                RefreshOpacDatabaseList();
                RefreshOpacBrowseFormatTree();

                // ���»�ø��ֿ������б�
                this.MainForm.StartPrepareNames(false);
            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        /*
        static string GetTypeName(string strType)
        {
            if (strType == "publisher")
                return "�����߿�";
            if (strType == "amerce")
                return "ΥԼ���";
            if (strType == "arrived")
                return "ԤԼ�����";
            if (strType == "biblio")
                return "��Ŀ��";
            if (strType == "entity")
                return "ʵ���";
            if (strType == "order")
                return "������";
            if (strType == "issue")
                return "�ڿ�";
            if (strType == "message")
                return "��Ϣ";

            return strType;
        }
         * */

        private void listView_databases_DoubleClick(object sender, EventArgs e)
        {
            toolStripButton_modifyDatabase_Click(sender, e);
        }

        private void listView_databases_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = null;

            /*
            ListViewItem item = null;
            
            if (this.listView_databases.SelectedItems.Count > 0)
                this.listView_databases.SelectedItems[0];
             * */

            string strName = "";
            string strType = "";
            if (this.listView_databases.SelectedItems.Count > 0)
            {
                strName = this.listView_databases.SelectedItems[0].Text;
                strType = ListViewUtil.GetItemText(this.listView_databases.SelectedItems[0], 1);
            }


            // �޸����ݿ�
            {
                menuItem = new MenuItem("�޸�" + strType + "�� '" + strName + "'(&M)");
                menuItem.Click += new System.EventHandler(this.toolStripButton_modifyDatabase_Click);
                if (this.listView_databases.SelectedItems.Count == 0)
                    menuItem.Enabled = false;
                // ȱʡ����
                menuItem.DefaultItem = true;
                contextMenu.MenuItems.Add(menuItem);
            }

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("������Ŀ��(&B)");
            menuItem.Click += new System.EventHandler(this.ToolStripMenuItem_createBiblioDatabase_Click);
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("�������߿�(&V)");
            menuItem.Click += new System.EventHandler(this.ToolStripMenuItem_createReaderDatabase_Click);
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("����ΥԼ���(&A)");
            menuItem.Click += new System.EventHandler(this.ToolStripMenuItem_createAmerceDatabase_Click);
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("����ԤԼ�����(&R)");
            menuItem.Click += new System.EventHandler(this.ToolStripMenuItem_createArrivedDatabase_Click);
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("���������߿�(&P)");
            menuItem.Click += new System.EventHandler(this.ToolStripMenuItem_createPublisherDatabase_Click);
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("������Ϣ��(&M)");
            menuItem.Click += new System.EventHandler(this.ToolStripMenuItem_createMessageDatabase_Click);
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("�����ִκſ�(&Z)");
            menuItem.Click += new System.EventHandler(this.ToolStripMenuItem_createZhongcihaoDatabase_Click);
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            string strText = "";
            if (this.listView_databases.SelectedItems.Count == 1)
                strText = "ɾ��" + strType + "�� '" + strName + "'(&D)";
            else
                strText = "ɾ����ѡ " + this.listView_databases.SelectedItems.Count.ToString() + " ��OPAC���ݿ�(&D)";

            menuItem = new MenuItem(strText);
            menuItem.Click += new System.EventHandler(this.toolStripButton_deleteDatabase_Click);
            if (this.listView_databases.SelectedItems.Count == 0)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);


            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("�۲���ѡ "+this.listView_databases.SelectedItems.Count.ToString()+" �����ݿ�Ķ���(&D)");
            menuItem.Click += new System.EventHandler(this.menu_viewDatabaseDefine_Click);
            if (this.listView_databases.SelectedItems.Count == 0)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("ˢ��(&R)");
            menuItem.Click += new System.EventHandler(this.toolStripButton_refresh_Click);
            contextMenu.MenuItems.Add(menuItem);

            contextMenu.Show(this.listView_databases, new Point(e.X, e.Y));		
        }

        // �۲����ݿⶨ��XML
        void menu_viewDatabaseDefine_Click(object sender, EventArgs e)
        {
            if (this.listView_databases.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�۲��䶨������ݿ�����");
                return;
            }

            string strXml = "";
            string strDbNameList = "";

            for (int i = 0; i < this.listView_databases.SelectedItems.Count; i++)
            {
                ListViewItem item = this.listView_databases.SelectedItems[i];
                string strName = item.Text;
                strXml += "<!-- ���ݿ� " + strName + " �Ķ��� -->";
                strXml += (string)item.Tag;

                if (String.IsNullOrEmpty(strDbNameList) == false)
                    strDbNameList += ",";
                strDbNameList += strName;
            }

            if (this.listView_databases.SelectedItems.Count > 1)
                strXml = "<root>" + strXml + "</root>";

            XmlViewerForm dlg = new XmlViewerForm();

            dlg.Text = "���ݿ�  " + strDbNameList + " �Ķ���";
            dlg.MainForm = this.MainForm;
            dlg.XmlString = strXml;
            // dlg.StartPosition = FormStartPosition.CenterScreen;
            this.MainForm.AppInfo.LinkFormState(dlg, "ManagerForm_viewXml_state");
            dlg.ShowDialog(this);
            this.MainForm.AppInfo.UnlinkFormState(dlg);
            return;
        }

        // �������߿�
        private void ToolStripMenuItem_createReaderDatabase_Click(object sender, EventArgs e)
        {
            ReaderDatabaseDialog dlg = new ReaderDatabaseDialog();

            dlg.Text = "�����¶��߿�";
            dlg.ManagerForm = this;
            dlg.CreateMode = true;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);


            if (dlg.DialogResult != DialogResult.OK)
                return;

            // ˢ�¿����б�
            string strError = "";
            int nRet = ListAllDatabases(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
            }

            // ѡ���մ��������ݿ�
            SelectDatabaseLine(dlg.ReaderDatabaseName);

            // ���»�ø��ֿ������б�
            this.MainForm.StartPrepareNames(false);
        }

        // ����ΥԼ���
        private void ToolStripMenuItem_createAmerceDatabase_Click(object sender, EventArgs e)
        {
            CreateSimpleDatabase("amerce", "", "");
        }

        // parameters:
        //      strDatabaseName ���ݿ����������Ϊ�գ���Ի������д�������������޸���
        // return:
        //      -1  errpr
        //      0   cancel
        //      1   created
        int CreateSimpleDatabase(string strType,
            string strDatabaseName,
            string strComment)
        {
            SimpleDatabaseDialog dlg = new SimpleDatabaseDialog();

            string strTypeName = GetTypeName(strType);
            if (strTypeName == null)
                strTypeName = strType;

            if (String.IsNullOrEmpty(strDatabaseName) == false)
            {
                dlg.DatabaseName = strDatabaseName;
                dlg.DatabaseNameReadOnly = true;
            }

            if (String.IsNullOrEmpty(strComment) == false)
                dlg.Comment = strComment;

            dlg.DatabaseType = strType;
            dlg.Text = "������" + strTypeName + "��";
            dlg.ManagerForm = this;
            dlg.CreateMode = true;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);


            if (dlg.DialogResult != DialogResult.OK)
                return 0;

            // ˢ�¿����б�
            string strError = "";
            int nRet = ListAllDatabases(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
                return -1;
            }

            // ѡ���մ��������ݿ�
            SelectDatabaseLine(dlg.DatabaseName);

            // ���»�ø��ֿ������б�
            this.MainForm.StartPrepareNames(false);
            return 1;
        }

        // ����ԤԼ�����
        private void ToolStripMenuItem_createArrivedDatabase_Click(object sender, EventArgs e)
        {
            CreateSimpleDatabase("arrived", "", "");
        }

        private void ToolStripMenuItem_createPublisherDatabase_Click(object sender, EventArgs e)
        {
            CreateSimpleDatabase("publisher", "", "");
        }

        private void ToolStripMenuItem_createMessageDatabase_Click(object sender, EventArgs e)
        {
            CreateSimpleDatabase("message", "", "");
        }

        private void ToolStripMenuItem_createZhongcihaoDatabase_Click(object sender, EventArgs e)
        {
            CreateSimpleDatabase("zhongcihao", "", "");
        }

        // ˢ�����ݿ����б�
        private void toolStripButton_refresh_Click(object sender, EventArgs e)
        {
            RefreshDatabaseList();
        }

        void RefreshDatabaseList()
        {
            string strError = "";
            int nRet = ListAllDatabases(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
            }
        }

        // Ϊȷ����ݶ���¼
        // return:
        //      -1  ����
        //      0   ������¼
        //      1   ��¼�ɹ�
        int ConfirmLogin(out string strError)
        {
            strError = "";

            ConfirmSupervisorDialog login_dlg = new ConfirmSupervisorDialog();
            login_dlg.UserName = this.MainForm.AppInfo.GetString(
                    "default_account",
                    "username",
                    "");
            login_dlg.ServerUrl = this.MainForm.LibraryServerUrl;
            login_dlg.Comment = "��Ҫ����ǰ����Ҫ��֤�������";

            login_dlg.StartPosition = FormStartPosition.CenterScreen;
            login_dlg.ShowDialog(this);

            if (login_dlg.DialogResult != DialogResult.OK)
                return 0;

            string strLocation = this.MainForm.AppInfo.GetString(
                "default_account",
                "location",
                "");


            // return:
            //      -1  error
            //      0   ��¼δ�ɹ�
            //      1   ��¼�ɹ�
            long lRet = this.Channel.Login(login_dlg.UserName,
                login_dlg.Password,
                strLocation,
                false,
                out strError);
            if (lRet == -1)
                return -1;


            if (lRet == 0)
            {
                // strError = "";
                return -1;
            }

            return 1;
        }

        // ��ʼ��
        private void toolStripButton_initializeDatabase_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;
            if (this.listView_databases.SelectedIndices.Count == 0)
            {
                strError = "��δѡ��Ҫ��ʼ�������ݿ�����";
                goto ERROR1;
            }

            string strDbNameList = "";
            for (int i = 0; i < this.listView_databases.SelectedItems.Count; i++)
            {
                if (i > 0)
                    strDbNameList += ",";
                strDbNameList += this.listView_databases.SelectedItems[i].Text;
            }

            // �Ի��򾯸�
            DialogResult result = MessageBox.Show(this,
                "ȷʵҪ��ʼ�����ݿ� " + strDbNameList + "?\r\n\r\n���棺1) ���ݿ�һ������ʼ�������ڵ����ݼ�¼��ȫ����ʧ������Ҳ�޷���ԭ��\r\n      2) �����ʼ��������Ŀ�⣬����Ŀ�������ʵ��⡢�����⡢�ڿ�Ҳ��һ������ʼ����",
                "ManagerForm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
                return;

            // Ϊȷ����ݶ���¼
            // return:
            //      -1  ����
            //      0   ������¼
            //      1   ��¼�ɹ�
            nRet = ConfirmLogin(out strError);
            if (nRet == -1)
                goto ERROR1;
            if (nRet == 0)
            {
                strError = "��ʼ�����ݿ����������";
                goto ERROR1;
            }

            EnableControls(false);

            try
            {

                for (int i = this.listView_databases.SelectedIndices.Count - 1;
                    i >= 0;
                    i--)
                {
                    int index = this.listView_databases.SelectedIndices[i];

                    string strDatabaseName = this.listView_databases.Items[index].Text;

                    string strOutputInfo = "";
                    nRet = InitializeDatabase(strDatabaseName,
                        out strOutputInfo,
                        out strError);
                    if (nRet == -1)
                        goto ERROR1;
                }
            }
            finally
            {
                EnableControls(true);
            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }


        #region OPAC���ݿ����ù���


        // ��listview���г����в���OPAC�����ݿ�
        int ListAllOpacDatabases(out string strError)
        {
            strError = "";

            this.listView_opacDatabases.Items.Clear();

            string strOutputInfo = "";
            int nRet = GetAllOpacDatabaseInfo(out strOutputInfo,
                    out strError);
            if (nRet == -1)
                return -1;

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<root />");

            XmlDocumentFragment fragment = dom.CreateDocumentFragment();
            try
            {
                fragment.InnerXml = strOutputInfo;
            }
            catch (Exception ex)
            {
                strError = "fragment XMLװ��XmlDocumentFragmentʱ����: " + ex.Message;
                return -1;
            }

            dom.DocumentElement.AppendChild(fragment);

            /*
        <virtualDatabase>
            <caption lang="zh-cn">�����鿯</caption>
            <caption lang="en">Chinese Books and Series</caption>
            <from style="title">
                <caption lang="zh-cn">����</caption>
                <caption lang="en">Title</caption>
            </from>
            <from style="author">
                <caption lang="zh-cn">����</caption>
                <caption lang="en">Author</caption>
            </from>
            <database name="����ͼ��" />
            <database name="�����ڿ�" />
        </virtualDatabase>
        <database name="�û�">
            <caption lang="zh-cn">�û�</caption>
            <caption lang="en">account</caption>
            <from name="�û���">
                <caption lang="zh-cn">�û���</caption>
                <caption lang="en">username</caption>
            </from>
            <from name="__id" />
        </database>
        <database name="����ͼ��">
            <caption lang="zh-cn">����ͼ��</caption>
            <caption lang="en">Chinese book</caption>
            <from name="ISBN">
                <caption lang="zh-cn">ISBN</caption>
                <caption lang="en">ISBN</caption>
            </from>
            <from name="ISSN">
                <caption lang="zh-cn">ISSN</caption>
                <caption lang="en">ISSN</caption>
            </from>
            <from name="����">
                <caption lang="zh-cn">����</caption>
                <caption lang="en">Title</caption>
            </from>
            <from name="����ƴ��">
                <caption lang="zh-cn">����ƴ��</caption>
                <caption lang="en">Title pinyin</caption>
            </from>
            <from name="�����">
                <caption lang="zh-cn">�����</caption>
                <caption lang="en">Thesaurus</caption>
            </from>
            <from name="�ؼ���">
                <caption lang="zh-cn">�ؼ���</caption>
                <caption lang="en">Keyword</caption>
            </from>
            <from name="�����">
                <caption lang="zh-cn">�����</caption>
                <caption lang="en">Class number</caption>
            </from>
            <from name="������">
                <caption lang="zh-cn">������</caption>
                <caption lang="en">Contributor</caption>
            </from>
            <from name="������ƴ��">
                <caption lang="zh-cn">������ƴ��</caption>
                <caption lang="en">Contributor pinyin</caption>
            </from>
            <from name="������">
                <caption lang="zh-cn">������</caption>
                <caption lang="en">Publisher</caption>
            </from>
            <from name="�����">
                <caption lang="zh">�����</caption>
                <caption lang="en">Call number</caption>
            </from>
            <from name="�ղص�λ">
                <caption lang="zh-cn">�ղص�λ</caption>
                <caption lang="en">Rights holder</caption>
            </from>
            <from name="�������">
                <caption lang="zh">�������</caption>
                <caption lang="en">Class of call number</caption>
            </from>
            <from name="���κ�">
                <caption lang="zh">���κ�</caption>
                <caption lang="en">Batch number</caption>
            </from>
            <from name="__id" />
        </database>
             * */


            XmlNodeList nodes = dom.DocumentElement.SelectNodes("database | virtualDatabase");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];


                string strName = DomUtil.GetAttr(node, "name");
                string strType = node.Name;

                // ����<virtualDatabase>Ԫ�أ�Ҫѡ��<caption>�������������
                if (node.Name == "virtualDatabase")
                    strName = DomUtil.GetCaption("zh", node);

                int nImageIndex = 0;
                if (strType == "virtualDatabase")
                    nImageIndex = 1;

                ListViewItem item = new ListViewItem(strName, nImageIndex);
                item.SubItems.Add(GetOpacDatabaseTypeDisplayString(strType));
                item.Tag = node.OuterXml;   // ����XML����Ƭ��

                this.listView_opacDatabases.Items.Add(item);
            }

            return 0;
        }

        // ���OPAC���ݿ����͵���ʾ�ַ���
        // ��ν��ʾ�ַ��������ǡ�����⡱ ����ͨ�⡱
        static string GetOpacDatabaseTypeDisplayString(string strType)
        {
            if (strType == "virtualDatabase")
                return "�����";

            if (strType == "database")
                return "��ͨ��";

            return strType;
        }

        // ���OPAC���ݿ����͵��ڲ�ʹ���ַ���
        // ��ν�ڲ�ʹ���ַ���������"virtualDatabase" "database"
        static string GetOpacDatabaseTypeString(string strDisplayString)
        {
            if (strDisplayString == "�����")
                return "virtualDatabase";

            if (strDisplayString == "��ͨ��")
                return "database";

            return strDisplayString;
        }

        // ���ȫ��OPAC���ݿⶨ��
        int GetAllOpacDatabaseInfo(out string strOutputInfo,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڻ�ȡȫ��OPAC���ݿⶨ�� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.GetSystemParameter(
                    stop,
                    "opac",
                    "databases",
                    out strOutputInfo,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // �޸�/����ȫ��OPAC���ݿⶨ��
        int SetAllOpacDatabaseInfo(string strDatabaseDef,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("��������ȫ��OPAC���ݿⶨ�� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.SetSystemParameter(
                    stop,
                    "opac",
                    "databases",
                    strDatabaseDef,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // �����ͨ���ݿⶨ��
        public int GetDatabaseInfo(
            string strDbName,
            out string strOutputInfo,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڻ�ȡ���ݿ� "+strDbName+" �Ķ���...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.GetSystemParameter(
                    stop,
                    "database_def",
                    strDbName,
                    out strOutputInfo,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // ��������ⶨ��
        private void toolStripMenuItem_insertOpacDatabase_virtual_Click(object sender, EventArgs e)
        {
            string strError = "";

            OpacVirtualDatabaseDialog dlg = new OpacVirtualDatabaseDialog();

            dlg.Text = "��������ⶨ��";
            /*
            dlg.ManagerForm = this;
            dlg.CreateMode = true;
             * */
            int nRet = dlg.Initial(this,
                true,
                "",
                out strError);
            if (nRet == -1)
                goto ERROR1;

            // dlg.StartPosition = FormStartPosition.CenterScreen;
            this.MainForm.AppInfo.LinkFormState(dlg, "ManagerForm_OpacVirtualDatabaseDialog_state");
            dlg.ShowDialog(this);
            this.MainForm.AppInfo.UnlinkFormState(dlg);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(dlg.Xml);
            }
            catch (Exception ex)
            {
                strError = "�ӶԻ����л�õ�XMLװ��DOMʱ����: " + ex.Message;
                goto ERROR1;
            }

            // ��<virtualDatabase>Ԫ���µ�����<caption>�У�ѡ�����ϵ�ǰ�������Ե�һ�������ַ���
            // ��һ��Ԫ�ص��¼�<caption>Ԫ����, ��ȡ���Է��ϵ�����ֵ
            string strName = DomUtil.GetCaption("zh",
                dom.DocumentElement);
            string strType = dom.DocumentElement.Name;

            ListViewItem item = new ListViewItem(strName, 1);
            item.SubItems.Add(GetOpacDatabaseTypeDisplayString(strType));
            item.Tag = dom.DocumentElement.OuterXml;   // ����XML����Ƭ��

            this.listView_opacDatabases.Items.Add(item);

            // ��Ҫ������������ύ�޸�
            nRet = SubmitOpacDatabaseDef(out strError);
            if (nRet == -1)
            {
                item.ImageIndex = 2;    // ��ʾδ���ύ����������
                goto ERROR1;
            }

            // ѡ���ող���������
            item.Selected = true;
            this.listView_opacDatabases.FocusedItem = item;

            // �۲�����ղ���������ĳ�Ա�⣬�����û�о߱�OPAC��ʾ��ʽ���壬�������Զ�����
            List<string> newly_biblio_dbnames = new List<string>();
            List<string> member_dbnames = dlg.MemberDatabaseNames;
            for (int i = 0; i < member_dbnames.Count; i++)
            {
                string strMemberDbName = member_dbnames[i];

                if (IsDatabaseBiblioType(strMemberDbName) == false)
                    continue;

                if (HasBrowseFormatDatabaseExist(strMemberDbName) == true)
                    continue;

                newly_biblio_dbnames.Add(strMemberDbName);
            }

            if (newly_biblio_dbnames.Count > 0)
            {
                DialogResult result = MessageBox.Show(this,
    "������������� " + strName + " ���Ա���У��� " + StringUtil.MakePathList(newly_biblio_dbnames) + " ��û��OPAC��¼��ʾ��ʽ���塣\r\n\r\nҪ�Զ�����(��)���������OPAC��¼��ʾ��ʽ����ô? ",
    "ManagerForm",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question,
    MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    for (int i = 0; i < newly_biblio_dbnames.Count; i++)
                    {

                        // Ϊ��Ŀ�����OPAC��ʾ��ʽ�ڵ�(���)
                        nRet = NewBiblioOpacBrowseFormat(newly_biblio_dbnames[i],
                            out strError);
                        if (nRet == -1)
                            goto ERROR1;
                    }
                }
            }
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // �����������������Ƿ�͵�ǰ�Ѿ����ڵ���������ظ�
        // return:
        //      -1  ���Ĺ��̷�������
        //      0   û���ظ�
        //      1   ���ظ�
        public int DetectVirtualDatabaseNameDup(string strCaptionsXml,
            out string strError)
        {
            strError = "";

            XmlDocument domCaptions = new XmlDocument();
            domCaptions.LoadXml("<root />");
            domCaptions.DocumentElement.InnerXml = strCaptionsXml;

            XmlNodeList nodes = domCaptions.DocumentElement.SelectNodes("caption");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                string strOneLang = DomUtil.GetAttr(node, "lang");
                string strOneName = node.InnerText;

                for (int j = 0; j < this.listView_opacDatabases.Items.Count; j++)
                {
                    ListViewItem item = this.listView_opacDatabases.Items[j];

                    string strName = ListViewUtil.GetItemText(item, 0);

                    string strXml = (string)item.Tag;
                    string strType = GetOpacDatabaseTypeString(ListViewUtil.GetItemText(item, 1));
                    if (strType == "virtualDatabase")
                    {
                        XmlDocument temp = new XmlDocument();
                        try
                        {
                            temp.LoadXml(strXml);
                        }
                        catch (Exception ex)
                        {
                            strError = "����� '" + strName + "' ��XML����װ��DOM�����г���: " + ex.Message;
                            return -1;
                        }
                        XmlNodeList exist_nodes = temp.DocumentElement.SelectNodes("caption");
                        for (int k = 0; k < exist_nodes.Count; k++)
                        {
                            string strExistLang = DomUtil.GetAttr(exist_nodes[k], "lang");
                            string strExistName = exist_nodes[k].InnerText;

                            if (strExistName == strOneName)
                            {
                                strError = "���Դ��� '" + strOneLang + "' �µ�������� '" + strOneName + "' �͵�ǰ�Ѿ����ڵ��б��е� " + (j + 1).ToString() + " �е����� '"+strExistLang+"' �µ�������� '"+strExistName+"' �������ظ�";
                                return 1;
                            }
                        }
                    }
                    else if (strType == "database")
                    {
                        if (strName == strOneName)
                        {
                            strError = "���Դ��� '" + strOneLang + "' �µ�������� '" + strOneName + "' �͵�ǰ�Ѿ����ڵ���ͨ����(�б��е� "+(j+1).ToString()+" ��)�������ظ�";
                            return 1;
                        }
                    }
                }
            }

            return 0;
        }

        private void listView_opacDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_opacDatabases.SelectedItems.Count > 0)
            {
                this.toolStripButton_modifyOpacDatabase.Enabled = true;
                this.toolStripButton_removeOpacDatabase.Enabled = true;
            }
            else
            {
                this.toolStripButton_modifyOpacDatabase.Enabled = false;
                this.toolStripButton_removeOpacDatabase.Enabled = false;
            }
        }

        private void listView_opacDatabases_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = null;

            string strName = "";
            string strType = "";
            if (this.listView_opacDatabases.SelectedItems.Count > 0)
            {
                strName = this.listView_opacDatabases.SelectedItems[0].Text;
                strType = ListViewUtil.GetItemText(this.listView_opacDatabases.SelectedItems[0], 1);
            }


            // �޸�OPAC���ݿ�
            {
                menuItem = new MenuItem("�޸�" + strType + " " + strName + "(&M)");
                menuItem.Click += new System.EventHandler(this.toolStripButton_modifyOpacDatabase_Click);
                if (this.listView_opacDatabases.SelectedItems.Count == 0)
                    menuItem.Enabled = false;
                // ȱʡ����
                menuItem.DefaultItem = true;
                contextMenu.MenuItems.Add(menuItem);
            }


            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("������ͨ��(&N)");
            menuItem.Click += new System.EventHandler(this.toolStripMenuItem_insertOpacDatabase_normal_Click);
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("���������(&V)");
            menuItem.Click += new System.EventHandler(this.toolStripMenuItem_insertOpacDatabase_virtual_Click);
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            string strText = "";
            if (this.listView_opacDatabases.SelectedItems.Count == 1)
                strText = "�Ƴ�" + strType + " " + strName + "(&D)";
            else
                strText = "�Ƴ���ѡ " + this.listView_opacDatabases.SelectedItems.Count.ToString() + " ��OPAC���ݿ�(&D)";

            menuItem = new MenuItem(strText);
            menuItem.Click += new System.EventHandler(this.toolStripButton_removeOpacDatabase_Click);
            if (this.listView_opacDatabases.SelectedItems.Count == 0)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);


            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);


            menuItem = new MenuItem("�۲���ѡ " + this.listView_opacDatabases.SelectedItems.Count.ToString() + " ��OPAC���ݿ�Ķ���(&D)");
            menuItem.Click += new System.EventHandler(this.menu_viewOpacDatabaseDefine_Click);
            if (this.listView_opacDatabases.SelectedItems.Count == 0)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);


            // 
            menuItem = new MenuItem("����(&U)");
            menuItem.Click += new System.EventHandler(this.menu_opacDatabase_up_Click);
            if (this.listView_opacDatabases.SelectedItems.Count == 0
                || this.listView_opacDatabases.Items.IndexOf(this.listView_opacDatabases.SelectedItems[0]) == 0)
                menuItem.Enabled = false;
            else
                menuItem.Enabled = true;
            contextMenu.MenuItems.Add(menuItem);



            // 
            menuItem = new MenuItem("����(&D)");
            menuItem.Click += new System.EventHandler(this.menu_opacDatabase_down_Click);
            if (this.listView_opacDatabases.SelectedItems.Count == 0
                || this.listView_opacDatabases.Items.IndexOf(this.listView_opacDatabases.SelectedItems[0]) >= this.listView_opacDatabases.Items.Count - 1)
                menuItem.Enabled = false;
            else
                menuItem.Enabled = true;
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("ˢ��(&R)");
            menuItem.Click += new System.EventHandler(this.toolStripButton_refreshOpacDatabaseList_Click);
            contextMenu.MenuItems.Add(menuItem);

            contextMenu.Show(this.listView_opacDatabases, new Point(e.X, e.Y));		

        }

        void menu_opacDatabase_up_Click(object sender, EventArgs e)
        {
            MoveOpacDatabaseItemUpDown(true);
        }

        void menu_opacDatabase_down_Click(object sender, EventArgs e)
        {
            MoveOpacDatabaseItemUpDown(false);
        }


        void MoveOpacDatabaseItemUpDown(bool bUp)
        {
            string strError = "";
            int nRet = 0;

            if (this.listView_opacDatabases.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ���������ƶ���OPAC���ݿ�����");
                return;
            }

            ListViewItem item = this.listView_opacDatabases.SelectedItems[0];
            int index = this.listView_opacDatabases.Items.IndexOf(item);

            Debug.Assert(index >= 0 && index <= this.listView_opacDatabases.Items.Count - 1,"");

            bool bChanged = false;

            if (bUp == true)
            {
                if (index == 0)
                {
                    strError = "��ͷ";
                    goto ERROR1;
                }

                this.listView_opacDatabases.Items.RemoveAt(index);
                index--;
                this.listView_opacDatabases.Items.Insert(index, item);
                this.listView_opacDatabases.FocusedItem = item;

                bChanged = true;
            }

            if (bUp == false)
            {
                if (index >= this.listView_opacDatabases.Items.Count - 1)
                {
                    strError = "��β";
                    goto ERROR1;
                }
                this.listView_opacDatabases.Items.RemoveAt(index);
                index++;
                this.listView_opacDatabases.Items.Insert(index, item);
                this.listView_opacDatabases.FocusedItem = item;

                bChanged = true;
            }


            // TODO: �Ƿ�����ӳ��ύ?
            if (bChanged == true)
            {
                // ��Ҫ������������ύ�޸�
                nRet = SubmitOpacDatabaseDef(out strError);
                if (nRet == -1)
                {
                    // TODO: ��α�ʾδ���ύ������λ���ƶ�����?
                    goto ERROR1;
                }
            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // �۲�OPAC���ݿⶨ��XML
        void menu_viewOpacDatabaseDefine_Click(object sender, EventArgs e)
        {
            if (this.listView_opacDatabases.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�۲��䶨���OPAC���ݿ�����");
                return;
            }

            string strXml = "";
            string strDbNameList = "";

            for (int i = 0; i < this.listView_opacDatabases.SelectedItems.Count; i++)
            {
                ListViewItem item = this.listView_opacDatabases.SelectedItems[i];
                string strName = item.Text;
                strXml += "<!-- OPAC���ݿ� " + strName + " �Ķ��� -->";
                strXml += (string)item.Tag;

                if (String.IsNullOrEmpty(strDbNameList) == false)
                    strDbNameList += ",";
                strDbNameList += strName;
            }

            if (this.listView_opacDatabases.SelectedItems.Count > 1)
                strXml = "<virtualDatabases>" + strXml + "</virtualDatabases>";


            XmlViewerForm dlg = new XmlViewerForm();

            dlg.Text = "OPAC���ݿ�  " + strDbNameList + " �Ķ���";
            dlg.MainForm = this.MainForm;
            dlg.XmlString = strXml;
            // dlg.StartPosition = FormStartPosition.CenterScreen;

            this.MainForm.AppInfo.LinkFormState(dlg, "ManagerForm_viewXml_state");
            dlg.ShowDialog(this);
            this.MainForm.AppInfo.UnlinkFormState(dlg);

            return;
        }

        // �޸�OPAC���ݿⶨ��
        private void toolStripButton_modifyOpacDatabase_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            if (this.listView_opacDatabases.SelectedItems.Count == 0)
            {
                strError = "��δѡ��Ҫ�޸ĵ�OPAC���ݿ�����";
                goto ERROR1;
            }

            ListViewItem item = this.listView_opacDatabases.SelectedItems[0];

            string strType = GetOpacDatabaseTypeString(ListViewUtil.GetItemText(item, 1));

            if (strType == "virtualDatabase")
            {

                OpacVirtualDatabaseDialog dlg = new OpacVirtualDatabaseDialog();

                dlg.Text = "�޸�����ⶨ��";
                /*
                dlg.ManagerForm = this;
                dlg.CreateMode = false;
                 * */

                nRet = dlg.Initial(this,
                    false,
                    (string)item.Tag,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;

                // dlg.StartPosition = FormStartPosition.CenterScreen;
                this.MainForm.AppInfo.LinkFormState(dlg, "ManagerForm_OpacVirtualDatabaseDialog_state");
                dlg.ShowDialog(this);
                this.MainForm.AppInfo.UnlinkFormState(dlg);


                if (dlg.DialogResult != DialogResult.OK)
                    return;

                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.LoadXml(dlg.Xml);
                }
                catch (Exception ex)
                {
                    strError = "�ӶԻ����л�õ�XMLװ��DOMʱ����: " + ex.Message;
                    goto ERROR1;
                }

                // ��<virtualDatabase>Ԫ���µ�����<caption>�У�ѡ�����ϵ�ǰ�������Ե�һ�������ַ���
                // ��һ��Ԫ�ص��¼�<caption>Ԫ����, ��ȡ���Է��ϵ�����ֵ
                string strName = DomUtil.GetCaption("zh",
                    dom.DocumentElement);

                strType = dom.DocumentElement.Name;

                item.Text = strName;
                ListViewUtil.ChangeItemText(item, 1, GetOpacDatabaseTypeDisplayString(strType));
                item.Tag = dlg.Xml;   // ����XML����Ƭ��

                // ��Ҫ������������ύ�޸�
                nRet = SubmitOpacDatabaseDef(out strError);
                if (nRet == -1)
                {
                    item.ImageIndex = 2;    // ��ʾδ���ύ���޸�����
                    goto ERROR1;
                }


                // �۲�������޸ĵ������ĳ�Ա�⣬�����û�о߱�OPAC��ʾ��ʽ���壬�������Զ�����
                List<string> newly_biblio_dbnames = new List<string>();
                List<string> member_dbnames = dlg.MemberDatabaseNames;
                for (int i = 0; i < member_dbnames.Count; i++)
                {
                    string strMemberDbName = member_dbnames[i];

                    if (IsDatabaseBiblioType(strMemberDbName) == false)
                        continue;

                    if (HasBrowseFormatDatabaseExist(strMemberDbName) == true)
                        continue;

                    newly_biblio_dbnames.Add(strMemberDbName);
                }

                if (newly_biblio_dbnames.Count > 0)
                {
                    DialogResult result = MessageBox.Show(this,
        "�ձ��޸ĵ������ " + strName + " ���Ա���У��� " + StringUtil.MakePathList(newly_biblio_dbnames) + " ��û��OPAC��¼��ʾ��ʽ���塣\r\n\r\nҪ�Զ�����(��)���������OPAC��¼��ʾ��ʽ����ô? ",
        "ManagerForm",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {
                        for (int i = 0; i < newly_biblio_dbnames.Count; i++)
                        {

                            // Ϊ��Ŀ�����OPAC��ʾ��ʽ�ڵ�(���)
                            nRet = NewBiblioOpacBrowseFormat(newly_biblio_dbnames[i],
                                out strError);
                            if (nRet == -1)
                                goto ERROR1;
                        }
                    }
                }
            }
            else if (strType == "database")
            {
                OpacNormalDatabaseDialog dlg = new OpacNormalDatabaseDialog();

                string strXml = (string)item.Tag;

                XmlDocument dom = new XmlDocument();
                try {
                dom.LoadXml(strXml);
                }
                catch (Exception ex)
                {
                    strError = "XMLװ��DOMʱ����: " + ex.Message;
                    goto ERROR1;
                }

                dlg.Text = "��ͨ����";
                dlg.ManagerForm = this;
                dlg.DatabaseName = DomUtil.GetAttr(dom.DocumentElement, "name");
                this.MainForm.AppInfo.LinkFormState(dlg, "ManagerForm_OpacNormalDatabaseDialog_state");
                dlg.ShowDialog(this);
                this.MainForm.AppInfo.UnlinkFormState(dlg);


                if (dlg.DialogResult != DialogResult.OK)
                    return;

                DomUtil.SetAttr(dom.DocumentElement, "name", dlg.DatabaseName);

                item.Text = dlg.DatabaseName;
                item.Tag = dom.DocumentElement.OuterXml;   // ����XML����Ƭ��

                // ��Ҫ������������ύ�޸�
                nRet = SubmitOpacDatabaseDef(out strError);
                if (nRet == -1)
                {
                    item.ImageIndex = 2;    // ��ʾδ���ύ���޸�����
                    goto ERROR1;
                }

            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // �޸�OPAC���ݿⶨ��
        private void listView_opacDatabases_DoubleClick(object sender, EventArgs e)
        {
            toolStripButton_modifyOpacDatabase_Click(sender, e);
        }

        // ����OPAC��ͨ��
        private void toolStripMenuItem_insertOpacDatabase_normal_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            // �Ѿ����ڵĿ���
            List<string> existing_dbnames = new List<string>();
            for (int i = 0; i < this.listView_opacDatabases.Items.Count; i++)
            {
                existing_dbnames.Add(this.listView_opacDatabases.Items[i].Text);
            }

            OpacNormalDatabaseDialog dlg = new OpacNormalDatabaseDialog();

            dlg.Text = "������ͨ�ⶨ��";
            dlg.ManagerForm = this;
            dlg.ExcludingDbNames = existing_dbnames;

            this.MainForm.AppInfo.LinkFormState(dlg, "ManagerForm_OpacNormalDatabaseDialog_state");
            dlg.ShowDialog(this);
            this.MainForm.AppInfo.UnlinkFormState(dlg);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<database name='' />");


            // ��<virtualDatabase>Ԫ���µ�����<caption>�У�ѡ�����ϵ�ǰ�������Ե�һ�������ַ���
            // ��һ��Ԫ�ص��¼�<caption>Ԫ����, ��ȡ���Է��ϵ�����ֵ
            string strName = dlg.DatabaseName;
            string strType = "database";

            DomUtil.SetAttr(dom.DocumentElement, "name", strName);

            ListViewItem item = new ListViewItem(strName, 0);
            item.SubItems.Add(GetOpacDatabaseTypeDisplayString(strType));
            item.Tag = dom.DocumentElement.OuterXml;   // ����XML����Ƭ��

            this.listView_opacDatabases.Items.Add(item);

            // ��Ҫ������������ύ�޸�
            nRet = SubmitOpacDatabaseDef(out strError);
            if (nRet == -1)
            {
                item.ImageIndex = 2;    // ��ʾδ���ύ����������
                goto ERROR1;
            }

            // ѡ���ող������ͨ��
            item.Selected = true;
            this.listView_opacDatabases.FocusedItem = item;

            // �������Ŀ�⣬����������ݿ����ʾ��ʽ�����Ƿ��Ѿ����ڣ�
            // ��������ڣ���ʾ���뽨��
            if (IsDatabaseBiblioType(strName) == true
                && HasBrowseFormatDatabaseExist(strName) == false)
            {
                DialogResult result = MessageBox.Show(this,
                    "����������Ŀ�� "+strName+" ��û��OPAC��¼��ʾ��ʽ���塣\r\n\r\nҪ�Զ��������������OPAC��¼��ʾ��ʽ����ô? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                            // Ϊ��Ŀ�����OPAC��ʾ��ʽ�ڵ�(���)
                    nRet = NewBiblioOpacBrowseFormat(strName,
                        out strError);
                    if (nRet == -1)
                        goto ERROR1;
                }

            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }




        // �ύOPAC���ݿⶨ���޸�
        int SubmitOpacDatabaseDef(out string strError)
        {
            strError = "";
            string strDatabaseDef = "";
            int nRet = BuildOpacDatabaseDef(out strDatabaseDef,
                out strError);
            if (nRet == -1)
                return -1;

            nRet = SetAllOpacDatabaseInfo(strDatabaseDef,
                out strError);
            if (nRet == -1)
                return -1;

            return 0;
        }


        // ����OPAC���ݿⶨ���XMLƬ��
        // ע�����¼�Ƭ�϶��壬û��<virtualDatabases>Ԫ����Ϊ����
        int BuildOpacDatabaseDef(out string strDatabaseDef,
            out string strError)
        {
            strError = "";
            strDatabaseDef = "";

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<virtualDatabases />");

            for (int i = 0; i < this.listView_opacDatabases.Items.Count; i++)
            {
                ListViewItem item = this.listView_opacDatabases.Items[i];
                string strName = item.Text;
                string strType = ListViewUtil.GetItemText(item, 1);

                string strXmlFragment = (string)item.Tag;

                XmlDocumentFragment fragment = dom.CreateDocumentFragment();
                try
                {
                    fragment.InnerXml = strXmlFragment;
                }
                catch (Exception ex)
                {
                    strError = "fragment XMLװ��XmlDocumentFragmentʱ����: " + ex.Message;
                    return -1;
                }

                dom.DocumentElement.AppendChild(fragment);
            }

            strDatabaseDef = dom.DocumentElement.InnerXml;

            return 0;
        }

        // �Ƴ�һ��OPAC���ݿ�
        private void toolStripButton_removeOpacDatabase_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            if (this.listView_opacDatabases.SelectedItems.Count == 0)
            {
                strError = "��δѡ��Ҫ�Ƴ���OPAC���ݿ�����";
                goto ERROR1;
            }

            string strDbNameList = "";
            for (int i = 0; i < this.listView_opacDatabases.SelectedItems.Count; i++)
            {
                if (i > 0)
                    strDbNameList += ",";
                strDbNameList += this.listView_opacDatabases.SelectedItems[i].Text;
            }

            // �Ի��򾯸�
            DialogResult result = MessageBox.Show(this,
                "ȷʵҪ�Ƴ�OPAC���ݿ� " + strDbNameList + "?\r\n\r\nע���Ƴ����ݿⲻ��ɾ�����ݿ⣬ֻ��ʹ��Щ���ݿⲻ�ܱ�OPAC��������",
                "ManagerForm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
                return;

            for (int i = this.listView_opacDatabases.SelectedIndices.Count - 1;
                i >= 0;
                i--)
            {
                int index = this.listView_opacDatabases.SelectedIndices[i];
                string strDatabaseName = this.listView_opacDatabases.Items[index].Text;
                this.listView_opacDatabases.Items.RemoveAt(index);
            }


            // ��Ҫ������������ύ�޸�
            nRet = SubmitOpacDatabaseDef(out strError);
            if (nRet == -1)
            {
                // TODO: �Ƿ���Ҫ�Ѹղ�ɾ������������ȥ��
                // item.ImageIndex = 2;    // ��ʾδ���ύ���޸�����
                goto ERROR1;
            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);

        }

        private void toolStripButton_refreshOpacDatabaseList_Click(object sender, EventArgs e)
        {
            RefreshOpacDatabaseList();
        }


        void RefreshOpacDatabaseList()
        {
            string strError = "";
            int nRet = ListAllOpacDatabases(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
            }
        }

        #endregion // of OPAC���ݿ����ù���

        // ����������ݿ��ڵļ�¼��Ҳ���ǳ�ʼ���������ݿ����˼��
        private void toolStripButton_initialAllDatabases_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = ClearAllDbs(out strError);
            if (nRet == -1)
                MessageBox.Show(this, strError);
            else
                MessageBox.Show(this, "OK");
        }

        #region OPAC��¼��ʾ��ʽ

        // ��treeview���г�����OPAC������ʾ��ʽ
        int ListAllOpacBrowseFormats(out string strError)
        {
            strError = "";

            this.treeView_opacBrowseFormats.Nodes.Clear();

            string strOutputInfo = "";
            int nRet = GetAllOpacBrowseFormats(out strOutputInfo,
                    out strError);
            if (nRet == -1)
                return -1;

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<root />");

            XmlDocumentFragment fragment = dom.CreateDocumentFragment();
            try
            {
                fragment.InnerXml = strOutputInfo;
            }
            catch (Exception ex)
            {
                strError = "fragment XMLװ��XmlDocumentFragmentʱ����: " + ex.Message;
                return -1;
            }

            dom.DocumentElement.AppendChild(fragment);

            /*
    <browseformats>
        <database name="����ͼ��">
            <format name="��ϸ" type="biblio" />
        </database>
    	<database name="��ɫ��Դ">
	    	<format name="��ϸ" scriptfile="./cfgs/opac_detail.fltx" />
	    </database>
        <database name="����">
            <format name="��ϸ" scriptfile="./cfgs/opac_detail.cs" />
        </database>
    </browseformats>
             * */


            XmlNodeList database_nodes = dom.DocumentElement.SelectNodes("database");
            for (int i = 0; i < database_nodes.Count; i++)
            {
                XmlNode node = database_nodes[i];

                string strDatabaseName = DomUtil.GetAttr(node, "name");

                TreeNode database_treenode = new TreeNode(strDatabaseName, 0, 0);

                this.treeView_opacBrowseFormats.Nodes.Add(database_treenode);

                // �����ʽ�ڵ�
                XmlNodeList format_nodes = node.SelectNodes("format");
                for (int j = 0; j < format_nodes.Count; j++)
                {
                    XmlNode format_node = format_nodes[j];

                    string strFormatName = DomUtil.GetAttr(format_node, "name");
                    string strType = DomUtil.GetAttr(format_node, "type");
                    string strScriptFile = DomUtil.GetAttr(format_node, "scriptfile");

                    string strDisplayText = strFormatName;
                    
                    if (String.IsNullOrEmpty(strType) == false)
                        strDisplayText += " type=" + strType;

                    if (String.IsNullOrEmpty(strScriptFile) == false)
                        strDisplayText += " scriptfile=" + strScriptFile;

                    TreeNode format_treenode = new TreeNode(strDisplayText, 1, 1);
                    format_treenode.Tag = format_node.OuterXml;

                    database_treenode.Nodes.Add(format_treenode);
                }
            }

            this.treeView_opacBrowseFormats.ExpandAll();

            return 0;

        }

        // ���ȫ��OPAC�����ʽ����
        int GetAllOpacBrowseFormats(out string strOutputInfo,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڻ�ȡȫ��OPAC��¼��ʾ��ʽ���� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.GetSystemParameter(
                    stop,
                    "opac",
                    "browseformats",
                    out strOutputInfo,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // �޸�/����ȫ��OPAC��¼��ʾ��ʽ����
        int SetAllOpacBrowseFormatsDef(string strDatabaseDef,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("��������ȫ��OPAC��¼��ʾ��ʽ���� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.SetSystemParameter(
                    stop,
                    "opac",
                    "browseformats",
                    strDatabaseDef,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // ��������ڵ�(���)
        private void toolStripMenuItem_opacBrowseFormats_insertDatabaseNameNode_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            // ��ǰ�ڵ�
            TreeNode current_treenode = this.treeView_opacBrowseFormats.SelectedNode;

            // ������Ǹ����Ľڵ㣬�������ҵ�������
            if (current_treenode != null && current_treenode.Parent != null)
            {
                current_treenode = current_treenode.Parent;
            }

            // �����
            int index = this.treeView_opacBrowseFormats.Nodes.IndexOf(current_treenode);
            if (index == -1)
                index = this.treeView_opacBrowseFormats.Nodes.Count;
            else
                index++;
            

            // ��ǰ�Ѿ����ڵ����ݿ���������Ҫ�ų���
            List<string> existing_dbnames = new List<string>();
            for (int i = 0; i < this.treeView_opacBrowseFormats.Nodes.Count; i++)
            {
                string strDatabaseName = this.treeView_opacBrowseFormats.Nodes[i].Text;
                existing_dbnames.Add(strDatabaseName);
            }

            // ѯ�����ݿ���
            OpacNormalDatabaseDialog dlg = new OpacNormalDatabaseDialog();

            dlg.Text = "��ָ�����ݿ���";
            dlg.ManagerForm = this;
            dlg.DatabaseName = "";
            dlg.ExcludingDbNames = existing_dbnames;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            TreeNode new_treenode = new TreeNode(dlg.DatabaseName, 0, 0);

            this.treeView_opacBrowseFormats.Nodes.Insert(index, new_treenode);

            this.treeView_opacBrowseFormats.SelectedNode = new_treenode;

            // ��Ҫ������������ύ�޸�
            nRet = SubmitOpacBrowseFormatDef(out strError);
            if (nRet == -1)
            {
                new_treenode.ImageIndex = 2;    // ��ʾδ���ύ���²���ڵ�����
                goto ERROR1;
            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);

        }

        private void treeView_opacBrowseFormats_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode current_treenode = this.treeView_opacBrowseFormats.SelectedNode;

            // �����ʽ�ڵ�Ĳ˵��ֻ���ڵ�ǰ�ڵ�Ϊ���ݿ����ͻ��߸�ʽ����ʱ����enabled

            if (current_treenode == null)
            {
                this.toolStripMenuItem_opacBrowseFormats_insertBrowseFormatNode.Enabled = false;
                this.toolStripButton_opacBrowseFormats_modify.Enabled = false;
                this.toolStripButton_opacBrowseFormats_remove.Enabled = false;
            }
            else
            {
                this.toolStripMenuItem_opacBrowseFormats_insertBrowseFormatNode.Enabled = true;
                this.toolStripButton_opacBrowseFormats_modify.Enabled = true;
                this.toolStripButton_opacBrowseFormats_remove.Enabled = true;
            }
        }

        // ������ʾ��ʽ�ڵ�(���)
        private void toolStripMenuItem_opacBrowseFormats_insertBrowseFormatNode_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            // ��ǰ�ڵ�
            TreeNode current_treenode = this.treeView_opacBrowseFormats.SelectedNode;

            if (current_treenode == null)
            {
                MessageBox.Show(this, "��δѡ���������ʽ���ڵ㣬����޷������µ���ʾ��ʽ�ڵ�");
                return;
            }

            int index = -1;

            Debug.Assert(current_treenode != null, "");

            // ����ǵ�һ���Ľڵ㣬�����Ϊ���뵽���Ķ��ӵ�β��
            if (current_treenode.Parent == null)
            {
                Debug.Assert(current_treenode != null, "");

                index = current_treenode.Nodes.Count;
            }
            else
            {
                index = current_treenode.Parent.Nodes.IndexOf(current_treenode);

                Debug.Assert(index != -1, "");

                index++;

                current_treenode = current_treenode.Parent; 
            }

            // ���ˣ�current_treenodeΪ���ݿ����͵Ľڵ���


            // �µ���ʾ��ʽ
            OpacBrowseFormatDialog dlg = new OpacBrowseFormatDialog();

            // TODO: ������ݿ�Ϊ��Ŀ�⣬��typeӦ��Ԥ��Ϊ"biblio"
            dlg.Text = "��ָ����ʾ��ʽ������";
            dlg.FormatName = "";
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<format />");

            string strDisplayText = dlg.FormatName;
            DomUtil.SetAttr(dom.DocumentElement, "name", dlg.FormatName);

            if (String.IsNullOrEmpty(dlg.FormatType) == false)
            {
                strDisplayText += " type=" + dlg.FormatType;
                DomUtil.SetAttr(dom.DocumentElement, "type", dlg.FormatType);
            }

            if (String.IsNullOrEmpty(dlg.ScriptFile) == false)
            {
                strDisplayText += " scriptfile=" + dlg.ScriptFile;
                DomUtil.SetAttr(dom.DocumentElement, "scriptfile", dlg.ScriptFile);
            }


            TreeNode new_treenode = new TreeNode(strDisplayText, 1, 1);
            new_treenode.Tag = dom.DocumentElement.OuterXml;

            current_treenode.Nodes.Insert(index, new_treenode);

            this.treeView_opacBrowseFormats.SelectedNode = new_treenode;

            // ��Ҫ������������ύ�޸�
            nRet = SubmitOpacBrowseFormatDef(out strError);
            if (nRet == -1)
            {
                new_treenode.ImageIndex = 2;    // ��ʾδ���ύ���²���ڵ�����
                goto ERROR1;
            }
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // �޸�һ���ڵ�Ķ���
        private void toolStripButton_opacBrowseFormats_modify_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            // ��ǰ�ڵ�
            TreeNode current_treenode = this.treeView_opacBrowseFormats.SelectedNode;

            if (current_treenode == null)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�޸ĵĿ������ʽ�ڵ�");
                return;
            }

            if (current_treenode.Parent == null)
            {
                // �����ڵ�


                // ��ǰ�Ѿ����ڵ����ݿ���������Ҫ�ų���
                List<string> existing_dbnames = new List<string>();
                for (int i = 0; i < this.treeView_opacBrowseFormats.Nodes.Count; i++)
                {
                    string strDatabaseName = this.treeView_opacBrowseFormats.Nodes[i].Text;
                    existing_dbnames.Add(strDatabaseName);
                }

                OpacNormalDatabaseDialog dlg = new OpacNormalDatabaseDialog();

                dlg.Text = "�޸����ݿ���";
                dlg.ManagerForm = this;
                dlg.DatabaseName = current_treenode.Text;
                dlg.ExcludingDbNames = existing_dbnames;
                dlg.StartPosition = FormStartPosition.CenterScreen;
                dlg.ShowDialog(this);

                if (dlg.DialogResult != DialogResult.OK)
                    return;

                current_treenode.Text = dlg.DatabaseName;

                // ȷ��չ��
                current_treenode.Parent.Expand();

                // ��Ҫ������������ύ�޸�
                nRet = SubmitOpacBrowseFormatDef(out strError);
                if (nRet == -1)
                {
                    current_treenode.ImageIndex = 2;    // ��ʾδ���ύ�Ķ���仯����
                    goto ERROR1;
                }
            }
            else
            {
                // ��ʽ�ڵ�

                string strXml = (string)current_treenode.Tag;

                if (String.IsNullOrEmpty(strXml) == true)
                {
                    strError = "�ڵ� " + current_treenode.Text + " û��Tag����";
                    goto ERROR1;
                }

                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.LoadXml(strXml);
                }
                catch (Exception ex)
                {
                    strError = "XMLװ��DOMʱ����: " + ex.Message;
                    goto ERROR1;
                }


                // �µ���ʾ��ʽ
                OpacBrowseFormatDialog dlg = new OpacBrowseFormatDialog();

                dlg.Text = "��ָ����ʾ��ʽ������";
                dlg.FormatName = DomUtil.GetAttr(dom.DocumentElement, "name");
                dlg.FormatType = DomUtil.GetAttr(dom.DocumentElement, "type");
                dlg.ScriptFile = DomUtil.GetAttr(dom.DocumentElement, "scriptfile");
                dlg.StartPosition = FormStartPosition.CenterScreen;
                dlg.ShowDialog(this);

                if (dlg.DialogResult != DialogResult.OK)
                    return;


                string strDisplayText = dlg.FormatName;
                DomUtil.SetAttr(dom.DocumentElement, "name", dlg.FormatName);

                if (String.IsNullOrEmpty(dlg.FormatType) == false)
                {
                    strDisplayText += " type=" + dlg.FormatType;
                    DomUtil.SetAttr(dom.DocumentElement, "type", dlg.FormatType);
                }

                if (String.IsNullOrEmpty(dlg.ScriptFile) == false)
                {
                    strDisplayText += " scriptfile=" + dlg.ScriptFile;
                    DomUtil.SetAttr(dom.DocumentElement, "scriptfile", dlg.ScriptFile);
                }

                current_treenode.Tag = dom.DocumentElement.OuterXml;

                // ȷ��չ��
                current_treenode.Parent.Expand();


                // ��Ҫ������������ύ�޸�
                nRet = SubmitOpacBrowseFormatDef(out strError);
                if (nRet == -1)
                {
                    current_treenode.ImageIndex = 2;    // ��ʾδ���ύ�Ķ���仯����
                    goto ERROR1;
                }
            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // popup menu
        private void treeView_opacBrowseFormats_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = null;

            TreeNode node = this.treeView_opacBrowseFormats.SelectedNode;

            //
            menuItem = new MenuItem("�޸�(&M)");
            menuItem.Click += new System.EventHandler(this.toolStripButton_opacBrowseFormats_modify_Click);
            if (node == null)
            {
                menuItem.Enabled = false;
            }

            // ȱʡ����
            if (node != null && node.Parent != null)
                menuItem.DefaultItem = true;
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            // ��������ڵ�
            string strText = "";
            if (node == null)
                strText = "[׷�ӵ���һ��ĩβ]";
            else if (node.Parent == null)
                strText = "[ͬ�����]";
            else
                strText = "[׷�ӵ���һ��ĩβ]";

            menuItem = new MenuItem("���������ڵ�(&N) " + strText);
            menuItem.Click += new System.EventHandler(this.toolStripMenuItem_opacBrowseFormats_insertDatabaseNameNode_Click);
            contextMenu.MenuItems.Add(menuItem);



            // ������ʾ��ʽ�ڵ�
            if (node == null)
                strText = "";   // ����������������
            else if (node.Parent == null)
                strText = "[׷�ӵ��¼�ĩβ]";
            else
                strText = "[ͬ�����]";

            menuItem = new MenuItem("������ʾ��ʽ�ڵ�(&F) " + strText);
            menuItem.Click += new System.EventHandler(this.toolStripMenuItem_opacBrowseFormats_insertBrowseFormatNode_Click);
            if (node == null)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);


            // 
            menuItem = new MenuItem("����(&U)");
            menuItem.Click += new System.EventHandler(this.menu_opacBrowseFormatNode_up_Click);
            if (this.treeView_opacBrowseFormats.SelectedNode == null
                || this.treeView_opacBrowseFormats.SelectedNode.PrevNode == null)
                menuItem.Enabled = false;
            else
                menuItem.Enabled = true;
            contextMenu.MenuItems.Add(menuItem);



            // 
            menuItem = new MenuItem("����(&D)");
            menuItem.Click += new System.EventHandler(this.menu_opacBrowseFormatNode_down_Click);
            if (treeView_opacBrowseFormats.SelectedNode == null
                || treeView_opacBrowseFormats.SelectedNode.NextNode == null)
                menuItem.Enabled = false;
            else
                menuItem.Enabled = true;
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            //
            menuItem = new MenuItem("�Ƴ�(&E)");
            menuItem.Click += new System.EventHandler(this.toolStripButton_opacBrowseFormats_remove_Click);
            if (node == null)
            {
                menuItem.Enabled = false;
            }
            contextMenu.MenuItems.Add(menuItem);

            contextMenu.Show(treeView_opacBrowseFormats, new Point(e.X, e.Y));		
			
        }

        void menu_opacBrowseFormatNode_up_Click(object sender, EventArgs e)
        {
            MoveUpDown(true);
        }

        void menu_opacBrowseFormatNode_down_Click(object sender, EventArgs e)
        {
            MoveUpDown(false);
        }

        void MoveUpDown(bool bUp)
        {
            string strError = "";
            int nRet = 0;

            // ��ǰ��ѡ���node
            if (this.treeView_opacBrowseFormats.SelectedNode == null)
            {
                MessageBox.Show("��δѡ��Ҫ���������ƶ��Ľڵ�");
                return;
            }

            TreeNodeCollection nodes = null;

            TreeNode parent = treeView_opacBrowseFormats.SelectedNode.Parent;

            if (parent == null)
                nodes = this.treeView_opacBrowseFormats.Nodes;
            else
                nodes = parent.Nodes;

            TreeNode node = treeView_opacBrowseFormats.SelectedNode;

            int index = nodes.IndexOf(node);

            Debug.Assert(index != -1, "");

            if (bUp == true)
            {
                if (index == 0)
                {
                    strError = "�Ѿ���ͷ";
                    goto ERROR1;
                }

                nodes.Remove(node);
                index--;
                nodes.Insert(index, node);
            }
            if (bUp == false)
            {
                if (index >= nodes.Count - 1)
                {
                    strError = "�Ѿ���β";
                    goto ERROR1;
                }

                nodes.Remove(node);
                index++;
                nodes.Insert(index, node);

            }

            this.treeView_opacBrowseFormats.SelectedNode = node;

            // ��Ҫ������������ύ�޸�
            nRet = SubmitOpacBrowseFormatDef(out strError);
            if (nRet == -1)
            {
                // TODO: ��α�ʾδ���ύ��λ�ñ仯����
                goto ERROR1;
            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        private void toolStripButton_opacBrowseFormats_remove_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            // ��ǰ�ڵ�
            TreeNode current_treenode = this.treeView_opacBrowseFormats.SelectedNode;

            if (current_treenode == null)
            {
                strError = "��δѡ��Ҫɾ���Ŀ������ʽ���ڵ�";
                goto ERROR1;
            }

            // ����
            string strText = "ȷʵҪ�Ƴ�";

            if (current_treenode.Parent == null)
                strText += "�����ڵ�";
            else
                strText += "��ʾ��ʽ�ڵ�";

            strText += " " + current_treenode.Text + " ";

            if (current_treenode.Parent == null)
                strText += "���������ڵ�";

            strText += "?";

            // �Ի��򾯸�
            DialogResult result = MessageBox.Show(this,
                strText,
                "ManagerForm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
                return;

            if (current_treenode.Parent != null)
                current_treenode.Parent.Nodes.Remove(current_treenode);
            else
            {
                Debug.Assert(current_treenode.Parent == null, "");
                this.treeView_opacBrowseFormats.Nodes.Remove(current_treenode);
            }

            // ��Ҫ������������ύ�޸�
            nRet = SubmitOpacBrowseFormatDef(out strError);
            if (nRet == -1)
            {
                // TODO: ��α�ʾδ���ύ���Ƴ�����
                goto ERROR1;
            }
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // treeview��˫�������ڿ����ڵ���Ȼ��չ���������������ã����Ը�ʽ�ڵ��Ǵ��޸ĶԻ��������
        private void treeView_opacBrowseFormats_DoubleClick(object sender, EventArgs e)
        {
            // ��ǰ��ѡ���node
            TreeNode node = treeView_opacBrowseFormats.SelectedNode;

            if (node == null)
                return;

            if (node.Parent == null) // �����ڵ�
                return;

            toolStripButton_opacBrowseFormats_modify_Click(sender, e);

        }

        // treeview�е�����������������Ҳ�ܶ�λ
        private void treeView_opacBrowseFormats_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode curSelectedNode = this.treeView_opacBrowseFormats.GetNodeAt(e.X, e.Y);

                if (treeView_opacBrowseFormats.SelectedNode != curSelectedNode)
                {
                    treeView_opacBrowseFormats.SelectedNode = curSelectedNode;

                    if (treeView_opacBrowseFormats.SelectedNode == null)
                        treeView_opacBrowseFormats_AfterSelect(null, null);	// ����
                }

            }
        }

        // ˢ��
        private void toolStripButton_opacBrowseFormats_refresh_Click(object sender, EventArgs e)
        {
            RefreshOpacBrowseFormatTree();
        }

        void RefreshOpacBrowseFormatTree()
        {
            string strError = "";
            int nRet = this.ListAllOpacBrowseFormats(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
            }
        }

        // �ύOPAC��¼��ʾ��ʽ�����޸�
        int SubmitOpacBrowseFormatDef(out string strError)
        {
            strError = "";
            string strFormatDef = "";
            int nRet = BuildOpacBrowseFormatDef(out strFormatDef,
                out strError);
            if (nRet == -1)
                return -1;

            nRet = this.SetAllOpacBrowseFormatsDef(strFormatDef,
                out strError);
            if (nRet == -1)
                return -1;

            return 0;
        }

        // ����OPAC��¼��ʾ��ʽ�����XMLƬ��
        // ע�����¼�Ƭ�϶��壬û��<browseformats>Ԫ����Ϊ����
        int BuildOpacBrowseFormatDef(out string strFormatDef,
            out string strError)
        {
            strError = "";
            strFormatDef = "";

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<browseformats />");

            for (int i = 0; i < this.treeView_opacBrowseFormats.Nodes.Count; i++)
            {
                TreeNode item = this.treeView_opacBrowseFormats.Nodes[i];

                string strDatabaseName = item.Text;

                XmlNode database_node = dom.CreateElement("database");
                DomUtil.SetAttr(database_node, "name", strDatabaseName);

                dom.DocumentElement.AppendChild(database_node);

                for (int j = 0; j < item.Nodes.Count; j++)
                {
                    TreeNode format_treenode = item.Nodes[j];

                    string strXmlFragment = (string)format_treenode.Tag;

                    XmlDocumentFragment fragment = dom.CreateDocumentFragment();
                    try
                    {
                        fragment.InnerXml = strXmlFragment;
                    }
                    catch (Exception ex)
                    {
                        strError = "fragment XMLװ��XmlDocumentFragmentʱ����: " + ex.Message;
                        return -1;
                    }

                    database_node.AppendChild(fragment);
                }
            }

            strFormatDef = dom.DocumentElement.InnerXml;

            return 0;
        }

        // ����һ�����ݿ����ʾ��ʽ�Ƿ���ڣ�
        bool HasBrowseFormatDatabaseExist(string strDatabaseName)
        {
            for (int i = 0; i < this.treeView_opacBrowseFormats.Nodes.Count; i++)
            {
                if (this.treeView_opacBrowseFormats.Nodes[i].Text == strDatabaseName)
                    return true;
            }

            return false;
        }

        // Ϊ��Ŀ�����OPAC��ʾ��ʽ�ڵ�(���)
        int NewBiblioOpacBrowseFormat(string strDatabaseName,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            // �����
            int index = this.treeView_opacBrowseFormats.Nodes.Count;

            // ��������ڵ�
            TreeNode new_database_treenode = new TreeNode(strDatabaseName, 0, 0);
            this.treeView_opacBrowseFormats.Nodes.Insert(index, new_database_treenode);

            // �����ʽ�ڵ�
            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<format />");

            string strDisplayText = "��ϸ";
            DomUtil.SetAttr(dom.DocumentElement, "name", "��ϸ");

            strDisplayText += " type=" + "biblio";
            DomUtil.SetAttr(dom.DocumentElement, "type", "biblio");

            TreeNode new_format_treenode = new TreeNode(strDisplayText, 1, 1);
            new_format_treenode.Tag = dom.DocumentElement.OuterXml;

            new_database_treenode.Nodes.Insert(index, new_format_treenode);

            this.treeView_opacBrowseFormats.SelectedNode = new_format_treenode;


            // ��Ҫ������������ύ�޸�
            nRet = SubmitOpacBrowseFormatDef(out strError);
            if (nRet == -1)
            {
                new_format_treenode.ImageIndex = 2;    // ��ʾδ���ύ���²���ڵ�����
                return -1;
            }

            return 0;
        }

        #endregion // of OPAC��¼��ʾ��ʽ

        #region ������ͨȨ��

        int ListRightsTables(out string strError)
        {
            strError = "";

            if (this.LoanPolicyDefChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�����ڶ�����ͨȨ�޶��屻�޸ĺ���δ���档����ʱˢ�´������ݣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪˢ��? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    return 0;
                }
            }

            /*
            // 2008/10/12 new add
            if (this.LoanPolicyDefChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�������ж�����ͨȨ�޶��屻�޸ĺ���δ���档����ʱ����װ�ض�����ͨȨ�޶��壬����δ������Ϣ����ʧ��\r\n\r\nȷʵҪ����װ��? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                    return 0;
            }*/

            string strRightsTableXml = "";
            string strRightsTableHtml = "";

            // �����ͨ����Ȩ����ض���
            int nRet = GetRightsTableInfo(out strRightsTableXml,
                out strRightsTableHtml,
                out strError);
            if (nRet == -1)
                return -1;

            strRightsTableXml = "<rightstable>" + strRightsTableXml + "</rightstable>";

            string strXml = "";
            nRet = DomUtil.GetIndentXml(strRightsTableXml,
                out strXml,
                out strError);
            if (nRet == -1)
                return -1;

            this.textBox_loanPolicy_rightsTableDef.Text = strXml;
            Global.SetHtmlString(this.webBrowser_rightsTableHtml,
                strRightsTableHtml);

            this.LoanPolicyDefChanged = false;

            this.m_nRightsTableHtmlVersion = this.m_nRightsTableXmlVersion;

            return 1;
        }

        // �����ͨ����Ȩ����ض���
        int GetRightsTableInfo(out string strRightsTableXml,
            out string strRightsTableHtml,
            out string strError)
        {
            strError = "";
            strRightsTableXml = "";
            strRightsTableHtml = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڻ�ȡ������ͨȨ�޶��� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.GetSystemParameter(
                    stop,
                    "circulation",
                    "rightsTable",
                    out strRightsTableXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                lRet = Channel.GetSystemParameter(
                    stop,
                    "circulation",
                    "rightsTableHtml",
                    out strRightsTableHtml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // ������ͨ����Ȩ����ض���
        // parameters:
        //      strRightsTableXml   ��ͨ����Ȩ�޶���XML��ע�⣬û�и�Ԫ��
        int SetRightsTableDef(string strRightsTableXml,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڱ��������ͨȨ�޶��� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.SetSystemParameter(
                    stop,
                    "circulation",
                    "rightsTable",
                    strRightsTableXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // ͬ������Ȩ��XML�������ͨȨ�ޱ�HTML��ʾ
        int SynchronizeRightsTableAndHtml()
        {
            string strError = "";

            if (this.m_nRightsTableXmlVersion == this.m_nRightsTableHtmlVersion)
                return 0;


            string strRightsTableXml = this.textBox_loanPolicy_rightsTableDef.Text;
            string strRightsTableHtml = "";

            if (String.IsNullOrEmpty(strRightsTableXml) == true)
            {
                Global.SetHtmlString(this.webBrowser_rightsTableHtml,
                    "<p>(blank)</p>");
                this.m_nRightsTableHtmlVersion = this.m_nRightsTableXmlVersion;
                return 0;
            }

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(strRightsTableXml);
            }
            catch (Exception ex)
            {
                strError = "XML�ַ���װ��XMLDOMʱ��������: " + ex.Message;
                goto ERROR1;
            }

            if (dom.DocumentElement == null)
            {
                Global.SetHtmlString(this.webBrowser_rightsTableHtml,
                    "<p>(blank)</p>");
                this.m_nRightsTableHtmlVersion = this.m_nRightsTableXmlVersion;
                return 0;
            }

            strRightsTableXml = dom.DocumentElement.InnerXml;

            // EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڻ�ȡ������ͨȨ�޶���HTML ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.GetSystemParameter(
                    stop,
                    "instance_rightstable_html",
                    strRightsTableXml,
                    out strRightsTableHtml,
                    out strError);
                if (lRet == -1)
                {
                    goto ERROR1;
                }

                Global.SetHtmlString(this.webBrowser_rightsTableHtml,
                    strRightsTableHtml);
                this.m_nRightsTableHtmlVersion = this.m_nRightsTableXmlVersion;

                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                // EnableControls(true);
            }

        ERROR1:
            Global.SetHtmlString(this.webBrowser_rightsTableHtml,
                HttpUtility.HtmlEncode(strError));
            return -1;
        }


        bool m_bLoanPolicyDefChanged = false;

        public bool LoanPolicyDefChanged
        {
            get
            {
                return this.m_bLoanPolicyDefChanged;
            }
            set
            {
                this.m_bLoanPolicyDefChanged = value;
                if (value == true)
                    this.toolStripButton_loanPolicy_save.Enabled = true;
                else
                    this.toolStripButton_loanPolicy_save.Enabled = false;
            }
        }

        private void textBox_loanPolicy_rightsTableDef_TextChanged(object sender, EventArgs e)
        {
            // XML�༭���еİ汾�����仯
            this.m_nRightsTableXmlVersion++;
            this.LoanPolicyDefChanged = true;
        }

        private void textBox_loanPolicy_rightsTableDef_Enter(object sender, EventArgs e)
        {
            SynchronizeRightsTableAndHtml();
        }

        private void textBox_loanPolicy_rightsTableDef_Leave(object sender, EventArgs e)
        {
            SynchronizeRightsTableAndHtml();
        }

        private void toolStripButton_loanPolicy_save_Click(object sender, EventArgs e)
        {
            string strError = "";
            string strRightsTableXml = this.textBox_loanPolicy_rightsTableDef.Text;

            if (String.IsNullOrEmpty(strRightsTableXml) == true)
            {
                strRightsTableXml = "";
            }
            else
            {

                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.LoadXml(strRightsTableXml);
                }
                catch (Exception ex)
                {
                    strError = "XML�ַ���װ��XMLDOMʱ��������: " + ex.Message;
                    goto ERROR1;
                }

                if (dom.DocumentElement == null)
                {
                    strRightsTableXml = "";
                }
                else
                    strRightsTableXml = dom.DocumentElement.InnerXml;
            }

            // ������ͨ����Ȩ����ض���
            // parameters:
            //      strRightsTableXml   ��ͨ����Ȩ�޶���XML��ע�⣬û�и�Ԫ��
            int nRet = SetRightsTableDef(strRightsTableXml,
                out strError);
            if (nRet == -1)
                goto ERROR1;

            this.LoanPolicyDefChanged = false;

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        private void toolStripButton_loanPolicy_refresh_Click(object sender, EventArgs e)
        {
            string strError = "";

            int nRet = this.ListRightsTables(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
            }
        }

        #endregion

        #region �ݲص�����

        // ��listview���г����йݲص�
        int ListAllLocations(out string strError)
        {
            strError = "";

            if (this.LocationTypesDefChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�������йݲصض��屻�޸ĺ���δ���档����ʱ����װ�عݲصض��壬����δ������Ϣ����ʧ��\r\n\r\nȷʵҪ����װ��? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                    return 0;
            }

            this.listView_location_list.Items.Clear();

            string strOutputInfo = "";
            int nRet = GetAllLocationInfo(out strOutputInfo,
                    out strError);
            if (nRet == -1)
                return -1;

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<root />");

            XmlDocumentFragment fragment = dom.CreateDocumentFragment();
            try
            {
                fragment.InnerXml = strOutputInfo;
            }
            catch (Exception ex)
            {
                strError = "fragment XMLװ��XmlDocumentFragmentʱ����: " + ex.Message;
                return -1;
            }

            dom.DocumentElement.AppendChild(fragment);

            /*
            <locationtypes>
                <item canborrow="yes">��ͨ��</item>
                <item>������</item>
            </locationtypes>
            */

            XmlNodeList nodes = dom.DocumentElement.SelectNodes("item");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                bool bCanBorrow = false;

                // ��ò����͵����Բ���ֵ
                // return:
                //      -1  ��������nValue���Ѿ�����nDefaultValueֵ�����Բ��Ӿ����ֱ��ʹ��
                //      0   ���������ȷ����Ĳ���ֵ
                //      1   ����û�ж��壬��˴�����ȱʡ����ֵ����
                nRet = DomUtil.GetBooleanParam(node,
                     "canborrow",
                     false,
                     out bCanBorrow,
                     out strError);
                if (nRet == -1)
                    return -1;

                string strText = node.InnerText;

                if (String.IsNullOrEmpty(strText) == true)
                    continue;

                ListViewItem item = new ListViewItem(strText, 0);
                item.SubItems.Add(bCanBorrow == true ? "��" : "��");

                this.listView_location_list.Items.Add(item);
            }

            this.LocationTypesDefChanged = false;

            return 1;
        }

        // <locationtypes>
        int GetAllLocationInfo(out string strOutputInfo,
    out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڻ�ȡ<locationtypes>���� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.GetSystemParameter(
                    stop,
                    "circulation",
                    "locationTypes",
                    out strOutputInfo,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // �޸�/����ȫ���ݲصض���<locationtypes>
        int SetAllLocationTypesInfo(string strLocationDef,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("��������<locationtypes>���� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.SetSystemParameter(
                    stop,
                    "circulation",
                    "locationTypes",
                    strLocationDef,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // ����<locationtypes>�����XMLƬ��
        // ע�����¼�Ƭ�϶��壬û��<locationtypes>Ԫ����Ϊ����
        int BuildLocationTypesDef(out string strLocationDef,
            out string strError)
        {
            strError = "";
            strLocationDef = "";

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<locationtypes />");

            for (int i = 0; i < this.listView_location_list.Items.Count; i++)
            {
                ListViewItem item = this.listView_location_list.Items[i];
                string strText = item.Text;
                string strCanBorrow = ListViewUtil.GetItemText(item, 1);

                bool bCanBorrow = false;

                if (strCanBorrow == "��" || strCanBorrow == "yes")
                    bCanBorrow = true;

                XmlNode nodeItem = dom.CreateElement("item");
                dom.DocumentElement.AppendChild(nodeItem);

                nodeItem.InnerText = strText;
                DomUtil.SetAttr(nodeItem, "canborrow", bCanBorrow == true ? "yes" : "no");
            }

            strLocationDef = dom.DocumentElement.InnerXml;

            return 0;
        }

        // �ύ<locationtypes>�����޸�
        int SubmitLocationTypesDef(out string strError)
        {
            strError = "";
            string strLocationTypesDef = "";
            int nRet = BuildLocationTypesDef(out strLocationTypesDef,
                out strError);
            if (nRet == -1)
                return -1;

            nRet = SetAllLocationTypesInfo(strLocationTypesDef,
                out strError);
            if (nRet == -1)
                return -1;

            return 0;
        }


        private void toolStripButton_location_refresh_Click(object sender, EventArgs e)
        {
            // ��listview���г����йݲص�
            string strError = "";
            int nRet = ListAllLocations(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
            }
        }

        bool m_bLocationTypesDefChanged = false;

        public bool LocationTypesDefChanged
        {
            get
            {
                return this.m_bLocationTypesDefChanged;
            }
            set
            {
                this.m_bLocationTypesDefChanged = value;
                if (value == true)
                    this.toolStripButton_location_save.Enabled = true;
                else
                    this.toolStripButton_location_save.Enabled = false;
            }
        }

        private void toolStripButton_location_save_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            nRet = SubmitLocationTypesDef(out strError);
            if (nRet == -1)
                MessageBox.Show(this, strError);
            else
            {
                this.LocationTypesDefChanged = false;
            }
        }

        // �´����ݲصص�����
        private void toolStripButton_location_new_Click(object sender, EventArgs e)
        {
            LocationItemDialog dlg = new LocationItemDialog();

            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            ListViewItem item = new ListViewItem(dlg.LocationString, 0);
            item.SubItems.Add(dlg.CanBorrow == true ? "��" : "��");

            this.listView_location_list.Items.Add(item);
            ListViewUtil.SelectLine(item, true);

            this.LocationTypesDefChanged = true;
        }

        // �޸Ĺݲصص�����
        private void toolStripButton_location_modify_Click(object sender, EventArgs e)
        {
            string strError = "";
            if (this.listView_location_list.SelectedItems.Count == 0)
            {
                strError = "��δѡ��Ҫ�޸ĵĹݲصص�����";
                goto ERROR1;
            }
            ListViewItem item = this.listView_location_list.SelectedItems[0];

            LocationItemDialog dlg = new LocationItemDialog();

            dlg.LocationString = ListViewUtil.GetItemText(item, 0);
            dlg.CanBorrow = (ListViewUtil.GetItemText(item, 1) == "��") ? true : false;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            ListViewUtil.ChangeItemText(item, 0, dlg.LocationString);
            ListViewUtil.ChangeItemText(item, 1, dlg.CanBorrow == true ? "��" : "��");

            ListViewUtil.SelectLine(item, true);
            this.LocationTypesDefChanged = true;
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // ɾ����ѡ���Ĺݲصص�����
        private void toolStripButton_location_delete_Click(object sender, EventArgs e)
        {
            string strError = "";
            if (this.listView_location_list.SelectedItems.Count == 0)
            {
                strError = "��δѡ��Ҫɾ���Ĺݲصص�����";
                goto ERROR1;
            }

            string strItemNameList = "";
            for (int i = 0; i < this.listView_location_list.SelectedItems.Count; i++)
            {
                if (i > 0)
                    strItemNameList += ",";
                strItemNameList += this.listView_location_list.SelectedItems[i].Text;
            }

            // �Ի��򾯸�
            DialogResult result = MessageBox.Show(this,
                "ȷʵҪɾ���ݲصص����� " + strItemNameList + "?",
                "ManagerForm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
                return;

            for (int i = this.listView_location_list.SelectedIndices.Count - 1;
                i >= 0;
                i--)
            {
                int index = this.listView_location_list.SelectedIndices[i];
                string strDatabaseName = this.listView_location_list.Items[index].Text;
                this.listView_location_list.Items.RemoveAt(index);
            }

            this.LocationTypesDefChanged = true;
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // listviewѡ�����䶯
        private void listView_location_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_location_list.SelectedItems.Count > 0)
            {
                this.toolStripButton_location_modify.Enabled = true;
                this.toolStripButton_location_delete.Enabled = true;
            }
            else
            {
                this.toolStripButton_location_modify.Enabled = false;
                this.toolStripButton_location_delete.Enabled = false;
            }

            if (this.listView_location_list.SelectedItems.Count == 0
                || this.listView_location_list.Items.IndexOf(this.listView_location_list.SelectedItems[0]) == 0)
                this.toolStripButton_location_up.Enabled = false;
            else
                this.toolStripButton_location_up.Enabled = true;

            if (this.listView_location_list.SelectedItems.Count == 0
                || this.listView_location_list.Items.IndexOf(this.listView_location_list.SelectedItems[0]) >= this.listView_location_list.Items.Count - 1)
                this.toolStripButton_location_down.Enabled = false;
            else
                this.toolStripButton_location_down.Enabled = true;
        }

        // listview����˫��
        private void listView_location_list_DoubleClick(object sender, EventArgs e)
        {
            toolStripButton_location_modify_Click(sender, e);
        }

        private void listView_location_list_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = null;

            string strName = "";
            string strCanBorrow = "";
            if (this.listView_location_list.SelectedItems.Count > 0)
            {
                strName = this.listView_location_list.SelectedItems[0].Text;
                strCanBorrow = ListViewUtil.GetItemText(this.listView_location_list.SelectedItems[0], 1);
            }


            // �޸Ĺݲ�����
            {
                menuItem = new MenuItem("�޸� " + strName + "(&M)");
                menuItem.Click += new System.EventHandler(this.toolStripButton_location_modify_Click);
                if (this.listView_location_list.SelectedItems.Count == 0)
                    menuItem.Enabled = false;
                // ȱʡ����
                menuItem.DefaultItem = true;
                contextMenu.MenuItems.Add(menuItem);
            }


            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("����(&N)");
            menuItem.Click += new System.EventHandler(this.toolStripButton_location_new_Click);
            contextMenu.MenuItems.Add(menuItem);


            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);



            string strText = "";
            if (this.listView_location_list.SelectedItems.Count == 1)
                strText = "ɾ�� " + strName + "(&D)";
            else
                strText = "ɾ����ѡ " + this.listView_location_list.SelectedItems.Count.ToString() + " ���ݲصص�����(&D)";

            menuItem = new MenuItem(strText);
            menuItem.Click += new System.EventHandler(this.toolStripButton_location_delete_Click);
            if (this.listView_location_list.SelectedItems.Count == 0)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("����(&S)");
            menuItem.Click += new System.EventHandler(this.toolStripButton_location_save_Click);
            if (this.LocationTypesDefChanged == false)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);



            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            /*
            menuItem = new MenuItem("�۲���ѡ " + this.listView_location_list.SelectedItems.Count.ToString() + " ���ݲ�����Ķ���(&D)");
            menuItem.Click += new System.EventHandler(this.menu_viewOpacDatabaseDefine_Click);
            if (this.listView_location_list.SelectedItems.Count == 0)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);
             * */

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);


            // 
            menuItem = new MenuItem("����(&U)");
            menuItem.Click += new System.EventHandler(this.menu_location_up_Click);
            if (this.listView_location_list.SelectedItems.Count == 0
                || this.listView_location_list.Items.IndexOf(this.listView_location_list.SelectedItems[0]) == 0)
                menuItem.Enabled = false;
            else
                menuItem.Enabled = true;
            contextMenu.MenuItems.Add(menuItem);



            // 
            menuItem = new MenuItem("����(&D)");
            menuItem.Click += new System.EventHandler(this.menu_location_down_Click);
            if (this.listView_location_list.SelectedItems.Count == 0
                || this.listView_location_list.Items.IndexOf(this.listView_location_list.SelectedItems[0]) >= this.listView_location_list.Items.Count - 1)
                menuItem.Enabled = false;
            else
                menuItem.Enabled = true;
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("ˢ��(&R)");
            menuItem.Click += new System.EventHandler(this.toolStripButton_location_refresh_Click);
            contextMenu.MenuItems.Add(menuItem);

            contextMenu.Show(this.listView_location_list, new Point(e.X, e.Y));		

        }


        void menu_location_up_Click(object sender, EventArgs e)
        {
            MoveLocationItemUpDown(true);
        }

        void menu_location_down_Click(object sender, EventArgs e)
        {
            MoveLocationItemUpDown(false);
        }

        void MoveLocationItemUpDown(bool bUp)
        {
            string strError = "";
            // int nRet = 0;

            if (this.listView_location_list.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ���������ƶ��Ĺݲصص�����");
                return;
            }

            ListViewItem item = this.listView_location_list.SelectedItems[0];
            int index = this.listView_location_list.Items.IndexOf(item);

            Debug.Assert(index >= 0 && index <= this.listView_location_list.Items.Count - 1, "");

            bool bChanged = false;

            if (bUp == true)
            {
                if (index == 0)
                {
                    strError = "��ͷ";
                    goto ERROR1;
                }

                this.listView_location_list.Items.RemoveAt(index);
                index--;
                this.listView_location_list.Items.Insert(index, item);
                this.listView_location_list.FocusedItem = item;

                bChanged = true;
            }

            if (bUp == false)
            {
                if (index >= this.listView_location_list.Items.Count - 1)
                {
                    strError = "��β";
                    goto ERROR1;
                }
                this.listView_location_list.Items.RemoveAt(index);
                index++;
                this.listView_location_list.Items.Insert(index, item);
                this.listView_location_list.FocusedItem = item;

                bChanged = true;
            }


            // TODO: �Ƿ�����ӳ��ύ?
            if (bChanged == true)
            {
                /*
                // ��Ҫ������������ύ�޸�
                nRet = this.SubmitLocationTypesDef(out strError);
                if (nRet == -1)
                {
                    // TODO: ��α�ʾδ���ύ������λ���ƶ�����?
                    goto ERROR1;
                }
                 * */
                this.LocationTypesDefChanged = true;
            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        private void toolStripButton_location_up_Click(object sender, EventArgs e)
        {
            MoveLocationItemUpDown(true);
        }

        private void toolStripButton_location_down_Click(object sender, EventArgs e)
        {
            MoveLocationItemUpDown(false);
        }

        #endregion

        #region �ű�

        int ListScript(out string strError)
        {
            strError = "";

            if (this.ScriptChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�����ڽű����屻�޸ĺ���δ���档����ʱˢ�´������ݣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪˢ��? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    return 0;
                }
            }

            string strScriptXml = "";

            // ��ýű���ض���
            int nRet = GetScriptInfo(out strScriptXml,
                out strError);
            if (nRet == -1)
                return -1;

            strScriptXml = "<script>" + strScriptXml + "</script>";

            string strXml = "";
            nRet = DomUtil.GetIndentXml(strScriptXml,
                out strXml,
                out strError);
            if (nRet == -1)
                return -1;

            this.textBox_script.Text = strXml;
            this.ScriptChanged = false;

            return 1;
        }

        // ��ýű���ض���
        int GetScriptInfo(out string strScriptXml,
            out string strError)
        {
            strError = "";
            strScriptXml = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڻ�ȡ�ű����� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.GetSystemParameter(
                    stop,
                    "circulation",
                    "script",
                    out strScriptXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // ����ű�����
        // parameters:
        //      strRightsTableXml   �ű�����XML��ע�⣬û�и�Ԫ��
        int SetScriptDef(string strScriptXml,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڱ���ű����� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.SetSystemParameter(
                    stop,
                    "circulation",
                    "script",
                    strScriptXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        bool m_bScriptChanged = false;

        public bool ScriptChanged
        {
            get
            {
                return this.m_bScriptChanged;
            }
            set
            {
                this.m_bScriptChanged = value;
                if (value == true)
                    this.toolStripButton_script_save.Enabled = true;
                else
                    this.toolStripButton_script_save.Enabled = false;
            }
        }

        private void toolStripButton_script_save_Click(object sender, EventArgs e)
        {
            string strError = "";
            string strScriptXml = this.textBox_script.Text;

            if (String.IsNullOrEmpty(strScriptXml) == true)
            {
                strScriptXml = "";
            }
            else
            {

                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.LoadXml(strScriptXml);
                }
                catch (Exception ex)
                {
                    strError = "XML�ַ���װ��XMLDOMʱ��������: " + ex.Message;
                    goto ERROR1;
                }

                if (dom.DocumentElement == null)
                {
                    strScriptXml = "";
                }
                else
                    strScriptXml = dom.DocumentElement.InnerXml;
            }

            int nRet = SetScriptDef(strScriptXml,
                out strError);
            if (nRet == -1)
            {
                this.textBox_script_comment.Text = strError;
                this.ScriptChanged = false;
                goto ERROR1;
            }
            else
            {
                this.textBox_script_comment.Text = "";
            }

            this.ScriptChanged = false;

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        private void toolStripButton_script_refresh_Click(object sender, EventArgs e)
        {
            string strError = "";

            int nRet = this.ListScript(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
            }
        }

        private void textBox_script_TextChanged(object sender, EventArgs e)
        {
            this.ScriptChanged = true;
        }

        private void textBox_script_comment_DoubleClick(object sender, EventArgs e)
        {
            int x = 0;
            int y = 0;
            API.GetEditCurrentCaretPos(
                this.textBox_script_comment,
                out x,
                out y);

            string strLine = textBox_script_comment.Lines[y];

            // ����"(�У���)"ֵ

            int nRet = strLine.IndexOf("(");
            if (nRet == -1)
                goto ERROR1;

            strLine = strLine.Substring(nRet + 1);
            nRet = strLine.IndexOf(")");
            if (nRet != -1)
                strLine = strLine.Substring(0, nRet);
            strLine = strLine.Trim();

            // �ҵ�','
            nRet = strLine.IndexOf(",");
            if (nRet == -1)
                goto ERROR1;
            y = Convert.ToInt32(strLine.Substring(0, nRet).Trim()) - 1;
            x = Convert.ToInt32(strLine.Substring(nRet + 1).Trim()) - 1;

            // MessageBox.Show(Convert.ToString(x) + " , "+Convert.ToString(y));

            this.textBox_script.Focus();
            this.textBox_script.DisableEmSetSelMsg = false;
            API.SetEditCurrentCaretPos(
                textBox_script,
                x,
                y,
                true);
            this.textBox_script.DisableEmSetSelMsg = true;
            OnScriptTextCaretChanged();
            return;
            ERROR1:
            // ���������Ե�����
            Console.Beep();
        }

        void OnScriptTextCaretChanged()
        {
            int x = 0;
            int y = 0;
            API.GetEditCurrentCaretPos(
                textBox_script,
                out x,
                out y);
            toolStripLabel_script_caretPos.Text = Convert.ToString(y + 1) + ", " + Convert.ToString(x + 1);
        }

        private void textBox_script_KeyDown(object sender, KeyEventArgs e)
        {
            OnScriptTextCaretChanged();

        }

        private void textBox_script_MouseUp(object sender, MouseEventArgs e)
        {
            OnScriptTextCaretChanged();

        }

        #endregion

        #region �ִκ�

        int ListZhongcihao(out string strError)
        {
            strError = "";

            /*
            if (this.ZhongcihaoChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�������ִκŶ��屻�޸ĺ���δ���档����ʱˢ�´������ݣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪˢ��? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    return 0;
                }
            }*/

            this.treeView_zhongcihao.Nodes.Clear();


            string strZhongcihaoXml = "";

            // ����ִκ���ض���
            int nRet = GetZhongcihaoInfo(out strZhongcihaoXml,
                out strError);
            if (nRet == -1)
                return -1;

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<zhogncihao />");

            XmlDocumentFragment fragment = dom.CreateDocumentFragment();
            try
            {
                fragment.InnerXml = strZhongcihaoXml;
            }
            catch (Exception ex)
            {
                strError = "fragment XMLװ��XmlDocumentFragmentʱ����: " + ex.Message;
                return -1;
            }

            dom.DocumentElement.AppendChild(fragment);

            /*
    <zhongcihao>
        <nstable name="nstable">
            <item prefix="marc" uri="http://dp2003.com/UNIMARC" />
        </nstable>
        <group name="������Ŀ" zhongcihaodb="�ִκ�">
            <database name="����ͼ��" leftfrom="�������" 

rightxpath="//marc:record/marc:datafield[@tag='905']/marc:subfield[@code='e']/text()" 

titlexpath="//marc:record/marc:datafield[@tag='200']/marc:subfield[@code='a']/text()" 

authorxpath="//marc:record/marc:datafield[@tag='200']/marc:subfield[@code='f' or @code='g']/text()" 

/>
        </group>
    </zhongcihao>
 * */
            XmlNodeList nstable_nodes = dom.DocumentElement.SelectNodes("nstable");
            for (int i = 0; i < nstable_nodes.Count; i++)
            {
                XmlNode node = nstable_nodes[i];

                string strNstableName = DomUtil.GetAttr(node, "name");

                TreeNode nstable_treenode = new TreeNode(strNstableName,
                    TYPE_NSTABLE, TYPE_NSTABLE);
                nstable_treenode.Tag = node.OuterXml;

                this.treeView_zhongcihao.Nodes.Add(nstable_treenode);
            }

            XmlNodeList group_nodes = dom.DocumentElement.SelectNodes("group");
            for (int i = 0; i < group_nodes.Count; i++)
            {
                XmlNode node = group_nodes[i];

                string strGroupName = DomUtil.GetAttr(node, "name");
                string strZhongcihaoDbName = DomUtil.GetAttr(node, "zhongcihaodb");

                TreeNode group_treenode = new TreeNode(strGroupName + " �ִκſ�='" + strZhongcihaoDbName + "'",
                    TYPE_GROUP, TYPE_GROUP);
                group_treenode.Tag = node.OuterXml;

                this.treeView_zhongcihao.Nodes.Add(group_treenode);

                // ����database�ڵ�
                XmlNodeList database_nodes = node.SelectNodes("format");
                for (int j = 0; j < database_nodes.Count; j++)
                {
                    XmlNode database_node = database_nodes[j];

                    string strDatabaseName = DomUtil.GetAttr(database_node, "name");

                    string strDisplayText = strDatabaseName;

                    TreeNode database_treenode = new TreeNode(strDisplayText,
                        TYPE_DATABASE, TYPE_DATABASE);
                    database_treenode.Tag = database_node.OuterXml;

                    group_treenode.Nodes.Add(database_treenode);
                }
            }

            this.treeView_zhongcihao.ExpandAll();
            // this.ZhongcihaoChanged = false;

            return 1;
        }

        // ����ִκ���ض���
        int GetZhongcihaoInfo(out string strZhongcihaoXml,
            out string strError)
        {
            strError = "";
            strZhongcihaoXml = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڻ�ȡ�ִκŶ��� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.GetSystemParameter(
                    stop,
                    "circulation",
                    "zhongcihao",
                    out strZhongcihaoXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;
                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        // �����ִκŶ���
        // parameters:
        //      strZhongcihaoXml   �ű�����XML��ע�⣬û�и�Ԫ��
        int SetZhongcihaoDef(string strZhongcihaoXml,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڱ����ִκŶ��� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.SetSystemParameter(
                    stop,
                    "circulation",
                    "zhongcihao",
                    strZhongcihaoXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                return (int)lRet;
            }
            finally
            {
                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);
            }

        ERROR1:
            return -1;
        }

        #endregion

        private void treeView_zhongcihao_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void treeView_zhongcihao_MouseUp(object sender, MouseEventArgs e)
        {

        }

        // ����<group>���ͽڵ�
        private void toolStripMenuItem_zhongcihao_insert_group_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            // ��ǰ�ڵ�
            TreeNode current_treenode = this.treeView_zhongcihao.SelectedNode;

            // ������Ǹ����Ľڵ㣬�������ҵ�������
            if (current_treenode != null && current_treenode.Parent != null)
            {
                current_treenode = current_treenode.Parent;
            }

            // �����
            int index = this.treeView_zhongcihao.Nodes.IndexOf(current_treenode);
            if (index == -1)
                index = this.treeView_zhongcihao.Nodes.Count;
            else
                index++;

            // ѯ��<group>��
            ZhongcihaoGroupDialog dlg = new ZhongcihaoGroupDialog();

            dlg.Text = "��ָ��������";
            dlg.AllZhongcihaoDatabaseInfoXml = GetAllZhongcihaoDbInfoXml();
            dlg.ExcludingDbNames = GetAllUsedZhongcihaoDbName();
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            // �����ָ�����ִκſ��Ƿ���ڡ���������ڣ����Ѵ�����

                    // ���ָ�����ֵ��ִκſ��Ƿ��Ѿ�����
        // return:
        //      -2  ��ָ�����ִκſ����֣�ʵ������һ���Ѿ����ڵ��������͵Ŀ���
        //      -1  error
        //      0   ��û�д���
        //      1   �Ѿ�����
            nRet = CheckZhongcihaoDbCreated(dlg.ZhongcihaoDbName,
                out strError);
            if (nRet == -1)
                goto ERROR1;
            if (nRet == -2)
                goto ERROR1;
            if (nRet == 0)
            {
                string strComment = "�ִκſ� '" + dlg.ZhongcihaoDbName + "' ��δ�������봴������";
                // return:
                //      -1  errpr
                //      0   cancel
                //      1   created
                nRet = CreateSimpleDatabase("zhongcihao",
                    dlg.ZhongcihaoDbName,
                    strComment);
                if (nRet == -1)
                    goto ERROR1;
                if (nRet == 0)
                    return;
                Debug.Assert(nRet == 1, "");
            }

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<group />");
            DomUtil.SetAttr(dom.DocumentElement, "name", dlg.GroupName);
            DomUtil.SetAttr(dom.DocumentElement, "zhongcihaodb", dlg.ZhongcihaoDbName);

            TreeNode new_treenode = new TreeNode(dlg.GroupName, TYPE_GROUP, TYPE_GROUP);
            new_treenode.Tag = dom.OuterXml;
            this.treeView_zhongcihao.Nodes.Insert(index, new_treenode);

            this.treeView_zhongcihao.SelectedNode = new_treenode;

            /*
            // ��Ҫ������������ύ�޸�
            nRet = SubmitZhongcihaoDef(out strError);
            if (nRet == -1)
            {
                new_treenode.ImageIndex = TYPE_ERROR;    // ��ʾδ���ύ���²���ڵ�����
                goto ERROR1;
            }*/

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // ���ָ�����ֵ��ִκſ��Ƿ��Ѿ�����
        // return:
        //      -2  ��ָ�����ִκſ����֣�ʵ������һ���Ѿ����ڵ��������͵Ŀ���
        //      -1  error
        //      0   ��û�д���
        //      1   �Ѿ�����
        int CheckZhongcihaoDbCreated(string strZhongcihaoDbName,
            out string strError)
        {
            strError = "";

            if (String.IsNullOrEmpty(strZhongcihaoDbName) == true)
            {
                strError = "����strZhongcihaoDbName��ֵ����Ϊ��";
                return -1;
            }

            if (String.IsNullOrEmpty(this.AllDatabaseInfoXml) == true)
            {
                return 0;
            }

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(this.AllDatabaseInfoXml);
            }
            catch (Exception ex)
            {
                strError = "XMLװ��DOMʱ����: " + ex.Message;
                return -1;
            }

            XmlNodeList nodes = dom.DocumentElement.SelectNodes("database");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                string strName = DomUtil.GetAttr(node, "name");
                string strType = DomUtil.GetAttr(node, "type");

                if ("zhongcihao" == strType)
                {
                    if (strName == strZhongcihaoDbName)
                        return 1;
                }

                if (strType == "biblio")
                {
                    if (strName == strZhongcihaoDbName)
                    {
                        strError = "���ⶨ���ִκſ����͵�ǰ�Ѿ����ڵ�С��Ŀ���� '" + strName + "' ������";
                        return -2;
                    }

                    string strEntityDbName = DomUtil.GetAttr(node, "entityDbName");
                    if (strEntityDbName == strZhongcihaoDbName)
                    {
                        strError = "���ⶨ���ִκſ����͵�ǰ�Ѿ����ڵ�ʵ����� '" + strEntityDbName + "' ������";
                        return -2;
                    }

                    string strOrderDbName = DomUtil.GetAttr(node, "orderDbName");
                    if (strOrderDbName == strZhongcihaoDbName)
                    {
                        strError = "���ⶨ���ִκſ����͵�ǰ�Ѿ����ڵĶ������� '" + strOrderDbName + "' ������";
                        return -2;
                    }

                    string strIssueDbName = DomUtil.GetAttr(node, "issueDbName");
                    if (strIssueDbName == strZhongcihaoDbName)
                    {
                        strError = "���ⶨ���ִκſ����͵�ǰ�Ѿ����ڵ��ڿ��� '" + strIssueDbName + "' ������";
                        return -2;
                    }

                }

                string strTypeName = GetTypeName(strType);
                if (strTypeName == null)
                    strTypeName = strType;

                if (strName == strZhongcihaoDbName)
                {
                    strError = "���ⶨ���ִκſ����͵�ǰ�Ѿ����ڵ�" + strTypeName + "���� '" + strName + "' ������";
                    return -2;
                }

            }

            return 0;
        }

        string GetAllZhongcihaoDbInfoXml()
        {
            if (String.IsNullOrEmpty(this.AllDatabaseInfoXml) == true)
                return null;

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.LoadXml(this.AllDatabaseInfoXml);
            }
            catch (Exception ex)
            {
                // strError = "XMLװ��DOMʱ����: " + ex.Message;
                // return -1;
                Debug.Assert(false, "");
                return "";
            }

            XmlNodeList nodes = dom.DocumentElement.SelectNodes("database");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                string strName = DomUtil.GetAttr(node, "name");
                string strType = DomUtil.GetAttr(node, "type");

                if (StringUtil.IsInList("zhongcihao", strType) == true)
                    continue;

                node.ParentNode.RemoveChild(node);
            }

            return dom.OuterXml;
        }

        // ���treeview���Ѿ�ʹ�ù���ȫ���ִκ���
        List<string> GetAllUsedZhongcihaoDbName()
        {
            List<string> existing_dbnames = new List<string>();
            for (int i = 0; i < this.treeView_zhongcihao.Nodes.Count; i++)
            {
                TreeNode tree_node = this.treeView_zhongcihao.Nodes[i];
                if (tree_node.ImageIndex != TYPE_GROUP)
                    continue;

                string strXml = (string)tree_node.Tag;

                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.LoadXml(strXml);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, "");
                    continue;
                }

                string strZhongcihaoDbName = DomUtil.GetAttr(dom.DocumentElement, "zhongcihaodb");

                if (String.IsNullOrEmpty(strZhongcihaoDbName) == false)
                    existing_dbnames.Add(strZhongcihaoDbName);
            }

            return existing_dbnames;
        }

        private void toolStripMenuItem_zhongcihao_insert_database_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_zhongcihao_insert_nstable_Click(object sender, EventArgs e)
        {

        }




    }
}