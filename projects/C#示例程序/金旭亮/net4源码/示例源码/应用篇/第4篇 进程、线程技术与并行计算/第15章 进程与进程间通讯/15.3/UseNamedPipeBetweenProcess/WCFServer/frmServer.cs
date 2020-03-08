using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

namespace WCFServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class frmServer : Form, IWCFServerService
    {
        public frmServer()
        {
            InitializeComponent();
        }




        public void SaySomething(string ClientName, string Message)
        {
            richTextBox1.AppendText(ClientName + "说：" + Message + "\n");
        }
        ServiceHost host = null;
        private void frmServer_Load(object sender, EventArgs e)
        {
            try
            {
                host = new ServiceHost(this);
                host.Open();
                richTextBox1.AppendText("服务成功启动。\n");
            }
            catch (Exception ex)
            {

                richTextBox1.AppendText(ex.Message + "\n");
            }

        }


    }
}
