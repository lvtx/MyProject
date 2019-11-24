using System;
using System.Windows.Forms;

namespace ButtonCounterInSingleForm
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        //定义一个内部实例字段
        private int counter = 0;
        //Click事件响应代码是类的实例方法
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            //实例方法内部可以直接访问实例字段
            counter++;
            //上面这句其实相当于：this.counter++;

            //标签实际上也是类的实例字段
            lblCount.Text = counter.ToString();
            //上面这句其实相当于：this.lblCount.Text=this.counter.ToString();
        }
       
    }
}
