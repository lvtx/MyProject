using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.IO;

using DigitalPlatform;
using DigitalPlatform.GUI;
using DigitalPlatform.Install;
using DigitalPlatform.rms.Client;
using DigitalPlatform.Xml;
using DigitalPlatform.Text;
using DigitalPlatform.IO;
using DigitalPlatform.rms.Client.rmsws_localhost;

namespace dp2Library
{
    public partial class InstallLibraryParamDlg : Form
    {
        string ManagerUserName = "root";
        string ManagerPassword = "";
        bool SavePassword = true;

        // public string RootDir = "";

        // bool bBbsUserNameTouched = false;
        // int nNest = 0;  // �Ƿ����޸����ֵ�Ƕ���¼���


        public InstallLibraryParamDlg()
        {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            this.Update();

            try
            {
                string strError = "";

                if (this.KernelUrl == "")
                {
                    MessageBox.Show(this, "��δ����dp2Kernel��������URL");
                    return;
                }

                // ��֤asmx�Ƿ���ڣ�

                if (this.ManageUserName == "")
                {
                    MessageBox.Show(this, "��δָ�������ʻ��û�����");
                    return;
                }

                if (this.textBox_managePassword.Text != this.textBox_confirmManagePassword.Text)
                {
                    strError = "�����ʻ� ���� �� �ٴ��������� ��һ�¡����������롣";
                    MessageBox.Show(this, strError);
                    return;
                }



                // ��֤�����ʻ��û��Ƿ��Ѿ����ã�
                // return:
                //       -1  ����
                //      0   ������
                //      1   ����, ������һ��
                //      2   ����, �����벻һ��
                int nRet = DetectManageUser(out strError);
                if (nRet == -1)
                {
                    MessageBox.Show(this, "��֤�����ʻ�ʱ��������: " + strError + "\r\n\r\n��ȷ�������ʻ��Ѿ���ȷ����");
                    return;
                }

                if (nRet == 2)
                {
                    string strText = "�����ʻ��Ѿ�����, ����������͵�ǰ����������õ����벻һ�¡�\r\n\r\n�Ƿ�Ҫ����������?\r\n\r\n(Yes �������벢������װ��No ���������벢������װ��Cancel �����������)";
                    DialogResult result = MessageBox.Show(this,
                        strText,
                        "setup_dp2library",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {
                        nRet = ResetManageUserPassword(out strError);
                        if (nRet == -1)
                        {
                            MessageBox.Show(this, "��������ʻ�����ʱ����: " + strError + "\r\n\r\n��ȷ�������ʻ��Ѿ���ȷ����");
                            return;
                        }
                    }

                    if (result == DialogResult.Cancel)
                        return; // �������
                }

                // root�û�������
                else if (nRet == 0)
                {
                    // �Զ�����?
                    string strText = "�����ʻ� '" + this.textBox_manageUserName.Text + "' ��δ����, �Ƿ񴴽�֮?\r\n\r\n(OK ������Cancel �������������������)";
                    DialogResult result = MessageBox.Show(this,
                        strText,
                        "setup_dp2library",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Cancel)
                        return;

                    nRet = CreateManageUser(out strError);
                    if (nRet == -1)
                    {
                        MessageBox.Show(this, strError);
                        return;
                    }
                }


            }
            finally
            {
                EnableControls(true);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public string KernelUrl
        {
            get
            {
                return this.textBox_kernelUrl.Text;
            }
            set
            {
                this.textBox_kernelUrl.Text = value;
            }
        }

        public string ManageUserName
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

        public string ManagePassword
        {
            get
            {
                return this.textBox_managePassword.Text;
            }
            set
            {
                this.textBox_managePassword.Text = value;
                this.textBox_confirmManagePassword.Text = value;
            }
        }

        // �������û��Ƿ��Ѿ�����?
        // return:
        //       -1  ����
        //      0   ������
        //      1   ����, ������һ��
        //      2   ����, �����벻һ��
        int DetectManageUser(out string strError)
        {
            strError = "";
            if (this.textBox_kernelUrl.Text == "")
            {
                strError = "��δָ��dp2Kernel������URL";
                return -1;
            }

            if (this.textBox_manageUserName.Text == "")
            {
                strError = "��δָ�������ʻ����û���";
                return -1;
            }

            if (this.textBox_managePassword.Text != this.textBox_confirmManagePassword.Text)
            {
                strError = "�����ʻ� ���� �� �ٴ��������� ��һ�¡����������롣";
                return -1;
            }

            RmsChannelCollection channels = new RmsChannelCollection();

            RmsChannel channel = channels.GetChannel(this.textBox_kernelUrl.Text);
            if (channel == null)
            {
                strError = "channel == null";
                return -1;
            }

            // Debug.Assert(false, "");

            int nRet = channel.Login(this.textBox_manageUserName.Text,
                this.textBox_managePassword.Text,
                out strError);
            if (nRet == -1)
            {
                strError = "���û��� '" + this.textBox_manageUserName.Text + "' �������¼ʧ��: " + strError;
                return -1;
            }

            channel.DoLogout(out strError);

            if (nRet == 0)
            {
                channels.AskAccountInfo -= new AskAccountInfoEventHandle(channels_AskAccountInfo);
                channels.AskAccountInfo += new AskAccountInfoEventHandle(channels_AskAccountInfo);

                nRet = channel.UiLogin("Ϊȷ�ϴ����ʻ��Ƿ����, ����root�û���ݵ�¼��",
                    "",
                    LoginStyle.None,
                    out strError);
                if (nRet == -1 || nRet == 0)
                {
                    strError = "��root�û���ݵ�¼ʧ��: " + strError + "\r\n\r\n����޷�ȷ�������ʻ��Ƿ����";
                    return -1;
                }

                string strRecPath = "";
                string strXml = "";
                byte[] baTimeStamp = null;


                // ����û���¼
                // return:
                //      -1  error
                //      0   not found
                //      >=1   �������е�����
                nRet = GetUserRecord(
                    channel,
                    this.textBox_manageUserName.Text,
                    out strRecPath,
                    out strXml,
                    out baTimeStamp,
                    out strError);
                if (nRet == -1)
                {
                    strError = "��ȡ�û� '" + this.textBox_manageUserName.Text + "' ��Ϣʱ��������: " + strError + "\r\n\r\n����޷�ȷ�������ʻ��Ƿ���ڡ�";
                    return -1;
                }

                if (nRet == 1)
                {
                    strError = "�����ʻ� '" + this.textBox_manageUserName.Text + "' �Ѿ�����, ��������͵�ǰ��������õ����벻һ�¡�";
                    return 2;
                }
                if (nRet >= 1)
                {
                    strError = "�� '" + this.textBox_manageUserName.Text + "' Ϊ�û��� ���û���¼���ڶ���������һ�����ش���������root�������dp2manager���������˴���";
                    return -1;
                }

                strError = "�����ʻ� '" + this.textBox_manageUserName.Text + "' �����ڡ�";
                return 0;
            }

            strError = "�����ʻ� '" + this.textBox_manageUserName.Text + "' �����ʻ���������ڡ�";
            return 1;
        }


        // ����û���¼
        // return:
        //      -1  error
        //      0   not found
        //      >=1   �������е�����
        public static int GetUserRecord(
            RmsChannel channel,
            string strUserName,
            out string strRecPath,
            out string strXml,
            out byte[] baTimeStamp,
            out string strError)
        {
            strError = "";

            strXml = "";
            strRecPath = "";
            baTimeStamp = null;

            if (strUserName == "")
            {
                strError = "�û���Ϊ��";
                return -1;
            }

            string strQueryXml = "<target list='" + Defs.DefaultUserDb.Name
                + ":" + Defs.DefaultUserDb.SearchPath.UserName + "'><item><word>"
                + strUserName + "</word><match>exact</match><relation>=</relation><dataType>string</dataType><maxCount>10</maxCount></item><lang>chi</lang></target>";

            long nRet = channel.DoSearch(strQueryXml,
                "default",
                "", // strOutputStyle
                out strError);
            if (nRet == -1)
            {
                strError = "�����ʻ���ʱ����: " + strError;
                return -1;
            }

            if (nRet == 0)
                return 0;	// not found

            long nSearchCount = nRet;

            List<string> aPath = null;
            nRet = channel.DoGetSearchResult(
                "default",
                1,
                "zh",
                null,	// stop,
                out aPath,
                out strError);
            if (nRet == -1)
            {
                strError = "����ע���û����ȡ�������ʱ����: " + strError;
                return -1;
            }
            if (aPath.Count == 0)
            {
                strError = "����ע���û����ȡ�ļ������Ϊ��";
                return -1;
            }

            // strRecID = ResPath.GetRecordId((string)aPath[0]);
            strRecPath = (string)aPath[0];

            string strStyle = "content,data,timestamp,withresmetadata";
            string strMetaData;
            string strOutputPath;

            nRet = channel.GetRes((string)aPath[0],
                strStyle,
                out strXml,
                out strMetaData,
                out baTimeStamp,
                out strOutputPath,
                out strError);
            if (nRet == -1)
            {
                strError = "��ȡע���û����¼��ʱ����: " + strError;
                return -1;
            }


            return (int)nSearchCount;
        }

        // ���������ʻ�
        int CreateManageUser(out string strError)
        {
            strError = "";
            if (this.textBox_kernelUrl.Text == "")
            {
                strError = "��δָ��dp2Kernel������URL";
                return -1;
            }

            if (this.textBox_manageUserName.Text == "")
            {
                strError = "��δָ�������ʻ����û���";
                return -1;
            }

            if (this.textBox_managePassword.Text != this.textBox_confirmManagePassword.Text)
            {
                strError = "������ ���� �� �ٴ��������� ��һ�¡����������롣";
                return -1;
            }

            RmsChannelCollection channels = new RmsChannelCollection();

            channels.AskAccountInfo -= new AskAccountInfoEventHandle(channels_AskAccountInfo);
            channels.AskAccountInfo += new AskAccountInfoEventHandle(channels_AskAccountInfo);



            RmsChannel channel = channels.GetChannel(this.textBox_kernelUrl.Text);
            if (channel == null)
            {
                strError = "channel == null";
                return -1;
            }

            int nRet = channel.UiLogin("����root�û���ݵ�¼���Ա㴴�������ʻ���",
                "",
                LoginStyle.None,
                out strError);
            if (nRet == -1 || nRet == 0)
            {
                strError = "��root�û���ݵ�¼ʧ��: " + strError;
                return -1;
            }

            // ����û�����


            string strRecPath = "";
            string strXml = "";
            byte[] baTimeStamp = null;


            // ���أ�������û����Ƿ��Ѿ�����
            // ����û���¼
            // return:
            //      -1  error
            //      0   not found
            //      >=1   �������е�����
            nRet = GetUserRecord(
                channel,
                this.textBox_manageUserName.Text,
                out strRecPath,
                out strXml,
                out baTimeStamp,
                out strError);
            if (nRet == -1)
            {
                strError = "��ȡ�û� '" + this.textBox_manageUserName.Text + "' ��Ϣʱ��������: " + strError;
                return -1;
            }

            if (nRet == 1)
            {
                strError = "�û� '" + this.textBox_manageUserName.Text + "' �Ѿ����ڡ�";
                return -1;
            }
            if (nRet >= 1)
            {
                strError = "�� '" + this.textBox_manageUserName.Text + "' Ϊ�û��� ���û���¼���ڶ���������һ�����ش���������root�������dp2manager���������˴���";
                return -1;
            }

            // ����һ���û���¼д��
            nRet = BuildUserRecord(out strXml,
                out strError);
            if (nRet == -1)
            {
                strError = "�����û���¼ʱ��������: " + strError;
                return -1;
            }

            string strOutputPath = "";
            byte[] baOutputTimeStamp;

            if (strRecPath == "")
                strRecPath = Defs.DefaultUserDb.Name + "/" + "?";

            long lRet = channel.DoSaveTextRes(
                strRecPath,
                strXml,
                false,	// bInlucdePreamble
                "",	// style
                baTimeStamp,	// baTimeStamp,
                out baOutputTimeStamp,
                out strOutputPath,
                out strError);
            if (lRet == -1)
            {
                strError = "�����û���¼ʱ��������: " + strError;
                return -1;
            }

            channel.DoLogout(out strError);

            return 0;
        }

        // �����ô����ʻ�����
        int ResetManageUserPassword(out string strError)
        {
            strError = "";
            if (this.textBox_kernelUrl.Text == "")
            {
                strError = "��δָ��dp2Kernel������URL";
                return -1;
            }

            if (this.textBox_manageUserName.Text == "")
            {
                strError = "��δָ�������ʻ����û���";
                return -1;
            }

            if (this.textBox_managePassword.Text != this.textBox_confirmManagePassword.Text)
            {
                strError = "�����ʻ� ���� �� �ٴ��������� ��һ�¡����������롣";
                return -1;
            }

            RmsChannelCollection channels = new RmsChannelCollection();

            channels.AskAccountInfo -= new AskAccountInfoEventHandle(channels_AskAccountInfo);
            channels.AskAccountInfo += new AskAccountInfoEventHandle(channels_AskAccountInfo);

            RmsChannel channel = channels.GetChannel(this.textBox_kernelUrl.Text);
            if (channel == null)
            {
                strError = "channel == null";
                return -1;
            }

            int nRet = channel.UiLogin("����root�û���ݵ�¼���Ա���������ʻ����롣",
                "",
                LoginStyle.None,
                out strError);
            if (nRet == -1 || nRet == 0)
            {
                strError = "��root�û���ݵ�¼ʧ��: " + strError;
                return -1;
            }

            // ����û�����


            string strRecPath = "";
            string strXml = "";
            byte[] baTimeStamp = null;


            // ���أ�������û����Ƿ��Ѿ�����
            // ����û���¼
            // return:
            //      -1  error
            //      0   not found
            //      >=1   �������е�����
            nRet = GetUserRecord(
                channel,
                this.textBox_manageUserName.Text,
                out strRecPath,
                out strXml,
                out baTimeStamp,
                out strError);
            if (nRet == -1)
            {
                strError = "��ȡ�û� '" + this.textBox_manageUserName.Text + "' ��Ϣʱ��������: " + strError;
                return -1;
            }

            if (nRet == 0)
            {
                strError = "�û� '" + this.textBox_manageUserName.Text + "' �в����ڣ�����޷����������롣��ֱ�Ӵ�����";
                return -1;
            }

            if (nRet > 1)
            {
                strError = "�� '" + this.textBox_manageUserName.Text + "' Ϊ�û��� ���û���¼���ڶ���������һ�����ش���������root�������dp2manager���������˴���";
                return -1;
            }

            // �޸�����
            nRet = ResetUserRecordPassword(ref strXml,
                out strError);
            if (nRet == -1)
            {
                strError = "�����û���¼ʱ��������: " + strError;
                return -1;
            }

            string strOutputPath = "";
            byte[] baOutputTimeStamp;

            if (strRecPath == "")
            {
                Debug.Assert(false, "�����ܳ��ֵ������");
                strRecPath = Defs.DefaultUserDb.Name + "/" + "?";
            }

            long lRet = channel.DoSaveTextRes(
                strRecPath,
                strXml,
                false,	// bInlucdePreamble
                "",	// style
                baTimeStamp,	// baTimeStamp,
                out baOutputTimeStamp,
                out strOutputPath,
                out strError);
            if (lRet == -1)
            {
                strError = "�����û���¼ʱ��������: " + strError;
                return -1;
            }

            channel.DoLogout(out strError);

            return 0;
        }

        int ResetUserRecordPassword(ref string strXml,
    out string strError)
        {
            strError = "";

            XmlDocument UserRecDom = new XmlDocument();
            try
            {
                UserRecDom.LoadXml(strXml);
            }
            catch (Exception ex)
            {
                strError = "�û���¼XMLװ�ص�DOMʱ����: " + ex.Message;
                return -1;
            }


            // ����
            DomUtil.SetElementText(UserRecDom.DocumentElement,
               "password",
                Cryptography.GetSHA1(this.textBox_managePassword.Text));

            /*
            XmlNode nodeServer = UserRecDom.DocumentElement.SelectSingleNode("server");
            if (nodeServer == null)
            {
                Debug.Assert(false, "�����ܵ����");
                return -1;
            }

            DomUtil.SetAttr(nodeServer, "rights", "children_database:create,list");
             */

            strXml = UserRecDom.OuterXml;

            return 0;
        }

        void channels_AskAccountInfo(object sender, AskAccountInfoEventArgs e)
        {
            e.Owner = this;

            LoginDlg dlg = new LoginDlg();
            GuiUtil.AutoSetDefaultFont(dlg);

            dlg.textBox_serverAddr.Text = this.textBox_kernelUrl.Text;
            dlg.textBox_serverAddr.ReadOnly = true;
            dlg.textBox_comment.Text = e.Comment;
            dlg.textBox_userName.Text = this.ManagerUserName;
            dlg.textBox_password.Text = this.ManagerPassword;
            dlg.checkBox_savePassword.Checked = this.SavePassword;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
            {
                e.Result = 0;
                return;
            }

            this.ManagerPassword = dlg.textBox_userName.Text;

            if (dlg.checkBox_savePassword.Checked == true)
                this.ManagerPassword = dlg.textBox_password.Text;
            else
                this.ManagerPassword = "";

            e.UserName = dlg.textBox_userName.Text;
            e.Password = dlg.textBox_password.Text;

            e.Result = 1;
        }

        int BuildUserRecord(out string strXml,
    out string strError)
        {
            strXml = "";
            strError = "";

            XmlDocument UserRecDom = new XmlDocument();
            UserRecDom.LoadXml("<record><name /><password /><server /></record>");


            // �����û���
            DomUtil.SetElementText(UserRecDom.DocumentElement,
                "name",
                this.textBox_manageUserName.Text);


            // ����
            DomUtil.SetElementText(UserRecDom.DocumentElement,
               "password",
                Cryptography.GetSHA1(this.textBox_managePassword.Text));

            XmlNode nodeServer = UserRecDom.DocumentElement.SelectSingleNode("server");
            if (nodeServer == null)
            {
                Debug.Assert(false, "�����ܵ����");
                return -1;
            }

            DomUtil.SetAttr(nodeServer, "rights", "children_database:create,list");

            strXml = UserRecDom.OuterXml;

            return 0;
        }

        void EnableControls(bool bEnable)
        {
            this.textBox_confirmManagePassword.Enabled = bEnable;
            this.textBox_managePassword.Enabled = bEnable;
            this.textBox_manageUserName.Enabled = bEnable;
            this.textBox_kernelUrl.Enabled = bEnable;

            this.button_OK.Enabled = bEnable;
            this.button_Cancel.Enabled = bEnable;
            this.button_createManageUser.Enabled = bEnable;
            this.button_detectManageUser.Enabled = bEnable;
            this.button_resetManageUserPassword.Enabled = bEnable;

        }

        // �������ʻ��Ƿ���ڣ���¼�Ƿ���ȷ
        private void button_detectManageUser_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            try
            {
                // Debug.Assert(false, "");

                string strError = "";
                // �������û��Ƿ��Ѿ�����?
                // return:
                //       -1  ����
                //      0   ������
                //      1   ����, ������һ��
                //      2   ����, �����벻һ��
                int nRet = DetectManageUser(out strError);
                if (nRet == -1)
                {
                    MessageBox.Show(this, strError);
                }
                else
                {
                    MessageBox.Show(this, strError);
                }
            }
            finally
            {
                EnableControls(true);
            }
        }

        // ����һ���µĹ����ʻ�����Ҫ�� root Ȩ�޵�¼���ܴ���
        private void button_createManageUser_Click(object sender, EventArgs e)
        {
            EnableControls(false);

            try
            {
                string strError = "";
                int nRet = CreateManageUser(out strError);
                if (nRet == -1)
                {
                    MessageBox.Show(this, strError);
                }
                else
                {
                    MessageBox.Show(this, "�����ʻ������ɹ���");
                }
            }
            finally
            {
                EnableControls(true);
            }

        }

        private void button_resetManageUserPassword_Click(object sender, EventArgs e)
        {
            // �����ô����ʻ�����
            EnableControls(false);
            try
            {
                string strError = "";
                int nRet = ResetManageUserPassword(out strError);
                if (nRet == -1)
                {
                    MessageBox.Show(this, strError);
                }
                else
                {
                    MessageBox.Show(this, "��������ʻ�����ɹ���");
                }
            }
            finally
            {
                EnableControls(true);
            }
        }

    }
}