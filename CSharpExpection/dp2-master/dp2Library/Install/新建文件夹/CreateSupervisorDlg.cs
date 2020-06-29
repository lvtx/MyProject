using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dp2Library
{
    public partial class CreateSupervisorDlg : Form
    {
        public CreateSupervisorDlg()
        {
            InitializeComponent();
        }

        private void CreateSupervisorDlg_Load(object sender, EventArgs e)
        {

        }

        private void button_createSupervisor_Click(object sender, EventArgs e)
        {
            string strError = "";

            if (this.textBox_supervisorUserName.Text == "")
            {
                strError = "��δָ���û���";
                goto ERROR1;
            }

            if (this.textBox_supervisorUserName.Text.ToLower() == "public"
                || this.textBox_supervisorUserName.Text.ToLower() == "reader"   //?
                || this.textBox_supervisorUserName.Text == "ͼ���")
            {
                strError = "�����������ܰ��û���ȡΪ '" + this.textBox_supervisorUserName.Text + "'����Ϊ���Ǳ������������û���";
                goto ERROR1;
            }

            if (this.textBox_supervisorPassword.Text == "")
            {
                /*
                strError = "��δָ������";
                goto ERROR1;
                 * */
                // 2009/10/10 changed
                DialogResult result = MessageBox.Show(this,
                    "�����û�������Ϊ�ա�������ܲ���ȫ����Ҳ�����ڰ�װ�ɹ��󣬾�������dp2Circulation�е��û���Ϊ�����û��������롣\r\n\r\nȷʵҪ���ֳ����û�������Ϊ����?",
                    "setup_dp2library",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                    return;

            }

            if (this.textBox_supervisorPassword.Text != this.textBox_confirmSupervisorPassword.Text)
            {
                strError = "���� �� ȷ������ ��һ�¡����������롣";
                goto ERROR1;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
            return;
            ERROR1:
            MessageBox.Show(this, strError);

        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public string UserName
        {
            get
            {
                return this.textBox_supervisorUserName.Text;
            }
            set
            {
                this.textBox_supervisorUserName.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return this.textBox_supervisorPassword.Text;
            }
            set
            {
                this.textBox_supervisorPassword.Text = value;
                this.textBox_confirmSupervisorPassword.Text = value;
            }
        }

        public string Rights
        {
            get
            {
                return this.textBox_rights.Text;
            }
            set
            {
                this.textBox_rights.Text = value;
            }
        }
    }
}