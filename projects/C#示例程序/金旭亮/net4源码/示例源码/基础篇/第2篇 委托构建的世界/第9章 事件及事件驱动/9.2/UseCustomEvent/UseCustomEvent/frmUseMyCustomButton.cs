using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UseCustomEvent
{
    public partial class frmUseMyCustomButton : Form
    {
        public frmUseMyCustomButton()
        {
            InitializeComponent();
            //挂接事件响应
            myCustomButton1.MyClick += new MyClickEventHandler(myCustomButton1_MyClick);
            myCustomButtonUseGenericDelegate1.MyClick += new EventHandler<MyClickEventArgs>(myCustomButtonUseGenericDelegate1_MyClick);
        }

        void myCustomButtonUseGenericDelegate1_MyClick(object sender, MyClickEventArgs e)
        {
                   MessageBox.Show("I'm clicked " + e.ClickCount.ToString() + " times");
        }

        void myCustomButton1_MyClick(object sender, MyClickEventArgs e)
        {
            MessageBox.Show("I'm clicked " + e.ClickCount.ToString() + " times");
        }
    }
}
