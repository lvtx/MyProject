using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MouseLocation
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            lblInfo.Text = string.Format(
                "事件信息：({0},{1}) \n事件源自：{2}",
                e.X, e.Y,sender.GetType());
        }
    }
}
