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

using DigitalPlatform.Xml;
using DigitalPlatform.GUI;

namespace DigitalPlatform.DTLP
{
  
    public partial class DtlpDupForm : Form
    {
        DigitalPlatform.StopManager stopManager = null; // ����
        DigitalPlatform.Stop stop = null;   // ����ʹ��

        public event LoadDetailEventHandler LoadDetail = null;

        // ����������к�����
        SortColumns SortColumns = new SortColumns();

        bool m_bInSearch = false;
        public bool AutoBeginSearch = false;

        public string CfgFilename = ""; // dup.xml�����ļ���
        XmlDocument dom = null;

        XmlNode m_nodeProject = null;   // ��ǰ��ע��<project>Ԫ�ؽڵ�

        public DtlpChannel DtlpChannel = null;

        const int WM_INITIAL = API.WM_USER + 201;

        const int ITEMTYPE_NORMAL = 0;  // ��ͨ����
        const int ITEMTYPE_OVERTHRESHOLD = 1; // Ȩֵ������ֵ������

        /// <summary>
        /// ���������ź�
        /// </summary>
        public AutoResetEvent EventFinish = new AutoResetEvent(false);

        string m_strMarcRecord = "";

        // MARC��¼
        public string MarcRecord
        {
            get
            {
                return this.m_strMarcRecord;
            }
            set
            {
                this.m_strMarcRecord = value;
            }
        }

        public string ProjectName
        {
            get
            {
                return this.comboBox_projectName.Text;
            }
            set
            {
                this.comboBox_projectName.Text = value;
            }
        }

        // ��¼·��������������������
        public string RecordPath
        {
            get
            {
                return this.textBox_recordPath.Text;
            }
            set
            {
                this.textBox_recordPath.Text = value;

                this.Text = "DTLP����: " + value;
            }
        }

        public DtlpDupForm()
        {
            InitializeComponent();
        }

        // return:
        //      -1  ����
        //      0   �����ļ�������
        //      1   �ɹ�
        public int Initial(string strCfgFilename,
            StopManager stopManager,
            out string strError)
        {
            strError = "";

            this.dom = new XmlDocument();
            try
            {
                this.dom.Load(strCfgFilename);
                this.CfgFilename = strCfgFilename;
            }
            catch (FileNotFoundException /*ex*/)
            {
                strError = "�����ļ� '" + strCfgFilename + "' ������";
                this.dom.LoadXml("<root />");   // ���ں��潨��������
                this.CfgFilename = strCfgFilename;
                return 0;
            }
            catch (Exception ex)
            {
                strError = "�����ļ� '" + strCfgFilename + "' װ�ص�XMLDOMʱ����: " + ex.Message;
                return -1;
            }

            this.stopManager = stopManager;

            return 1;
        }

        private void DupForm_Load(object sender, EventArgs e)
        {
            Debug.Assert(this.dom != null, "��δInitial()");

            stop = new DigitalPlatform.Stop();
            if (this.stopManager != null)
                stop.Register(this.stopManager, true);	// ����������

                        // �Զ���������
            if (this.AutoBeginSearch == true)
            {
                API.PostMessage(this.Handle, WM_INITIAL, 0, 0);
            }
        }

        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_INITIAL:
                    {
                        string strError = "";
                        int nRet = DoSearch(out strError);
                        if (nRet == -1)
                            goto ERROR1;

                        return;
                    ERROR1:
                        MessageBox.Show(this, strError);
                    }
                    return;
            }
            base.DefWndProc(ref m);
        }


        private void button_search_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = DoSearch(out strError);
            if (nRet == -1)
                goto ERROR1;

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // ����·���е�servername����
        static string GetServerName(string strPath)
        {
            string[] parts = strPath.Split(new char [] {'/'} );

            if (parts.Length == 0)
                return "";

            Debug.Assert(parts.Length >= 1, "");

            return parts[0];
        }

        void EnableControls(bool bEnable)
        {
            this.button_search.Enabled = bEnable;

            this.button_viewMarcRecord.Enabled = bEnable;

            this.comboBox_projectName.Enabled = bEnable;
            this.textBox_recordPath.Enabled = bEnable;

            // this.textBox_serverName.Enabled = bEnable;
            // this.button_findServerName.Enabled = bEnable;
        }

        int DoSearch(out string strError)
        {
            strError = "";

            /*
            string strServerName = "";

            strServerName = this.textBox_serverName.Text;

            if (String.IsNullOrEmpty(strServerName) == true)
            {
                // ���servername textΪ�գ���ӷ���·���л��servername
                strServerName = GetServerName(this.RecordPath);
                if (String.IsNullOrEmpty(strServerName) == true)
                {
                    strError = "��û��ָ����������������£�����ָ������·��";
                    return -1;
                }
            }*/

            string strProjectName = "";

            if (this.comboBox_projectName.Text == ""
                || this.comboBox_projectName.Text == "{default}"
                || this.comboBox_projectName.Text == "<default>"
                || this.comboBox_projectName.Text == "<Ĭ��>")
            {
                // ��ú�һ����Դ·����ص�ȱʡ���ط���
                string strDatabasePath = "";
                strProjectName = GetDefaultProjectName(this.RecordPath,
                    out strDatabasePath);
                if (String.IsNullOrEmpty(strProjectName) == true)
                {
                    strError = "û�ж���ͷ������ݿ� '" + strDatabasePath + "' ������ȱʡ���ط���������޷����в���...";
                    return -1;
                }

                this.comboBox_projectName.Text = strProjectName;    // ��ʵ���õ��Ĳ��ط�������ʾ����
            }
            else
            {
                strProjectName = this.comboBox_projectName.Text;
            }

            this.m_nodeProject = this.dom.DocumentElement.SelectSingleNode("//project[@name='"+strProjectName+"']");
            if (this.m_nodeProject == null)
            {
                strError = "û�ж�������Ϊ '" + strProjectName + "' �Ĳ��ط���";
                return -1;
            }

            stop.OnStop += new StopEventHandler(this.DoStop);
            stop.SetMessage("��ʼ���� ...");
            stop.BeginLoop();

            this.Update();

            this.EnableControls(false);

            this.EventFinish.Reset();
            this.m_bInSearch = true;
            try
            {
                this.ClearDupState(true);
                this.listView_browse.Items.Clear();

                // �г�<project>Ԫ���µ�����<database>Ԫ��
                XmlNodeList databases = this.m_nodeProject.SelectNodes("database");
                for (int i = 0; i < databases.Count; i++)
                {
                    XmlNode nodeDatabase = databases[i];

                    int nRet = DoOneDatabase(
                        nodeDatabase,
                        out strError);
                    if (nRet == -1)
                        return -1;
                }

                // ����
                {
                    ColumnSortStyle sortStyle = ColumnSortStyle.RightAlign;

                    this.SortColumns.SetFirstColumn(1,
                        sortStyle,
                        this.listView_browse.Columns,
                        true);

                    this.SortColumns[0].Asc = false;    // �����ǰ
                    this.SortColumns.RefreshColumnDisplay(this.listView_browse.Columns);

                    // ����
                    this.listView_browse.ListViewItemSorter = new SortColumnsComparer(this.SortColumns);
                    this.listView_browse.ListViewItemSorter = null;
                }

                // ��ʾ����״̬
                this.SetDupState();
            }
            finally
            {
                this.EventFinish.Set();
                this.m_bInSearch = false;

                stop.EndLoop();
                stop.OnStop -= new StopEventHandler(this.DoStop);
                stop.Initial("");

                EnableControls(true);

            }

            // ������
            return this.listView_browse.Items.Count;
        }

        // ��������ļ������еķ�����
        List<string> GetAllProjectName()
        {
            List<string> results = new List<string>();

            XmlNodeList nodes = this.dom.DocumentElement.SelectNodes("//project");
            for (int i = 0; i < nodes.Count; i++)
            {
                results.Add(DomUtil.GetAttr(nodes[i], "name"));
            }

            return results;
        }

        // ͨ���������ݿ⣬�õ����ط�����
        // parameters:
        //      strStartPath    �����¼·������һ��Ҫ���ݿ�·���������������ݿ�
        //      strDatabasePath [out]˳�㷵�����ݿ�·��
        string GetDefaultProjectName(string strStartPath,
            out string strDatabasePath)
        { 
            // �Ѽ�¼·���ӹ�Ϊ������ʽ��(��������/���ݿ���)
            strDatabasePath = GetDbNameStylePath(strStartPath);

            XmlNode node = this.dom.DocumentElement.SelectSingleNode("//defaultProject/sourceDatabase[@name='" + strDatabasePath + "']");
            if (node == null)
                return null;

            return DomUtil.GetAttr(node, "defaultProject");
        }

        // �Ѽ�¼·���ӹ�Ϊ������̬
        string GetDbNameStylePath(string strPath)
        {
            string[] parts = strPath.Split(new char [] {'/'} );

            if (parts.Length == 0)
                return "";

            if (parts.Length == 1)
                return parts[0];

            Debug.Assert(parts.Length >= 2, "");

            return parts[0] + "/" + parts[1];
        }

        // ��һ�����ݿ��ڵļ���
        // parameters:
        //      strServerName   Ŀ����������������<database>Ԫ��name�����еķ����������ֿɲ�һ��һ��
        //      strDatabaseName ���ݿ�·��
        int DoOneDatabase(
            XmlNode nodeDatabase,
            out string strError)
        {
            strError = "";

            string strDatabasePath = DomUtil.GetAttr(nodeDatabase, "name");
            string strThreshold = DomUtil.GetAttr(nodeDatabase, "threshold");

            int nThreshold = 0;

            try
            {
                nThreshold = Convert.ToInt32(strThreshold);
            }
            catch
            {
                strError = "thresholdֵ '" + strThreshold + "' ��ʽ����ȷ��Ӧ��Ϊ������";
                return -1;
            }

            // string strServerName = GetServerName(strDatabasePath);

            List<string> results = null;

            // ���һ����¼�Ĺ���������ݿ�ļ�����
            int nRet = this.DtlpChannel.GetAccessPoint(strDatabasePath + "/ctlno/?", // ȫ·��
                this.MarcRecord,
                out results,
                out strError);
            if (nRet == -1)
                return -1;

            // ��results�ӹ�Ϊ���õ�����
            List<KeyFromItem> items = BuildKeyFromList(results);

            // ����������ʵ�ʴ��ڵ�ÿ��key+from
            for (int i = 0; i < items.Count; i++)
            {
                KeyFromItem item = items[i];

                string strWeight = "";
                string strSearchStyle = "";

                // ������from�йص�weight/searchstyle
                // ��ù���һ��������Ķ�����Ϣ weight / searchstyle
                // parameters:
                //      strFromName from����ע�������31�ַ�Ӧ���滻Ϊ'$'
                // return:
                //      -1  error
                //      0   not found
                //      1   found
                nRet = GetAccessPointDef(nodeDatabase,
                    item.From,
                    out strWeight,
                    out strSearchStyle,
                    out strError);
                if (nRet == -1)
                    return -1;
                if (nRet == 0)
                    continue;

                Debug.Assert(strWeight != "", "");

                // ���м���
                nRet = DoOneSearch(
                    strDatabasePath,
                    item.From,
                    item.Key,
                    strWeight,
                    strSearchStyle,
                    nThreshold,
                    out strError);
                if (nRet == -1)
                    return -1;
            }

            // TODO: ������<accessPoint>�ڵ�洢��һ��hashtable�У��Ա���������ٶȣ����ڲ�죬������xpath��ѡ��?


            return 0;
        }

        // ��ù���һ��������Ķ�����Ϣ weight / searchstyle
        // parameters:
        //      strFromName from����ע�������31�ַ�Ӧ���滻Ϊ'$'
        // return:
        //      -1  error
        //      0   not found
        //      1   found
        static int GetAccessPointDef(XmlNode nodeDatabase,
            string strFromName,
            out string strWeight,
            out string strSearchStyle,
            out string strError)
        {
            strWeight = "";
            strSearchStyle = "";
            strError = "";

            XmlNode node = nodeDatabase.SelectSingleNode("accessPoint[@name='"+strFromName+"']");
            if (node == null)
                return 0;

            strWeight = DomUtil.GetAttr(node, "weight");
            strSearchStyle = DomUtil.GetAttr(node, "searchStyle");

            return 1;
        }

        // ���һ�������ʵļ���
        int DoOneSearch(
            string strDatabasePath,
            string strFromName,
            string strWord,
            string strWeight,
            string strSearchStyle,
            int nThreshold,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            byte[] baNext = null;
            int nStyle = DtlpChannel.CTRLNO_STYLE;

            if (strSearchStyle.ToLower() == "exact")
                nStyle |= DtlpChannel.EXACT_RECORD;

            string strPath = "";
            strPath = strDatabasePath + "/" + strFromName.Replace('$', (char)31) + "/" + strWord;

            // int nDupCount = 0;

            List<string> path_list = new List<string>();

            bool bFirst = true;       // ��һ�μ���
            while (true)
            {
                Application.DoEvents();	// ���ý������Ȩ

                if (stop != null)
                {
                    if (stop.State != 0)
                    {
                        strError = "�û��ж�";
                        goto ERROR1;
                    }
                }

                Encoding encoding = this.DtlpChannel.GetPathEncoding(strPath);

                byte[] baPackage;
                if (bFirst == true)
                {
                    nRet = this.DtlpChannel.Search(strPath,
                        nStyle,
                        out baPackage);
                }
                else
                {
                    nRet = this.DtlpChannel.Search(strPath,
                        baNext,
                        nStyle,
                        out baPackage);
                }
                if (nRet == -1)
                {
                    int errorcode = this.DtlpChannel.GetLastErrno();
                    if (errorcode == DtlpChannel.GL_NOTEXIST)
                    {
                        break;
                    }
                    strError = this.DtlpChannel.GetErrorString();
                    goto ERROR1;
                }

                bFirst = false;

                Package package = new Package();
                package.LoadPackage(baPackage,
                    encoding);

                nRet = package.Parse(PackageFormat.String);
                if (nRet == -1)
                {
                    strError = "Package::Parse() error";
                    goto ERROR1;
                }

                // TODO: �����������Ƚ���һ���ڴ�ṹ��ȥ���Ժ��ٺϲ�����listview
                AddToPathList(package,
                    ref path_list);

                if (package.ContinueString != "")
                {
                    nStyle |= DtlpChannel.CONT_RECORD;
                    baNext = package.ContinueBytes;
                }
                else
                {
                    break;
                }
            }

            // ����ȥ��
            path_list.Sort();
            Global.RemoveDup(ref path_list);

            nRet = FillBrowseList(path_list,
                strWeight,
                nThreshold,
                out strError);
            if (nRet == -1)
                goto ERROR1;


            return 0;
        ERROR1:
            return -1;
        }



        // ��������·��list
        void AddToPathList(Package package,
            ref List<string> list)
        {
            for (int i = 0; i < package.Count; i++)
            {
                Cell cell = (Cell)package[i];

                string strShortPath = Global.ModifyDtlpRecPath(cell.Path,
    "");
                list.Add(strShortPath);
            }
        }

        int FillBrowseList(List<string> path_list,
            string strWeight,
            int nThreshold,
            out string strError)
        {
            strError = "";

            int nWeight = 0;
            try
            {
                nWeight = Convert.ToInt32(strWeight);
            }
            catch
            {
                strError = "weight�ַ��� '" + strWeight + "' ��ʽ����ȷ��Ӧ��Ϊ������";
                return -1;
            }


            string strShorterStartRecordPath = Global.ModifyDtlpRecPath(this.RecordPath, "");

            // int nDupCount = 0;

            // ����ÿ����¼
            for (int i = 0; i < path_list.Count; i++)
            {
                Application.DoEvents();	// ���ý������Ȩ

                if (stop != null)
                {
                    if (stop.State != 0)
                    {
                        strError = "�û��ж�";
                        return -1;
                    }
                }

                string strPath = path_list[i];


                // ����
                ListViewItem item = DetectDup(strPath);

                int nCurrentWeight = 0;

                if (item == null)
                {
                    string strCompletePath = Global.ModifyDtlpRecPath(strPath,
                        "ctlno");

                    item = new ListViewItem();
                    item.Text = strPath;
                    item.SubItems.Add(strWeight);
                    this.listView_browse.Items.Add(item);

                    nCurrentWeight = nWeight;

                    string[] cols = null;

                    int nRet = GetOneBrowseRecord(
                        strCompletePath,
                        out cols,
                        out strError);
                    if (nRet == -1)
                    {
                        item.SubItems.Add(strError);
                        continue;
                    }
                    if (cols != null)
                    {
                        // ȷ���б��������㹻
                        ListViewUtil.EnsureColumns(this.listView_browse,
                            cols.Length,
                            200);
                        for (int j = 0; j < cols.Length; j++)
                        {
                            item.SubItems.Add(cols[j]);
                        }
                    }
                }
                else
                {
                    // �Ȼ���Ѿ��еķ���
                    string strExistWeight = item.SubItems[1].Text;

                    int nExistWeight = 0;
                    try
                    {
                        nExistWeight = Convert.ToInt32(strExistWeight);
                    }
                    catch
                    {
                        strError = "ԭ�е�weight�ַ��� '" +strExistWeight + "' ��ʽ����ȷ��Ӧ��Ϊ������";
                        return -1;
                    }

                    nCurrentWeight = nExistWeight + nWeight;

                    item.SubItems[1].Text = nCurrentWeight.ToString();
                }

                if (strPath == strShorterStartRecordPath)
                {
                    // ������Ƿ����¼�Լ�  2008/2/29
                    item.ImageIndex = ITEMTYPE_OVERTHRESHOLD;
                    item.BackColor = Color.LightGoldenrodYellow;
                    item.ForeColor = SystemColors.GrayText; // ��ʾ���Ƿ����¼�Լ�
                }
                else if (nCurrentWeight >= nThreshold)
                {
                    item.ImageIndex = ITEMTYPE_OVERTHRESHOLD;
                    item.BackColor = Color.LightYellow;
                    item.Font = new Font(item.Font, FontStyle.Bold);
                }
                else
                {
                    item.ImageIndex = ITEMTYPE_NORMAL;
                }

                // this.listView_browse.UpdateItem(this.listView_browse.Items.Count - 1);
            }

            return 0;
        }

        // ��������¼����
        // parameters:
        //      strPath ��¼·����Ӧ��Ϊ��ȫ��̬
        int GetOneBrowseRecord(
            string strPath,
            out string[] cols,
            out string strError)
        {
            strError = "";
            cols = null;

            int nRet = 0;

            int nStyle = DtlpChannel.JH_STYLE; // ��ü򻯼�¼

            byte[] baPackage;
            nRet = this.DtlpChannel.Search(strPath,
                nStyle,
                out baPackage);
            if (nRet == -1)
            {
                strError = "Search() path '" + strPath + "' ʱ��������: " + this.DtlpChannel.GetErrorString();
                goto ERROR1;
            }

            Package package = new Package();
            package.LoadPackage(baPackage,
                this.DtlpChannel.GetPathEncoding(strPath));
            nRet = package.Parse(PackageFormat.String);
            if (nRet == -1)
            {
                strError = "Package::Parse() error";
                goto ERROR1;
            }

            string strContent = package.GetFirstContent();

            if (String.IsNullOrEmpty(strContent) == false)
            {
                cols = strContent.Split(new char[] { '\t' });
            }

            return 0;
        ERROR1:
            return -1;
        }

        // TODO: �������֮���ظ��ģ���Ҫ�ۼӷ�����һ��֮�ڣ��������ظ�
        // ����Ƿ�����
        // TODO: ������ܣ���Hashtable������ٶ�
        ListViewItem DetectDup(string strPath)
        {
            for (int i = 0; i < this.listView_browse.Items.Count; i++)
            {
                ListViewItem item = this.listView_browse.Items[i];
                if (item.Text == strPath)
                    return item;
            }

            return null;
        }

        /*
        // ��·���ӹ�Ϊ���Ի���������̬
        // parameters:
        //      strCtlnoPart    ���Ϊ""����ʾ�ӹ�Ϊ���Եġ�ֻ����ǰ����ʾ��̬�����Ϊ"ctlno"����"��¼������"����ʾ�ӹ�Ϊ������̬
        string ModifyDtlpRecPath(string strPath,
            string strCtlnoPart)
        {
            int nRet = strPath.LastIndexOf("/");

            if (nRet == -1)
                return strPath;

            string strNumber = strPath.Substring(nRet + 1).Trim();

            nRet = strPath.LastIndexOf("/", nRet - 1);
            if (nRet == -1)
                return strPath;

            string strPrevPart = strPath.Substring(0, nRet).Trim();

            return strPrevPart + "/" + strCtlnoPart + "/" + strNumber;
        }*/

        private void comboBox_projectName_DropDown(object sender, EventArgs e)
        {
            if (this.comboBox_projectName.Items.Count == 0)
            {
                List<string> projectnames = GetAllProjectName();
                for (int i = 0; i < projectnames.Count; i++)
                {
                    this.comboBox_projectName.Items.Add(projectnames[i]);
                }

                this.comboBox_projectName.Items.Add("{default}");
            }
        }

        static List<KeyFromItem> BuildKeyFromList(List<string> results)
        {
            List<KeyFromItem> items = new List<KeyFromItem>();
            for (int i = 0; i < results.Count / 2; i++)
            {
                KeyFromItem item = new KeyFromItem();
                item.Key = results[i * 2];
                item.From = results[i * 2 + 1].Replace((char)31, '$');
                items.Add(item);
            }

            return items;
        }

        private void DtlpDupForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            EventFinish.Set();

            if (stop != null) // �������
            {
                stop.Style = StopStyle.None;    // ��Ҫǿ���ж�
                stop.DoStop();

                stop.Unregister();	// �������������
                stop = null;
            }
        }

        void DoStop(object sender, StopEventArgs e)
        {
            if (this.DtlpChannel != null)
                this.DtlpChannel.Cancel();
        }

        void ClearDupState(bool bSearching)
        {
            if (bSearching == true)
                this.label_dupMessage.Text = "���ڲ���...";
            else
                this.label_dupMessage.Text = "��δ����";
        }

        // ���ò���״̬
        void SetDupState()
        {
            int nCount = 0;

            string strShorterStartRecordPath = Global.ModifyDtlpRecPath(this.RecordPath,
                "");

            for (int i = 0; i < this.listView_browse.Items.Count; i++)
            {
                ListViewItem item = this.listView_browse.Items[i];

                if (item.Text == strShorterStartRecordPath)
                    continue;   // �����������¼�Լ� 2008/2/29

                if (item.ImageIndex == ITEMTYPE_OVERTHRESHOLD)
                    nCount++;
                else
                    break;  // �ٶ�����Ȩֵ�������ǰ����һ������һ�����ǵ����ѭ���ͽ���
            }

            if (nCount > 0)
                this.label_dupMessage.Text = "�� " + Convert.ToString(nCount) + " ���ظ���¼��";
            else
                this.label_dupMessage.Text = "û���ظ���¼��";

        }

        private void listView_browse_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int nClickColumn = e.Column;

            ColumnSortStyle sortStyle = ColumnSortStyle.LeftAlign;

            // ��һ��Ϊ��¼·��������������
            if (nClickColumn == 0)
                sortStyle = ColumnSortStyle.RecPath;

            this.SortColumns.SetFirstColumn(nClickColumn,
                sortStyle,
                this.listView_browse.Columns,
                true);

            // ����
            this.listView_browse.ListViewItemSorter = new SortColumnsComparer(this.SortColumns);

            this.listView_browse.ListViewItemSorter = null;

        }

        private void listView_browse_DoubleClick(object sender, EventArgs e)
        {
            // ��ֹ����
            if (m_bInSearch == true)
            {
                MessageBox.Show(this, "��ǰ�������ڱ�һ��δ�����ĳ�����ʹ�ã��޷�װ�ؼ�¼�����Ժ����ԡ�");
                return;
            }

            if (this.LoadDetail != null)
            {
                int nIndex = -1;
                if (this.listView_browse.SelectedIndices.Count > 0)
                    nIndex = this.listView_browse.SelectedIndices[0];
                else
                {
                    if (this.listView_browse.FocusedItem == null)
                    {
                        MessageBox.Show(this, "��δѡ��Ҫװ����ϸ��������");
                        return;
                    }
                    nIndex = this.listView_browse.Items.IndexOf(this.listView_browse.FocusedItem);
                }

                LoadDetailEventArgs e1 = new LoadDetailEventArgs();
                e1.RecordPath = Global.ModifyDtlpRecPath(this.listView_browse.Items[nIndex].Text, "ctlno");
                this.LoadDetail(this, e1);
            }
        }
    }

    class KeyFromItem
    {
        public string Key = "";
        public string From = "";
    }

    // �������ݼӹ�ģ��
    public delegate void LoadDetailEventHandler(object sender,
        LoadDetailEventArgs e);

    public class LoadDetailEventArgs : EventArgs
    {
        public string ServerName = "";
        public string RecordPath = "";
        public bool OpenNewWindow = true;
    }
}