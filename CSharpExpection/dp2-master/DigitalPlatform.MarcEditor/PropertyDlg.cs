using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DigitalPlatform.Marc
{
    /// <summary>
    /// MARC �༭������ �Ի���
    /// </summary>
    internal partial class PropertyDlg : Form
    {
        /// <summary>
        /// MARC �༭��
        /// </summary>
        public MarcEditor MarcEditor = null;

        /// <summary>
        /// ���캯��
        /// </summary>
        public PropertyDlg()
        {
            InitializeComponent();
        }

        private void PropertyDlg_Load(object sender, EventArgs e)
        {
            this.checkBox_enterAsAutoGenerate.Checked = this.MarcEditor.EnterAsAutoGenerate;

            AddLangCodeIfNeed(this.MarcEditor.Lang);

            this.comboBox_uiLanguage.Text = this.MarcEditor.Lang;
        }

        // ��ô�������Դ���
        static string GetPureLangCode(string strText)
        {
            int nRet = strText.IndexOf("\t");
            if (nRet == -1)
                return strText;
            return strText.Substring(0, nRet);
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.MarcEditor.EnterAsAutoGenerate = this.checkBox_enterAsAutoGenerate.Checked;

            string strNewLang = GetPureLangCode(this.comboBox_uiLanguage.Text);

            if (strNewLang != this.MarcEditor.Lang)
            {
                this.MarcEditor.Lang = strNewLang;
                this.MarcEditor.RefreshNameCaption();
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // �����е����Դ����combobox�����еĴ���Ƚϣ����û�У�������
        void AddLangCodeIfNeed(string strLang)
        {
            for (int i = 0; i < this.comboBox_uiLanguage.Items.Count; i++)
            {
                string strExistLang = GetPureLangCode((string)this.comboBox_uiLanguage.Items[i]);
                if (strLang == strExistLang)
                    return;
            }

            this.comboBox_uiLanguage.Items.Add(strLang);
        }
    }
}