using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wizard
{
    public partial class frmTestWizard : Form
    {
        public frmTestWizard()
        {
            InitializeComponent();
        }
        //向导控件对象
        private WizardController wizard = null;
        private void ShowInfo()
        {
            if ((wizard == null) || (wizard.info == null))
                return;

            if (wizard.info.IsMale)
                lblIsMale.Text = "男";
            else
                lblIsMale.Text = "女";

            lblEduBackground.Text = wizard.info.EduBackground;
            lblProgramLanguage.Text = wizard.info.ProgrameLanguage;
            lblName.Text = wizard.info.Name;

        }

        private void btnShowWizard_Click(object sender, EventArgs e)
        {
            wizard = new WizardController();
            wizard.BeginWizard();
        }

        private void btnShowInfo_Click(object sender, EventArgs e)
        {
            ShowInfo();
        }

       
    }
}
