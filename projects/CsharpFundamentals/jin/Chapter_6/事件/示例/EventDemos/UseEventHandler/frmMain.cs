using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseEventHandler
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            //运行时才设定事件响应函数
            button1.Click += new EventHandler(button1_Click);
        }

        void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("I'm Clicked!");
        }

    }
}
