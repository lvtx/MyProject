using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DigitalPlatform.Text;

namespace DigitalPlatform.DTLP
{
    public partial class AccessPointDialog : Form
    {
        public AccessPointDialog()
        {
            InitializeComponent();
        }

        private void AccessPointDialog_Load(object sender, EventArgs e)
        {

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            string strError = "";

            if (this.textBox_fromName.Text == "")
            {
                strError = "��δ������Դ��";
                goto ERROR1;
            }

            if (this.textBox_weight.Text == "")
            {
                strError = "��δ����Ȩֵ";
                goto ERROR1;
            }

            if (this.comboBox_searchStyle.Text == "")
            {
                strError = "��δָ��������ʽ";
                goto ERROR1;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
            return;
        ERROR1:
            MessageBox.Show(this, strError);

        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public string FromName
        {
            get
            {
                return this.textBox_fromName.Text;
            }
            set
            {
                this.textBox_fromName.Text = value;
            }
        }

        public string Weight
        {
            get
            {
                return this.textBox_weight.Text;
            }
            set
            {
                this.textBox_weight.Text = value;
            }
        }

        public string SearchStyle
        {
            get
            {
                return this.comboBox_searchStyle.Text;
            }
            set
            {
                this.comboBox_searchStyle.Text = value;
            }
        }

        private void textBox_weight_Validating(object sender, CancelEventArgs e)
        {
            if (StringUtil.IsPureNumber(this.textBox_weight.Text) == false)
            {
                MessageBox.Show(this, "Ȩֵ����Ϊ������");
                e.Cancel = true;
            }
        }
    }
}