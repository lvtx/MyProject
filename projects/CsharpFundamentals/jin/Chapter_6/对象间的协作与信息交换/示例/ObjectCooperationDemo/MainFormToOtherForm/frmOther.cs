using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultiFormProgram1
{
    public partial class frmOther : Form
    {
        public frmOther()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 公有只写属性，用Label显示外界传入的信息
        /// </summary>
        public string Info
        {
            set
            {
                lblInfo.Text ="老大说："+ value;
            }
        }

        public void Receive(String Info)
        {
            lblInfo.Text = Info;
        }
    }
}
