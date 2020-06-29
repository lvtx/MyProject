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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            
        }

        private void CreateProxy()
        {
            EndpointAddress ep = new EndpointAddress("net.tcp://localhost:8080/MyWCFService");

            ChannelFactory<IMyWCFService> factory = new ChannelFactory<IMyWCFService>(
                new NetTcpBinding(), ep);
            proxy = factory.CreateChannel();
        }

        IMyWCFService proxy = null;
        private void btnVisitWCFService_Click(object sender, EventArgs e)
        {
            VisitWCFService();
        }

        private void VisitWCFService()
        {
            try
            {
                if(proxy==null)
                    CreateProxy();
                lblInfo.Text = proxy.DoWork();
            }
            catch (Exception ex)
            {

                lblInfo.Text = ex.Message;
            }

        }

      
    }
}
