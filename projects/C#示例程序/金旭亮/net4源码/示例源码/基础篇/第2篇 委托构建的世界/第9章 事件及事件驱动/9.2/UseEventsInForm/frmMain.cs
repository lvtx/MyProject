using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UseEventsInForm
{
    public partial class frmMain : Form
    {

        //用标签显示信息,注意，此方法可以为Private的
        private  void ShowCount(String count)
        {
            lblCount.Text = count;
        }

        public frmMain()
        {
            InitializeComponent();
            //创建从窗体对象并显示
            frmOther frm = new frmOther();
            frm.ButtonClicked += ShowCount; //挂接事件
            frm.Show();
        } 
    }
}