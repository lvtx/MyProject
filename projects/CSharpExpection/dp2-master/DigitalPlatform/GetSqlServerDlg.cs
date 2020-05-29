using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.Sql;

namespace DigitalPlatform
{
    public partial class GetSqlServerDlg : Form
    {
        public GetSqlServerDlg()
        {
            InitializeComponent();
        }

        private void SqlServerDlg_Load(object sender, EventArgs e)
        {
            // FillList();

            this.BeginInvoke(new Delegate_FillList(FillList));
        }

        public delegate void Delegate_FillList();

        void FillList()
        {
            EnableControls(false);
            this.listView_sqlServers.Items.Clear();
            ListViewItem item = new ListViewItem("���ڻ�ȡSQL��������Ϣ ...");
            this.listView_sqlServers.Items.Add(item);

            this.Update();

            this.listView_sqlServers.Items.Clear();

            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;

            System.Data.DataTable table = instance.GetDataSources();


            foreach (System.Data.DataRow row in table.Rows)
            {
                string strServerName = row["ServerName"].ToString();
                string strInstanceName = row["InstanceName"].ToString();
                string strIsClustered = row["IsClustered"].ToString();
                string strVersion = row["Version"].ToString();

                item = new ListViewItem(strServerName);

                item.SubItems.Add(strInstanceName);
                item.SubItems.Add(strIsClustered);
                item.SubItems.Add(strVersion);
                this.listView_sqlServers.Items.Add(item);

                // ����ͱ��ؼ��������ͬ������û��instancename������������Ӵ�
                if (strServerName == SystemInformation.ComputerName
                    && String.IsNullOrEmpty(strInstanceName) == true)
                    item.Font = new Font(item.Font, FontStyle.Bold);
            }

            EnableControls(true);
        }

        /// <summary>
        /// ������߽�ֹ����ؼ����ڳ�����ǰ��һ����Ҫ��ֹ����ؼ���������ɺ�������
        /// </summary>
        /// <param name="bEnable">�Ƿ��������ؼ���true Ϊ���� false Ϊ��ֹ</param>
        public void EnableControls(bool bEnable)
        {
            this.textBox_sqlServerName.Enabled = bEnable;
            this.listView_sqlServers.Enabled = bEnable;
            this.button_Cancel.Enabled = bEnable;
            this.button_OK.Enabled = bEnable;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.textBox_sqlServerName.Text))
            {
                MessageBox.Show(this, "��δָ��SQL��������");
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

        private void listView_sqlServers_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (this.listView_sqlServers.SelectedItems.Count == 0)
                this.textBox_sqlServerName.Text = "";
            else
            {
                string strServerName = this.listView_sqlServers.SelectedItems[0].Text;
                if (this.listView_sqlServers.SelectedItems[0].SubItems[1].Text != "")
                    strServerName += "\\" + this.listView_sqlServers.SelectedItems[0].SubItems[1].Text;

                this.textBox_sqlServerName.Text = strServerName;
            }
        }

        // ˫��
        private void listView_sqlServers_DoubleClick(object sender, EventArgs e)
        {
            button_OK_Click(sender, e);
        }
    }
}