using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiFormProgram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnShowForm2_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            //以showDialog()方式显示的窗体，不关闭它，主窗体将无法响应鼠标点击
            frm.ShowDialog();
            //以Show()方法显示的窗体，主窗体和新显示的窗体都可以被激活
            //frm.Show();
        }
    }
}

