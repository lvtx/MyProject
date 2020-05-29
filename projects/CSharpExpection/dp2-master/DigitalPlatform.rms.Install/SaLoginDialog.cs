using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace DigitalPlatform.rms
{
    /// <summary>
    /// MS SQL Server ����Ա�˻���¼�Ի���
    /// </summary>
    public partial class SaLoginDialog : Form
    {
        public SaLoginDialog()
        {
            InitializeComponent();
        }

        private void SaLoginDialog_Load(object sender, EventArgs e)
        {
            radioButton_SSPI_CheckedChanged(null, null);
        }

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

        public bool SSPI
        {
            get
            {
                return this.radioButton_SSPI.Checked;
            }
            set
            {
                this.radioButton_SSPI.Checked = true;
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (this.textBox_sqlServerName.Text == "")
            {
                MessageBox.Show(this, "��δָ��SQL��������");
                return;
            }

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
                strError = strError + "\r\n" + "������ָ����¼��Ϣ��";
                MessageBox.Show(this, strError);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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

        public void EnableControls(bool bEnable)
        {
            this.textBox_sqlServerName.Enabled = bEnable;

            if (this.radioButton_SSPI.Checked == true)
            {
                this.textBox_sqlUserName.Enabled = false;
                this.textBox_sqlPassword.Enabled = false;
            }
            else
            {
                this.textBox_sqlUserName.Enabled = bEnable;
                this.textBox_sqlPassword.Enabled = bEnable;
            }

            this.button_detect.Enabled = bEnable;

            this.button_OK.Enabled = bEnable;
            this.button_Cancel.Enabled = bEnable;
        }

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
        }
    }
}