using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseObjectCollection
{
    public partial class frmOther : Form
    {
        public frmOther()
        {
            InitializeComponent();
        }
        //供主窗体“回调”，显示主窗体传入的计数器值
        public void ShowCounter(int counter)
        {
            lblInfo.Text = counter.ToString();

        }
    }
}
