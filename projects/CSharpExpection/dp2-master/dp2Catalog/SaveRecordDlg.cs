using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DigitalPlatform.DTLP;
using DigitalPlatform.CirculationClient;
using DigitalPlatform.CommonControl;

namespace dp2Catalog
{
    public partial class SaveRecordDlg : Form
    {
        // ��ǰ�û���
        public string CurrentUserName = "";

        bool m_bFirst = true;

        bool m_bDtlpInitialized = false;
        bool m_bDp2Initialized = false;

        public ISearchForm LinkedSearchForm = null;

        // �ⲿ���ô�ֵ
        public DtlpChannelArray DtlpChannels = null;
        public DtlpChannel DtlpChannel = null;	// 

        public MainForm MainForm = null;
        public LibraryChannelCollection dp2Channels = null;
        // public LibraryChannel dp2Channel = null;

        public event GetDtlpSearchParamEventHandle GetDtlpSearchParam = null;
        public event GetDp2SearchParamEventHandle GetDp2SearchParam = null;

        public SaveRecordDlg()
        {
            InitializeComponent();

            this.dtlpResDirControl1.PathSeparator = "/";
        }

        private void SaveRecordDlg_Load(object sender, EventArgs e)
        {
            if (this.MainForm != null
                && this.dp2ResTree1.cfgCache == null)
                this.dp2ResTree1.cfgCache = this.MainForm.cfgCache;

            tabControl_main_SelectedIndexChanged(null, null);   // 2007/10/11
        }

        private void SaveRecordDlg_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        void InitialDtlpProtocol()
        {
            if (m_bDtlpInitialized == true)
                return;

            if (this.GetDtlpSearchParam == null)
            {
                throw new Exception("GetDtlpSearchParamEventHandle not set");
            }

            GetDtlpSearchParamEventArgs e = new GetDtlpSearchParamEventArgs();
            this.GetDtlpSearchParam(this, e);


            // ��ʼ��DTLPЭ����ز���
            this.dtlpResDirControl1.channelarray = e.DtlpChannels;
            dtlpResDirControl1.Channel = e.DtlpChannel;

            /*
            dtlpResDirControl1.procItemSelected = new Delegate_ItemSelected(
                this.DtlpItemSelected);
            dtlpResDirControl1.procItemText = new Delegate_ItemText(
                this.DtlpItemText);
             * */
            dtlpResDirControl1.FillSub(null);

            // չ��
            if (this.textBox_dtlpRecPath.Text != "")
            {
                string strSavePath = this.textBox_dtlpRecPath.Text;

                this.dtlpResDirControl1.SelectedPath1 = this.textBox_dtlpRecPath.Text;

                this.textBox_dtlpRecPath.Text = strSavePath;
            }

            m_bDtlpInitialized = true;
        }

        void InitialDp2Protocol()
        {
            if (m_bDp2Initialized == true)
                return;

            if (this.GetDp2SearchParam == null)
            {
                throw new Exception("Getdp2SearchParamEventHandle not set");
            }

            GetDp2SearchParamEventArgs e = new GetDp2SearchParamEventArgs();
            this.GetDp2SearchParam(this, e);

            if (this.MainForm != null
    && this.dp2ResTree1.cfgCache == null)
                this.dp2ResTree1.cfgCache = this.MainForm.cfgCache;

            // ��ʼ��dp2Э����ز���
            this.dp2ResTree1.stopManager = e.MainForm.stopManager;
            this.dp2ResTree1.Servers = e.MainForm.Servers;	// ����
#if OLD_CHANNEL
            this.dp2ResTree1.Channels = e.dp2Channels;
#endif
            this.dp2ResTree1.ChannelManager = Program.MainForm;

            this.dp2ResTree1.EnabledIndices = new int[] { dp2ResTree.RESTYPE_DB };
            this.dp2ResTree1.Fill(null);

            m_bDp2Initialized = true;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.CurrentUserName = "";

            if (this.tabControl_main.SelectedIndex == 0)
            {
                if (this.textBox_dtlpRecPath.Text == "")
                {
                    MessageBox.Show(this, "��δָ������·��");
                    return;
                }
            }

            else if (this.tabControl_main.SelectedIndex == 1)
            {
                if (this.textBox_dp2RecPath.Text == "")
                {
                    MessageBox.Show(this, "��δָ������·��");
                    return;
                }

                this.CurrentUserName = Program.MainForm.CurrentUserName;  // 2016/11/21 changed  // this.dp2ResTree1.CurrentUserName;
            }

            else if (this.tabControl_main.SelectedIndex == 2)
            {
                if (this.textBox_unionCatalogRecPath.Text == "")
                {
                    MessageBox.Show(this, "��δָ������·��");
                    return;
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



        public string RecPath
        {
            get
            {
                if (this.tabControl_main.SelectedIndex == 0)
                    return "dtlp:" + this.textBox_dtlpRecPath.Text;

                if (this.tabControl_main.SelectedIndex == 1)
                    return "dp2library:" + this.textBox_dp2RecPath.Text;

                if (this.tabControl_main.SelectedIndex == 2)
                    return "unioncatalog:" + this.textBox_unionCatalogRecPath.Text;

                return null;
            }
            set
            {
                string strFullPath = value;

                string strProtocol = "";
                string strPath = "";
                string strError = "";
                // ����·��
                int nRet = Global.ParsePath(strFullPath,
                    out strProtocol,
                    out strPath,
                    out strError);
                if (nRet == -1)
                {
                    // throw new Exception(strError);
                    textBox_dtlpRecPath.Text = "";
                    return;
                }

                if (strProtocol.ToLower() == "dtlp")
                {
                    InitialDtlpProtocol();

                    this.tabControl_main.SelectedIndex = 0;

                    this.dtlpResDirControl1.SelectedPath1 = strPath;
                    this.textBox_dtlpRecPath.Text = strPath;
                }

                if (strProtocol.ToLower() == "dp2library")
                {
                    InitialDp2Protocol();

                    this.tabControl_main.SelectedIndex = 1;

                    this.dp2ResTree1.ExpandPath(strPath);
                    // this.dp2ResTree1.SelectedPath = strPath;
                    this.textBox_dp2RecPath.Text = strPath;
                }


                if (strProtocol.ToLower() == "unioncatalog")
                {
                    this.tabControl_main.SelectedIndex = 2;

                    this.textBox_unionCatalogRecPath.Text = strPath;
                }
            }
        }

        private void tabControl_main_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl_main.SelectedIndex == 0)
            {
                InitialDtlpProtocol();
            }

            if (this.tabControl_main.SelectedIndex == 1)
            {
                InitialDp2Protocol();
            }
        }

#if NOOOOOOOOOOOOOOO
        public void DtlpItemSelected(string strPath, Int32 nMask)
        {
            if ((nMask & DtlpChannel.TypeStdbase) != 0)
                this.textBox_dtlpRecPath.Text = strPath;
            else
                this.textBox_dtlpRecPath.Text = "";

        }

        // �������Item���ֲ���
        public int DtlpItemText(string strPath,
            Int32 nMask,
            out string strFontFace,
            out int nFontSize,
            out FontStyle FontStyle,
            ref Color ForeColor)
        {
            strFontFace = "";
            nFontSize = 0;
            FontStyle = FontStyle.Regular;

            if ((nMask & DtlpChannel.TypeStdbase) != 0)
            {

                /*
                strFontFace = "����";
                nFontSize = 12;
                FontStyle = FontStyle.Bold;
                ForeColor = Color.Red;
                */

                return 0;
            }
            else
            {
                ForeColor = ControlPaint.LightLight(ForeColor);
                return 1;
            }

        }
#endif

        // ���selected
        private void dtlpResDirControl1_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            // ���Ե���һ��ItemSelected�¼�
            if (m_bFirst == true)
            {
                m_bFirst = false;
                return;
            }


            if ((e.Mask & DtlpChannel.TypeStdbase) != 0)
                this.textBox_dtlpRecPath.Text = e.Path;
            else
                this.textBox_dtlpRecPath.Text = "";

        }

        // �������Item���ֲ���
        private void dtlpResDirControl1_GetItemTextStyle(object sender, GetItemTextStyleEventArgs e)
        {
            e.FontFace = "";
            e.FontSize = 0;
            e.FontStyle = FontStyle.Regular;

            if ((e.Mask & DtlpChannel.TypeStdbase) != 0)
            {
                e.Result = 0;
            }
            else
            {
                e.ForeColor = ControlPaint.LightLight(ForeColor);
                e.Result = 1;
            }
        }

        private void dp2ResTree1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string strServerName = "";
            string strServerUrl = "";
            string strDbName = "";
            string strFrom = "";
            string strFromStyle = "";
            string strError = "";

            int nRet = dp2ResTree.GetNodeInfo(this.dp2ResTree1.SelectedNode,
                out strServerName,
                out strServerUrl,
                out strDbName,
                out strFrom,
                out strFromStyle,
                out strError);
            if (nRet == -1)
                goto ERROR1;

            if (String.IsNullOrEmpty(strServerName) == true
                || String.IsNullOrEmpty(strDbName) == true)
            {
                this.textBox_dp2RecPath.Text = "";
                return;
            }

            // ����ԭ��textbox������·��
            string strExistServerName = "";
            string strExistLocalPath = "";
            // ������¼·����
            // ��¼·��Ϊ������̬ "����ͼ��/1 @������"
            dp2SearchForm.ParseRecPath(this.textBox_dp2RecPath.Text,
                out strExistServerName,
                out strExistLocalPath);

            string strExistDbName = dp2SearchForm.GetDbName(strExistLocalPath);


            if (strExistServerName != strServerName
                || strExistDbName != strDbName)
            {
                this.textBox_dp2RecPath.Text = strDbName + "/?@" + strServerName;
            }
            // ���򲻶�

            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        public string ActiveProtocol
        {
            get
            {
                if (this.tabControl_main.SelectedTab == this.tabPage_dp2)
                    return "dp2library";
                if (this.tabControl_main.SelectedTab == this.tabPage_DTLP)
                    return "dtlp";

                return "";
            }
            set
            {
                if (value.ToLower() == "dp2library")
                    this.tabControl_main.SelectedTab = this.tabPage_dp2;
                if (value.ToLower() == "dtlp")
                    this.tabControl_main.SelectedTab = this.tabPage_DTLP;
            }
        }

        private void button_dp2_append_Click(object sender, EventArgs e)
        {
            string strError = "";
            // ��·���޸�Ϊ׷����̬
            // ��strPath����Ϊserver url��local path��������
            string strServerName = "";
            string strPurePath = "";
            dp2SearchForm.ParseRecPath(this.textBox_dp2RecPath.Text,
                out strServerName,
                out strPurePath);
            if (String.IsNullOrEmpty(strServerName) == true)
            {
                strError = "·�����Ϸ�: ȱ��������������";
                goto ERROR1;
            }
            if (String.IsNullOrEmpty(strPurePath) == true)
            {
                strError = "·�����Ϸ���ȱ����·������";
                goto ERROR1;
            }

            string strDbName = dp2SearchForm.GetDbName(strPurePath);

            this.textBox_dp2RecPath.Text = strDbName + "/?@" + strServerName;

            button_OK_Click(null, null);
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        private void button_dtlp_append_Click(object sender, EventArgs e)
        {
            // localhost/����ͼ��/ctlno/1
            // 3�������ϱ��������

            string strPath = this.textBox_dtlpRecPath.Text;

            string[] parts = strPath.Split(new char[] { '/' });
            if (parts.Length > 2)
                strPath = parts[0] + "/" + parts[1];

            // MessageBox.Show(this, strPath);

            this.textBox_dtlpRecPath.Text = strPath;


            button_OK_Click(null, null);
            return;
            /*
        ERROR1:
            MessageBox.Show(this, strError);
             * */

        }

        private void button_unionCatalog_append_Click(object sender, EventArgs e)
        {
            string strError = "";
            // ��·���޸�Ϊ׷����̬
            // ��strPath����Ϊserver url��local path��������
            string strServerName = "";
            string strPurePath = "";
            dp2SearchForm.ParseRecPath(this.textBox_unionCatalogRecPath.Text,
                out strServerName,
                out strPurePath);
            if (String.IsNullOrEmpty(strServerName) == true)
            {
                strError = "·�����Ϸ�: ȱ��������������";
                goto ERROR1;
            }
            if (String.IsNullOrEmpty(strPurePath) == true)
            {
                strError = "·�����Ϸ���ȱ����·������";
                goto ERROR1;
            }

            string strDbName = dp2SearchForm.GetDbName(strPurePath);

            this.textBox_unionCatalogRecPath.Text = strDbName + "/?@" + strServerName;

            button_OK_Click(null, null);
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        bool m_bSaveToDbMode = false;

        public bool SaveToDbMode
        {
            get
            {
                return this.m_bSaveToDbMode;
            }
            set
            {
                this.m_bSaveToDbMode = value;
                if (this.m_bSaveToDbMode == true)
                {
                    this.button_dp2_append.Visible = false;
                    this.button_dtlp_append.Visible = false;
                    this.textBox_dp2RecPath.ReadOnly = true;
                    this.textBox_dtlpRecPath.ReadOnly = true;
                }
                else
                {
                    this.button_dp2_append.Visible = true;
                    this.button_dtlp_append.Visible = true;
                    this.textBox_dp2RecPath.ReadOnly = false;
                    this.textBox_dtlpRecPath.ReadOnly = false;
                }
            }
        }

        private void textBox_dtlpRecPath_Validating(object sender, CancelEventArgs e)
        {
            string strError = ValidateRecPath(this.textBox_dtlpRecPath.Text);
            if (string.IsNullOrEmpty(strError) == false)
            {
                MessageBox.Show(this, strError);
                e.Cancel = true;
                return;
            }

        }

        static string ValidateRecPath(string strRecPath)
        {
            if (strRecPath.IndexOfAny(new char[] {
            '��',
            '��',
            '��',
            '��',
            '��',
            '��',
            '��',
            '��',
            '��',
            '��',
            '��',
            '��',
            '��'}) != -1)
                return "��¼·���к��зǷ��ַ���ע�� ?1234567890/@ �������ַ���Ӧ��ʹ�ð��(����)�ַ�";
            return null;
        }

        private void textBox_dp2RecPath_Validating(object sender, CancelEventArgs e)
        {
            string strError = ValidateRecPath(this.textBox_dp2RecPath.Text);
            if (string.IsNullOrEmpty(strError) == false)
            {
                MessageBox.Show(this, strError);
                e.Cancel = true;
                return;
            }
        }

        private void textBox_unionCatalogRecPath_Validating(object sender, CancelEventArgs e)
        {
            string strError = ValidateRecPath(this.textBox_unionCatalogRecPath.Text);
            if (string.IsNullOrEmpty(strError) == false)
            {
                MessageBox.Show(this, strError);
                e.Cancel = true;
                return;
            }
        }

        public string UiState
        {
            get
            {
                List<object> controls = new List<object>();
                controls.Add(this.tabControl_main);
                return GuiState.GetUiState(controls);
            }
            set
            {
                List<object> controls = new List<object>();
                controls.Add(this.tabControl_main);
                GuiState.SetUiState(controls, value);
            }
        }

    }


    public delegate void GetDtlpSearchParamEventHandle(object sender,
    GetDtlpSearchParamEventArgs e);

    public class GetDtlpSearchParamEventArgs : EventArgs
    {
        public DtlpChannelArray DtlpChannels = null;
        public DtlpChannel DtlpChannel = null;	// 
    }

    //
    public delegate void GetDp2SearchParamEventHandle(object sender,
        GetDp2SearchParamEventArgs e);

    public class GetDp2SearchParamEventArgs : EventArgs
    {
        public MainForm MainForm = null;
        // public LibraryChannelCollection dp2Channels = null;
    }
}