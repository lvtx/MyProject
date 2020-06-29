using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using DigitalPlatform;
using DigitalPlatform.LibraryClient;
using DigitalPlatform.Text;
using DigitalPlatform.Xml;

namespace dp2ZServer.Install
{
    public partial class InstallZServerDlg : Form
    {
        public InstallZServerDlg()
        {
            InitializeComponent();
        }

        private void button_detectManageUser_Click(object sender, EventArgs e)
        {
            EnableControls(false);

            try
            {
                string strError = "";

                if (this.LibraryWsUrl == "")
                {
                    MessageBox.Show(this, "��δ���� dp2Library �������� URL");
                    return;
                }

                if (this.UserName == "")
                {
                    MessageBox.Show(this, "��δָ�� dp2Library �����û�����");
                    return;
                }

                /*
                if (this.textBox_managePassword.Text != this.textBox_confirmManagePassword.Text)
                {
                    strError = "dp2Library �����û� ���� �� �ٴ��������� ��һ�¡����������롣";
                    MessageBox.Show(this, strError);
                    return;
                }*/

                // ����ʻ���¼�Ƿ�ɹ�?


                // ���е�¼
                // return:
                //      -1  error
                //      0   ��¼δ�ɹ�
                //      1   ��¼�ɹ�
                int nRet = DoLogin(
                    this.comboBox_librarywsUrl.Text,
                    this.textBox_manageUserName.Text,
                    this.textBox_managePassword.Text,
                    out strError);
                if (nRet == -1)
                {
                    MessageBox.Show(this, "��� dp2library �ʻ�ʱ��������: " + strError);
                    return;
                }
                if (nRet == 0)
                {
                    MessageBox.Show(this, "��ָ���� dp2library �ʻ� ����ȷ: " + strError);
                    return;
                }


                MessageBox.Show(this, "��ָ���� dp2library �ʻ� ��ȷ");
            }
            finally
            {
                EnableControls(true);
            }
        }

        int VerifyXml(out string strError)
        {
            strError = "";

            if (string.IsNullOrEmpty(this.textBox_databaseDef.Text) == false)
            {
                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.LoadXml(this.textBox_databaseDef.Text);
                }
                catch (Exception ex)
                {
                    this.tabControl_main.SelectedTab = this.tabPage_database;
                    strError = "���ݿⶨ�� XML ���ڸ�ʽ����: " + ex.Message;
                    return -1;
                }
            }

            return 0;
        }

        // ��ס Control ������Խ����� dp2library server �Ĳ���
        private void button_OK_Click(object sender, EventArgs e)
        {
            string strError = "";

            bool bControl = Control.ModifierKeys == Keys.Control;

            EnableControls(false);
            try
            {
                if (string.IsNullOrEmpty(this.LibraryWsUrl))
                {
                    strError = "��δ���� dp2Library �������� URL";
                    goto ERROR1;
                }

                if (string.IsNullOrEmpty(this.UserName))
                {
                    strError = "��δָ�� dp2Library �����û�����";
                    goto ERROR1;
                }

                if (this.textBox_anonymousUserName.Text == ""
                    && this.textBox_anonymousPassword.Text != "")
                {
                    strError = "��δָ��������¼�û���������£�������ָ��������¼���롣";
                    goto ERROR1;
                }

                if (VerifyXml(out strError) == -1)
                    goto ERROR1;

                /*
                if (this.textBox_managePassword.Text != this.textBox_confirmManagePassword.Text)
                {
                    strError = "dp2Library �����û� ���� �� �ٴ��������� ��һ�¡����������롣";
                    MessageBox.Show(this, strError);
                    return;
                }*/

                // ����ʻ���¼�Ƿ�ɹ�?
                if (bControl == false)
                {
                    // ���е�¼
                    // return:
                    //      -1  error
                    //      0   ��¼δ�ɹ�
                    //      1   ��¼�ɹ�
                    int nRet = DoLogin(
                        this.comboBox_librarywsUrl.Text,
                        this.textBox_manageUserName.Text,
                        this.textBox_managePassword.Text,
                        out strError);
                    if (nRet == -1)
                    {
                        strError = "��� dp2library �ʻ�ʱ��������: " + strError;
                        goto ERROR1;
                    }
                    if (nRet == 0)
                    {
                        strError = "��ָ���� dp2library �ʻ� ����ȷ: " + strError;
                        goto ERROR1;
                    }
                }
            }
            finally
            {
                EnableControls(true);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // ���е�¼
        // return:
        //      -1  error
        //      0   ��¼δ�ɹ�
        //      1   ��¼�ɹ�
        static int DoLogin(
            string strLibraryWsUrl,
            string strUserName,
            string strPassword,
            out string strError)
        {
            strError = "";

            using (LibraryChannel Channel = new LibraryChannel())
            {

                Channel.Url = strLibraryWsUrl;

                // return:
                //      -1  error
                //      0   ��¼δ�ɹ�
                //      1   ��¼�ɹ�
                long lRet = Channel.Login(strUserName,
                    strPassword,
                    "location=z39.50 server,type=worker,client=dp2ZServer|0.01",
                    /*
                    "z39.50 server",    // string strLocation,
                    false,  // bReader,
                     * */
                    out strError);
                if (lRet == -1)
                    return -1;

                return (int)lRet;
            }
        }

        void EnableControls(bool bEnable)
        {
            // this.textBox_confirmManagePassword.Enabled = bEnable;
            this.textBox_managePassword.Enabled = bEnable;
            this.textBox_manageUserName.Enabled = bEnable;
            this.comboBox_librarywsUrl.Enabled = bEnable;

            this.button_OK.Enabled = bEnable;
            this.button_Cancel.Enabled = bEnable;
            this.button_detectManageUser.Enabled = bEnable;

            this.textBox_anonymousUserName.Enabled = bEnable;
            this.textBox_anonymousPassword.Enabled = bEnable;
            this.button_detectAnonymousUser.Enabled = bEnable;

            this.textBox_databaseDef.Enabled = bEnable;
            this.button_import_databaseDef.Enabled = bEnable;

            this.numericUpDown_z3950_port.Enabled = bEnable;
            this.textBox_z3950_maxResultCount.Enabled = bEnable;
            this.textBox_z3950_maxSessions.Enabled = bEnable;

            this.Update();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public string LibraryWsUrl
        {
            get
            {
                return this.comboBox_librarywsUrl.Text;
            }
            set
            {
                this.comboBox_librarywsUrl.Text = value;
            }
        }

        public string UserName
        {
            get
            {
                return this.textBox_manageUserName.Text;
            }
            set
            {
                this.textBox_manageUserName.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return this.textBox_managePassword.Text;
            }
            set
            {
                this.textBox_managePassword.Text = value;
                // this.textBox_confirmManagePassword.Text = value;
            }
        }

        public string AnonymousUserName
        {
            get
            {
                return this.textBox_anonymousUserName.Text;
            }
            set
            {
                this.textBox_anonymousUserName.Text = value;
            }
        }

        public string AnonymousPassword
        {
            get
            {
                return this.textBox_anonymousPassword.Text;
            }
            set
            {
                this.textBox_anonymousPassword.Text = value;
            }
        }

        private void button_detectAnonymousUser_Click(object sender, EventArgs e)
        {
            EnableControls(false);

            try
            {
                string strError = "";

                if (this.LibraryWsUrl == "")
                {
                    MessageBox.Show(this, "��δ���� dp2Library �������� URL");
                    return;
                }

                if (this.AnonymousUserName == "")
                {
                    MessageBox.Show(this, "��δָ�� ������¼�û�����");
                    return;
                }


                // ����ʻ���¼�Ƿ�ɹ�?


                // ���е�¼
                // return:
                //      -1  error
                //      0   ��¼δ�ɹ�
                //      1   ��¼�ɹ�
                int nRet = DoLogin(
                    this.comboBox_librarywsUrl.Text,
                    this.textBox_anonymousUserName.Text,
                    this.textBox_anonymousPassword.Text,
                    out strError);
                if (nRet == -1)
                {
                    MessageBox.Show(this, "��� ������¼ �û�ʱ��������: " + strError);
                    return;
                }
                if (nRet == 0)
                {
                    MessageBox.Show(this, "��ָ���� ������¼ �û� ����ȷ: " + strError);
                    return;
                }

                MessageBox.Show(this, "��ָ���� ������¼ �û� ��ȷ");
            }
            finally
            {
                EnableControls(true);
            }
        }

        private void button_import_databaseDef_Click(object sender, EventArgs e)
        {
            string strError = "";
            string strXml = "";

            this.EnableControls(false);
            try
            {
                if (string.IsNullOrEmpty(this.comboBox_librarywsUrl.Text))
                {
                    strError = "��δ���� dp2Library �������� URL";
                    goto ERROR1;
                }

                if (string.IsNullOrEmpty(this.textBox_manageUserName.Text))
                {
                    strError = "��δָ�� dp2Library �����û���";
                    goto ERROR1;
                }

                // this.textBox_databaseDef.Text = "";

                int nRet = GetDatabaseDef(
        this.comboBox_librarywsUrl.Text,
        this.textBox_manageUserName.Text,
        this.textBox_managePassword.Text,
        out strXml,
        out strError);
                if (nRet == -1)
                    goto ERROR1;

                string strOutputXml = "";
                nRet = BuildZDatabaseDef(strXml,
                    out strOutputXml,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;

                if (nRet == 0)
                {
                    this.DatabasesXml = "";
                    MessageBox.Show(this, "dp2library ����δ���� OPAC �������ݿ⣬��û��Ϊ�κ����ݿⶨ�������������������ϵͳ������OPAC������ҳ�������ã���ʹ�ñ�����");
                }
                else
                    this.DatabasesXml = strOutputXml;
                return;
            }
            finally
            {
                this.EnableControls(true);
            }
        ERROR1:
            MessageBox.Show(this, strError);
        }

        // ��� dp2library <virtualDatabases> ���ݿⶨ��
        // return:
        //      -1  error
        //      0   �ɹ�
        static int GetDatabaseDef(
            string strLibraryWsUrl,
            string strUserName,
            string strPassword,
            out string strXml,
            out string strError)
        {
            strError = "";
            strXml = "";

            using (LibraryChannel Channel = new LibraryChannel())
            {

                Channel.Url = strLibraryWsUrl;

                // return:
                //      -1  error
                //      0   ��¼δ�ɹ�
                //      1   ��¼�ɹ�
                long lRet = Channel.Login(strUserName,
                    strPassword,
                    "location=z39.50 server,type=worker,client=dp2ZServer|0.01",
                    out strError);
                if (lRet == -1)
                    return -1;

                if (lRet == 0)
                {
                    strError = "��¼δ�ɹ�:" + strError;
                    return -1;
                }

                lRet = Channel.GetSystemParameter(
    null,
    "opac",
    "databases",
    out strXml,
    out strError);
                if (lRet == -1)
                    return -1;

                return 0;
            }
        }

        /*
  <databases>
    <database name="����ͼ��" alias="cbook">
      <use value="4" from="����" />
      <use value="7" from="ISBN" />
      <use value="8" from="ISSN" />
      <use value="21" from="�����" />
      <use value="1003" from="������" />
    </database>
    <database name="Ӣ��ͼ��" alias="ebook">
      <use value="4" from="����" />
      <use value="7" from="ISBN" />
      <use value="8" from="ISSN" />
      <use value="21" from="�����" />
      <use value="1003" from="������" />
    </database>
  </databases>
         * */
        // ���� library.xml �е� <virtualDatabases> Ԫ�ع��� dp2zserver.xml �е� <databases>
        // return:
        //      -1  ����
        //      0   strXml ��û�з������������Ϣ
        //      1   ����ɹ�
        int BuildZDatabaseDef(string strXml,
            out string strOutputXml,
            out string strError)
        {
            strError = "";
            strOutputXml = "";

            XmlDocument source_dom = new XmlDocument();
            source_dom.LoadXml("<root />");
            try
            {
                source_dom.DocumentElement.InnerXml = strXml;
            }
            catch (Exception ex)
            {
                strError = "����� XML �ַ�����ʽ����: " + ex.Message;
                return -1;
            }

            XmlDocument target_dom = new XmlDocument();
            target_dom.LoadXml("<databases />");

            int createCount = 0;
            XmlNodeList databases = source_dom.DocumentElement.SelectNodes("database");
            foreach (XmlElement database in databases)
            {
                string name = database.GetAttribute("name");
                string alias = database.GetAttribute("alias");

                // û�б��������ݿⲻ������ Z39.50 ������
                if (string.IsNullOrEmpty(alias))
                    continue;

                XmlElement new_database = target_dom.CreateElement("database");
                target_dom.DocumentElement.AppendChild(new_database);
                new_database.SetAttribute("name", name);
                new_database.SetAttribute("alias", alias);

                createCount++;

                // ���� use
                string from_name = FindFromByStyle(database, "title");
                if (string.IsNullOrEmpty(from_name) == false)
                    CreateUseElement(new_database, "4", from_name);

                from_name = FindFromByStyle(database, "isbn");
                if (string.IsNullOrEmpty(from_name) == false)
                    CreateUseElement(new_database, "7", from_name);

                from_name = FindFromByStyle(database, "issn");
                if (string.IsNullOrEmpty(from_name) == false)
                    CreateUseElement(new_database, "8", from_name);

                from_name = FindFromByStyle(database, "subject");
                if (string.IsNullOrEmpty(from_name) == false)
                    CreateUseElement(new_database, "21", from_name);

                from_name = FindFromByStyle(database, "contributor");
                if (string.IsNullOrEmpty(from_name) == false)
                    CreateUseElement(new_database, "1003", from_name);
            }

            if (createCount == 0)
                return 0;

            strOutputXml = target_dom.DocumentElement.OuterXml;
            return 1;
        }

        /*
      <use value="4" from="����" />
         * */
        static void CreateUseElement(XmlElement database, string number, string from)
        {
            XmlElement element = database.OwnerDocument.CreateElement("use");
            database.AppendChild(element);
            element.SetAttribute("value", number);
            element.SetAttribute("from", from);
        }

        /*
        <database name="����ͼ��" alias="cbook">
            <caption lang="zh">����ͼ��</caption>
            <from name="ISBN" style="isbn">
                <caption lang="zh-CN">ISBN</caption>
                <caption lang="en">ISBN</caption>
            </from>
            <from name="ISSN" style="issn">
                <caption lang="zh-CN">ISSN</caption>
                <caption lang="en">ISSN</caption>
            </from>
            <from name="����" style="title">
                <caption lang="zh-CN">����</caption>
                <caption lang="en">Title</caption>
            </from>
            <from name="����ƴ��" style="pinyin_title">
                <caption lang="zh-CN">����ƴ��</caption>
                <caption lang="en">Title pinyin</caption>
            </from>
            <from name="�����" style="subject">
                <caption lang="zh-CN">�����</caption>
                <caption lang="en">Thesaurus</caption>
            </from>
            <from name="��ͼ�������" style="clc,__class">
                <caption lang="zh-CN">��ͼ�������</caption>
                <caption lang="en">CLC Class number</caption>
            </from>
            <from name="������" style="contributor">
                <caption lang="zh-CN">������</caption>
                <caption lang="en">Contributor</caption>
            </from>
            <from name="������ƴ��" style="pinyin_contributor">
                <caption lang="zh-CN">������ƴ��</caption>
                <caption lang="en">Contributor pinyin</caption>
            </from>
            <from name="���淢����" style="publisher">
                <caption lang="zh-CN">���淢����</caption>
                <caption lang="en">Publisher</caption>
            </from>
            <from name="����ʱ��" style="publishtime,_time,_freetime">
                <caption lang="zh-CN">����ʱ��</caption>
                <caption lang="en">Publish Time</caption>
            </from>
            <from name="���κ�" style="batchno">
                <caption lang="zh-CN">���κ�</caption>
                <caption lang="en">Batch number</caption>
            </from>
            <from name="Ŀ���¼·��" style="targetrecpath">
                <caption lang="zh-CN">Ŀ���¼·��</caption>
                <caption lang="en">Target Record Path</caption>
            </from>
            <from name="״̬" style="state">
                <caption lang="zh-CN">״̬</caption>
                <caption lang="en">State</caption>
            </from>
            <from name="����ʱ��" style="opertime,_time,_utime">
                <caption lang="zh-CN">����ʱ��</caption>
                <caption lang="en">OperTime</caption>
            </from>
            <from name="������ʶ��" style="identifier">
                <caption lang="zh-CN">������ʶ��</caption>
                <caption lang="en">Identifier</caption>
            </from>
            <from name="__id" style="recid" />
        </database>
         * */
        static string FindFromByStyle(XmlElement database, string strStyle)
        {
            XmlNodeList froms = database.SelectNodes("from");
            foreach (XmlElement from in froms)
            {
                string name = from.GetAttribute("name");
                string style = from.GetAttribute("style");

                if (string.IsNullOrEmpty(style))
                    continue;

                if (StringUtil.IsInList(strStyle, style) == true)
                    return name;
            }

            return null;    // not found
        }

        // <databases> Ԫ�� OuterXml
        public string DatabasesXml
        {
            get
            {
                return this.textBox_databaseDef.Text;
            }
            set
            {
                this.textBox_databaseDef.Text = DomUtil.GetIndentXml(value);
            }
        }

        public int Port
        {
            get
            {
                return Convert.ToInt32(this.numericUpDown_z3950_port.Value);
            }
            set
            {
                this.numericUpDown_z3950_port.Value = value;
            }
        }

        public string MaxSessions
        {
            get
            {
                return this.textBox_z3950_maxSessions.Text;
            }
            set
            {
                this.textBox_z3950_maxSessions.Text = value;
            }
        }

        public string MaxResultCount
        {
            get
            {
                return this.textBox_z3950_maxResultCount.Text;
            }
            set
            {
                this.textBox_z3950_maxResultCount.Text = value;
            }
        }
    }
}