using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Xml;

namespace DigitalPlatform.DTLP
{
    /// <summary>
    /// ָ����֪���ݿ� �� ȱʡ���ط�ʽ�� �Ի���
    /// </summary>
    public partial class DefaultProjectDialog : Form
    {
        public DupCfgDialog DupCfgDialog = null;
        public XmlDocument dom = null;

        public DefaultProjectDialog()
        {
            InitializeComponent();
        }

        private void DefaultProjectDialog_Load(object sender, EventArgs e)
        {
            // ���û�и������ݿ�����������Ӧ�����Ա༭
            if (String.IsNullOrEmpty(this.DatabaseName) == true)
            {
                this.textBox_databaseName.ReadOnly = false;
                this.button_findDatabaseName.Enabled = true;
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button_findProjectName_Click(object sender, EventArgs e)
        {
            GetProjectNameDialog dlg = new GetProjectNameDialog();

            dlg.dom = this.dom;
            dlg.ProjectName = this.textBox_defaultProjectName.Text;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);

            if (dlg.DialogResult != DialogResult.OK)
                return;

            this.textBox_defaultProjectName.Text = dlg.ProjectName;
        }

        public string DatabaseName
        {
            get
            {
                return this.textBox_databaseName.Text;
            }
            set
            {
                this.textBox_databaseName.Text = value;
            }
        }

        public string DefaultProjectName
        {
            get
            {
                return this.textBox_defaultProjectName.Text;
            }
            set
            {
                this.textBox_defaultProjectName.Text = value;
            }
        }

        private void button_findDatabaseName_Click(object sender, EventArgs e)
        {
            // ��Ҫ��DTLP��Դ�Ի�����Ҫ��DtlpChannels��֧��
            if (this.DupCfgDialog == null)
            {
                MessageBox.Show(this, "DupCfgDialog��ԱΪ�գ��޷���ѡ��Ŀ�����ݿ�ĶԻ���");
                return;
            }

            GetDtlpResDialog dlg = new GetDtlpResDialog();

            dlg.Text = "��ѡ�����ݿ�";
            dlg.Initial(this.DupCfgDialog.DtlpChannels,
                this.DupCfgDialog.DtlpChannel);
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.Path = this.textBox_databaseName.Text;
            dlg.EnabledIndices = new int[] { DtlpChannel.TypeStdbase };
            dlg.ShowDialog(this);

            this.textBox_databaseName.Text = dlg.Path;
        }


    }
}