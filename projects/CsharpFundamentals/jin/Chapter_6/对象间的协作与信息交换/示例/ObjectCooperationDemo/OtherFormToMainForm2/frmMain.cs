using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtherFormToMainForm2
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnShowDialog_Click(object sender, EventArgs e)
        {
            //主窗体把自己的引用传给从窗体对象
            frmOther frm = new frmOther(this);
            frm.Show();
        }
        /// <summary>
        /// 供从窗体调用，向主窗体“汇报”工作完成情况
        /// </summary>
        /// <param name="Info"></param>
        public void Report(String Info)
        {
            lblInfo.Text = Info;
        }
    }
}
