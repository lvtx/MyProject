using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SplashScreenForWinForm
{

    public partial class frmSplash : Form
    {
        public frmSplash()
        {
            InitializeComponent();

            //��ʾ��ʼ�����̣������ڴ˶���������Ҫ����ʾ��ʽ
            ShowProgress = delegate(String ProgressInfo)
            { 
                lblInfo.Text = String.Format("���ڳ�ʼ����{0}%", ProgressInfo);
                lblInfo.Refresh(); //ǿ�Ʊ�ǩ�����ػ棬�Լ�ʱ��ʾ����̴ܶ������Ϣ
                progressBar1.Value = Convert.ToInt32(ProgressInfo);
            };

            CloseSplash = this.Close;

        }

            //��ʾ��ʼ������,��������
            public  Action<string> ShowProgress ;
            //�ر�Splash���壬��������
            public  Action CloseSplash;

        private void frmSplash_Load(object sender, EventArgs e)
        {
            //֪ͨ���߳����������Ѵ������,�Ӷ������߳̿�ʼ������ʼ������
            Program.mre.Set();
        }

 
   }
}