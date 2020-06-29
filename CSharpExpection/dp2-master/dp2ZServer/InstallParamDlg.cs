using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// using DigitalPlatform.CirculationClient;
using DigitalPlatform.LibraryClient;

namespace dp2ZServer
{
    public partial class InstallParamDlg : Form
    {
        public InstallParamDlg()
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
                    this.textBox_librarywsUrl.Text,
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

        private void button_OK_Click(object sender, EventArgs e)
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

                if (this.textBox_anonymousUserName.Text == ""
                    && this.textBox_anonymousPassword.Text != "")
                {
                    MessageBox.Show(this, "��δָ��������¼�û���������£�����ָ��������¼���롣");
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
                    this.textBox_librarywsUrl.Text,
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

            }
            finally
            {
                EnableControls(true);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
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
            this.textBox_librarywsUrl.Enabled = bEnable;

            this.button_OK.Enabled = bEnable;
            this.button_Cancel.Enabled = bEnable;
            this.button_detectManageUser.Enabled = bEnable;

            this.textBox_anonymousUserName.Enabled = bEnable;
            this.textBox_anonymousPassword.Enabled = bEnable;
            this.button_detectAnonymousUser.Enabled = bEnable;

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
                return this.textBox_librarywsUrl.Text;
            }
            set
            {
                this.textBox_librarywsUrl.Text = value;
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
                    this.textBox_librarywsUrl.Text,
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
    }
}