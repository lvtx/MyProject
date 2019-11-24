using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DynamicEventsInvoke
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
         
        }
        
      
        //定义两个事件响应函数
        private void Event1(Object sender,EventArgs e)
        {
            MessageBox.Show("事件处理程序一");
        }
        private void Event2(Object sender,EventArgs e)
        {
            MessageBox.Show("事件处理程序二");
        }

       
        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RemoveAllHandler();
            Button1.Click += Event1;
         

        }

        private void RemoveAllHandler()
        {
        
            Button1.Click -= Event1;
            Button1.Click -= Event2;
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RemoveAllHandler();
            Button1.Click += Event2;

        }
    }
}