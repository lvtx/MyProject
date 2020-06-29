using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dp2Library
{
    public partial class HostNameDialog : Form
    {
        public List<string> HostNames = null;

        public HostNameDialog()
        {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (this.comboBox_hostName.Text == "")
            {
                MessageBox.Show(this, "��δָ��������");
                return;
            }

            // ����Ƿ�Ϊlocalhost��127.0.0.1?
            string strPureHostName = "";
            int nRet = this.comboBox_hostName.Text.IndexOf(":");
            if (nRet != -1)
                strPureHostName = this.comboBox_hostName.Text.Substring(0, nRet).Trim().ToLower();
            else
                strPureHostName = this.comboBox_hostName.Text.Trim().ToLower();

            if (strPureHostName == "localhost"
                || strPureHostName == "127.0.0.1")
            {
                DialogResult result = MessageBox.Show(this,
                    "���棺����ѡ���������� '"+strPureHostName+"'�����������λ�ڱ�Ļ�����ǰ�˳��������̨ͼ���Ӧ�÷�������ʱ��������������û����һЩ���ϣ�ǰ��ĳЩHTML��ʾ������޷��ҵ���ȷ��css�ļ����ȵȡ�\r\n�������������á������������ĽǶȿ���������̨������������������������̨������������(��loopback)IP��ַ��\r\n\r\nȷʵҪ���ʹ�������� '" + strPureHostName + "' ?",
                    "HostNameDialog",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
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

        private void HostNameDialog_Load(object sender, EventArgs e)
        {
            if (this.HostNames != null)
            {
                for (int i = 0; i < this.HostNames.Count; i++)
                {
                    this.comboBox_hostName.Items.Add(this.HostNames[i]);
                }
            }
        }

        public string HostName
        {
            get
            {
                return this.comboBox_hostName.Text;
            }
            set
            {
                this.comboBox_hostName.Text = value;
            }
        }
    }
}