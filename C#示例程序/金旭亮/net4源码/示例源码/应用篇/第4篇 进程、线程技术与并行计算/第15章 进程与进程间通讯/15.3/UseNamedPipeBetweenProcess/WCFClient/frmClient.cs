using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

namespace WCFClient
{
    public partial class frmClient : Form
    {
        public frmClient()
        {
            InitializeComponent();
        }

        private IWCFServerService proxy = null;
        private void frmClient_Load(object sender, EventArgs e)
        {
            ConnectServer();
        }
        /// <summary>
        /// 尝试连接数据库
        /// </summary>
        private void ConnectServer()
        {
            try
            {
                EndpointAddress ep = new EndpointAddress("net.pipe://localhost/WCFServer");
                NetNamedPipeBinding binding = new NetNamedPipeBinding();
                proxy = ChannelFactory<IWCFServerService>.CreateChannel(binding, ep);
            }
            catch (Exception ex)
            {
                proxy = null;
                MessageBox.Show(ex.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (proxy != null)
                    proxy.SaySomething(txtClientName.Text, txtMessage.Text);
                else
                    ConnectServer();
            }
           catch (Exception ex)
            {
                proxy = null;
                MessageBox.Show(ex.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
