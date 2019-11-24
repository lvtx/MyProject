using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReceiveInfoFromCenter
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            identityCode = IdentityCode.MainForm;
        }
        private int counter = 0;
        public IdentityCode identityCode { get; }
        
        private void btnNewForm_Click(object sender, EventArgs e)
        {
            Program.center.AddForm(new frmSub());
            center.CurrentForm.Show();
        }
        /// <summary>
        /// 其他窗体调用
        /// </summary>
        /// <param name="counter"></param>
        public void ShowCounter(int counter)
        {
            txtInfo.Text = counter.ToString();
        }
        private void btnClickMe_Click(object sender, EventArgs e)
        {
            counter++;
            foreach (var item in center.forms)
            {
                if (item.identityCode == IdentityCode.SubForm)
                {
                    item.ShowCounter(counter);
                }
            }
        }
    }
}
