using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DigitalPlatform.Xml;

namespace dp2Circulation
{
    /// <summary>
    /// ��ӡ������Ϣ ͳ�Ʒ��� ������Ի���
    /// </summary>
    public partial class PrintReaderInfoInputDialog : Form
    {
        /// <summary>
        /// AppInfo ����
        /// </summary>
        public ApplicationInfo AppInfo = null;
        /// <summary>
        /// Ҫ�� AppInfo �д�ȡ��������� Entry �����ַ���
        /// </summary>
        public string EntryTitle = "print_readerinfo_input_dialog";

        /// <summary>
        /// ���캯��
        /// </summary>
        public PrintReaderInfoInputDialog()
        {
            InitializeComponent();
        }

        private void PrintReaderInfoInputDialog_Load(object sender, EventArgs e)
        {
            if (this.AppInfo != null)
            {
                this.checkBox_range_noBorrowAndOverdueItem.Checked =
                    this.AppInfo.GetBoolean(this.EntryTitle,
                    "range_noBorrowAndOverdueItem",
                    true);

                this.checkBox_range_inPeriod.Checked =
                    this.AppInfo.GetBoolean(this.EntryTitle,
                    "range_inPeriod",
                    true);

                this.checkBox_range_outofPeriod.Checked =
                    this.AppInfo.GetBoolean(this.EntryTitle,
                    "outofPeriod",
                    true);

                this.checkBox_range_hasOverdueItem.Checked =
                    this.AppInfo.GetBoolean(this.EntryTitle,
                    "range_hasOverdueItem",
                    true);

                this.radioButton_range_all.Checked =
    this.AppInfo.GetBoolean(this.EntryTitle,
    "range_all",
    true);

                this.radioButton_range_part.Checked =
                    this.AppInfo.GetBoolean(this.EntryTitle,
                    "range_part",
                    false);

                //
                this.textBox_pageHeader.Text =
                    this.AppInfo.GetString(this.EntryTitle,
                    "page_header",
                    "%pageno% / %pagecount%");

                this.textBox_pageFooter.Text =
                    this.AppInfo.GetString(this.EntryTitle,
                    "page_footer",
                    "%date%");

                this.textBox_tableTitle.Text =
                    this.AppInfo.GetString(this.EntryTitle,
                    "table_title",
                    "�������");

                this.textBox_linesPerPage.Text =
                    this.AppInfo.GetString(this.EntryTitle,
                    "lines_per_page",
                    "20");

                this.textBox_maxSummaryChars.Text =
                    this.AppInfo.GetString(this.EntryTitle,
                    "max_summary_chars",
                    "18");

                // 
                string strSortDefLines = this.AppInfo.GetString(this.EntryTitle,
                    "sort_def_lines",
                    "");
                SetSortDefLines(strSortDefLines);

                this.listBox_sortDef.SelectedIndex =
                    this.AppInfo.GetInt(this.EntryTitle,
                    "sort_def",
                    0);

            }

            EnableCheckBoxes();

        }

        void SetSortDefLines(string strLines)
        {
            if (String.IsNullOrEmpty(strLines) == true)
                return;

            this.listBox_sortDef.Items.Clear();
            string[] lines = strLines.Split(new char[] {'\n'});
            for (int i = 0; i < lines.Length; i++)
            {
                this.listBox_sortDef.Items.Add(lines[i]);
            }
        }

        string GetSortDefLines()
        {
            string strResult = "";
            for (int i = 0; i < this.listBox_sortDef.Items.Count; i++)
            {
                if (i > 0)
                    strResult += "\n";
                   
                strResult += this.listBox_sortDef.Items[i];
            }

            return strResult;
        }

        private void PrintReaderInfoInputDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.AppInfo != null)
            {
                this.AppInfo.SetBoolean(this.EntryTitle,
                "range_all",
                this.radioButton_range_all.Checked);

                this.AppInfo.SetBoolean(this.EntryTitle,
                "range_part",
                this.radioButton_range_part.Checked);

                this.AppInfo.SetBoolean(this.EntryTitle,
                "range_noBorrowAndOverdueItem",
                this.checkBox_range_noBorrowAndOverdueItem.Checked);

                this.AppInfo.SetBoolean(this.EntryTitle,
                "range_inPeriod",
                this.checkBox_range_inPeriod.Checked);

                this.AppInfo.SetBoolean(this.EntryTitle,
                "outofPeriod",
                this.checkBox_range_outofPeriod.Checked);

                this.AppInfo.SetBoolean(this.EntryTitle,
                "range_hasOverdueItem",
                this.checkBox_range_hasOverdueItem.Checked);

                //
                this.AppInfo.SetString(this.EntryTitle,
                "page_header",
                this.textBox_pageHeader.Text);

                this.AppInfo.SetString(this.EntryTitle,
                "page_footer",
                this.textBox_pageFooter.Text);

                this.AppInfo.SetString(this.EntryTitle,
                "table_title",
                this.textBox_tableTitle.Text);

                this.AppInfo.SetString(this.EntryTitle,
                "lines_per_page",
                this.textBox_linesPerPage.Text);

                this.AppInfo.SetString(this.EntryTitle,
                "max_summary_chars",
                this.textBox_maxSummaryChars.Text);

                // 
                string strSortDefLines = this.GetSortDefLines();
                
                this.AppInfo.SetString(this.EntryTitle,
                    "sort_def_lines",
                    strSortDefLines);

                this.AppInfo.SetInt(this.EntryTitle,
                    "sort_def",
                    this.listBox_sortDef.SelectedIndex);
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            string strError = "";

            if (String.IsNullOrEmpty(this.SortDef) == true)
            {
                strError = "��δָ������ʽ";
                this.tabControl_main.SelectedTab = tabPage_sortOption;
                goto ERROR1;
            }

            if (this.LinesPerPage <= 0)
            {
                strError = "ÿҳ����ֵ����С��0";
                goto ERROR1;
            }

            if (this.MaxSummaryChars <= 0)
            {
                strError = "ժҪ��������ֵ����С��0";
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

        void EnableCheckBoxes()
        {
            if (this.radioButton_range_all.Checked == true)
            {
                this.checkBox_range_noBorrowAndOverdueItem.Enabled = false;
                this.checkBox_range_hasBorrowItem.Enabled = false;
                this.checkBox_range_outofPeriod.Enabled = false;
                this.checkBox_range_inPeriod.Enabled = false;
                this.checkBox_range_hasOverdueItem.Enabled = false;

                this.checkBox_range_noBorrowAndOverdueItem.Checked = true;
                this.checkBox_range_hasBorrowItem.Checked = true;
                this.checkBox_range_outofPeriod.Checked = true;
                this.checkBox_range_inPeriod.Checked = true;
                this.checkBox_range_hasOverdueItem.Checked = true;

            }
            else
            {
                this.checkBox_range_noBorrowAndOverdueItem.Enabled = true;
                this.checkBox_range_hasBorrowItem.Enabled = true;
                this.checkBox_range_outofPeriod.Enabled = true;
                this.checkBox_range_inPeriod.Enabled = true;
                this.checkBox_range_hasOverdueItem.Enabled = true;
            }
        }

        // ȫ�����
        private void radioButton_range_all_CheckedChanged(object sender, EventArgs e)
        {
            EnableCheckBoxes();
        }

        private void radioButton_range_part_CheckedChanged(object sender, EventArgs e)
        {
            EnableCheckBoxes();
        }

        private void checkBox_hasBorrowItem_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_range_hasBorrowItem.Checked == true)
            {

                if (this.checkBox_range_hasBorrowItem.CheckState == CheckState.Indeterminate)
                {
                }
                else
                {
                    this.checkBox_range_outofPeriod.Checked = true;
                    this.checkBox_range_inPeriod.Checked = true;
                }
            }
            else
            {
                this.checkBox_range_outofPeriod.Checked = false;
                this.checkBox_range_inPeriod.Checked = false;
            }
        }

        private void checkBox_range_inPeriod_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_range_inPeriod.Checked == true
                && this.checkBox_range_outofPeriod.Checked == true)
            {
                this.checkBox_range_hasBorrowItem.Checked = true;
                this.checkBox_range_hasBorrowItem.CheckState = CheckState.Checked;
                return;
            }

            if (this.checkBox_range_inPeriod.Checked == true
                || this.checkBox_range_outofPeriod.Checked == true)
            {
                this.checkBox_range_hasBorrowItem.CheckState = CheckState.Indeterminate;
                this.checkBox_range_hasBorrowItem.Checked = true;
                return;
            }

            if (this.checkBox_range_inPeriod.Checked == false
                && this.checkBox_range_outofPeriod.Checked == false)
            {
                this.checkBox_range_hasBorrowItem.Checked = false;
                return;
            }
        }

        private void checkBox_range_outofPeriod_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox_range_inPeriod.Checked == true
    && this.checkBox_range_outofPeriod.Checked == true)
            {
                this.checkBox_range_hasBorrowItem.Checked = true;
                this.checkBox_range_hasBorrowItem.CheckState = CheckState.Checked;
                return;
            }

            if (this.checkBox_range_inPeriod.Checked == true
                || this.checkBox_range_outofPeriod.Checked == true)
            {
                this.checkBox_range_hasBorrowItem.CheckState = CheckState.Indeterminate;
                this.checkBox_range_hasBorrowItem.Checked = true;
                return;
            }

            if (this.checkBox_range_inPeriod.Checked == false
                && this.checkBox_range_outofPeriod.Checked == false)
            {
                this.checkBox_range_hasBorrowItem.Checked = false;
                return;
            }
        }

        /// <summary>
        /// �����ڽ���ΥԼ��ġ��Ƿ񱻹�ѡ
        /// </summary>
        public bool NoBorrowAndOverdueItem
        {
            get
            {
                return this.checkBox_range_noBorrowAndOverdueItem.Checked;
            }
            set
            {
                this.checkBox_range_noBorrowAndOverdueItem.Checked = value;
            }

        }

        /// <summary>
        /// �����ڽ��� / �ѳ��ڡ��Ƿ񱻹�ѡ
        /// </summary>
        public bool OutofPeriodItem
        {
            get
            {
                return this.checkBox_range_outofPeriod.Checked;
            }
            set
            {
                this.checkBox_range_outofPeriod.Checked = value;
            }

        }

        /// <summary>
        /// �����ڽ��� / δ���ڡ��Ƿ񱻹�ѡ
        /// </summary>
        public bool InPeriodItem
        {
            get
            {
                return this.checkBox_range_inPeriod.Checked;
            }
            set
            {
                this.checkBox_range_inPeriod.Checked = value;
            }

        }

        /// <summary>
        /// ����ΥԼ��ġ��Ƿ񱻹�ѡ
        /// </summary>
        public bool HasOverdueItem
        {
            get
            {
                return this.checkBox_range_hasOverdueItem.Checked;
            }
            set
            {
                this.checkBox_range_hasOverdueItem.Checked = value;
            }
        }

        // ҳü
        /// <summary>
        /// ҳü����
        /// </summary>
        public string PageHeader
        {
            get
            {
                return this.textBox_pageHeader.Text;
            }
            set
            {
                this.textBox_pageHeader.Text = value;
            }
        }

        // ҳ��
        /// <summary>
        /// ҳ������
        /// </summary>
        public string PageFooter
        {
            get
            {
                return this.textBox_pageFooter.Text;
            }
            set
            {
                this.textBox_pageFooter.Text = value;
            }
        }

        // 
        /// <summary>
        /// ������
        /// </summary>
        public string TableTitle
        {
            get
            {
                return this.textBox_tableTitle.Text;
            }
            set
            {
                this.textBox_tableTitle.Text = value;
            }
        }

        // 
        /// <summary>
        /// ÿҳ����
        /// </summary>
        public int LinesPerPage
        {
            get
            {
                if (this.textBox_linesPerPage.Text == "")
                    return 0;
                return Convert.ToInt32(this.textBox_linesPerPage.Text);
            }
            set
            {
                this.textBox_linesPerPage.Text = value.ToString();
            }
        }

        // 
        /// <summary>
        /// ժҪ��������
        /// </summary>
        public int MaxSummaryChars
        {
            get
            {
                if (this.textBox_maxSummaryChars.Text == "")
                    return 0;

                return Convert.ToInt32(this.textBox_maxSummaryChars.Text);
            }
            set
            {
                this.textBox_maxSummaryChars.Text = value.ToString();
            }
        }

        // 
        /// <summary>
        /// ��ô��������ʽ�����ַ�����
        /// ������� "xxxxx:xxxxx" ������ð���ұߵĲ�������
        /// </summary>
        /// <param name="strText">Ҫ������ַ���</param>
        /// <returns>����ð���ұߵĲ������ݡ����û��ð�ţ��򷵻�ԭ�ַ�������</returns>
        public static string GetSortColumnDefString(string strText)
        {
            int nRet = strText.IndexOf(":");

            if (nRet == -1)
                return strText;

            return strText.Substring(nRet + 1).Trim();
        }

        /// <summary>
        /// ����ʽ
        /// </summary>
        public string SortDef
        {
            get
            {
                return (string)this.listBox_sortDef.SelectedItem;
            }
            set
            {
                // ����ҵ�����ѡ����
                for (int i = 0; i < this.listBox_sortDef.Items.Count; i++)
                {
                    if (value == (string)this.listBox_sortDef.Items[i])
                    {
                        this.listBox_sortDef.SelectedIndex = i;
                        return;
                    }

                }

                // ����Ҳ������ͼ���
                this.listBox_sortDef.Items.Add(value);
                this.listBox_sortDef.SelectedIndex = this.listBox_sortDef.Items.Count - 1;
            }
        }

        private void textBox_linesPerPage_Validating(object sender,
        CancelEventArgs e)
        {
            if (this.textBox_linesPerPage.Text == "")
                return;

            try
            {
                int nValue = Convert.ToInt32(this.textBox_linesPerPage.Text);
            }
            catch
            {
                MessageBox.Show(this, "����������� '" + this.textBox_linesPerPage.Text + "' ��ʽ����ȷ");
                e.Cancel = true;
                return;
            }
        }

        private void textBox_maxSummaryChars_Validating(object sender, 
            CancelEventArgs e)
        {
            if (this.textBox_maxSummaryChars.Text == "")
                return;

            try
            {
                int nValue = Convert.ToInt32(this.textBox_maxSummaryChars.Text);
            }
            catch
            {
                MessageBox.Show(this, "����������� '" + this.textBox_maxSummaryChars.Text + "' ��ʽ����ȷ");
                e.Cancel = true;
                return;
            }
        }
    }
}