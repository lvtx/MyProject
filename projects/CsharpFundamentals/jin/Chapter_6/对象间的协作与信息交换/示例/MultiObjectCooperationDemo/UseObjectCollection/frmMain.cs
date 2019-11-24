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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnNewForm_Click(object sender, EventArgs e)
        {
            NewForm();
        }
        //窗体对象集合
        private List<frmOther> OtherForms = new List<frmOther>();
        private void NewForm()
        {
            //每新建一个窗体，就将其加入到OtherForms对象集合中
            frmOther frm = new frmOther();
            OtherForms.Add(frm);
            frm.Show();
        }

        private int counter = 0;//计数器
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            counter++;
            //遍历从窗体对象集合，逐个回调其ShowCounter方法，计数器新值成为其参数
            foreach (frmOther frm in OtherForms)
            {
                frm.ShowCounter(counter);
            }

        }
    }



}
