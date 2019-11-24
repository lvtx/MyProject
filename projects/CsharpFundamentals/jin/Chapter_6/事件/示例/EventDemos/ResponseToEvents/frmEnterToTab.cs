using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ResponseToEvents
{
    public partial class frmEnterToTab : Form
    {
        public frmEnterToTab()
        {
            InitializeComponent();
            //挂接事件
            foreach (Control ctl in groupBox1.Controls)
            {
                if (ctl is TextBox)
                    (ctl as TextBox).KeyDown += this.EnterToTab;
            }
        }
        //回车移动焦点
        private void EnterToTab(object sender, KeyEventArgs e)
        {
            TextBox txt=null;
            if (e.KeyCode == Keys.Enter) 
            {
                groupBox1.SelectNextControl(sender as Control  , true, true, true, true);
                //拥有焦点的文本框自动全选
                txt = (sender as TextBox);
                if(txt!=null)
                    txt.SelectAll();
            }
        }       
    }
}