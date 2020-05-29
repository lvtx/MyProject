using System;
using System.Collections;
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
    /// <summary>
    /// ϵͳ����
    /// </summary>
    public partial class ManagerForm : MyForm
    {
        const int TYPE_ZHONGCIHAO_NSTABLE = 0;
        const int TYPE_ZHONGCIHAO_GROUP = 1;
        const int TYPE_ZHONGCIHAO_DATABASE = 2;
        const int TYPE_ZHONGCIHAO_ERROR = 3;

        const int TYPE_ARRANGEMENT_GROUP = 0;
        const int TYPE_ARRANGEMENT_LOCATION = 1;
        const int TYPE_ARRANGEMENT_ERROR = 2;

        // 
        /// <summary>
        /// ��ʾ��ǰȫ�����ݿ���Ϣ��XML�ַ���
        /// </summary>
        public string AllDatabaseInfoXml = "";

        const int WM_INITIAL = API.WM_USER + 201;
        const int WM_LOADSIZE = API.WM_USER + 202;

#if NO
        public LibraryChannel Channel = new LibraryChannel();
        public string Lang = "zh";

        /// <summary>
        /// ��ܴ���
        /// </summary>
        public MainForm MainForm = null;

        DigitalPlatform.Stop stop = null;
#endif

        string [] type_names = new string[] {
            "biblio","��Ŀ",
            "entity","ʵ��",
            "order","����",
            "issue","��",
            "reader","����",
            "message","��Ϣ",
            "arrived","ԤԼ����",
            "amerce","ΥԼ��",
            "invoice","��Ʊ",
            "publisher","������",
            "zhongcihao","�ִκ�",
            "dictionary","�ʵ�",
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
        internal string GetTypeName(string strTypeString)
        {
            for (int i = 0; i < type_names.Length / 2; i++)
            {
                if (type_names[i * 2] == strTypeString)
                    return type_names[i * 2+1];
            }

            return null;    // not found
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        public ManagerForm()
        {
            InitializeComponent();
        }

        private void ManagerForm_Load(object sender, EventArgs e)
        {
            if (this.MainForm != null)
            {
                MainForm.SetControlFont(this, this.MainForm.DefaultFont);
            }

#if NO
            this.Channel.Url = this.MainForm.LibraryServerUrl;

            this.Channel.BeforeLogin -= new BeforeLoginEventHandle(Channel_BeforeLogin);
            this.Channel.BeforeLogin += new BeforeLoginEventHandle(Channel_BeforeLogin);

            stop = new DigitalPlatform.Stop();
            stop.Register(MainForm.stopManager, true);	// ����������
#endif

            API.PostMessage(this.Handle, WM_LOADSIZE, 0, 0);

            API.PostMessage(this.Handle, WM_INITIAL, 0, 0);

            this.listView_opacDatabases.SmallImageList = this.imageList_opacDatabaseType;
            this.listView_opacDatabases.LargeImageList = this.imageList_opacDatabaseType;

            this.listView_databases.SmallImageList = this.imageList_opacDatabaseType;
            this.listView_databases.LargeImageList = this.imageList_opacDatabaseType;

            this.treeView_opacBrowseFormats.ImageList = this.imageList_opacBrowseFormatType;

            this.treeView_zhongcihao.ImageList = this.imageList_zhongcihao;

            this.treeView_arrangement.ImageList = this.imageList_arrangement;
        }

        /*public*/
        void LoadSize()
        {
#if NO
            // ���ô��ڳߴ�״̬
            MainForm.AppInfo.LoadMdiChildFormStates(this,
                "mdi_form_state");
#endif
        }

        /*public*/
        void SaveSize()
        {
#if NO
            MainForm.AppInfo.SaveMdiChildFormStates(this,
                "mdi_form_state");
#endif

            /*
            // ���MDI�Ӵ��ڲ���MainForm�ո�׼���˳�ʱ��״̬���ָ�����Ϊ�˼���ߴ���׼��
            if (this.WindowState != this.MainForm.MdiWindowState)
                this.WindowState = this.MainForm.MdiWindowState;
             * */
        }

        /// <summary>
        /// ȱʡ���ڹ���
        /// </summary>
        /// <param name="m">��Ϣ</param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_LOADSIZE:
                    LoadSize();
                    return;
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

                        treeView_zhongcihao_AfterSelect(this, null);


                        // �г��ż���ϵ����
                        nRet = this.ListArrangement(out strError);
                        if (nRet == -1)
                        {
                            MessageBox.Show(this, strError);
                        }

                        treeView_arrangement_AfterSelect(this, null);


                        // �г��ű�
                        nRet = this.ListScript(out strError);
                        if (nRet == -1)
                        {
                            MessageBox.Show(this, strError);
                        }

                        nRet = this.ListDup(out strError);
                        if (nRet == -1)
                        {
                            MessageBox.Show(this, strError);
                        }

                        // �г�ֵ�б�
                        nRet = this.ListValueTables(out strError);
                        if (nRet == -1)
                        {
                            MessageBox.Show(this, strError);
                        }

                        // �г����ķ�����
                        nRet = this.ListCenter(out strError);
                        if (nRet == -1)
                        {
                            MessageBox.Show(this, strError);
                        }
                    }
                    return;
            }
            base.DefWndProc(ref m);
        }

        /// <summary>
        /// �����Ƿ������޸�
        /// </summary>
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
#if NO
            if (stop != null)
            {
                if (stop.State == 0)    // 0 ��ʾ���ڴ���
                {
                    MessageBox.Show(this, "���ڹرմ���ǰֹͣ���ڽ��еĳ�ʱ������");
                    e.Cancel = true;
                    return;
                }

            }
#endif

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
                    "��ǰ������ ������ͨȨ�� ���屻�޸ĺ���δ���档����ʱ�رմ��ڣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪ�رմ���? ",
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
                    "��ǰ�������� �ݲصص� ���屻�޸ĺ���δ���档����ʱ�رմ��ڣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪ�رմ���? ",
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
                    "��ǰ�������� �ű� ���屻�޸ĺ���δ���档����ʱ�رմ��ڣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪ�رմ���? ",
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

            if (this.ValueTableChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�������� ֵ�б� ���屻�޸ĺ���δ���档����ʱ�رմ��ڣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪ�رմ���? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    this.tabControl_main.SelectedTab = this.tabPage_valueTable;
                    return;
                }
            }

            if (this.ZhongcihaoChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�������� �ִκ� ���屻�޸ĺ���δ���档����ʱ�رմ��ڣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪ�رմ���? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    this.tabControl_main.SelectedTab = this.tabPage_zhongcihaoDatabases;
                    return;
                }
            }

            if (this.ArrangementChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�������� �ż���ϵ ���屻�޸ĺ���δ���档����ʱ�رմ��ڣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪ�رմ���? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    this.tabControl_main.SelectedTab = this.tabPage_bookshelf;
                    return;
                }
            }

            if (this.DupChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ�������� ���� ���屻�޸ĺ���δ���档����ʱ�رմ��ڣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪ�رմ���? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    this.tabControl_main.SelectedTab = this.tabPage_dup;
                    return;
                }
            }
        }

        private void ManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
#if NO
            if (stop != null)
            {
                stop.Unregister(); // �������
                stop = null;
            }
#endif

            SaveSize();
        }

#if NO
        void Channel_BeforeLogin(object sender, BeforeLoginEventArgs e)
        {
            this.MainForm.Channel_BeforeLogin(this, e);
        }

        void DoStop(object sender, StopEventArgs e)
        {
            if (this.Channel != null)
                this.Channel.Abort();
        }
#endif

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

        /// <summary>
        /// ������߽�ֹ����ؼ����ڳ�����ǰ��һ����Ҫ��ֹ����ؼ���������ɺ�������
        /// </summary>
        /// <param name="bEnable">�Ƿ��������ؼ���true Ϊ���� false Ϊ��ֹ</param>
        public override void EnableControls(bool bEnable)
        {
            // this.button_clearAllDbs.Enabled = bEnable;
            this.toolStrip_databases.Enabled = bEnable;
        }

        private void ManagerForm_Activated(object sender, EventArgs e)
        {
            this.MainForm.stopManager.Active(this.stop);

            this.MainForm.MenuItem_recoverUrgentLog.Enabled = false;
            this.MainForm.MenuItem_font.Enabled = false;
            this.MainForm.MenuItem_restoreDefaultFont.Enabled = false;
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
                string strRole = DomUtil.GetAttr(node, "role");
                string strLibraryCode = DomUtil.GetAttr(node, "libraryCode");

                // 2008/7/2 new add
                // �յ����ֽ�������
                if (String.IsNullOrEmpty(strName) == true)
                    continue;

                string strTypeName = GetTypeName(strType);
                if (strTypeName == null)
                    strTypeName = strType;

                string strShuoming = "";
                if (string.IsNullOrEmpty(strRole) == false)
                    strShuoming += "��ɫ: " + strRole;
                {
                    if (string.IsNullOrEmpty(strLibraryCode) == false)
                    {
                        if (string.IsNullOrEmpty(strShuoming) == false)
                            strShuoming += "; ";
                        strShuoming += "ͼ��ݴ���: " + strLibraryCode;
                    }
                }


                ListViewItem item = new ListViewItem(strName, 0);
                item.SubItems.Add(strTypeName);
                item.SubItems.Add(strShuoming);
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
                this.toolStripButton_refreshDatabaseDef.Enabled = true;
            }
            else
            {
                this.toolStripButton_modifyDatabase.Enabled = false;
                this.toolStripButton_deleteDatabase.Enabled = false;
                this.toolStripButton_initializeDatabase.Enabled = false;
                this.toolStripButton_refreshDatabaseDef.Enabled = false;
            }
        }

        // ������Ŀ��
        private void ToolStripMenuItem_createBiblioDatabase_Click(object sender, EventArgs e)
        {
            BiblioDatabaseDialog dlg = new BiblioDatabaseDialog();
            MainForm.SetControlFont(dlg, this.Font, false);

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

        // 
        /// <summary>
        /// �������ݿ⡣
        /// ��ο� dp2Library API ManageDatabase() ����ϸ˵���������� strAction ����Ϊ "create" �� "recreate" ʱ�Ĺ���
        /// </summary>
        /// <param name="strDatabaseInfo">���ݿⶨ�� XML</param>
        /// <param name="bRecreate">�Ƿ�Ϊ���´���</param>
        /// <param name="strOutputInfo">���ز��������Ϣ</param>
        /// <param name="strError">���س�����Ϣ</param>
        /// <returns>-1: ����; 0: �ɹ�</returns>
        public int CreateDatabase(
            string strDatabaseInfo,
            bool bRecreate,
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
                    bRecreate == false ? "create" : "recreate",
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

        // 
        /// <summary>
        /// ɾ�����ݿ⡣
        /// ��ο� dp2Library API ManageDatabase() ����ϸ˵���������� strAction ����Ϊ "delete" ʱ�Ĺ���
        /// </summary>
        /// <param name="strDatabaseNames">Ҫɾ�������ݿ����б�</param>
        /// <param name="strOutputInfo">���ز��������Ϣ</param>
        /// <param name="strError">���س�����Ϣ</param>
        /// <returns>-1: ����; 0: �ɹ�</returns>
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

        // 2008/11/16 new add
        //      strDatabaseInfo Ҫˢ�µ������ļ����ԡ�<refreshStyle include="keys,browse" exclude="">(��ʾֻˢ��keys��browse������Ҫ�����ļ�)����<refreshStyle include="*" exclude="template">(��ʾˢ��ȫ���ļ������ǲ�Ҫˢ��template) �������ֵΪ�գ���ʾȫ��ˢ��
        // 
        /// <summary>
        /// ˢ�����ݿⶨ�塣
        /// ��ο� dp2Library API ManageDatabase() ����ϸ˵���������� strAction ����Ϊ "refresh" ʱ�Ĺ���
        /// </summary>
        /// <param name="strDatabaseNames">Ҫˢ�¶�������ݿ����б�</param>
        /// <param name="strDatabaseInfo">���ݿⶨ�� XML</param>
        /// <param name="strOutputInfo">���ز��������Ϣ</param>
        /// <param name="strError">���س�����Ϣ</param>
        /// <returns>-1: ����; 0: �ɹ�</returns>
        public int RefreshDatabasesDefs(
            string strDatabaseNames,
            string strDatabaseInfo,
            out string strOutputInfo,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("����ˢ�����ݿ� " + strDatabaseNames + " �Ķ���...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.ManageDatabase(
                    stop,
                    "refresh",
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

        // 
        /// <summary>
        /// ��ʼ�����ݿ⡣
        /// ��ο� dp2Library API ManageDatabase() ����ϸ˵���������� strAction ����Ϊ "initialize" ʱ�Ĺ���
        /// </summary>
        /// <param name="strDatabaseNames">Ҫ��ʼ�������ݿ����б�</param>
        /// <param name="strOutputInfo">���ز��������Ϣ</param>
        /// <param name="strError">���س�����Ϣ</param>
        /// <returns>-1: ����; 0: �ɹ�</returns>
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

        // 
        /// <summary>
        /// �޸����ݿⶨ�塣
        /// ��ο� dp2Library API ManageDatabase() ����ϸ˵���������� strAction ����Ϊ "change" ʱ�Ĺ���
        /// </summary>
        /// <param name="strDatabaseNames">Ҫ�޸Ķ�������ݿ����б�</param>
        /// <param name="strDatabaseInfo">���ݿⶨ�� XML</param>
        /// <param name="strOutputInfo">���ز��������Ϣ</param>
        /// <param name="strError">���س�����Ϣ</param>
        /// <returns>-1: ����; 0: �ɹ�</returns>
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

            string strDbNameList = ListViewUtil.GetItemNameList(this.listView_databases.SelectedItems);
            /*
            foreach (ListViewItem item in this.listView_databases.SelectedItems)
            {
                if (string.IsNullOrEmpty(strDbNameList) == false)
                    strDbNameList += ",";
                strDbNameList += item.Text;
            }
             * */

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


        // ���´������ݿ�
        private void menu_recreateDatabase_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            if (this.listView_databases.SelectedItems.Count == 0)
            {
                strError = "��δѡ��Ҫ���´��������ݿ�";
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
                MainForm.SetControlFont(dlg, this.Font, false);

                dlg.Text = "���´�����Ŀ��";
                dlg.ManagerForm = this;
                dlg.CreateMode = true;
                dlg.Recreate = true;
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
                MainForm.SetControlFont(dlg, this.Font, false);

                dlg.Text = "���´������߿�";
                dlg.ManagerForm = this;
                dlg.CreateMode = true;
                dlg.Recreate = true;
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
                MainForm.SetControlFont(dlg, this.Font, false);

                /*
                string strTypeName = GetTypeName(strType);
                if (strTypeName == null)
                    strTypeName = strType;
                 * */

                dlg.Text = "���´���" + strTypeName + "��";
                dlg.ManagerForm = this;
                dlg.CreateMode = true;
                dlg.Recreate = true;
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
                MainForm.SetControlFont(dlg, this.Font, false);

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
                MainForm.SetControlFont(dlg, this.Font, false);

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
            else if (strType == "message"
                || strType == "amerce"
                || strType == "invoice"
                || strType == "arrived"
                || strType == "zhongcihao"
                || strType == "publisher"
                || strType == "dictionary")
            {
                SimpleDatabaseDialog dlg = new SimpleDatabaseDialog();
                MainForm.SetControlFont(dlg, this.Font, false);

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

            // ���´������ݿ�
            {
                menuItem = new MenuItem("���´���" + strType + "�� '" + strName + "'(&M)");
                menuItem.Click += new System.EventHandler(this.menu_recreateDatabase_Click);
                if (this.listView_databases.SelectedItems.Count == 0)
                    menuItem.Enabled = false;
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

            menuItem = new MenuItem("������Ʊ��(&I)");
            menuItem.Click += new System.EventHandler(this.ToolStripMenuItem_createInvoiceDatabase_Click);
            contextMenu.MenuItems.Add(menuItem);


            menuItem = new MenuItem("����ԤԼ�����(&R)");
            menuItem.Click += new System.EventHandler(this.ToolStripMenuItem_createArrivedDatabase_Click);
            contextMenu.MenuItems.Add(menuItem);


            menuItem = new MenuItem("������Ϣ��(&M)");
            menuItem.Click += new System.EventHandler(this.ToolStripMenuItem_createMessageDatabase_Click);
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("�����ִκſ�(&Z)");
            menuItem.Click += new System.EventHandler(this.ToolStripMenuItem_createZhongcihaoDatabase_Click);
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("���������߿�(&P)");
            menuItem.Click += new System.EventHandler(this.ToolStripMenuItem_createPublisherDatabase_Click);
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("�����ʵ��(&D)");
            menuItem.Click += new System.EventHandler(this.ToolStripMenuItem_createDictionaryDatabase_Click);
            contextMenu.MenuItems.Add(menuItem);
            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            string strText = "";
            if (this.listView_databases.SelectedItems.Count == 1)
                strText = "ɾ��" + strType + "�� '" + strName + "'(&D)";
            else
                strText = "ɾ����ѡ " + this.listView_databases.SelectedItems.Count.ToString() + " �����ݿ�(&D)";

            menuItem = new MenuItem(strText);
            menuItem.Click += new System.EventHandler(this.toolStripButton_deleteDatabase_Click);
            if (this.listView_databases.SelectedItems.Count == 0)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            if (this.listView_databases.SelectedItems.Count == 1)
                strText = "��ʼ��" + strType + "�� '" + strName + "'(&I)";
            else
                strText = "��ʼ����ѡ " + this.listView_databases.SelectedItems.Count.ToString() + " �����ݿ�(&I)";

            menuItem = new MenuItem(strText);
            menuItem.Click += new System.EventHandler(this.toolStripButton_initializeDatabase_Click);
            if (this.listView_databases.SelectedItems.Count == 0)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);


            if (this.listView_databases.SelectedItems.Count == 1)
                strText = "ˢ��" + strType + "�� '" + strName + "' �Ķ���(&R)";
            else
                strText = "ˢ����ѡ " + this.listView_databases.SelectedItems.Count.ToString() + " �����ݿ�Ķ���(&R)";

            menuItem = new MenuItem(strText);
            menuItem.Click += new System.EventHandler(this.toolStripButton_refreshDatabaseDef_Click);
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

            foreach (ListViewItem item in this.listView_databases.SelectedItems)
            {
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
            MainForm.SetControlFont(dlg, this.Font, false);

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

        // ������Ʊ��
        private void ToolStripMenuItem_createInvoiceDatabase_Click(object sender, EventArgs e)
        {
            CreateSimpleDatabase("invoice", "", "");
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
            MainForm.SetControlFont(dlg, this.Font, false);

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

        private void ToolStripMenuItem_createDictionaryDatabase_Click(object sender, EventArgs e)
        {
            CreateSimpleDatabase("dictionary", "", "");
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
        internal int ConfirmLogin(out string strError)
        {
            strError = "";

            ConfirmSupervisorDialog login_dlg = new ConfirmSupervisorDialog();
            MainForm.SetControlFont(login_dlg, this.MainForm.DefaultFont);
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
            string strParameters = "location=" + strLocation + ",type=worker";

            // return:
            //      -1  error
            //      0   ��¼δ�ɹ�
            //      1   ��¼�ɹ�
            long lRet = this.Channel.Login(login_dlg.UserName,
                login_dlg.Password,
                strParameters,
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

        // parameters:
        //      strDbPaths  �ֺŷָ�����ݿ�ȫ·���б�
        void ReplaceHostName(ref string strDbPaths)
        {
            Uri library_uri = new Uri(this.MainForm.LibraryServerDir1);
            if (library_uri.IsLoopback == true)
                return; // ˵��ǰ�˺�ͼ��ݷ�����ͬ��һ̨�������Ͳ����滻��

            string[] parts = strDbPaths.Split(new char[] {';'});
            string strResult = "";
            for (int i = 0; i < parts.Length; i++)
            {
                string strDbPath = parts[i].Trim();
                if (String.IsNullOrEmpty(strDbPaths) == true)
                    continue;
                
                Uri uri = new Uri(strDbPath);
                if (uri.IsLoopback == true)
                {
                    string strQuery = "";  // ����У��Ѿ�����ǰ����ʺ�
                    int nRet = strDbPath.LastIndexOf("?");
                    if (nRet != -1)
                        strQuery = strDbPath.Substring(nRet);

                    strDbPath = uri.Scheme + Uri.SchemeDelimiter + library_uri.Host
                        + (uri.IsDefaultPort == true ? "" : ":" + uri.Port.ToString())  // 2012/3/30 ����ð��
                        // + uri.PathAndQuery �������������õģ����Ǻ��ֵ����ݿ�����escape��
                        + uri.LocalPath
                        + strQuery;
                }

                if (String.IsNullOrEmpty(strResult) == false)
                    strResult += ";";

                strResult += strDbPath;
            }

            strDbPaths = strResult;
        }

        // ˢ�����ݿⶨ��
        private void toolStripButton_refreshDatabaseDef_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;
            if (this.listView_databases.SelectedIndices.Count == 0)
            {
                strError = "��δѡ��Ҫˢ�¶�������ݿ�����";
                goto ERROR1;
            }

            string strDbNameList = ListViewUtil.GetItemNameList(this.listView_databases.SelectedItems);
            /*
            foreach (ListViewItem item in this.listView_databases.SelectedItems)
            {
                if (string.IsNullOrEmpty(strDbNameList) == false)
                    strDbNameList += ",";
                strDbNameList += item.Text;
            }
             * */

            // �Ի��򾯸�
            DialogResult result = MessageBox.Show(this,
                "ȷʵҪˢ�����ݿ� " + strDbNameList + " �Ķ���?\r\n\r\n˵����1) ���ݿⱻˢ�¶���󣬸������������Ҫ����ˢ�����ݿ��ڼ�¼�ļ�����Ĳ���(�������м�¼�ļ�������ܻ᲻ȫ)��\r\n      2) ���ˢ�µ���(��)��Ŀ��Ķ��壬��(��)��Ŀ�������ʵ��⡢�����⡢�ڿ�Ҳ��һ����ˢ�¶��塣",
                "ManagerForm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
                return;

            RefreshStyleDialog style_dlg = new RefreshStyleDialog();
            MainForm.SetControlFont(style_dlg, this.Font, false);

            style_dlg.StartPosition = FormStartPosition.CenterScreen;
            style_dlg.ShowDialog(this);

            if (style_dlg.DialogResult == DialogResult.Cancel)
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
                strError = "ˢ�����ݿⶨ��Ĳ���������";
                goto ERROR1;
            }

            EnableControls(false);

            try
            {
                /*
                List<string> dbnames = new List<string>();

                for (int i = this.listView_databases.SelectedIndices.Count - 1;
                    i >= 0;
                    i--)
                {
                    int index = this.listView_databases.SelectedIndices[i];

                    string strDatabaseName = this.listView_databases.Items[index].Text;

                    dbnames.Add(strDatabaseName);
                }
                string strDbNameList = StringUtil.MakePathList(dbnames);
                 * */

                XmlDocument style_dom = new XmlDocument();
                style_dom.LoadXml("<refreshStyle />");
                DomUtil.SetAttr(style_dom.DocumentElement,
                    "include", style_dlg.IncludeFilenames);
                DomUtil.SetAttr(style_dom.DocumentElement,
                     "exclude", style_dlg.ExcludeFilenames);

                string strKeysChangedDbpaths = "";

                string strOutputInfo = "";

                //      strDatabaseInfo Ҫˢ�µ������ļ����ԡ�<refreshStyle include="keys,browse" exclude="">(��ʾֻˢ��keys��browse������Ҫ�����ļ�)����<refreshStyle include="*" exclude="template">(��ʾˢ��ȫ���ļ������ǲ�Ҫˢ��template) �������ֵΪ�գ���ʾȫ��ˢ��
                nRet = RefreshDatabasesDefs(strDbNameList,
                    style_dom.DocumentElement.OuterXml,
                    out strOutputInfo,
                    out strError);
                if (String.IsNullOrEmpty(strOutputInfo) == false)
                {
                    XmlDocument dom = new XmlDocument();
                    try
                    {
                        dom.LoadXml(strOutputInfo);
                    }
                    catch (Exception ex)
                    {
                        strError = "RefreshDatabasesDefs()�����ص�strOutputInfoװ��XMLDOMʱ����: " + ex.Message;
                        goto ERROR1;
                    }
                    strKeysChangedDbpaths = DomUtil.GetAttr(dom.DocumentElement, "dbpaths");

                    ReplaceHostName(ref strKeysChangedDbpaths);
                }

                if (nRet == -1)
                {
                    if (String.IsNullOrEmpty(strKeysChangedDbpaths) == false)
                        strError += "�����������ں����ݿ�ļ����㶨���Ѿ������޸�:\r\n---\r\n" + strKeysChangedDbpaths.Replace(";","\r\n") + "\r\n---\r\n��Ҫ����dp2Batch��������ˢ����Щ�ں����ݿ�ļ�¼�ļ�����";
                    goto ERROR1;
                }


                // TODO: ������Щ���ݿ���Ҫˢ�¼�����
                if (String.IsNullOrEmpty(strKeysChangedDbpaths) == false)
                {
                    /*
                    string strPathList = "";
                    string strNameList = "";

                    string[] dbnames = strKeysChangedDbpaths.Split(new char[] {';'});
                    for (int i = 0; i < dbnames.Length; i++)
                    {
                        string strPath = dbnames[i].Trim();
                        if (String.IsNullOrEmpty(strPath) == true)
                            continue;

                        string strDbName = "";
                        nRet = strPath.IndexOf("?");
                        if (nRet != -1)
                            strDbName = strPath.Substring(nRet+1).Trim();
                        else
                            strDbName = strPath;

                        if (String.IsNullOrEmpty(strPathList) == false)
                            strPathList += ";";

                        strPathList += strPath;

                        if (String.IsNullOrEmpty(strNameList) == false)
                            strNameList += ",";
                        strNameList += strDbName;
                    }
                     * */

                    strError = "�����ں����ݿ�ļ����㶨���Ѿ������޸�: \r\n---\r\n" + strKeysChangedDbpaths.Replace(";","\r\n") + "\r\n---\r\n��Ҫ����dp2Batch(��dp2������)���ؽ���Щ�ں����ݿ������(��Щ�ں����ݿ��·���Ѿ�����Windows����������)";
                    Clipboard.SetDataObject(strKeysChangedDbpaths);

                }
                else
                    strError = "";
            }
            finally
            {
                EnableControls(true);
            }

            if (String.IsNullOrEmpty(strError) == false)
                MessageBox.Show(this, strError);
            return;
        ERROR1:
            MessageBox.Show(this, strError);
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

            string strDbNameList = ListViewUtil.GetItemNameList(this.listView_databases.SelectedItems);
            /*
            foreach (ListViewItem item in this.listView_databases.SelectedItems)
            {
                if (string.IsNullOrEmpty(strDbNameList) == false)
                    strDbNameList += ",";
                strDbNameList += item.Text;
            }
             * */

            // �Ի��򾯸�
            DialogResult result = MessageBox.Show(this,
                "ȷʵҪ��ʼ�����ݿ� " + strDbNameList + "?\r\n\r\n���棺\r\n1) ���ݿ�һ������ʼ�������ڵ����ݼ�¼��ȫ����ʧ������Ҳ�޷���ԭ��\r\n2) �����ʼ��������Ŀ�⣬����Ŀ�������ʵ��⡢�����⡢�ڿ�Ҳ��һ������ʼ����",
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
            <caption lang="zh-CN">�����鿯</caption>
            <caption lang="en">Chinese Books and Series</caption>
            <from style="title">
                <caption lang="zh-CN">����</caption>
                <caption lang="en">Title</caption>
            </from>
            <from style="author">
                <caption lang="zh-CN">����</caption>
                <caption lang="en">Author</caption>
            </from>
            <database name="����ͼ��" />
            <database name="�����ڿ�" />
        </virtualDatabase>
        <database name="�û�">
            <caption lang="zh-CN">�û�</caption>
            <caption lang="en">account</caption>
            <from name="�û���">
                <caption lang="zh-CN">�û���</caption>
                <caption lang="en">username</caption>
            </from>
            <from name="__id" />
        </database>
        <database name="����ͼ��">
            <caption lang="zh-CN">����ͼ��</caption>
            <caption lang="en">Chinese book</caption>
            <from name="ISBN">
                <caption lang="zh-CN">ISBN</caption>
                <caption lang="en">ISBN</caption>
            </from>
            <from name="ISSN">
                <caption lang="zh-CN">ISSN</caption>
                <caption lang="en">ISSN</caption>
            </from>
            <from name="����">
                <caption lang="zh-CN">����</caption>
                <caption lang="en">Title</caption>
            </from>
            <from name="����ƴ��">
                <caption lang="zh-CN">����ƴ��</caption>
                <caption lang="en">Title pinyin</caption>
            </from>
            <from name="�����">
                <caption lang="zh-CN">�����</caption>
                <caption lang="en">Thesaurus</caption>
            </from>
            <from name="�ؼ���">
                <caption lang="zh-CN">�ؼ���</caption>
                <caption lang="en">Keyword</caption>
            </from>
            <from name="�����">
                <caption lang="zh-CN">�����</caption>
                <caption lang="en">Class number</caption>
            </from>
            <from name="������">
                <caption lang="zh-CN">������</caption>
                <caption lang="en">Contributor</caption>
            </from>
            <from name="������ƴ��">
                <caption lang="zh-CN">������ƴ��</caption>
                <caption lang="en">Contributor pinyin</caption>
            </from>
            <from name="������">
                <caption lang="zh-CN">������</caption>
                <caption lang="en">Publisher</caption>
            </from>
            <from name="��ȡ��">
                <caption lang="zh">��ȡ��</caption>
                <caption lang="en">Call number</caption>
            </from>
            <from name="�ղص�λ">
                <caption lang="zh-CN">�ղص�λ</caption>
                <caption lang="en">Rights holder</caption>
            </from>
            <from name="��ȡ���">
                <caption lang="zh">��ȡ���</caption>
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

        // 
        /// <summary>
        /// �����ͨ���ݿⶨ�塣
        /// ��ο� dp2Library API GetSystemParameter()����ϸ˵���������� strCategory ����Ϊ "database_def" ʱ�Ĺ���
        /// </summary>
        /// <param name="strDbName">���ݿ���</param>
        /// <param name="strOutputInfo">�������ݿⶨ�� XML</param>
        /// <param name="strError">���س�����Ϣ</param>
        /// <returns>-1: ����; 0: û�еõ���Ҫ�����Ϣ; 1: �õ���Ҫ�����Ϣ</returns>
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

            // �Ѿ����ڵĿ���
            List<string> existing_opac_normal_dbnames = new List<string>();
            for (int i = 0; i < this.listView_opacDatabases.Items.Count; i++)
            {
                ListViewItem current_item = this.listView_opacDatabases.Items[i];
                string strCurrentName = current_item.Text;
                string strCurrentType = ListViewUtil.GetItemText(current_item, 1);

                if (strCurrentType == "��ͨ��")
                    existing_opac_normal_dbnames.Add(strCurrentName);
            }

            OpacVirtualDatabaseDialog dlg = new OpacVirtualDatabaseDialog();
            MainForm.SetControlFont(dlg, this.Font, false);

            dlg.Text = "��������ⶨ��";
            dlg.ExistingOpacNormalDbNames = existing_opac_normal_dbnames;
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
        internal int DetectVirtualDatabaseNameDup(string strCaptionsXml,
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

            foreach (ListViewItem item in this.listView_opacDatabases.SelectedItems)
            {
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
                // �Ѿ����ڵĿ���
                List<string> existing_opac_normal_dbnames = new List<string>();
                for (int i = 0; i < this.listView_opacDatabases.Items.Count; i++)
                {
                    ListViewItem current_item = this.listView_opacDatabases.Items[i];
                    string strCurrentName = current_item.Text;
                    string strCurrentType = ListViewUtil.GetItemText(current_item, 1);

                    if (strCurrentType == "��ͨ��")
                        existing_opac_normal_dbnames.Add(strCurrentName);
                }

                OpacVirtualDatabaseDialog dlg = new OpacVirtualDatabaseDialog();
                MainForm.SetControlFont(dlg, this.Font, false);

                dlg.Text = "�޸�����ⶨ��";
                dlg.ExistingOpacNormalDbNames = existing_opac_normal_dbnames;
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
                MainForm.SetControlFont(dlg, this.Font, false);

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
            MainForm.SetControlFont(dlg, this.Font, false);

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

            string strDbNameList = ListViewUtil.GetItemNameList(this.listView_opacDatabases.SelectedItems);
            /*
            foreach (ListViewItem item in this.listView_opacDatabases.SelectedItems)
            {
                if (string.IsNullOrEmpty(strDbNameList) == false)
                    strDbNameList += ",";
                strDbNameList += item.Text;
            }
             * */

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
            int nRet = 0;

            // �Ի��򾯸�
            DialogResult result = MessageBox.Show(this,
                "ȷʵҪ��ʼ��***����***���ݿ� ?\r\n\r\n���棺\r\n1) ���ݿ�һ������ʼ�������ڵ����ݼ�¼��ȫ����ʧ������Ҳ�޷���ԭ��\r\n2) �����ʼ��������Ŀ�⣬����Ŀ�������ʵ��⡢�����⡢�ڿ�Ҳ��һ������ʼ����",
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
                strError = "��ʼ���������ݿ����������";
                goto ERROR1;
            }

            EnableControls(false);

            try
            {
                nRet = ClearAllDbs(out strError);
                if (nRet == -1)
                    goto ERROR1;
                else
                    MessageBox.Show(this, "OK");
            }
            finally
            {
                EnableControls(true);
            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);
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

                    /*
                    string strFormatName = DomUtil.GetAttr(format_node, "name");
                    string strType = DomUtil.GetAttr(format_node, "type");
                    string strScriptFile = DomUtil.GetAttr(format_node, "scriptfile");
                    string strStyle = DomUtil.GetAttr(format_node, "style");

                    string strDisplayText = strFormatName;
                    
                    if (String.IsNullOrEmpty(strType) == false)
                        strDisplayText += " type=" + strType;

                    if (String.IsNullOrEmpty(strScriptFile) == false)
                        strDisplayText += " scriptfile=" + strScriptFile;
                     * */

                    TreeNode format_treenode = new TreeNode(GetFormatDisplayString(format_node), 1, 1);
                    format_treenode.Tag = format_node.OuterXml;

                    database_treenode.Nodes.Add(format_treenode);
                }
            }

            this.treeView_opacBrowseFormats.ExpandAll();

            return 0;
        }

        static string GetFormatDisplayString(XmlNode format_node)
        {
            string strFormatName = DomUtil.GetAttr(format_node, "name");
            string strType = DomUtil.GetAttr(format_node, "type");
            string strScriptFile = DomUtil.GetAttr(format_node, "scriptfile");
            string strStyle = DomUtil.GetAttr(format_node, "style");

            string strDisplayText = strFormatName;

            if (String.IsNullOrEmpty(strType) == false)
                strDisplayText += " type=" + strType;

            if (String.IsNullOrEmpty(strScriptFile) == false)
                strDisplayText += " scriptfile=" + strScriptFile;

            if (String.IsNullOrEmpty(strStyle) == false)
                strDisplayText += " style=" + strStyle;

            return strDisplayText;
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
            MainForm.SetControlFont(dlg, this.Font, false);

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
            MainForm.SetControlFont(dlg, this.Font, false);

            // TODO: ������ݿ�Ϊ��Ŀ�⣬��typeӦ��Ԥ��Ϊ"biblio"
            dlg.Text = "��ָ����ʾ��ʽ������";
            // dlg.FormatName = "";
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<format />");

            /*
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

            if (String.IsNullOrEmpty(dlg.FormatStyle) == false)
            {
                strDisplayText += " style=" + dlg.FormatStyle;
                DomUtil.SetAttr(dom.DocumentElement, "style", dlg.FormatStyle);
            }
             * */

            // 2009/6/27 new add
            if (String.IsNullOrEmpty(dlg.CaptionsXml) == false)
                dom.DocumentElement.InnerXml = dlg.CaptionsXml;

            TreeNode new_treenode = new TreeNode(GetFormatDisplayString(dom.DocumentElement), 1, 1);
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
                MainForm.SetControlFont(dlg, this.Font, false);

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
                if (current_treenode.Parent != null)
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


                // ��ʾ��ʽ
                OpacBrowseFormatDialog dlg = new OpacBrowseFormatDialog();
                MainForm.SetControlFont(dlg, this.Font, false);

                dlg.Text = "��ָ����ʾ��ʽ������";
                // dlg.FormatName = DomUtil.GetAttr(dom.DocumentElement, "name");
                dlg.CaptionsXml = dom.DocumentElement.InnerXml; // 2009/6/27 new add
                dlg.FormatType = DomUtil.GetAttr(dom.DocumentElement, "type");
                dlg.ScriptFile = DomUtil.GetAttr(dom.DocumentElement, "scriptfile");
                dlg.FormatStyle = DomUtil.GetAttr(dom.DocumentElement, "style");
                dlg.StartPosition = FormStartPosition.CenterScreen;
                dlg.ShowDialog(this);

                if (dlg.DialogResult != DialogResult.OK)
                    return;

                /*
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

                if (String.IsNullOrEmpty(dlg.FormatStyle) == false)
                {
                    strDisplayText += " style=" + dlg.FormatStyle;
                    DomUtil.SetAttr(dom.DocumentElement, "style", dlg.FormatStyle);
                }
                 * */

                // 2009/6/27 new add
                if (String.IsNullOrEmpty(dlg.CaptionsXml) == false)
                    dom.DocumentElement.InnerXml = dlg.CaptionsXml;

                // 2009/6/27 new add
                current_treenode.Text = GetFormatDisplayString(dom.DocumentElement);

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

            // 2009/6/27 new add
            dom.DocumentElement.InnerXml = "<caption lang=\"zh-CN\">��ϸ</caption><caption lang=\"en\">Detail</caption>";

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

        // �������ߺͲ������б�
        private void toolStripButton_loanPolicy_createTypes_Click(object sender, EventArgs e)
        {
            string strError = "";
            List<string> booktypes = null;
            List<string> readertypes = null;

            // ��XML������
            // ��ö��ߺ�ͼ�������б�
            int nRet = GetReaderAndBookTypes(out readertypes,
                out booktypes,
                out strError);
            if (nRet == -1)
                goto ERROR1;

            // ��library.xml��<valueTables>ȡ��

            // 

            if (booktypes.Count > 0)
            {
                if (this.textBox_loanPolicy_bookTypes.Text != "")
                {
                    // ������δ����
                    DialogResult result = MessageBox.Show(this,
                        "��ǰͼ�������ı������Ѿ������ݡ�\r\n\r\n�Ƿ�Ҫ׷����ֵ������?\r\n\r\n(Yes ׷��; No ����; Cancel ����)",
                        "ManagerForm",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        // ׷��
                        List<string> old = MakeStringList(this.textBox_loanPolicy_bookTypes.Text);
                        old.AddRange(booktypes);
                        StringUtil.RemoveDupNoSort(ref old);
                        this.textBox_loanPolicy_bookTypes.Text = StringUtil.MakePathList(old,
                            "\r\n");
                    }
                    else if (result == DialogResult.No)
                    {
                        // ����
                        this.textBox_loanPolicy_bookTypes.Text = StringUtil.MakePathList(booktypes,
                            "\r\n");
                    }
                }
                else
                {
                    // ����
                    this.textBox_loanPolicy_bookTypes.Text = StringUtil.MakePathList(booktypes,
                        "\r\n");
                }
            }
            else
            {
                MessageBox.Show(this, "û�з����κ�ͼ������");
            }

            if (readertypes.Count > 0)
            {
                if (this.textBox_loanPolicy_readerTypes.Text != "")
                {
                    // ������δ����
                    DialogResult result = MessageBox.Show(this,
                        "��ǰ���������ı������Ѿ������ݡ�\r\n\r\n�Ƿ�Ҫ׷����ֵ������?\r\n\r\n(Yes ׷��; No ����; Cancel ����)",
                        "ManagerForm",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        // ׷��
                        List<string> old = MakeStringList(this.textBox_loanPolicy_readerTypes.Text);
                        old.AddRange(readertypes);
                        StringUtil.RemoveDupNoSort(ref old);
                        this.textBox_loanPolicy_readerTypes.Text = StringUtil.MakePathList(old,
                            "\r\n");
                    }
                    else if (result == DialogResult.No)
                    {
                        // ����
                        this.textBox_loanPolicy_readerTypes.Text = StringUtil.MakePathList(readertypes,
                            "\r\n");
                    }
                }
                else
                {
                    // ����
                    this.textBox_loanPolicy_readerTypes.Text = StringUtil.MakePathList(readertypes,
                        "\r\n");
                }
            }
            else
            {
                MessageBox.Show(this, "û�з����κζ�������");
            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        private void tabControl_loanPolicy_down_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl_loanPolicy_down.SelectedTab == this.tabPage_loanPolicy_html)
            {
                this.toolStripButton_loanPolicy_createTypes.Enabled = false;

                // HTML�������ҳ�����Ҫͬ��
                SynchronizeLoanPolicy();
            }
            else
            {
                Debug.Assert(this.tabControl_loanPolicy_down.SelectedTab == this.tabPage_loanPolicy_types, "");

                this.toolStripButton_loanPolicy_createTypes.Enabled = true;

                // types����ҳ�����Ҫͬ��
                SynchronizeRightsTableAndTypes();
            }

        }

        private void textBox_loanPolicy_rightsTableDef_TextChanged(object sender, EventArgs e)
        {
            // XML�༭���еİ汾�����仯
            this.m_nRightsTableXmlVersion++;
            this.LoanPolicyDefChanged = true;
        }

        private void textBox_loanPolicy_readerTypes_TextChanged(object sender, EventArgs e)
        {
            // �������ͷ����仯
            this.m_nRightsTableTypesVersion++;
            this.LoanPolicyDefChanged = true;
        }

        private void textBox_loanPolicy_bookTypes_TextChanged(object sender, EventArgs e)
        {
            // ͼ�����ͷ����仯
            this.m_nRightsTableTypesVersion++;
            this.LoanPolicyDefChanged = true;
        }

        private void textBox_loanPolicy_rightsTableDef_Enter(object sender, EventArgs e)
        {
            SynchronizeLoanPolicy();
        }

        private void textBox_loanPolicy_rightsTableDef_Leave(object sender, EventArgs e)
        {
            if (this.Disposing == false)
                SynchronizeLoanPolicy();
        }

        private void toolStripButton_loanPolicy_save_Click(object sender, EventArgs e)
        {
            string strError = "";

            SynchronizeLoanPolicy();

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
                    dom.LoadXml("<rightsTable />");
                }

                /*
                string strReaderTypesXml = BuildTypesXml(this.textBox_loanPolicy_readerTypes);
                string strBookTypesXml = BuildTypesXml(this.textBox_loanPolicy_bookTypes);

                {
                    XmlNode nodeReaderTypes = dom.DocumentElement.SelectSingleNode("readerTypes");
                    if (nodeReaderTypes == null)
                    {
                        nodeReaderTypes = dom.CreateElement("readerTypes");
                        dom.DocumentElement.AppendChild(nodeReaderTypes);
                    }

                    nodeReaderTypes.InnerXml = strReaderTypesXml;
                }

                {
                    XmlNode nodeBookTypes = dom.DocumentElement.SelectSingleNode("bookTypes");
                    if (nodeBookTypes == null)
                    {
                        nodeBookTypes = dom.CreateElement("bookTypes");
                        dom.DocumentElement.AppendChild(nodeBookTypes);
                    }

                    nodeBookTypes.InnerXml = strBookTypesXml;
                }
                 * */

                // �ƺ�û�б�Ҫ����һ��
                // types�༭���� --> DOM�е�<readerTypes>��<bookTypes>����
                // ����ǰdom��Ӧ���Ѿ�װ����Ȩ��XML����
                TypesToRightsXml(ref dom);

                strRightsTableXml = dom.DocumentElement.InnerXml;
            }


            // ������ͨ����Ȩ����ض���
            // parameters:
            //      strRightsTableXml   ��ͨ����Ȩ�޶���XML��ע�⣬û�и�Ԫ��
            int nRet = SetRightsTableDef(strRightsTableXml,
                //strReaderTypesXml,
                //strBookTypesXml,
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
            <locationTypes>
                <item canborrow="yes">��ͨ��</item>
                <item>������</item>
                <library code="�ֹ�1">
                    <item canborrow="yes">��ͨ��</item>
                    <item>������</item>
                </library>
            </locationTypes>
            */

            XmlNodeList nodes = dom.DocumentElement.SelectNodes("//item");
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

                // 
                string strLibraryCode = "";
                XmlNode parent = node.ParentNode;
                if (parent.Name == "library")
                {
                    strLibraryCode = DomUtil.GetAttr(parent, "code");
                }

                ListViewItem item = new ListViewItem(strLibraryCode, 0);
                item.SubItems.Add(strText);
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
            stop.Initial("���ڻ�ȡ<locationTypes>���� ...");
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

        // �޸�/����ȫ���ݲصض���<locationTypes>
        int SetAllLocationTypesInfo(string strLocationDef,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("��������<locationTypes>���� ...");
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

        // ����<locationTypes>�����XMLƬ��
        // ע�����¼�Ƭ�϶��壬û��<locationTypes>Ԫ����Ϊ����
        int BuildLocationTypesDef(out string strLocationDef,
            out string strError)
        {
            strError = "";
            strLocationDef = "";

            Hashtable table = new Hashtable();

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<locationTypes />");

            for (int i = 0; i < this.listView_location_list.Items.Count; i++)
            {
                ListViewItem item = this.listView_location_list.Items[i];
                string strLibraryCode = ListViewUtil.GetItemText(item, 0);
                string strLocation = ListViewUtil.GetItemText(item, 1);
                string strCanBorrow = ListViewUtil.GetItemText(item, 2);

                bool bCanBorrow = false;

                if (strCanBorrow == "��" || strCanBorrow == "yes")
                    bCanBorrow = true;

                if (string.IsNullOrEmpty(strLibraryCode) == true)
                {
                    XmlNode nodeItem = dom.CreateElement("item");
                    dom.DocumentElement.AppendChild(nodeItem);

                    nodeItem.InnerText = strLocation;
                    DomUtil.SetAttr(nodeItem, "canborrow", bCanBorrow == true ? "yes" : "no");
                }
                else
                {
                    // ���չݴ������ۼ�
                    List<ListViewItem> items = (List<ListViewItem>)table[strLibraryCode];
                    if (items == null)
                    {
                        items = new List<ListViewItem>();
                        table[strLibraryCode] = items;
                    }
                    items.Add(item);
                }
            }

            if (table.Count > 0)
            {
                string[] keys = new string[table.Count];
                table.Keys.CopyTo(keys, 0);
                Array.Sort(keys);

                foreach (string key in keys)
                {
                    List<ListViewItem> items = (List<ListViewItem>)table[key];

                    XmlNode nodeLibrary = dom.CreateElement("library");
                    dom.DocumentElement.AppendChild(nodeLibrary);
                    DomUtil.SetAttr(nodeLibrary, "code", key);

                    foreach (ListViewItem item in items)
                    {
                        string strLocation = ListViewUtil.GetItemText(item, 1);
                        string strCanBorrow = ListViewUtil.GetItemText(item, 2);

                        bool bCanBorrow = false;

                        if (strCanBorrow == "��" || strCanBorrow == "yes")
                            bCanBorrow = true;

                        XmlNode nodeItem = dom.CreateElement("item");
                        nodeLibrary.AppendChild(nodeItem);

                        nodeItem.InnerText = strLocation;
                        DomUtil.SetAttr(nodeItem, "canborrow", bCanBorrow == true ? "yes" : "no");
                    }
                }
            }

            strLocationDef = dom.DocumentElement.InnerXml;
            return 0;
        }

        // �ύ<locationTypes>�����޸�
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

        /// <summary>
        /// �ݲصض����Ƿ��޸�
        /// </summary>
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

        // ���� �ݲص� ����
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
            MainForm.SetControlFont(dlg, this.Font, false);

            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            // TODO: ����?

            ListViewItem item = new ListViewItem(dlg.LibraryCode, 0);
            item.SubItems.Add(dlg.LocationString);
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
            MainForm.SetControlFont(dlg, this.Font, false);

            dlg.LibraryCode = ListViewUtil.GetItemText(item, 0);
            dlg.LocationString = ListViewUtil.GetItemText(item, 1);
            dlg.CanBorrow = (ListViewUtil.GetItemText(item, 2) == "��") ? true : false;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            ListViewUtil.ChangeItemText(item, 0, dlg.LibraryCode);
            ListViewUtil.ChangeItemText(item, 1, dlg.LocationString);
            ListViewUtil.ChangeItemText(item, 2, dlg.CanBorrow == true ? "��" : "��");

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

            string strItemNameList = ListViewUtil.GetItemNameList(this.listView_location_list.SelectedItems);

            // �Ի��򾯸�
            DialogResult result = MessageBox.Show(this,
                "ȷʵҪɾ���ݲصص����� " + strItemNameList + "?",
                "ManagerForm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
                return;

#if NO
            for (int i = this.listView_location_list.SelectedIndices.Count - 1;
                i >= 0;
                i--)
            {
                int index = this.listView_location_list.SelectedIndices[i];
                string strDatabaseName = this.listView_location_list.Items[index].Text;
                this.listView_location_list.Items.RemoveAt(index);
            }
#endif
            // 2012/3/11
            ListViewUtil.DeleteSelectedItems(listView_location_list);

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

        #region ֵ�б�

        int ListValueTables(out string strError)
        {
            strError = "";

            if (this.ValueTableChanged == true)
            {
                // ������δ����
                DialogResult result = MessageBox.Show(this,
                    "��ǰ������ֵ�б��屻�޸ĺ���δ���档����ʱˢ�´������ݣ�����δ������Ϣ����ʧ��\r\n\r\nȷʵҪˢ��? ",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result != DialogResult.Yes)
                {
                    return 0;
                }
            }

            string strValueTableXml = "";

            // ��ýű���ض���
            int nRet = GetValueTableInfo(out strValueTableXml,
                out strError);
            if (nRet == -1)
                return -1;
            if (nRet == 0)
            {
                // ���ݾɰ汾
                this.textBox_valueTables.Text = "";
                this.ValueTableChanged = false;
                this.textBox_valueTables.Enabled = false;
                return 0;
            }

            strValueTableXml = "<valueTables>" + strValueTableXml + "</valueTables>";

            string strXml = "";
            nRet = DomUtil.GetIndentXml(strValueTableXml,
                out strXml,
                out strError);
            if (nRet == -1)
                return -1;

            this.textBox_valueTables.Enabled = true;
            this.textBox_valueTables.Text = strXml;
            this.ValueTableChanged = false;

            return 1;
        }

        // ��ýű���ض���
        int GetValueTableInfo(out string strValueTableXml,
            out string strError)
        {
            strError = "";
            strValueTableXml = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڻ�ȡֵ�б��� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.GetSystemParameter(
                    stop,
                    "circulation",
                    "valueTables",
                    out strValueTableXml,
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

        // ����ֵ�б���
        // parameters:
        //      strValueTableXml   ֵ�б���XML��ע�⣬û�и�Ԫ��
        int SetValueTableDef(string strValueTableXml,
            out string strError)
        {
            strError = "";

            EnableControls(false);

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.Initial("���ڱ���ֵ�б��� ...");
            stop.BeginLoop();

            this.Update();
            this.MainForm.Update();

            try
            {
                long lRet = Channel.SetSystemParameter(
                    stop,
                    "circulation",
                    "valueTables",
                    strValueTableXml,
                    out strError);
                if (lRet == -1)
                    goto ERROR1;

                // ��������л����ֵ�б���
                this.MainForm.ClearValueTableCache();

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

        bool m_bValueTableChanged = false;

        /// <summary>
        /// ֵ�б����Ƿ��޸�
        /// </summary>
        public bool ValueTableChanged
        {
            get
            {
                return this.m_bValueTableChanged;
            }
            set
            {
                this.m_bValueTableChanged = value;
                if (value == true)
                    this.toolStripButton_valueTable_save.Enabled = true;
                else
                    this.toolStripButton_valueTable_save.Enabled = false;
            }
        }

        private void toolStripButton_valueTable_save_Click(object sender, EventArgs e)
        {
            string strError = "";
            string strValueTableXml = this.textBox_valueTables.Text;

            if (String.IsNullOrEmpty(strValueTableXml) == true)
            {
                strValueTableXml = "";
            }
            else
            {

                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.LoadXml(strValueTableXml);
                }
                catch (Exception ex)
                {
                    strError = "XML�ַ���װ��XMLDOMʱ��������: " + ex.Message;
                    goto ERROR1;
                }

                if (dom.DocumentElement == null)
                {
                    strValueTableXml = "";
                }
                else
                    strValueTableXml = dom.DocumentElement.InnerXml;
            }

            int nRet = SetValueTableDef(strValueTableXml,
                out strError);
            if (nRet == -1)
            {
                this.ScriptChanged = false;
                goto ERROR1;
            }

            this.ValueTableChanged = false;
            return;
        ERROR1:
            MessageBox.Show(this, strError);

        }

        private void toolStripButton_valueTable_refresh_Click(object sender, EventArgs e)
        {
            string strError = "";

            int nRet = this.ListValueTables(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
            }
        }

        private void textBox_valueTables_TextChanged(object sender, EventArgs e)
        {
            this.ValueTableChanged = true;
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

            // Ϊ����ʾ<script>Ԫ���еĽű��Ļ���
            strXml = strXml.Replace("\r\n", "\n");
            strXml = strXml.Replace("\n", "\r\n");

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
        //      strScriptXml   �ű�����XML��ע�⣬û�и�Ԫ��
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

        /// <summary>
        /// �ű��������Ƿ��޸�
        /// </summary>
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

        private void treeView_zhongcihao_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode current_treenode = this.treeView_zhongcihao.SelectedNode;

            // �����ʽ�ڵ�Ĳ˵��ֻ���ڵ�ǰ�ڵ�Ϊ���ݿ����ͻ��߸�ʽ����ʱ����enabled

            if (current_treenode == null)
            {
                this.ToolStripMenuItem_zhongcihao_insert_nstable.Enabled = true;
                this.toolStripMenuItem_zhongcihao_insert_database.Enabled = false;
                this.toolStripMenuItem_zhongcihao_insert_group.Enabled = true;

                this.toolStripButton_zhongcihao_modify.Enabled = false;
                this.toolStripButton_zhongcihao_remove.Enabled = false;
            }
            else if (current_treenode.ImageIndex == TYPE_ZHONGCIHAO_NSTABLE)
            {
                // nstable�ڵ�
                this.ToolStripMenuItem_zhongcihao_insert_nstable.Enabled = false;
                this.toolStripMenuItem_zhongcihao_insert_database.Enabled = false;
                this.toolStripMenuItem_zhongcihao_insert_group.Enabled = true;

                this.toolStripButton_zhongcihao_modify.Enabled = true;
                this.toolStripButton_zhongcihao_remove.Enabled = true;
            }
            else if (current_treenode.ImageIndex == TYPE_ZHONGCIHAO_GROUP)
            {
                // group�ڵ�
                this.ToolStripMenuItem_zhongcihao_insert_nstable.Enabled = true;
                this.toolStripMenuItem_zhongcihao_insert_database.Enabled = true;
                this.toolStripMenuItem_zhongcihao_insert_group.Enabled = true;

                this.toolStripButton_zhongcihao_modify.Enabled = true;
                this.toolStripButton_zhongcihao_remove.Enabled = true;
            }
            else if (current_treenode.ImageIndex == TYPE_ZHONGCIHAO_DATABASE)
            {
                // database�ڵ�
                this.ToolStripMenuItem_zhongcihao_insert_nstable.Enabled = false;
                this.toolStripMenuItem_zhongcihao_insert_database.Enabled = true;
                this.toolStripMenuItem_zhongcihao_insert_group.Enabled = false;

                this.toolStripButton_zhongcihao_modify.Enabled = true;
                this.toolStripButton_zhongcihao_remove.Enabled = true;
            }
            else if (current_treenode.ImageIndex == TYPE_ZHONGCIHAO_ERROR)
            {
                // error�ڵ�
                this.ToolStripMenuItem_zhongcihao_insert_nstable.Enabled = false;
                this.toolStripMenuItem_zhongcihao_insert_database.Enabled = false;
                this.toolStripMenuItem_zhongcihao_insert_group.Enabled = false;

                this.toolStripButton_zhongcihao_modify.Enabled = false;
                this.toolStripButton_zhongcihao_remove.Enabled = true;
            }
        }

        // popup menu
        private void treeView_zhongcihao_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = null;

            TreeNode node = this.treeView_zhongcihao.SelectedNode;

            //
            menuItem = new MenuItem("�޸�(&M)");
            menuItem.Click += new System.EventHandler(this.toolStripButton_zhongcihao_modify_Click);
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

            // ����group�ڵ�
            string strText = "";
            if (node == null)
                strText = "[׷�ӵ���һ��ĩβ]";
            else if (node.Parent == null)
                strText = "[ͬ�����]";
            else
                strText = "[׷�ӵ���һ��ĩβ]";

            menuItem = new MenuItem("������ڵ�(&N) " + strText);
            menuItem.Click += new System.EventHandler(this.toolStripMenuItem_zhongcihao_insert_group_Click);
            if (node != null && node.ImageIndex == TYPE_ZHONGCIHAO_DATABASE)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);

            // ������Ŀ��ڵ�
            if (node == null)
                strText = "";   // ����������������
            else if (node.Parent == null)
                strText = "[׷�ӵ��¼�ĩβ]";
            else
                strText = "[ͬ�����]";

            menuItem = new MenuItem("������Ŀ��ڵ�(&F) " + strText);
            menuItem.Click += new System.EventHandler(this.toolStripMenuItem_zhongcihao_insert_database_Click);
            if (node == null
                || (node != null && node.ImageIndex == TYPE_ZHONGCIHAO_NSTABLE))
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);

            // ����nstable�ڵ�
            strText = "";
            if (node == null)
                strText = "[׷�ӵ���һ��ĩβ]";
            else if (node.Parent == null)
                strText = "[ͬ�����]";
            else if (node.Parent != null)
                strText = "";
            else
                strText = "[׷�ӵ���һ��ĩβ]";

            menuItem = new MenuItem("�������ֱ�ڵ�(&N) " + strText);
            menuItem.Click += new System.EventHandler(this.ToolStripMenuItem_zhongcihao_insert_nstable_Click);
            if (node != null
                && (node.Parent != null || node.ImageIndex == TYPE_ZHONGCIHAO_NSTABLE))
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            // 
            menuItem = new MenuItem("����(&U)");
            menuItem.Click += new System.EventHandler(this.menu_zhongcihao_up_Click);
            if (this.treeView_zhongcihao.SelectedNode == null
                || this.treeView_zhongcihao.SelectedNode.PrevNode == null)
                menuItem.Enabled = false;
            else
                menuItem.Enabled = true;
            contextMenu.MenuItems.Add(menuItem);



            // 
            menuItem = new MenuItem("����(&D)");
            menuItem.Click += new System.EventHandler(this.menu_zhongcihao_down_Click);
            if (treeView_zhongcihao.SelectedNode == null
                || treeView_zhongcihao.SelectedNode.NextNode == null)
                menuItem.Enabled = false;
            else
                menuItem.Enabled = true;
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            //
            menuItem = new MenuItem("�Ƴ�(&E)");
            menuItem.Click += new System.EventHandler(this.toolStripButton_zhongcihao_remove_Click);
            if (node == null)
            {
                menuItem.Enabled = false;
            }
            contextMenu.MenuItems.Add(menuItem);

            contextMenu.Show(treeView_zhongcihao, new Point(e.X, e.Y));		
			
        }

        // ����<group>���ͽڵ㡣һ���ڵ㡣
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

            List<string> used_dbnames = GetAllUsedZhongcihaoDbName(null);

            // ѯ��<group>��
            ZhongcihaoGroupDialog dlg = new ZhongcihaoGroupDialog();
            MainForm.SetControlFont(dlg, this.Font, false);

            dlg.Text = "��ָ��������";
            dlg.AllZhongcihaoDatabaseInfoXml = GetAllZhongcihaoDbInfoXml();
            dlg.ExcludingDbNames = used_dbnames;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            /* �Ѿ���ZhongcihaoGroupDialog�Ի����м�����
            // ���Ի����еõ����ִκſ⣬�ǲ��Ǳ����ù����ִκſ⣿
            if (used_dbnames.IndexOf(dlg.ZhongcihaoDbName) != -1)
            {
                strError = "����ָ�����ִκſ� '" + dlg.ZhongcihaoDbName + "' �Ѿ���������ʹ�ù��ˡ����������顣";
                goto ERROR1;
            }
             * */


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
                string strComment = "�ִκſ� '" + dlg.ZhongcihaoDbName + "' ��δ��������ȷ����ť�ɴ�������";
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

            string strGroupCaption = MakeZhongcihaoGroupNodeName(dlg.GroupName, dlg.ZhongcihaoDbName);

            TreeNode new_treenode = new TreeNode(strGroupCaption, TYPE_ZHONGCIHAO_GROUP, TYPE_ZHONGCIHAO_GROUP);
            new_treenode.Tag = dom.OuterXml;
            this.treeView_zhongcihao.Nodes.Insert(index, new_treenode);

            this.treeView_zhongcihao.SelectedNode = new_treenode;

            this.ZhongcihaoChanged = true;


            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // ����<database>���ͽڵ㡣�����ڵ�
        private void toolStripMenuItem_zhongcihao_insert_database_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            // ��ǰ�ڵ�
            TreeNode current_treenode = this.treeView_zhongcihao.SelectedNode;

            if (current_treenode == null)
            {
                strError = "��δѡ��������ݿ����ڵ㣬����޷������µ����ݿ����ڵ�";
                goto ERROR1;
            }

            if (current_treenode.ImageIndex == TYPE_ZHONGCIHAO_NSTABLE)
            {
                strError = "ѡ���Ľڵ㲻��Ϊ���ֱ�ڵ㣬��������������ݿ����ڵ㣬���ܲ����µ����ݿ����ڵ�";
                goto ERROR1;
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

            // ���ˣ�current_treenodeΪ<group>���͵Ľڵ���

            List<string> used_dbnames = Zhongcihao_GetAllUsedBiblioDbName(null);

            // �µ����ݿ���
            GetOpacMemberDatabaseNameDialog dlg = new GetOpacMemberDatabaseNameDialog();
            MainForm.SetControlFont(dlg, this.Font, false);

            dlg.Text = "��ѡ��һ��Ҫ���������Ŀ��";
            dlg.AllDatabaseInfoXml = GetAllBiblioDbInfoXml();
            dlg.ExcludingDbNames = used_dbnames;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            // ���Ի����еõ�����Ŀ�������ǲ��Ǳ����ù�����Ŀ������
            if (used_dbnames.IndexOf(dlg.SelectedDatabaseName) != -1)
            {
                strError = "����ָ������Ŀ�� '" + dlg.SelectedDatabaseName + "' �Ѿ����������ݿ�ڵ�ʹ�ù��ˡ��������δ������ݿ�ڵ������";
                goto ERROR1;
            }

            // ���ָ�����ֵ���Ŀ���Ƿ��Ѿ�����
            // return:
            //      -2  ��ָ������Ŀ�����֣�ʵ������һ���Ѿ����ڵ��������͵Ŀ���
            //      -1  error
            //      0   ��û�д���
            //      1   �Ѿ�����
            nRet = CheckBiblioDbCreated(dlg.SelectedDatabaseName,
                out strError);
            if (nRet == -1)
                goto ERROR1;
            if (nRet == -2)
                goto ERROR1;
            if (nRet == 0)
            {
                strError = "��Ŀ�� '" + dlg.SelectedDatabaseName + "' ��δ���������ȴ������������������ݿ�ڵ㡣";
                goto ERROR1;
            }

            // ������ݿ�syntax
            string strSyntax = "";
                    // �����Ŀ���syntax
        // return:
        //      -1  error
        //      0   not found
        //      1   found
            nRet = GetBiblioSyntax(dlg.SelectedDatabaseName,
                out strSyntax,
                out strError);
            if (nRet == -1)
                goto ERROR1;
            if (nRet == 0)
            {
                strError = "�ڵ���GetBiblioSyntax()�����У����ֲ���������Ŀ�� '" + dlg.SelectedDatabaseName + "' �Ķ���";
                goto ERROR1;
            }

            string strPrefix = "";
            string strUri = "";
            if (strSyntax == "unimarc")
                strUri = Ns.unimarcxml;
            else if (strSyntax == "usmarc")
                strUri = Ns.usmarcxml;
            else
            {
                nRet = ExistingPrefix(strSyntax, out strError);
                if (nRet == -1)
                    goto ERROR1;
                if (nRet == 1)
                    strPrefix = strSyntax;
                Debug.Assert(nRet == 0, "");
                if (nRet == 0)
                {
                    strError = "Ŀǰ���ֱ�����δ������Ŀ���ʽ '" + strSyntax + "' ����Ӧ��namespace URI�������޷������ø�ʽ����Ŀ��ڵ�";
                    goto ERROR1;
                }
            }


            if (String.IsNullOrEmpty(strPrefix) == true)
            {
                // �������ֿռ�URI���Ҷ�Ӧ��prefix
                // return:
                //      -1  error
                //      0   not found
                //      1   found
                nRet = FindNamespacePrefix(strUri,
                    out strPrefix,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;
                if (nRet == 0)
                {
                    strError = "�����ֱ���û���ҵ���namespace URI '"+strUri+"' (��Դ����Ŀ���ʽ '"+strSyntax+"') ��Ӧ��prefix���޷������ø�ʽ����Ŀ��ڵ�";
                    goto ERROR1;
                }
                Debug.Assert(nRet == 1, "");
            }


            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<database />");

            DomUtil.SetAttr(dom.DocumentElement, "name", dlg.SelectedDatabaseName);
            DomUtil.SetAttr(dom.DocumentElement, "leftfrom", "��ȡ���");

            if (strSyntax == "unimarc")
            {
                DomUtil.SetAttr(dom.DocumentElement,
                                    "rightxpath",
                                    "//" + strPrefix + ":record/" + strPrefix + ":datafield[@tag='905']/" + strPrefix + ":subfield[@code='e']/text()");
                DomUtil.SetAttr(dom.DocumentElement,
                    "titlexpath",
                    "//" + strPrefix + ":record/" + strPrefix + ":datafield[@tag='200']/" + strPrefix + ":subfield[@code='a']/text()");
                DomUtil.SetAttr(dom.DocumentElement,
                    "authorxpath",
                    "//" + strPrefix + ":record/" + strPrefix + ":datafield[@tag='200']/" + strPrefix + ":subfield[@code='f' or @code='g']/text()");
            }
            else if (strSyntax == "usmarc")
            {
                DomUtil.SetAttr(dom.DocumentElement,
                    "rightxpath",
                    "//" + strPrefix + ":record/" + strPrefix + ":datafield[@tag='905']/" + strPrefix + ":subfield[@code='e']/text()");
                DomUtil.SetAttr(dom.DocumentElement,
                    "titlexpath",
                    "//" + strPrefix + ":record/" + strPrefix + ":datafield[@tag='245']/" + strPrefix + ":subfield[@code='a']/text()");
                DomUtil.SetAttr(dom.DocumentElement,
                    "authorxpath",
                    "//" + strPrefix + ":record/" + strPrefix + ":datafield[@tag='245']/" + strPrefix + ":subfield[@code='c']/text()");
            }
            else
            {
                strError = "Ŀǰ��ʱ���ܴ���syntaxΪ '" + strSyntax + "' ����Ŀ��ڵ㴴��...";
                goto ERROR1;
            }

            string strDatabaseCaption = MakeZhongcihaoDatabaseNodeName(dlg.SelectedDatabaseName);

            TreeNode new_treenode = new TreeNode(strDatabaseCaption, 
                TYPE_ZHONGCIHAO_DATABASE, TYPE_ZHONGCIHAO_DATABASE);
            new_treenode.Tag = dom.DocumentElement.OuterXml;

            current_treenode.Nodes.Insert(index, new_treenode);

            this.treeView_zhongcihao.SelectedNode = new_treenode;

            this.ZhongcihaoChanged = true;

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }


        // ����<nstable>���ͽڵ㡣һ���ڵ�
        private void ToolStripMenuItem_zhongcihao_insert_nstable_Click(object sender, EventArgs e)
        {
            string strError = "";
            // int nRet = 0;

            // ������ǰ�Ƿ��Ѿ�����nstable�ڵ�
            TreeNode existing_node = FindExistNstableNode();
            if (existing_node != null)
            {
                this.treeView_zhongcihao.SelectedNode = existing_node;
                strError = "���ֱ�ڵ��Ѿ����ڡ������ظ�������";
                goto ERROR1;
            }

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

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<nstable />");
            DomUtil.SetAttr(dom.DocumentElement, "name", "nstable");

            // unimarc
            XmlNode item_node = dom.CreateElement("item");
            dom.DocumentElement.AppendChild(item_node);

            DomUtil.SetAttr(item_node, "prefix", "unimarc");
            DomUtil.SetAttr(item_node, "uri", Ns.unimarcxml);

            // usmarc
            item_node = dom.CreateElement("item");
            dom.DocumentElement.AppendChild(item_node);

            DomUtil.SetAttr(item_node, "prefix", "usmarc");
            DomUtil.SetAttr(item_node, "uri", Ns.usmarcxml);

            string strNstableCaption = MakeZhongcihaoNstableNodeName("nstable");

            TreeNode new_treenode = new TreeNode(strNstableCaption,
                TYPE_ZHONGCIHAO_NSTABLE, TYPE_ZHONGCIHAO_NSTABLE);
            new_treenode.Tag = dom.OuterXml;
            this.treeView_zhongcihao.Nodes.Insert(index, new_treenode);

            this.treeView_zhongcihao.SelectedNode = new_treenode;

            this.ZhongcihaoChanged = true;

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // ����
        private void toolStripButton_zhongcihao_save_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            nRet = SubmitZhongcihaoDef(out strError);
            if (nRet == -1)
                MessageBox.Show(this, strError);
            else
            {
                this.ZhongcihaoChanged = false;
            }

        }

        // �޸�һ���ڵ�Ķ���
        private void toolStripButton_zhongcihao_modify_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            // ��ǰ�ڵ�
            TreeNode current_treenode = this.treeView_zhongcihao.SelectedNode;

            if (current_treenode == null)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�޸ĵĽڵ�");
                return;
            }

            if (current_treenode.ImageIndex == TYPE_ZHONGCIHAO_GROUP)
            {
                // ��ڵ�


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
                    strError = "<group>�ڵ��XMLװ��DOMʱ����: " + ex.Message;
                    goto ERROR1;
                }

                string strGroupName = DomUtil.GetAttr(dom.DocumentElement,
                    "name");
                string strZhongcihaoDbName = DomUtil.GetAttr(dom.DocumentElement,
                    "zhongcihaodb");

                List<string> used_dbnames = GetAllUsedZhongcihaoDbName(current_treenode);

                ZhongcihaoGroupDialog dlg = new ZhongcihaoGroupDialog();
                MainForm.SetControlFont(dlg, this.Font, false);

                dlg.Text = "�޸�������";
                dlg.ZhongcihaoDbName = strZhongcihaoDbName;
                dlg.GroupName = strGroupName;
                dlg.AllZhongcihaoDatabaseInfoXml = GetAllZhongcihaoDbInfoXml();
                dlg.ExcludingDbNames = used_dbnames;
                dlg.StartPosition = FormStartPosition.CenterScreen;
                dlg.ShowDialog(this);

                if (dlg.DialogResult != DialogResult.OK)
                    return;

                DomUtil.SetAttr(dom.DocumentElement, "name", dlg.GroupName);
                DomUtil.SetAttr(dom.DocumentElement, "zhongcihaodb", dlg.ZhongcihaoDbName);

                current_treenode.Text = MakeZhongcihaoGroupNodeName(dlg.GroupName, dlg.ZhongcihaoDbName);
                current_treenode.Tag = dom.DocumentElement.OuterXml;    // 2009/3/3 new add

                // ȷ��չ��
                // current_treenode.Parent.Expand();

                this.ZhongcihaoChanged = true;

            }
            else if (current_treenode.ImageIndex == TYPE_ZHONGCIHAO_DATABASE)
            {
                // ��Ŀ��ڵ�

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
                    strError = "<database>�ڵ��XMLװ��DOMʱ����: " + ex.Message;
                    goto ERROR1;
                }

                string strDatabaseName = DomUtil.GetAttr(dom.DocumentElement,
                    "name");

                List<string> used_dbnames = Zhongcihao_GetAllUsedBiblioDbName(current_treenode);
                
                // �µ���Ŀ����
                GetOpacMemberDatabaseNameDialog dlg = new GetOpacMemberDatabaseNameDialog();
                MainForm.SetControlFont(dlg, this.Font, false);

                dlg.Text = "�޸���Ŀ����";
                dlg.SelectedDatabaseName = strDatabaseName;
                dlg.AllDatabaseInfoXml = GetAllBiblioDbInfoXml();
                dlg.ExcludingDbNames = used_dbnames;
                dlg.StartPosition = FormStartPosition.CenterScreen;
                dlg.ShowDialog(this);

                if (dlg.DialogResult != DialogResult.OK)
                    return;

                // ���Ի����еõ�����Ŀ�������ǲ��Ǳ����ù�����Ŀ������
                if (used_dbnames.IndexOf(dlg.SelectedDatabaseName) != -1)
                {
                    strError = "����ָ������Ŀ�� '" + dlg.SelectedDatabaseName + "' �Ѿ����������ݿ�ڵ�ʹ�ù��ˡ����������޸����ݿ�ڵ������";
                    goto ERROR1;
                }

                // ���ָ�����ֵ���Ŀ���Ƿ��Ѿ�����
                // return:
                //      -2  ��ָ������Ŀ�����֣�ʵ������һ���Ѿ����ڵ��������͵Ŀ���
                //      -1  error
                //      0   ��û�д���
                //      1   �Ѿ�����
                nRet = CheckBiblioDbCreated(dlg.SelectedDatabaseName,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;
                if (nRet == -2)
                    goto ERROR1;
                if (nRet == 0)
                {
                    strError = "��Ŀ�� '" + dlg.SelectedDatabaseName + "' ��δ���������ȴ������������޸����ݿ�ڵ㡣";
                    goto ERROR1;
                }

                // ������ݿ�syntax
                string strSyntax = "";
                // �����Ŀ���syntax
                // return:
                //      -1  error
                //      0   not found
                //      1   found
                nRet = GetBiblioSyntax(dlg.SelectedDatabaseName,
                    out strSyntax,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;
                if (nRet == 0)
                {
                    strError = "�ڵ���GetBiblioSyntax()�����У����ֲ���������Ŀ�� '" + dlg.SelectedDatabaseName + "' �Ķ���";
                    goto ERROR1;
                }

                string strPrefix = "";
                string strUri = "";
                if (strSyntax == "unimarc")
                    strUri = Ns.unimarcxml;
                else if (strSyntax == "usmarc")
                    strUri = Ns.usmarcxml;
                else
                {
                    nRet = ExistingPrefix(strSyntax, out strError);
                    if (nRet == -1)
                        goto ERROR1;
                    if (nRet == 1)
                        strPrefix = strSyntax;
                    Debug.Assert(nRet == 0, "");
                    if (nRet == 0)
                    {
                        strError = "Ŀǰ���ֱ�����δ������Ŀ���ʽ '" + strSyntax + "' ����Ӧ��namespace URI�������޷������ø�ʽ����Ŀ��ڵ�";
                        goto ERROR1;
                    }
                }


                if (String.IsNullOrEmpty(strPrefix) == true)
                {
                    // �������ֿռ�URI���Ҷ�Ӧ��prefix
                    // return:
                    //      -1  error
                    //      0   not found
                    //      1   found
                    nRet = FindNamespacePrefix(strUri,
                        out strPrefix,
                        out strError);
                    if (nRet == -1)
                        goto ERROR1;
                    if (nRet == 0)
                    {
                        strError = "�����ֱ���û���ҵ���namespace URI '" + strUri + "' (��Դ����Ŀ���ʽ '" + strSyntax + "') ��Ӧ��prefix���޷������ø�ʽ����Ŀ��ڵ�";
                        goto ERROR1;
                    }
                    Debug.Assert(nRet == 1, "");
                }

                DomUtil.SetAttr(dom.DocumentElement, "name", dlg.SelectedDatabaseName);
                DomUtil.SetAttr(dom.DocumentElement, "leftfrom", "��ȡ���");

                if (strSyntax == "unimarc")
                {
                    DomUtil.SetAttr(dom.DocumentElement,
                                        "rightxpath",
                                        "//" + strPrefix + ":record/" + strPrefix + ":datafield[@tag='905']/" + strPrefix + ":subfield[@code='e']/text()");
                    DomUtil.SetAttr(dom.DocumentElement,
                        "titlexpath",
                        "//" + strPrefix + ":record/" + strPrefix + ":datafield[@tag='200']/" + strPrefix + ":subfield[@code='a']/text()");
                    DomUtil.SetAttr(dom.DocumentElement,
                        "authorxpath",
                        "//" + strPrefix + ":record/" + strPrefix + ":datafield[@tag='200']/" + strPrefix + ":subfield[@code='f' or @code='g']/text()");
                }
                else if (strSyntax == "usmarc")
                {
                    DomUtil.SetAttr(dom.DocumentElement,
                        "rightxpath",
                        "//" + strPrefix + ":record/" + strPrefix + ":datafield[@tag='905']/" + strPrefix + ":subfield[@code='e']/text()");
                    DomUtil.SetAttr(dom.DocumentElement,
                        "titlexpath",
                        "//" + strPrefix + ":record/" + strPrefix + ":datafield[@tag='245']/" + strPrefix + ":subfield[@code='a']/text()");
                    DomUtil.SetAttr(dom.DocumentElement,
                        "authorxpath",
                        "//" + strPrefix + ":record/" + strPrefix + ":datafield[@tag='245']/" + strPrefix + ":subfield[@code='c']/text()");
                }
                else
                {
                    strError = "Ŀǰ��ʱ���ܴ���syntaxΪ '" + strSyntax + "' ����Ŀ��ڵ��޸�...";
                    goto ERROR1;
                }


                string strDisplayText = MakeZhongcihaoDatabaseNodeName(dlg.SelectedDatabaseName);

                current_treenode.Text = strDisplayText;
                current_treenode.Tag = dom.DocumentElement.OuterXml;

                // ȷ��չ��
                current_treenode.Parent.Expand();

                this.ZhongcihaoChanged = true;
            }
            else if (current_treenode.ImageIndex == TYPE_ZHONGCIHAO_NSTABLE)
            {
                // ���ֱ�ڵ�
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
                    strError = "<nstable>�ڵ��XMLװ��DOMʱ����: " + ex.Message;
                    goto ERROR1;
                }

                ZhongcihaoNstableDialog dlg = new ZhongcihaoNstableDialog();
                MainForm.SetControlFont(dlg, this.Font, false);

                dlg.XmlString = strXml;
                dlg.StartPosition = FormStartPosition.CenterScreen;
                dlg.ShowDialog(this);

                if (dlg.DialogResult != DialogResult.OK)
                    return;

                dom = new XmlDocument();
                try
                {
                    dom.LoadXml(dlg.XmlString);
                }
                catch (Exception ex)
                {
                    strError = "�޸ĺ�ĵ�XMLװ��DOMʱ����: " + ex.Message;
                    goto ERROR1;
                }

                string strNstableName = DomUtil.GetAttr(dom.DocumentElement, "name");

                current_treenode.Text = MakeZhongcihaoNstableNodeName(strNstableName);
                current_treenode.Tag = dlg.XmlString;

                // ȷ��չ��
                current_treenode.Parent.Expand();

                this.ZhongcihaoChanged = true;
            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // �Ƴ�һ���ڵ�
        private void toolStripButton_zhongcihao_remove_Click(object sender, EventArgs e)
        {
            string strError = "";
            // int nRet = 0;

            // ��ǰ�ڵ�
            TreeNode current_treenode = this.treeView_zhongcihao.SelectedNode;

            if (current_treenode == null)
            {
                strError = "��δѡ��Ҫɾ���Ľڵ�";
                goto ERROR1;
            }

            // ����
            string strText = "ȷʵҪ�Ƴ�";

            if (current_treenode.ImageIndex == TYPE_ZHONGCIHAO_DATABASE)
                strText += "�����ڵ�";
            else if (current_treenode.ImageIndex == TYPE_ZHONGCIHAO_GROUP)
                strText += "��ڵ�";
            else if (current_treenode.ImageIndex == TYPE_ZHONGCIHAO_NSTABLE)
                strText += "���ֱ�ڵ�";
            else if (current_treenode.ImageIndex == TYPE_ZHONGCIHAO_ERROR)
                strText += "����ڵ�";
            else
            {
                strError = "δ֪�Ľڵ����� " + current_treenode.ImageIndex.ToString();
                goto ERROR1;
            }

            strText += " " + current_treenode.Text + " ";

            if (current_treenode.Nodes.Count > 0)
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
                this.treeView_zhongcihao.Nodes.Remove(current_treenode);
            }

            this.ZhongcihaoChanged = true;

            treeView_zhongcihao_AfterSelect(this, null);

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        private void treeView_zhongcihao_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode curSelectedNode = this.treeView_zhongcihao.GetNodeAt(e.X, e.Y);

                if (treeView_zhongcihao.SelectedNode != curSelectedNode)
                {
                    treeView_zhongcihao.SelectedNode = curSelectedNode;

                    if (treeView_zhongcihao.SelectedNode == null)
                        treeView_zhongcihao_AfterSelect(null, null);	// ����
                }

            }
        }

        private void toolStripButton_zhongcihao_refresh_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = this.ListZhongcihao(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
            }
        }


        #endregion // �ִκ�

        #region �ż���ϵ


        private void treeView_arrangement_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode current_treenode = this.treeView_arrangement.SelectedNode;

            // �����ʽ�ڵ�Ĳ˵��ֻ���ڵ�ǰ�ڵ�Ϊ���ݿ����ͻ��߸�ʽ����ʱ����enabled

            if (current_treenode == null)
            {
                this.toolStripMenuItem_arrangement_insert_location.Enabled = false;
                this.toolStripMenuItem_arrangement_insert_group.Enabled = true;

                this.toolStripButton_arrangement_modify.Enabled = false;
                this.toolStripButton_arrangement_remove.Enabled = false;
            }
            else if (current_treenode.ImageIndex == TYPE_ARRANGEMENT_GROUP)
            {
                // group�ڵ�
                this.toolStripMenuItem_arrangement_insert_location.Enabled = true;
                this.toolStripMenuItem_arrangement_insert_group.Enabled = true;

                this.toolStripButton_arrangement_modify.Enabled = true;
                this.toolStripButton_arrangement_remove.Enabled = true;
            }
            else if (current_treenode.ImageIndex == TYPE_ARRANGEMENT_LOCATION)
            {
                // location�ڵ�
                this.toolStripMenuItem_arrangement_insert_location.Enabled = true;
                this.toolStripMenuItem_arrangement_insert_group.Enabled = false;

                this.toolStripButton_arrangement_modify.Enabled = true;
                this.toolStripButton_arrangement_remove.Enabled = true;
            }
            else if (current_treenode.ImageIndex == TYPE_ARRANGEMENT_ERROR)
            {
                // error�ڵ�
                this.toolStripMenuItem_arrangement_insert_location.Enabled = false;
                this.toolStripMenuItem_arrangement_insert_group.Enabled = false;

                this.toolStripButton_arrangement_modify.Enabled = false;
                this.toolStripButton_arrangement_remove.Enabled = true;
            }
        }

        // �۲�XML�������
        private void toolStripButton_arrangement_viewXml_Click(object sender, EventArgs e)
        {
            if (this.MainForm.CallNumberCfgDom == null
                || this.MainForm.CallNumberCfgDom.DocumentElement == null)
            {
                MessageBox.Show(this, "��ǰ�ڴ�����δ�߱��ż���ϵXML�������");
                return;
            }

            XmlViewerForm dlg = new XmlViewerForm();

            dlg.Text = "��ǰ�ڴ��е��ż���ϵXML�������";
            dlg.MainForm = this.MainForm;
            dlg.XmlString = this.MainForm.CallNumberCfgDom.DocumentElement.OuterXml;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog();
            return;
        }

        private void toolStripButton_arrangement_save_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            nRet = SubmitArrangementDef(out strError);
            if (nRet == -1)
                MessageBox.Show(this, strError);
            else
            {
                this.ArrangementChanged = false;
            }

        }

        private void toolStripButton_arrangement_refresh_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = this.ListArrangement(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
            }
        }

        private void toolStripMenuItem_arrangement_insert_group_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            // ��ǰ�ڵ�
            TreeNode current_treenode = this.treeView_arrangement.SelectedNode;

            // ������Ǹ����Ľڵ㣬�������ҵ�������
            if (current_treenode != null && current_treenode.Parent != null)
            {
                current_treenode = current_treenode.Parent;
            }

            // �����
            int index = this.treeView_arrangement.Nodes.IndexOf(current_treenode);
            if (index == -1)
                index = this.treeView_arrangement.Nodes.Count;
            else
                index++;

            List<string> used_dbnames = GetArrangementAllUsedZhongcihaoDbName(null);

            // ѯ��<group>��
            ArrangementGroupDialog dlg = new ArrangementGroupDialog();
            MainForm.SetControlFont(dlg, this.Font, false);

            dlg.Text = "��ָ���ż���ϵ����";
            dlg.AllZhongcihaoDatabaseInfoXml = GetAllZhongcihaoDbInfoXml();
            dlg.ExcludingDbNames = used_dbnames;
            dlg.StartPosition = FormStartPosition.CenterScreen;
        REDO_INPUT:
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            // ����ż���ϵ�ڵ����Ƿ��ظ�
            nRet = CheckArrangementNameDup(dlg.ArrangementName,
                null,
                out strError);
            if (nRet == -1)
                goto ERROR1;
            if (nRet == 1)
            {
                MessageBox.Show(this, "�ż���ϵ�� '" + dlg.ArrangementName + "' �Ѿ����ڣ��޷��ظ����������޸�");
                goto REDO_INPUT;
            }

            // �����ָ�����ִκſ��Ƿ���ڡ���������ڣ����Ѵ�����
            if (String.IsNullOrEmpty(dlg.ZhongcihaoDbName) == false)
            {
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
                    string strComment = "�ִκſ� '" + dlg.ZhongcihaoDbName + "' ��δ��������ȷ����ť�ɴ�������";
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
            }

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<group />");
            DomUtil.SetAttr(dom.DocumentElement, "name", dlg.ArrangementName);
            DomUtil.SetAttr(dom.DocumentElement, "classType", dlg.ClassType);
            DomUtil.SetAttr(dom.DocumentElement, "qufenhaoType", dlg.QufenhaoType);
            DomUtil.SetAttr(dom.DocumentElement, "zhongcihaodb", dlg.ZhongcihaoDbName);
            DomUtil.SetAttr(dom.DocumentElement, "callNumberStyle", dlg.CallNumberStyle);

            string strGroupCaption = MakeArrangementGroupNodeName(
                dlg.ArrangementName,
                dlg.ClassType,
                dlg.QufenhaoType,
                dlg.ZhongcihaoDbName,
                dlg.CallNumberStyle);

            TreeNode new_treenode = new TreeNode(strGroupCaption, TYPE_ARRANGEMENT_GROUP, TYPE_ARRANGEMENT_GROUP);
            new_treenode.Tag = dom.OuterXml;
            this.treeView_arrangement.Nodes.Insert(index, new_treenode);

            this.treeView_arrangement.SelectedNode = new_treenode;

            this.ArrangementChanged = true;
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // ����ż���ϵ���Ƿ��ظ�
        int CheckArrangementNameDup(string strNewName,
            TreeNode exclude,
            out string strError)
        {
            strError = "";
            foreach (TreeNode node in this.treeView_arrangement.Nodes)
            {
                if (node == exclude)
                    continue;

                string strXml = (string)node.Tag;
                if (string.IsNullOrEmpty(strXml) == true)
                {
                    Debug.Assert(false, "");
                    continue;
                }

                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.LoadXml(strXml);
                }
                catch (Exception ex)
                {
                    strError = "XML�ַ���װ��DOMʱ����: " + ex.Message;
                    return -1;
                }

                string strName = DomUtil.GetAttr(dom.DocumentElement,
                    "name");

                if (strNewName == strName)
                    return 1;   // �����ظ�
            }

            return 0;
        }

        private void toolStripButton_arrangement_remove_Click(object sender, EventArgs e)
        {
            string strError = "";
            // int nRet = 0;

            // ��ǰ�ڵ�
            TreeNode current_treenode = this.treeView_arrangement.SelectedNode;

            if (current_treenode == null)
            {
                strError = "��δѡ��Ҫ�Ƴ��Ľڵ�";
                goto ERROR1;
            }

            // ����
            string strText = "ȷʵҪ�Ƴ�";

            if (current_treenode.ImageIndex == TYPE_ARRANGEMENT_LOCATION)
                strText += "�ݲصص����ڵ�";
            else if (current_treenode.ImageIndex == TYPE_ARRANGEMENT_GROUP)
                strText += "�ż���ϵ�ڵ�";
            else if (current_treenode.ImageIndex == TYPE_ARRANGEMENT_ERROR)
                strText += "����ڵ�";
            else
            {
                strError = "δ֪�Ľڵ����� " + current_treenode.ImageIndex.ToString();
                goto ERROR1;
            }

            strText += " " + current_treenode.Text + " ";

            if (current_treenode.Nodes.Count > 0)
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
                this.treeView_arrangement.Nodes.Remove(current_treenode);
            }

            this.ArrangementChanged = true;

            treeView_arrangement_AfterSelect(this, null);

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        private void toolStripMenuItem_arrangement_insert_location_Click(object sender, EventArgs e)
        {
            string strError = "";
            // int nRet = 0;

            // ��ǰ�ڵ�
            TreeNode current_treenode = this.treeView_arrangement.SelectedNode;

            if (current_treenode == null)
            {
                strError = "��δѡ���ż���ϵ�ڵ㣬����޷������µĹݲصص����ڵ�";
                goto ERROR1;
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

            // ���ˣ�current_treenodeΪ<group>���͵Ľڵ���

            List<string> used_locationnames = GetArrangementAllUsedLocationName(null);

            // �µĹݲص���
            ArrangementLocationDialog dlg = new ArrangementLocationDialog();
            MainForm.SetControlFont(dlg, this.Font, false);

            dlg.Text = "��ָ���ݲصص�";
            if (this.Channel != null)
                dlg.LibraryCodeList = this.Channel.LibraryCodeList;
            dlg.ExcludingLocationNames = used_locationnames;
            dlg.StartPosition = FormStartPosition.CenterScreen;

            dlg.GetValueTable -= new GetValueTableEventHandler(dlg_GetValueTable);
            dlg.GetValueTable += new GetValueTableEventHandler(dlg_GetValueTable);

            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            /*
            // ���Ի����еõ�����Ŀ�������ǲ��Ǳ����ù�����Ŀ������
            if (used_locationnames.IndexOf(dlg.SelectedDatabaseName) != -1)
            {
                strError = "����ָ������Ŀ�� '" + dlg.SelectedDatabaseName + "' �Ѿ����������ݿ�ڵ�ʹ�ù��ˡ��������δ������ݿ�ڵ������";
                goto ERROR1;
            }*/

            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<location />");

            DomUtil.SetAttr(dom.DocumentElement, "name", dlg.LocationString);

            string strLocationCaption = MakeArrangementLocationNodeName(dlg.LocationString);

            TreeNode new_treenode = new TreeNode(strLocationCaption,
                TYPE_ARRANGEMENT_LOCATION, TYPE_ARRANGEMENT_LOCATION);
            new_treenode.Tag = dom.DocumentElement.OuterXml;

            current_treenode.Nodes.Insert(index, new_treenode);

            this.treeView_arrangement.SelectedNode = new_treenode;

            this.ArrangementChanged = true;

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        void dlg_GetValueTable(object sender, GetValueTableEventArgs e)
        {
            string strError = "";
            string[] values = null;
            int nRet = MainForm.GetValueTable(e.TableName,
                e.DbName,
                out values,
                out strError);
            if (nRet == -1)
                MessageBox.Show(this, strError);
            e.values = values;
        }

        private void toolStripButton_arrangement_modify_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            // ��ǰ�ڵ�
            TreeNode current_treenode = this.treeView_arrangement.SelectedNode;

            if (current_treenode == null)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�޸ĵĽڵ�");
                return;
            }

            if (current_treenode.ImageIndex == TYPE_ARRANGEMENT_GROUP)
            {
                // ��ڵ�
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
                    strError = "<group>�ڵ��XMLװ��DOMʱ����: " + ex.Message;
                    goto ERROR1;
                }

                string strArrangementName = DomUtil.GetAttr(dom.DocumentElement,
                    "name");
                string strClassType = DomUtil.GetAttr(dom.DocumentElement,
                    "classType");
                string strQufenhaoType = DomUtil.GetAttr(dom.DocumentElement,
                    "qufenhaoType");
                string strZhongcihaoDbName = DomUtil.GetAttr(dom.DocumentElement,
                    "zhongcihaodb");
                string strCallNumberStyle = DomUtil.GetAttr(dom.DocumentElement,
    "callNumberStyle");

                List<string> used_dbnames = GetArrangementAllUsedZhongcihaoDbName(current_treenode);

                ArrangementGroupDialog dlg = new ArrangementGroupDialog();
                MainForm.SetControlFont(dlg, this.Font, false);

                dlg.Text = "�޸��ż���ϵ����";
                dlg.ArrangementName = strArrangementName;
                dlg.ClassType = strClassType;
                dlg.QufenhaoType = strQufenhaoType;
                dlg.ZhongcihaoDbName = strZhongcihaoDbName;
                dlg.CallNumberStyle = strCallNumberStyle;
                dlg.AllZhongcihaoDatabaseInfoXml = GetAllZhongcihaoDbInfoXml();
                dlg.ExcludingDbNames = used_dbnames;
                dlg.StartPosition = FormStartPosition.CenterScreen;
            REDO_INPUT:
                dlg.ShowDialog(this);

                if (dlg.DialogResult != DialogResult.OK)
                    return;

                // ����ż���ϵ�ڵ����Ƿ��ظ�
                nRet = CheckArrangementNameDup(dlg.ArrangementName,
                    current_treenode,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;
                if (nRet == 1)
                {
                    MessageBox.Show(this, "�ż���ϵ�� '" + dlg.ArrangementName + "' �Ѿ����ڣ��޷��ظ����������޸�");
                    goto REDO_INPUT;
                }

                DomUtil.SetAttr(dom.DocumentElement, "name", dlg.ArrangementName);
                DomUtil.SetAttr(dom.DocumentElement, "classType", dlg.ClassType);
                DomUtil.SetAttr(dom.DocumentElement, "qufenhaoType", dlg.QufenhaoType);
                DomUtil.SetAttr(dom.DocumentElement, "zhongcihaodb", dlg.ZhongcihaoDbName);
                DomUtil.SetAttr(dom.DocumentElement, "callNumberStyle", dlg.CallNumberStyle);

                current_treenode.Text = MakeArrangementGroupNodeName(
                    dlg.ArrangementName,
                    dlg.ClassType,
                    dlg.QufenhaoType,
                    dlg.ZhongcihaoDbName,
                    dlg.CallNumberStyle);
                current_treenode.Tag = dom.DocumentElement.OuterXml;

                // ȷ��չ��
                // current_treenode.Parent.Expand();

                this.ArrangementChanged = true;

            }
            else if (current_treenode.ImageIndex == TYPE_ARRANGEMENT_LOCATION)
            {
                // �ݲصص�ڵ�
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
                    strError = "<location>�ڵ��XMLװ��DOMʱ����: " + ex.Message;
                    goto ERROR1;
                }

                string strLocationName = DomUtil.GetAttr(dom.DocumentElement,
                    "name");

                List<string> used_locationnames = this.GetArrangementAllUsedLocationName(current_treenode);

                ArrangementLocationDialog dlg = new ArrangementLocationDialog();
                MainForm.SetControlFont(dlg, this.Font, false);

                dlg.Text = "�޸Ĺݲصص���";
                if (this.Channel != null)
                    dlg.LibraryCodeList = this.Channel.LibraryCodeList;
                dlg.LocationString = strLocationName;
                dlg.ExcludingLocationNames = used_locationnames;
                dlg.StartPosition = FormStartPosition.CenterScreen;

                dlg.GetValueTable -= new GetValueTableEventHandler(dlg_GetValueTable);
                dlg.GetValueTable += new GetValueTableEventHandler(dlg_GetValueTable);

                dlg.ShowDialog(this);

                if (dlg.DialogResult != DialogResult.OK)
                    return;

                /*
                // ���Ի����еõ�����Ŀ�������ǲ��Ǳ����ù�����Ŀ������
                if (used_locationnames.IndexOf(dlg.SelectedDatabaseName) != -1)
                {
                    strError = "����ָ������Ŀ�� '" + dlg.SelectedDatabaseName + "' �Ѿ����������ݿ�ڵ�ʹ�ù��ˡ����������޸����ݿ�ڵ������";
                    goto ERROR1;
                }*/

                DomUtil.SetAttr(dom.DocumentElement, "name", dlg.LocationString);

                string strDisplayText = MakeArrangementLocationNodeName(dlg.LocationString);

                current_treenode.Text = strDisplayText;
                current_treenode.Tag = dom.DocumentElement.OuterXml;

                // ȷ��չ��
                current_treenode.Parent.Expand();

                this.ArrangementChanged = true;
            }
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // ������Ҳ��ѡ�����ڵ�
        private void treeView_arrangement_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode curSelectedNode = this.treeView_arrangement.GetNodeAt(e.X, e.Y);

                if (treeView_arrangement.SelectedNode != curSelectedNode)
                {
                    treeView_arrangement.SelectedNode = curSelectedNode;

                    if (treeView_arrangement.SelectedNode == null)
                        treeView_arrangement_AfterSelect(null, null);	// ����
                }
            }
        }

        // context menu
        private void treeView_arrangement_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = null;

            TreeNode node = this.treeView_arrangement.SelectedNode;

            //
            menuItem = new MenuItem("�޸�(&M)");
            menuItem.Click += new System.EventHandler(this.toolStripButton_arrangement_modify_Click);
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

            // ����group�ڵ�
            string strText = "";
            if (node == null)
                strText = "[׷�ӵ���һ��ĩβ]";
            else if (node.Parent == null)
                strText = "[ͬ�����]";
            else
                strText = "[׷�ӵ���һ��ĩβ]";

            menuItem = new MenuItem("�����ż���ϵ�ڵ�(&N) " + strText);
            menuItem.Click += new System.EventHandler(this.toolStripMenuItem_arrangement_insert_group_Click);
            if (node != null && node.ImageIndex == TYPE_ARRANGEMENT_LOCATION)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);

            // ����ݲصص�ڵ�
            if (node == null)
                strText = "";   // ����������������
            else if (node.Parent == null)
                strText = "[׷�ӵ��¼�ĩβ]";
            else
                strText = "[ͬ�����]";

            menuItem = new MenuItem("�����ݲصص�ڵ�(&F) " + strText);
            menuItem.Click += new System.EventHandler(this.toolStripMenuItem_arrangement_insert_location_Click);
            if (node == null)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            // 
            menuItem = new MenuItem("����(&U)");
            menuItem.Click += new System.EventHandler(this.menu_arrangement_up_Click);
            if (this.treeView_arrangement.SelectedNode == null
                || this.treeView_arrangement.SelectedNode.PrevNode == null)
                menuItem.Enabled = false;
            else
                menuItem.Enabled = true;
            contextMenu.MenuItems.Add(menuItem);



            // 
            menuItem = new MenuItem("����(&D)");
            menuItem.Click += new System.EventHandler(this.menu_arrangement_down_Click);
            if (treeView_arrangement.SelectedNode == null
                || treeView_arrangement.SelectedNode.NextNode == null)
                menuItem.Enabled = false;
            else
                menuItem.Enabled = true;
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            //
            menuItem = new MenuItem("�Ƴ�(&E)");
            menuItem.Click += new System.EventHandler(this.toolStripButton_arrangement_remove_Click);
            if (node == null)
            {
                menuItem.Enabled = false;
            }
            contextMenu.MenuItems.Add(menuItem);

            contextMenu.Show(treeView_arrangement, new Point(e.X, e.Y));

        }

        #endregion // �ż���ϵ


        #region ����

        private void listView_dup_projects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_dup_projects.SelectedIndices.Count == 0)
            {
                this.toolStripButton_dup_project_modify.Enabled = false;
                this.toolStripButton_dup_project_delete.Enabled = false;
            }
            else
            {
                this.toolStripButton_dup_project_modify.Enabled = true;
                this.toolStripButton_dup_project_delete.Enabled = true;
            }
        }

        // ����һ������
        private void toolStripButton_dup_project_new_Click(object sender, EventArgs e)
        {
            // ���Ƴ�һ���µ�DOM
            XmlDocument new_dom = new XmlDocument();
            new_dom.LoadXml(this.m_dup_dom.OuterXml);

            ProjectDialog dlg = new ProjectDialog();
            MainForm.SetControlFont(dlg, this.Font, false);

            dlg.CreateMode = true;
            //dlg.DupCfgDialog = this;
            dlg.DbFromInfos = this.MainForm.BiblioDbFromInfos;
            dlg.BiblioDbNames = this.GetAllBiblioDbNames();
            dlg.ProjectName = "�µĲ��ط���";
            dlg.ProjectComment = "";
            dlg.dom = new_dom;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            this.m_dup_dom = new_dom;

            // ˢ���б�
            FillProjectNameList(this.m_dup_dom);

            // ѡ���ղ��������
            SelectProjectItem(dlg.ProjectName);

            this.DupChanged = true;

            FillDefaultList(this.m_dup_dom);  // �����ļ��Ͽ��ܷ����ı�
        }

        private void toolStripButton_dup_project_modify_Click(object sender, EventArgs e)
        {
            if (this.listView_dup_projects.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�޸ĵĲ��ط�������");
                return;
            }

            ListViewItem item = this.listView_dup_projects.SelectedItems[0];

            string strProjectName = item.Text;
            string strProjectComment = ListViewUtil.GetItemText(item, 1);

            // ���Ƴ�һ���µ�DOM
            XmlDocument new_dom = new XmlDocument();
            new_dom.LoadXml(this.m_dup_dom.OuterXml);

            ProjectDialog dlg = new ProjectDialog();
            MainForm.SetControlFont(dlg, this.Font, false);

            dlg.CreateMode = false;
            // dlg.DupCfgDialog = this;
            dlg.DbFromInfos = this.MainForm.BiblioDbFromInfos;
            dlg.BiblioDbNames = this.GetAllBiblioDbNames();
            dlg.ProjectName = strProjectName;
            dlg.ProjectComment = strProjectComment;
            dlg.dom = new_dom;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            this.m_dup_dom = new_dom;

            item.Text = dlg.ProjectName;
            ListViewUtil.ChangeItemText(item,
                1, dlg.ProjectComment);

            this.DupChanged = true;

            FillDefaultList(this.m_dup_dom); // �����ļ��Ͽ��ܷ����ı�

            if (strProjectName != dlg.ProjectName)
            {
                // �����������ı�󣬶��ֵ��·���ȱʡ��ϵ�б���
                ChangeDefaultProjectName(strProjectName,
                    dlg.ProjectName);
            }

        }

        private void listView_dup_projects_DoubleClick(object sender, EventArgs e)
        {
            toolStripButton_dup_project_modify_Click(sender, e);
        }

        private void toolStripButton_dup_project_delete_Click(object sender, EventArgs e)
        {
            string strError = "";

            if (this.listView_dup_projects.SelectedIndices.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫɾ���Ĳ��ط�������");
                return;
            }

            DialogResult result = MessageBox.Show(this,
                "ȷʵҪɾ����ѡ���� " + this.listView_dup_projects.SelectedIndices.Count.ToString() + " �����ط���?",
                "DupCfgDialog",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result == DialogResult.No)
                return;

            for (int i = this.listView_dup_projects.SelectedIndices.Count - 1; i >= 0; i--)
            {
                int index = this.listView_dup_projects.SelectedIndices[i];

                ListViewItem item = this.listView_dup_projects.Items[index];

                string strProjectName = item.Text;

                XmlNode nodeProject = this.m_dup_dom.DocumentElement.SelectSingleNode("//project[@name='" + strProjectName + "']");
                if (nodeProject == null)
                {
                    strError = "������name����ֵΪ '" + strProjectName + "' ��<project>Ԫ��";
                    goto ERROR1;
                }

                nodeProject.ParentNode.RemoveChild(nodeProject);

                this.listView_dup_projects.Items.RemoveAt(index);

                // ������ɾ�������ֵ��·���ȱʡ��ϵ�б��У�Ҳɾ����Ӧ����
                ChangeDefaultProjectName(strProjectName,
                        null);
            }

            this.DupChanged = true;

            FillDefaultList(this.m_dup_dom); // �����ļ��Ͽ��ܷ����ı�

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        private void toolStripButton_dup_default_new_Click(object sender, EventArgs e)
        {
            string strError = "";

            DefaultProjectDialog dlg = new DefaultProjectDialog();
            MainForm.SetControlFont(dlg, this.Font, false);

            dlg.Text = "����ȱʡ��ϵ����";
            // dlg.DupCfgDialog = this;
            dlg.BiblioDbNames = this.GetAllBiblioDbNames();
            dlg.dom = this.m_dup_dom;
            dlg.DatabaseName = "";  // ����������
            dlg.DefaultProjectName = "";

            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            // ���ֵ�DOM��
            XmlNode nodeDefault = this.m_dup_dom.DocumentElement.SelectSingleNode("//default[@origin='" + dlg.DatabaseName + "']");
            if (nodeDefault != null)
            {
                // ����
                strError = "����·��Ϊ '" + dlg.DatabaseName + "' ��ȱʡ��ϵ�����Ѿ����ڣ������ٴ��������ɱ༭�Ѿ����ڵĸ����";
                goto ERROR1;
            }

            {
                nodeDefault = this.m_dup_dom.CreateElement("default");

                this.m_dup_dom.DocumentElement.AppendChild(nodeDefault);
            }
            DomUtil.SetAttr(nodeDefault, "origin", dlg.DatabaseName);
            DomUtil.SetAttr(nodeDefault, "project", dlg.DefaultProjectName);

            // ���ֵ�listview��
            ListViewItem item = new ListViewItem(dlg.DatabaseName, 0);
            item.SubItems.Add(dlg.DefaultProjectName);
            this.listView_dup_defaults.Items.Add(item);

            // �������ݿ������Ƿ����Ѿ��õ������ݿ��������У�����ǣ�Ϊʵ����ɫ��������ǣ�Ϊ������ɫ
            List<string> database_names = GetAllBiblioDbNames();
            if (database_names.IndexOf(dlg.DatabaseName) == -1)
            {
                item.ForeColor = SystemColors.GrayText;
                item.Tag = null;
            }
            else
            {
                item.Tag = 1;
            }

            this.DupChanged = true;
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        private void toolStripButton_dup_default_modify_Click(object sender, EventArgs e)
        {
            if (this.listView_dup_defaults.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫ�޸ĵ�ȱʡ��ϵ����");
                return;
            }

            ListViewItem item = this.listView_dup_defaults.SelectedItems[0];

            DefaultProjectDialog dlg = new DefaultProjectDialog();
            MainForm.SetControlFont(dlg, this.Font, false);
            dlg.dom = this.m_dup_dom;
            dlg.DatabaseName = item.Text;
            dlg.DefaultProjectName = ListViewUtil.GetItemText(item, 1);

            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            // ���ֵ�DOM��
            XmlNode nodeDefault = this.m_dup_dom.DocumentElement.SelectSingleNode("//default[@origin='" + item.Text + "']");
            if (nodeDefault == null)
            {
                nodeDefault = this.m_dup_dom.CreateElement("default");
                this.m_dup_dom.DocumentElement.AppendChild(nodeDefault);
            }

            DomUtil.SetAttr(nodeDefault, "origin", item.Text);
            DomUtil.SetAttr(nodeDefault, "project", dlg.DefaultProjectName);


            // ���ֵ�listview��
            Debug.Assert(dlg.DatabaseName == item.Text, "");
            ListViewUtil.ChangeItemText(item, 1, dlg.DefaultProjectName);

            this.DupChanged = true;
        }

        private void listView_dup_defaults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_dup_defaults.SelectedIndices.Count == 0)
            {
                this.toolStripButton_dup_default_modify.Enabled = false;
                this.toolStripButton_dup_default_delete.Enabled = false;
            }
            else
            {
                this.toolStripButton_dup_default_modify.Enabled = true;
                this.toolStripButton_dup_default_delete.Enabled = true;
            }
        }

        private void listView_dup_defaults_DoubleClick(object sender, EventArgs e)
        {
            toolStripButton_dup_default_modify_Click(sender, e);
        }

        // ɾ��ȱʡ��ϵ����
        // TODO: ����һ�ο���ɾ���������
        private void toolStripButton_dup_default_delete_Click(object sender, EventArgs e)
        {
            // string strError = "";

            if (this.listView_dup_defaults.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "��δѡ��Ҫɾ����ȱʡ��ϵ����");
                return;
            }

            ListViewItem item = this.listView_dup_defaults.SelectedItems[0];
            string strText = item.Text + " -- " + ListViewUtil.GetItemText(item, 1);
            if (item.Tag == null)
            {
                DialogResult result = MessageBox.Show(this,
                    "ȷʵҪɾ��ȱʡ��ϵ���� " + strText + " ?",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                    return;

                // ������������ɾ��
                this.listView_dup_defaults.Items.Remove(item);
            }
            else
            {
                // TODO: ����������б������ǿգ���û�б�Ҫ����ɾ����

                DialogResult result = MessageBox.Show(this,
                    "ȷʵҪ���ȱʡ��ϵ���� " + strText + " ?",
                    "ManagerForm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                    return;

                // ʵ�ڵ����ֻ��Ĩ��������������
                ListViewUtil.ChangeItemText(item, 1, "");
            }

            // ���ֵ�DOM��
            XmlNode nodeDefault = this.m_dup_dom.DocumentElement.SelectSingleNode("//default[@origin='" + item.Text + "']");
            /*
            if (nodeDefault == null)
            {
                strError = "����·��Ϊ '" + item.Text + "' ��ȱʡ��ϵ�����Ȼ��DOM�в�����";
                goto ERROR1;
            }*/
            if (nodeDefault != null)
            {
                nodeDefault.ParentNode.RemoveChild(nodeDefault);
                this.DupChanged = true;
            }
            return;
            /*
        ERROR1:
            MessageBox.Show(this, strError);
             * */
        }

        private void toolStripButton_dup_refresh_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = this.ListDup(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
            }
        }

        private void toolStripButton_dup_save_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            nRet = this.SubmitDupDef(out strError);
            if (nRet == -1)
                MessageBox.Show(this, strError);
            else
            {
                this.DupChanged = false;
            }

        }

        ListViewItem m_currentLibraryCodeItem = null;

        // �ݴ����б�����ѡ��ı�
        private void listView_loanPolicy_libraryCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_loanPolicy_libraryCodes.SelectedItems.Count != 1)
            {
                FinishLibraryCodeTextbox();

                int nSave = this.m_nRightsTableTypesVersion;

                this.textBox_loanPolicy_bookTypes.Text = "";
                this.textBox_loanPolicy_readerTypes.Text = "";

                this.m_nRightsTableTypesVersion = nSave;    // �ָ������������ı仯����Ϊ���ֱ仯���������Ļ��������

                this.textBox_loanPolicy_bookTypes.Enabled = false;
                this.textBox_loanPolicy_readerTypes.Enabled = false;

                this.m_currentLibraryCodeItem = null;
            }
            else
            {
                // ��.tag��textbox
                LibraryCodeInfo info = (LibraryCodeInfo)this.listView_loanPolicy_libraryCodes.SelectedItems[0].Tag;

                int nSave = this.m_nRightsTableTypesVersion;

                this.textBox_loanPolicy_bookTypes.Text = info.BookTypeList;
                this.textBox_loanPolicy_readerTypes.Text = info.ReaderTypeList;

                this.m_nRightsTableTypesVersion = nSave;    // �ָ������������ı仯����Ϊ���ֱ仯���������Ļ��������

                if (this.textBox_loanPolicy_bookTypes.Enabled == false)
                    this.textBox_loanPolicy_bookTypes.Enabled = true;
                if (this.textBox_loanPolicy_readerTypes.Enabled == false)
                    this.textBox_loanPolicy_readerTypes.Enabled = true;

                this.m_currentLibraryCodeItem = this.listView_loanPolicy_libraryCodes.SelectedItems[0];
            }
        }

        private void listView_center_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = null;

            string strName = "";
            if (this.listView_center.SelectedItems.Count > 0)
            {
                strName = this.listView_center.SelectedItems[0].Text;
            }

            // �޸����ݿ�
            {
                menuItem = new MenuItem("�޸ķ����� '" + strName + "'(&M)");
                menuItem.Click += new System.EventHandler(this.toolStripButton_center_modify_Click);
                if (this.listView_center.SelectedItems.Count == 0)
                    menuItem.Enabled = false;
                // ȱʡ����
                menuItem.DefaultItem = true;
                contextMenu.MenuItems.Add(menuItem);
            }

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("��ӷ�����(&A)");
            menuItem.Click += new System.EventHandler(this.toolStripButton_center_add_Click);
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            string strText = "";
            if (this.listView_databases.SelectedItems.Count == 1)
                strText = "�Ƴ������� '" + strName + "'(&R)";
            else
                strText = "�Ƴ���ѡ " + this.listView_center.SelectedItems.Count.ToString() + " ��������(&R)";

            menuItem = new MenuItem(strText);
            menuItem.Click += new System.EventHandler(this.toolStripButton_center_delete_Click);
            if (this.listView_center.SelectedItems.Count == 0)
                menuItem.Enabled = false;
            contextMenu.MenuItems.Add(menuItem);

            // ---
            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("ˢ��(&R)");
            menuItem.Click += new System.EventHandler(this.toolStripButton_center_refresh_Click);
            contextMenu.MenuItems.Add(menuItem);

            contextMenu.Show(this.listView_center, new Point(e.X, e.Y));		

        }

        private void listView_center_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_center.SelectedItems.Count == 0)
            {
                this.toolStripButton_center_delete.Enabled = false;
                this.toolStripButton_center_modify.Enabled = false;
            }
            else
            {
                this.toolStripButton_center_delete.Enabled = true;
                this.toolStripButton_center_modify.Enabled = true;
            }
        }

        private void listView_center_DoubleClick(object sender, EventArgs e)
        {
            toolStripButton_center_modify_Click(sender, e);
        }

    }

    #endregion // ����
}