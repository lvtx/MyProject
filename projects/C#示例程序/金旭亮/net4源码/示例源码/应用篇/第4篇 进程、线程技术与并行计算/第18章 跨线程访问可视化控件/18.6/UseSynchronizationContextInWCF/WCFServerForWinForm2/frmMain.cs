using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.ServiceModel;

namespace WCFServerForWinForm2
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

           
            //通知服务对象，当用客户端调用时，调用此函数
            MyWCFService.UpdateUIFunc = UpdateCallCount;

            //启动WCF服务
            StartWCFService();

        }
        //启动WCF服务
        private void StartWCFService()
        {
            try
            {
                srvHost = new ServiceHost(
                    typeof(MyWCFService),
                    new Uri("net.tcp://localhost:8080/MyWCFService")
                    );
                srvHost.Open();
                lblStatus.Text = "服务已启动";
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }

        private ServiceHost srvHost = null;

        private void UpdateCallCount(int count)
        {
            lblInfo.Text = "客户端调用DoWork方法计数：" + count.ToString();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (srvHost != null)
                srvHost.Close();
        }
    }
}
