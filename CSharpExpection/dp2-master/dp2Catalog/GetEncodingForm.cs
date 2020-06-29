using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DigitalPlatform.Text;

namespace dp2Catalog
{
    public partial class GetEncodingForm : Form
    {
        public Encoding Encoding = null;

        public GetEncodingForm()
        {
            InitializeComponent();
        }

        private void GetEncodingForm_Load(object sender, EventArgs e)
        {
            // ���������б�
            /*
            this.comboBox_encoding.Items.Clear();
            List<string> encodings = ZSearchForm.GetEncodingList(false);
            for (int i = 0; i < encodings.Count; i++)
            {
                this.comboBox_encoding.Items.Add(encodings[i]);
            }
             * */
            Global.FillEncodingList(this.comboBox_encoding, false);

            if (this.Encoding != null)
            {
                /*
                EncodingInfo info = GetEncodingInfo(this.Encoding);
                if (info != null)
                    this.comboBox_encoding.Text = info.Name;
                 * */
                this.comboBox_encoding.Text = GetEncodingName(this.Encoding);
            }
            else
            {
                this.comboBox_encoding.Text = "";   // 2007/8/9
            }

        }

        // ���һ���������Ϣ��ע�⣬���������ܴ��������Marc8Encoding��
        static EncodingInfo GetEncodingInfo(Encoding encoding)
        {
            EncodingInfo [] infos = Encoding.GetEncodings();
            for (int i = 0; i < infos.Length; i++)
            {
                if (encoding.Equals(infos[i].GetEncoding()))
                    return infos[i];
            }


            return null;    // not found
        }

        // ���encoding����ʽ���֡�����������ʶ��Marc8Encoding��
        public static string GetEncodingName(Encoding encoding)
        {
            EncodingInfo info = GetEncodingForm.GetEncodingInfo(encoding);
            if (info != null)
            {
                return info.Name;
            }
            else
            {
                if (encoding is Marc8Encoding)
                {
                    return "MARC-8";
                }
                else
                {
                    return "Unknown encoding";
                }
            }
        }


        private void button_OK_Click(object sender, EventArgs e)
        {
            if (this.comboBox_encoding.Text == "")
            {
                MessageBox.Show(this, "��δѡ����뷽ʽ");
                return;
            }

            if (StringUtil.IsNumber(this.comboBox_encoding.Text) == true)
            {
                try
                {
                    Int32 nCodePage = Convert.ToInt32(this.comboBox_encoding.Text);
                    this.Encoding = Encoding.GetEncoding(nCodePage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "������뷽ʽ���̳���: " + ex.Message);
                    return;
                }
            }
            else
            {
                this.Encoding = Encoding.GetEncoding(this.comboBox_encoding.Text);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }



    }
}