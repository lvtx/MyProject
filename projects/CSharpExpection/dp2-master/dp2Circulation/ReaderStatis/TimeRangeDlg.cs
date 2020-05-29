using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dp2Circulation
{
    internal partial class TimeRangeDlg : Form
    {
        public bool AllowStartDateNull = false; // �Ƿ�������ʼʱ��Ϊ��?
        public bool AllowEndDateNull = false; // �Ƿ��������ʱ��Ϊ��?

        public TimeRangeDlg()
        {
            InitializeComponent();
        }

        public DateTime StartDate
        {
            get
            {
                return this.dateControl_start.Value;
            }
            set
            {
                this.dateControl_start.Value = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this.dateControl_end.Value;
            }
            set
            {
                this.dateControl_end.Value = value;
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (AllowStartDateNull == false)
            {
                if (this.dateControl_start.Value == new DateTime((long)0))
                {
                    MessageBox.Show(this, "��δָ���������");
                    return;
                }
            }

            if (AllowEndDateNull == false)
            {
                if (this.dateControl_end.Value == new DateTime((long)0))
                {
                    MessageBox.Show(this, "��δָ���յ�����");
                    return;
                }
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