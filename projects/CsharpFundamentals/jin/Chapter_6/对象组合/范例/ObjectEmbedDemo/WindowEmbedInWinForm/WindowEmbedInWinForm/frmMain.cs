using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        frmBottom bottomForm = new frmBottom();
        frmCenter centerForm = new frmCenter();
        frmLeft leftForm = new frmLeft();
        frmRight rightForm = new frmRight();

        private void btnCompose_Click(object sender, EventArgs e)
        {
            Compose();
        }
        
        private void Compose()
        {
            bottomForm.UIControlPanel.Parent = bottomPanel;
            centerForm.UIControlPanel.Parent = CenterPanel;
            leftForm.UIControlPanel.Parent = leftPanel;
            rightForm.UIControlPanel.Parent = rightPanel;
        }

        private void Restore()
        {
            bottomForm.UIControlPanel.Parent = bottomForm;
            centerForm.UIControlPanel.Parent = centerForm;
            leftForm.UIControlPanel.Parent = leftForm;
            rightForm.UIControlPanel.Parent = rightForm;
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            Restore();
        }



    }
}
