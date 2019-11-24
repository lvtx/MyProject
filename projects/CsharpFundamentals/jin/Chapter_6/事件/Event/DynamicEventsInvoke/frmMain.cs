using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamicEventsInvoke
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void Event1(object sender, EventArgs e)
        {
            MessageBox.Show("事件处理程序一");
        }
        private void Event2(object sender, EventArgs e)
        {
            MessageBox.Show("事件处理程序二");
        }
        private void RemoveEvent()
        {
            Button1.Click -= Event1;
            Button1.Click -= Event2;
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RemoveEvent();
            Button1.Click += Event1;
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RemoveEvent();
            Button1.Click += Event2;
        }
    }
}
