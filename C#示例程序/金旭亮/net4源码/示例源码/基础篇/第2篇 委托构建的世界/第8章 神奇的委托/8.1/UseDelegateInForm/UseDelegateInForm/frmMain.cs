using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UseDelegateInForm
{
    public partial class frmMain : Form
    {

        //用标签显示信息,注意，此方法可以为Private的
        private  void Record(String info)
        {
            lblCount.Text = info;
        }

        public frmMain()
        {
            InitializeComponent();
            //创建从窗体对象并显示
            frmOther frm = new frmOther();
            frm.recorder = this.Record;  //向从窗体的委托变量赋值
            frm.Show();

        }

    
    }
}