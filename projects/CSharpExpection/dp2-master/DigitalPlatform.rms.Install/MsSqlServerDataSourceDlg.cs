using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;

using DigitalPlatform;
using DigitalPlatform.GUI;
using DigitalPlatform.Install;

namespace DigitalPlatform.rms
{
    public partial class MsSqlServerDataSourceDlg : Form
    {
        public MsSqlServerDataSourceDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ������Ϣ��ѡ�� SQL Server �ʹ�����¼���Ĺ�����Ϣ
        /// </summary>
        public string DebugInfo
        {
            get;
            set;
        }

        public string SqlServerName
        {
            get
            {
                return this.textBox_sqlServerName.Text;
            }
            set 
            {
                this.textBox_sqlServerName.Text = value;
            }
        }

        public string KernelLoginName
        {
            get
            {
                return this.textBox_loginName.Text;
            }
            set
            {
                this.textBox_loginName.Text = value;
            }
        }

        public string KernelLoginPassword
        {
            get
            {
                return this.textBox_loginPassword.Text;
            }
            set
            {
                this.textBox_loginPassword.Text = value;
                this.textBox_confirmLoginPassword.Text = value;
            }
        }

        /*
        public bool SSPI
        {
            get
            {
                return this.radioButton_SSPI.Checked;
            }
            set
            {
                this.radioButton_SSPI.Checked = value;
            }
        }*/

        /*
        public string SqlUserName
        {
            get
            {
                return this.textBox_sqlUserName.Text;
            }
            set
            {
                this.textBox_sqlUserName.Text = value;
            }
        }

        public string SqlPassword
        {
            get
            {
                return this.textBox_sqlPassword.Text;
            }
            set
            {
                this.textBox_sqlPassword.Text = value;
            }
        }
         * */

        public string InstanceName
        {
            get
            {
                return this.textBox_instanceName.Text;
            }
            set
            {
                this.textBox_instanceName.Text = value;
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            if (this.textBox_sqlServerName.Text == "")
            {
                strError = "��δָ�� SQL ������";
                goto ERROR1;
            }

            if (string.Compare(this.textBox_sqlServerName.Text.Trim(), "~sqlite") == 0)
            {
                strError = "MS SQL ������������Ϊ '~sqlite'����Ϊ������ֱ������� SQLite �������ݿ�����";
                goto ERROR1;
            }

#if NO
                    // ��� SQL Server ��Ϣ
            SqlServerInfo info = null;
            nRet = GetSqlServerInfo(
        this.SqlServerName,
        dlg.SqlUserName,
        dlg.SqlPassword,
        dlg.SSPI,
            out info,
            out strError);
            if (nRet == -1)
                goto ERROR1;
#endif

            nRet = GetIntegratedSecurityOnlyMode(this.textBox_sqlServerName.Text, out strError);
            if (nRet == -1)
                MessageBox.Show(this, strError);

            bool bISOM = false;
            if (nRet == 1)
                bISOM = true;

            // ����Ȩ�޵�¼Ψһ��ʽ������£���Ҫ���� ��¼��
            if (bISOM == true)
            {
                this.textBox_loginName.Text = "";
                this.textBox_loginPassword.Text = "";
                this.textBox_confirmLoginPassword.Text = "";
            }
            else
            {
                if (this.textBox_loginName.Text == "")
                {
                    strError = "��δָ�� dp2Kernel ��¼��";
                    goto ERROR1;
                }

                if (this.textBox_loginPassword.Text != this.textBox_confirmLoginPassword.Text)
                {
                    strError = "dp2Kernel ��¼���������ȷ�����벻һ��";
                    goto ERROR1;
                }
            }

            /*
            if (this.SSPI == false && this.textBox_sqlUserName.Text == "")
            {
                MessageBox.Show(this, "��δָ��SQL�ʺš�");
                return;
            }


            // ���SQL�ʻ��Ƿ���ȷ
            EnableControls(false);
            string strError = "";
            int nRet = this.detect(this.textBox_sqlServerName.Text,
                this.textBox_sqlUserName.Text,
                this.textBox_sqlPassword.Text,
                radioButton_SSPI.Checked,
                out strError);
            EnableControls(true);
            if (nRet == -1)
            {
                strError = strError + "\r\n" + "������ָ����������Ϣ��";
                MessageBox.Show(this, strError);
                return;
            }
             * */

            SaLoginDialog dlg = new SaLoginDialog();
            GuiUtil.AutoSetDefaultFont(dlg);
            dlg.SqlServerName = this.textBox_sqlServerName.Text;
            dlg.StartPosition = FormStartPosition.CenterScreen;

            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            if (string.IsNullOrEmpty(this.textBox_loginName.Text) == false)
            {
                // ����dp2Kernel��¼��

                // ����һ���ʺ���dpKernel��SQL Server login
                // return:
                //      -1  ����
                //      0   �ɹ�
                //      1   ԭ���Ѿ����ڣ��Ҳ�����ɾ��
                nRet = CreateLogin(
                    this.SqlServerName,
                    dlg.SqlUserName,
                    dlg.SqlPassword,
                    dlg.SSPI,
                    this.textBox_loginName.Text,
                    this.textBox_loginPassword.Text,
                    out strError);
                if (nRet == -1 || nRet == 1)
                {
                    goto ERROR1;
                }
            }

            if (bISOM == true)
            {
                nRet = AddSystemDbCreatorRole(
        this.SqlServerName,
        dlg.SqlUserName,
        dlg.SqlPassword,
        dlg.SSPI,
        out strError);
                if (nRet == -1)
                {
                    strError = "Ϊ ��¼�� 'NT AUTHORITY\\SYSTEM' ��� 'dbcreator' ʱ����: " + strError;
                    goto ERROR1;
                }
            }

            /*
            if (nRet == 1)
            {
                string strText = "��¼�� '" + this.textBox_loginName.Text + "' ��SQL������ '" + this.SqlServerName + "' ���Ѿ����ڣ��������벻һ���͵�ǰָ����������ͬ��\r\n\r\n�Ƿ����ʹ�������¼��?\r\n(Yes)����ʹ�ã�(No)����ָ����¼��������";
                DialogResult result = MessageBox.Show(this,
                    strText,
                    "setup_dp2Kernel",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                {
                    this.textBox_loginPassword.Focus();
                    return;
                }
            }
             * */

            this.DialogResult = DialogResult.OK;
            this.Close();
            return;
        ERROR1:
            MessageBox.Show(this, strError);
            MessageBox.Show(this, "��Ȼ�ղŵĴ�����¼������ʧ���ˣ�����Ҳ����������ָ����¼����������ٴΰ���ȷ������ť������¼�����������а�װ");
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ����һ���ʺ���dp2Kernel��SQL Server login
        // return:
        //      -1  ����
        //      0   �ɹ�
        //      1   ԭ���Ѿ����ڣ��Ҳ�����ɾ��
        public int CreateLogin(
            string strSqlServerName,
            string strSqlUserName,
            string strSqlUserPassword,
            bool bSSPI,
            string strLoginName,
            string strLoginPassword,
            out string strError)
        {
            strError = "";

            string strConnection = @"Persist Security Info=False;"
                + "User ID=" + strSqlUserName + ";"    //�ʻ�������
                + "Password=" + strSqlUserPassword + ";"
                + "Data Source=" + strSqlServerName + ";"
                + "Connect Timeout=30";

            if (bSSPI == true)
            {
                strConnection = @"Persist Security Info=False;"
                    + "Integrated Security=SSPI; "      //��������
                    + "Data Source=" + strSqlServerName + ";"
                    + "Connect Timeout=30"; // 30��
            }


            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(strConnection);
            }
            catch (Exception ex)
            {
                strError = "�������ӳ���" + ex.Message + " ����:" + ex.GetType().ToString();
                return -1;
            }

            try
            {
                connection.Open();
            }
            catch (SqlException sqlEx)
            {
                strError = "����SQL���ݿⷢ������" + sqlEx.Message + "��";
                int nError = sqlEx.ErrorCode;
                return -1;
            }
            catch (Exception ex)
            {
                strError = "����SQL���ݿⷢ������" + ex.Message + " ����:" + ex.GetType().ToString();
                return -1;
            }

            /* http://support.microsoft.com/kb/321185
             * 10 -- SQL Server 2008
             * 9 -- SQL Server 2005
             * 8 -- SQL 2000
             * 7 -- SQL 7.0
             * */
           

            try
            {
                string strVersion = "7";
                string strCommand = "";
                SqlCommand command = null;

                // Debug.Assert(false, "");

                strCommand = "SELECT SERVERPROPERTY('productversion')";
                command = new SqlCommand(strCommand,
                    connection);
                try
                {
                    strVersion = (string)command.ExecuteScalar();
                    int nRet = strVersion.IndexOf(".");
                    if (nRet != -1)
                        strVersion = strVersion.Substring(0, nRet);
                }
                catch (Exception /*ex*/)
                {
                    // strError = "ִ������ " + strCommand + " ����" + ex.Message + " ���ͣ�" + ex.GetType().ToString();
                    // return -1;
                    strVersion = "7";
                }


                // ��ɾ��ͬ����login
                // strCommand = "IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'" + strLoginName + "') DROP LOGIN [" + strLoginName + "]";  // sql 2005

                if (strVersion == "10")
                    strCommand = "DROP LOGIN [" + strLoginName + "]";
                else
                    strCommand = "EXEC sp_droplogin @loginame = '" + strLoginName + "'";    // sql 2000
                command = new SqlCommand(strCommand,
                    connection);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // ��Ҫ�ж�SqlException�ľ������ͣ�����Եر�����߲�����
                    if (IsSqlErrorNo(ex, 15007) == true)
                    {
                        // 15007    The login '%s' does not exist.
                    }
                    else if (IsSqlErrorNo(ex, 15151) == true)
                    {
                        // ms-help://MS.SQLCC.v10/MS.SQLSVR.v10.zh-CHS/s10de_5techref/html/e9f7b86b-891e-4abb-938e-c39c707f5a5f.htm
                        // 15151    �޷��� %S_MSG '%.*ls' ִ�� %S_MSG����Ϊ�������ڣ�������û�������Ȩ�ޡ�
                    }
                    else if (IsSqlErrorNo(ex, 15174) == true)
                    {
                        //    15174   16   ��¼   ''%1!''   ӵ��һ���������ݿ⡣������������ݿ�������ߺ��ٳ�ȥ�õ�¼��   
                        strError = "���棺ִ������ " + strCommand + " ����" + ex.Message + " ���ͣ�" + ex.GetType().ToString();
                        return 1;
                    }
                    else
                    {
                        strError = "���棺ִ������ " + strCommand + " ����" + ex.Message + " ���ͣ�" + ex.GetType().ToString();
                        MessageBox.Show(this, strError);
                    }
                }
                catch (Exception ex)
                {
                    strError = "���棺ִ������ " + strCommand + " ����" + ex.Message + " ���ͣ�" + ex.GetType().ToString();
                    MessageBox.Show(this, strError);
                }
                
                if (strVersion == "10")
                    strCommand = "CREATE LOGIN [" + strLoginName + "] WITH PASSWORD=N'" + strLoginPassword + "', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF";
                else
                    strCommand = "EXEC sp_addlogin @loginame='"+strLoginName+"',   @passwd='"+strLoginPassword+"', @defdb='master'";
                command = new SqlCommand(strCommand,
                    connection);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    strError = "ִ������ "+strCommand+" ����" + ex.Message + " ���ͣ�" + ex.GetType().ToString();
                    return -1;
                }

                strCommand = "EXEC sp_addsrvrolemember @loginame = '"+strLoginName+"', @rolename = 'dbcreator'";
                command = new SqlCommand(strCommand,
                    connection);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    strError = "ִ������ " + strCommand + " ����" + ex.Message + " ���ͣ�" + ex.GetType().ToString();
                    return -1;
                }
            }
            finally
            {
                connection.Close();
            }

            return 0;
        }

        public int AddSystemDbCreatorRole(
string strSqlServerName,
string strSqlUserName,
string strSqlUserPassword,
bool bSSPI,
out string strError)
        {
            strError = "";

            string strConnection = @"Persist Security Info=False;"
                + "User ID=" + strSqlUserName + ";"    //�ʻ�������
                + "Password=" + strSqlUserPassword + ";"
                + "Data Source=" + strSqlServerName + ";"
                + "Connect Timeout=30";

            if (bSSPI == true)
            {
                strConnection = @"Persist Security Info=False;"
                    + "Integrated Security=SSPI; "      //��������
                    + "Data Source=" + strSqlServerName + ";"
                    + "Connect Timeout=30"; // 30��
            }


            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(strConnection);
            }
            catch (Exception ex)
            {
                strError = "�������ӳ���" + ex.Message + " ����:" + ex.GetType().ToString();
                return -1;
            }

            try
            {
                connection.Open();
            }
            catch (SqlException sqlEx)
            {
                strError = "����SQL����������" + sqlEx.Message + "��";
                int nError = sqlEx.ErrorCode;
                return -1;
            }
            catch (Exception ex)
            {
                strError = "����SQL����������" + ex.Message + " ����:" + ex.GetType().ToString();
                return -1;
            }

            /* http://support.microsoft.com/kb/321185
             * 10 -- SQL Server 2008
             * 9 -- SQL Server 2005
             * 8 -- SQL 2000
             * 7 -- SQL 7.0
             * */

            try
            {
                string strVersion = "7";
                string strCommand = "";
                SqlCommand command = null;

                // Debug.Assert(false, "");

                strCommand = "SELECT SERVERPROPERTY('productversion')";
                command = new SqlCommand(strCommand,
                    connection);
                try
                {
                    strVersion = (string)command.ExecuteScalar();
                    int nRet = strVersion.IndexOf(".");
                    if (nRet != -1)
                        strVersion = strVersion.Substring(0, nRet);
                }
                catch (Exception /*ex*/)
                {
                    // strError = "ִ������ " + strCommand + " ����" + ex.Message + " ���ͣ�" + ex.GetType().ToString();
                    // return -1;
                    strVersion = "7";
                }

                strCommand = "EXEC sp_addsrvrolemember @loginame = 'NT AUTHORITY\\SYSTEM', @rolename = 'dbcreator'";
                command = new SqlCommand(strCommand,
                    connection);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    strError = "ִ������ " + strCommand + " ����" + ex.Message + " ���ͣ�" + ex.GetType().ToString();
                    return -1;
                }
            }
            finally
            {
                connection.Close();
            }

            return 0;
        }


        // 2015/4/24
        // ̽�� SQL Server �Ƿ�Ϊ Integrated Security Only ģʽ
        // return:
        //      -1  ����
        //      0   ���� Integrated Security Only ģʽ
        //      1   �� Integrated Security Only ģʽ
        public int GetIntegratedSecurityOnlyMode(
string strSqlServerName,
out string strError)
        {
            strError = "";

            string strConnection = @"Persist Security Info=False;"
                    + "Integrated Security=SSPI; "      //��������
                    + "Data Source=" + strSqlServerName + ";"
                    + "Connect Timeout=30"; // 30��

            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(strConnection);
            }
            catch (Exception ex)
            {
                strError = "�������ӳ���" + ex.Message + " ����:" + ex.GetType().ToString();
                return -1;
            }

            try
            {
                connection.Open();
            }
            catch (SqlException sqlEx)
            {
                strError = "����SQL����������" + sqlEx.Message + "��";
                int nError = sqlEx.ErrorCode;
                return -1;
            }
            catch (Exception ex)
            {
                strError = "����SQL����������" + ex.Message + " ����:" + ex.GetType().ToString();
                return -1;
            }

            try
            {
                int nRet = 1;
                string strCommand = "";
                SqlCommand command = null;

                strCommand = "SELECT SERVERPROPERTY('IsIntegratedSecurityOnly')";
                command = new SqlCommand(strCommand,
                    connection);
                try
                {
                    nRet = (Int32)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    strError = "ִ������ " + strCommand + " ����" + ex.Message + " ���ͣ�" + ex.GetType().ToString();
                    return -1;
                    nRet = 1;
                }

                return nRet;
            }
            finally
            {
                connection.Close();
            }
        }

#if NO
        class SqlServerInfo
        {
            public string ServerName = "";

            // ��װʱ���ڷ��� SQL Server �ĵ�¼��Ϣ
            public string SqlUserName = "";
            public string SqlUserPassword = "";
            public bool SSPI = false;

            public string Version = "";
            public bool IntegratedSecurityOnlyMode = false;
        }

        // ��� SQL Server ��Ϣ
        // return:
        //      -1  ����
        //      0   ����
        //      1   �ɹ�
        public int GetSqlServerInfo(
            string strSqlServerName,
            out SqlServerInfo info,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            info = new SqlServerInfo();

            REDO_INPUT:
            SaLoginDialog dlg = new SaLoginDialog();
            GuiUtil.AutoSetDefaultFont(dlg);
            dlg.SqlServerName = strSqlServerName;
            dlg.StartPosition = FormStartPosition.CenterScreen;

            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return 0;

            info.ServerName = strSqlServerName;
            info.SqlUserName = dlg.SqlUserName;
            info.SqlUserPassword = dlg.SqlPassword;
            info.SSPI = dlg.SSPI;

            string strConnection = @"Persist Security Info=False;"
                + "User ID=" + info.SqlUserName + ";"    //�ʻ�������
                + "Password=" + info.SqlUserPassword + ";"
                + "Data Source=" + strSqlServerName + ";"
                + "Connect Timeout=30";

            if (info.SSPI == true)
            {
                strConnection = @"Persist Security Info=False;"
                    + "Integrated Security=SSPI; "      //��������
                    + "Data Source=" + strSqlServerName + ";"
                    + "Connect Timeout=30"; // 30��
            }

            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(strConnection);
            }
            catch (Exception ex)
            {
                strError = "��������ʱ����" + ex.Message + " ����:" + ex.GetType().ToString();
                return -1;
            }

            try
            {
                connection.Open();
            }
            catch (SqlException sqlEx)
            {
                strError = "����SQL����������" + sqlEx.Message + "��";
                int nError = sqlEx.ErrorCode;
                MessageBox.Show(this, strError);
                goto REDO_INPUT;
                return -1;
            }
            catch (Exception ex)
            {
                strError = "����SQL����������" + ex.Message + " ����:" + ex.GetType().ToString();
                return -1;
            }

            /* http://support.microsoft.com/kb/321185
             * 10 -- SQL Server 2008
             * 9 -- SQL Server 2005
             * 8 -- SQL 2000
             * 7 -- SQL 7.0
             * */

            try
            {
                string strVersion = "7";
                string strCommand = "";
                SqlCommand command = null;

                // Debug.Assert(false, "");

                strCommand = "SELECT SERVERPROPERTY('productversion')";
                command = new SqlCommand(strCommand,
                    connection);
                try
                {
                    strVersion = (string)command.ExecuteScalar();
                    // ȥ����Ҫ�汾��
                    nRet = strVersion.IndexOf(".");
                    if (nRet != -1)
                        strVersion = strVersion.Substring(0, nRet);
                }
                catch (Exception /*ex*/)
                {
                    // strError = "ִ������ " + strCommand + " ����" + ex.Message + " ���ͣ�" + ex.GetType().ToString();
                    // return -1;
                    strVersion = "7";
                }

                info.Version = strVersion;

                strCommand = "SELECT SERVERPROPERTY('IsIntegratedSecurityOnly')";
                command = new SqlCommand(strCommand,
                    connection);
                try
                {
                    nRet = (Int32)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    //strError = "ִ������ " + strCommand + " ����" + ex.Message + " ���ͣ�" + ex.GetType().ToString();
                    //return -1;
                    nRet = 1;
                }

                if (nRet == 1)
                    info.IntegratedSecurityOnlyMode = true;
                else
                    info.IntegratedSecurityOnlyMode = false;

            }
            finally
            {
                connection.Close();
            }

            return 0;
        }

#endif

        // �Ƿ������ָ���Ĵ����?
        // �������ms-help://MS.VSCC.v80/MS.MSDN.v80/MS.SQL.v2000.en/trblsql/tr_syserrors2_93i1.htm
        // 15007    The login '%s' does not exist.
        static bool IsSqlErrorNo(SqlException ex,
            int nNumber)
        {
            for (int i = 0; i < ex.Errors.Count; i++)
            {
                SqlError error = ex.Errors[i];

                if (error.Number == nNumber)
                    return true;
            }
            return false;
        }

        private void button_getSqlServerName_Click(object sender, EventArgs e)
        {
            GetSqlServerDlg dlg = new GetSqlServerDlg();
            GuiUtil.AutoSetDefaultFont(dlg);

            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            this.textBox_sqlServerName.Text = dlg.SqlServerName;

#if NO
            if (string.IsNullOrEmpty(this.textBox_sqlServerName.Text) == false)
            {
                string strError = "";
                int nRet = GetIntegratedSecurityOnlyMode(this.textBox_sqlServerName.Text, out strError);
                if (nRet == -1)
                    MessageBox.Show(this, strError);
                else
                {
                    if (nRet == 0)
                        this.groupBox_login.Enabled = true;
                    else
                        this.groupBox_login.Enabled = false;
                }
            }
#endif
        }

        /*
        private void button_detect_Click(object sender, EventArgs e)
        {
            if (this.textBox_sqlServerName.Text == "")
            {
                MessageBox.Show(this, "��δָ��SQL�����������޷���⡣");
                return;
            }
            if (this.SSPI == false && this.textBox_sqlUserName.Text == "")
            {
                MessageBox.Show(this, "��δָ��SQL�û����޷���⡣");
                return;
            }

            EnableControls(false);
            string strError = "";
            int nRet = this.detect(this.textBox_sqlServerName.Text,
                this.textBox_sqlUserName.Text,
                this.textBox_sqlPassword.Text,
                this.radioButton_SSPI.Checked,
                out strError);
            EnableControls(true);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
            }
            else
            {
                MessageBox.Show(this, "SQL��������Ϣ��ȷ��");
            }
        }

        // return:
        //      -1  ����
        //      0   �ɹ�
        public int detect(string strSqlServerName,
            string strSqlUserName,
            string strPassword,
            bool bSSPI,
            out string strError)
        {
            strError = "";

            string strConnection = @"Persist Security Info=False;"
                + "User ID=" + strSqlUserName + ";"    //�ʻ�������
                + "Password=" + strPassword + ";"
                + "Data Source=" + strSqlServerName + ";"
                + "Connect Timeout=30";

            if (bSSPI == true)
            {
                strConnection = @"Persist Security Info=False;"
                    + "Integrated Security=SSPI; "      //��������
                    + "Data Source=" + strSqlServerName + ";"
                    + "Connect Timeout=30"; // 30��
            }


            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(strConnection);
            }
            catch (Exception ex)
            {
                strError = "�������ӳ���" + ex.Message + " ����:" + ex.GetType().ToString();
                return -1;
            }

            try
            {
                connection.Open();
            }
            catch (SqlException sqlEx)
            {
                strError = "����SQL���ݿⷢ������" + sqlEx.Message + "��";
                int nError = sqlEx.ErrorCode;
                return -1;
            }
            catch (Exception ex)
            {
                strError = "����SQL���ݿⷢ������" + ex.Message + " ����:" + ex.GetType().ToString();
                return -1;
            }

            try
            {
                string strCommand = "";
                SqlCommand command = null;
                strCommand = "use master " + "\n";


                command = new SqlCommand(strCommand,
                    connection);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    strError = "ִ���������" + ex.Message + " ���ͣ�" + ex.GetType().ToString();
                    return -1;
                }
            }
            finally
            {
                connection.Close();
            }

            return 0;
        }
         * */

        public void EnableControls(bool bEnable)
        {
            this.textBox_sqlServerName.Enabled = bEnable;

            /*
            if (this.radioButton_SSPI.Checked == true)
            {
                this.textBox_sqlUserName.Enabled = false;
                this.textBox_sqlPassword.Enabled = false;
            }
            else
            {
                this.textBox_sqlUserName.Enabled = bEnable;
                this.textBox_sqlPassword.Enabled = bEnable;
            }*/

            this.textBox_instanceName.Enabled = bEnable;

            this.button_getSqlServerName.Enabled = bEnable;

            // this.button_detect.Enabled = bEnable;

            this.button_OK.Enabled = bEnable;
            this.button_Cancel.Enabled = bEnable;            
        }

        /*
        private void radioButton_SSPI_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton_SSPI.Checked == true)
            {
                this.textBox_sqlUserName.Enabled = false;
                this.textBox_sqlPassword.Enabled = false;
            }
            else
            {
                this.textBox_sqlUserName.Enabled = true;
                this.textBox_sqlPassword.Enabled = true;
            }

        }

        private void radioButton_sqlAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton_SSPI.Checked == true)
            {
                this.textBox_sqlUserName.Enabled = false;
                this.textBox_sqlPassword.Enabled = false;
            }
            else
            {
                this.textBox_sqlUserName.Enabled = true;
                this.textBox_sqlPassword.Enabled = true;
            }
        }*/

        private void DataSourceDlg_Load(object sender, EventArgs e)
        {
            // radioButton_SSPI_CheckedChanged(null, null);
        }

        public string Comment
        {
            get
            {
                return this.textBox_message.Text;
            }
            set
            {
                this.textBox_message.Text = value;
            }
        }

        private void textBox_sqlServerName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}