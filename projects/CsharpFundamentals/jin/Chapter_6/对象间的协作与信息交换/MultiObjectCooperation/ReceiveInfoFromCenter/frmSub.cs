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
    public partial class frmSub : Form
    {
        
        public frmSub()
        {
            InitializeComponent();
            identityCode = IdentityCode.SubForm;
        }
        public IdentityCode identityCode { get; }
        /// <summary>
        /// 其他窗体调用
        /// </summary>
        /// <param name="counter"></param>
        public void ShowCounter(int counter)
        {
            lblInfo.Text = counter.ToString();
        }
        public Center<IContainerControl> center = new Center<IContainerControl>();
        private int counter = 0;
        private void btnIncreaseCounter_Click(object sender, EventArgs e)
        {
            counter++;
            foreach (var item in center.forms)
            {
                if ((item as frmMain).identityCode == IdentityCode.MainForm)
                {
                    (item as frmMain).ShowCounter(counter);
                }
            }
        }

        private void btnNewForm_Click(object sender, EventArgs e)
        {
            center.AddForm(new frmMain());
            (center.CurrentForm as frmMain).Show();
        }
    }
}
