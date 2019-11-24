using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowEmbedInWinForm
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        //将被动态嵌入的窗体对象
        frmLeft leftForm = new frmLeft();
        frmBottom bottomForm = new frmBottom();
        frmRight rightForm = new frmRight();
        frmCenter centerForm = new frmCenter();
        private void Compose()
        {
            leftForm.UIControlPanel.Parent = leftPanel;
            rightForm.UIControlPanel.Parent = rightPanel;
            bottomForm.UIControlPanel.Parent = bottomPanel;
            centerForm.UIControlPanel.Parent = centerPanel;
        }
        private void Restore()
        {
            leftForm.UIControlPanel.Parent = leftForm;
            rightForm.UIControlPanel.Parent = rightForm;
            bottomForm.UIControlPanel.Parent = bottomForm;
            centerForm.UIControlPanel.Parent = centerForm;
        }

        private void btnCompose_Click(object sender, EventArgs e)
        {
            Compose();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            Restore();
        }
    }
}
