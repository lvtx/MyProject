using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContainerDemo
{
    public partial class frmPanel : Form
    {
        public frmPanel()
        {
            InitializeComponent();
        }

        private void btnShowOrHide_Click(object sender, EventArgs e)
        {
            if (rdoOuter.Checked)
            {
                pnlOuter.Visible = !pnlOuter.Visible;
                
            }
            else
            {
                pnlInner.Visible = !pnlInner.Visible;
            }
            ChangebtnShowOrHideText();
        }

        private void btnEnableOrDisable_Click(object sender, EventArgs e)
        {
            if (rdoOuter.Checked)
            {
                pnlOuter.Enabled = !pnlOuter.Enabled;
            }
            else
            {
                pnlInner.Enabled = !pnlInner.Enabled;
            }
            ChangebtnEnableOrDisableText();
        }

        private void ChangebtnShowOrHideText()
        {
            if (rdoOuter.Checked)
            {
                btnShowOrHide.Text = pnlOuter.Visible ? "隐藏" : "显示";
              
            }
            else
            {
                btnShowOrHide.Text = pnlInner.Visible ? "隐藏" : "显示";
            }
        }
        private void ChangebtnEnableOrDisableText()
        {
            if (rdoOuter.Checked)
            {
                btnEnableOrDisable.Text = pnlOuter.Enabled ? "禁用" : "激活";
            }
            else
            {
                btnEnableOrDisable.Text = pnlInner.Enabled ? "禁用" : "激活";
            }
        }

        private void rdoOuter_CheckedChanged(object sender, EventArgs e)
        {
            ChangebtnEnableOrDisableText();
            ChangebtnShowOrHideText();
        }

        private void rdoInner_CheckedChanged(object sender, EventArgs e)
        {
            ChangebtnEnableOrDisableText();
            ChangebtnShowOrHideText();
        }
    }
}
