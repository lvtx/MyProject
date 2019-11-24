using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonCounterForMultiFormUseReference
{
    public partial class frmOther : Form
    {
        //使用对象注入的方式，在主从对象之间建立关联
        public frmOther(frmMain main)
        {
            InitializeComponent();
            MainForm = main;
        }
        private frmMain MainForm = null;

        private void btnClickMe_Click(object sender, EventArgs e)
        {
            //调用主窗体的公有方法，显示按钮计数
            //此处无需向主窗体传送任何信息，因为我们只需要统计点击次数
            //如果确实有信息想发给主窗体，可以让主窗体定义一个有参数的公有方法或属性
            //在此处将信息以实参（或直接向主窗体公有属性赋值）的方式传送给主窗体
            if (MainForm != null)
            {
                MainForm.ShowCounter();
            }
        }
    }
}
