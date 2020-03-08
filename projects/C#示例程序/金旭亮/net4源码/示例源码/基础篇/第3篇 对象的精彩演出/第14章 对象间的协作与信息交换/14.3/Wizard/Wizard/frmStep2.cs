using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Wizard
{
    public partial class frmStep2 : Wizard.frmBase
    {
        public frmStep2()
        {
            InitializeComponent();
        }
        protected override void UpdateInfo()
        {
            controller.info.EduBackground = cboEduBackground.Items[cboEduBackground.SelectedIndex].ToString();


            string lang = "";

            foreach (Control ctl in Panel2.Controls)
            {
                if (ctl.GetType().Name == "CheckBox")
                {
                    if ((ctl as CheckBox).Checked)
                        lang += ctl.Text + "  ";

                }

            }
            controller.info.ProgrameLanguage = lang;
        }

        private void frmStep2_Load(object sender, EventArgs e)
        {
            this.cboEduBackground.SelectedIndex = 0;
        }
    }
}
