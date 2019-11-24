using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassAndObject
{
    public partial class ParentForm : Form
    {
        public ParentForm()
        {
            InitializeComponent();
        }

        private void BtnShowSubForm_Click(object sender, EventArgs e)
        {
            SubForm subForm = new SubForm();
            subForm.Show();
        }
    }
}
