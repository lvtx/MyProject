using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Wizard
{
    public partial class frmStep1 : Wizard.frmBase
    {
        public frmStep1()
        {
            InitializeComponent();
        }
        protected override void UpdateInfo()
        {
            controller.info.Name = txtName.Text;
            controller.info.IsMale = rdoMale.Checked;
        }
    }
}
