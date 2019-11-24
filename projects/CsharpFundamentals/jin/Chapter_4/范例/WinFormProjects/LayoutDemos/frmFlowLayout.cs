using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LayoutDemos
{
    public partial class frmFlowLayout : Form
    {
        public frmFlowLayout()
        {
            InitializeComponent();
        }

        private int counter = 0;
        private void btnAddButton_Click(object sender, EventArgs e)
        {
            counter++;
            Button btn = new Button();
            btn.Text = "按钮" + counter;
            flowLayoutPanel1.Controls.Add(btn);
        }

        private void chkWrapContents_CheckedChanged(object sender, EventArgs e)
        {
            
                flowLayoutPanel1.WrapContents = chkWrapContents.Checked;
            
        }

        private void chkAutoScroll_CheckedChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.AutoScroll = chkAutoScroll.Checked;
        }

        private void cboFlowDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboFlowDirection.Text)
            {
                case "BottomUp":
                    flowLayoutPanel1.FlowDirection = FlowDirection.BottomUp;
                    break;
                case "LeftToRight":
                    flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
                    break;
                case "RightToLeft":
                    flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
                    break;
                case "TopDown":
                    flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
                    break;
                default:
                    break;
            }
        }
    }
}
