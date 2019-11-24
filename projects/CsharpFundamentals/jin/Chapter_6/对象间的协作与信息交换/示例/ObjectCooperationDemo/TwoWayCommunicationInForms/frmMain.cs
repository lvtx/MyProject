using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TwoWayCommunicationInForms
{
    public partial class frmMain : Form
    {
        private frmOther frm = null; //引用从窗体
        public frmMain()
        {
            InitializeComponent();
            frm = new frmOther(this);
            frm.Show();
        }

      
        /// <summary>
        /// 向外界公开的方法
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(int value)
        {
            NumericUpDown1.Value = value;
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //通过从窗体的公有属性向其传送信息
            frm.WorkDownPercent = (int)NumericUpDown1.Value;
        }
    }
}
