using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DigitalPlatform.rms
{
    public partial class RootUserDlg : Form
    {
        public RootUserDlg()
        {
            InitializeComponent();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (this.UserName == "")
            {
                MessageBox.Show(this, "��δָ���û�����");
                return;
            }

            if (this.textBox_rootPassword.Text != this.textBox_confirmRootPassword.Text)
            {
                MessageBox.Show(this, "���� �� �ٴ��������벻һ�¡����������롣");
                return;
            }

            if (this.textBox_rootPassword.Text == "")
            {
                DialogResult result = MessageBox.Show(this,
                    "root�˻�����Ϊ�ա�������ܲ���ȫ����Ҳ�����ڰ�װ�ɹ��󣬾�������dp2manager����Ϊroot�˻��������롣\r\n\r\nȷʵҪ����root�˻�������Ϊ����?",
                    "setup_dp2Kernel",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No)
                    return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
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
                return this.textBox_rootUserName.Text;
            }
            set
            {
                this.textBox_rootUserName.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return this.textBox_rootPassword.Text;
            }
            set
            {
                this.textBox_rootPassword.Text = value;
                this.textBox_confirmRootPassword.Text = value;
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