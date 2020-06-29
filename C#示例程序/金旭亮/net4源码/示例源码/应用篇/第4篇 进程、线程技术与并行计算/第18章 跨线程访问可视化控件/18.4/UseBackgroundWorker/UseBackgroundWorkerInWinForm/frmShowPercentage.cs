using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace UseBackgroundWorker
{
    public partial class frmShowPercentage : Form
    {
        public frmShowPercentage()
        {
            InitializeComponent();
        }
      
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //��ȡBackgroundWorker���������
           BackgroundWorker bw=sender as BackgroundWorker;
           
            //��ʼ����1��2��������100
            int result = 0; //�ݴ���
            for (int i = 0; i <=100; i++)
            {
                //����û�ȡ���˲���
                if (bw.CancellationPending)
                {
                    e.Cancel = true;//�˽�����ᱻ���͵�RunWorkerCompleted�¼���
                    return ; //��ǰ������������
                }
                result += i; //�ۼ�
                
                //�������
                bw.ReportProgress(i,"�������"+(i).ToString()+"%");
                Thread.Sleep((int)e.Argument); //�ⲿ����Ĳ����������ض���ʱ��
            }

            e.Result = result; //����緵��1��2��������100��ֵ

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;//��ʾ�ٷֱ�
            lblInfo.Text = e.UserState.ToString(); //��ʾ������Ϣ
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                toolStripStatusLabel1.Text = "�û�ȡ���˲���";
                btnStart.Enabled = true;
                return;
            }
            if (e.Error != null)
            {
                toolStripStatusLabel1.Text = e.Error.Message;
                return;
            }
            lblResult.Text = e.Result.ToString();
            btnStart.Enabled = true;
            toolStripStatusLabel1.Text = "�������";

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            lblResult.Text = "?";
            lblResult.Refresh();
            backgroundWorker1.RunWorkerAsync(200);
            btnStart.Enabled = false;
            toolStripStatusLabel1.Text = "���ڹ�������";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

  
}