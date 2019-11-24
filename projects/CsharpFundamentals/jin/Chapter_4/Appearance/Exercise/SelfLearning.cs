using Exercise.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exercise
{
    public partial class SelfLearning : Form
    {
        public SelfLearning()
        {
            InitializeComponent();
            checkBox1.CheckState = CheckState.Checked;
            checkBox2.CheckState = CheckState.Unchecked;
            checkBox3.CheckState = CheckState.Indeterminate;
            pictureBox1.Image = Resources.hundred;
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //radioButton1.Checked = false;
            //radioButton2.Checked = true;
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //radioButton1.Checked = true;
            //radioButton2.Checked = false;
        }
    }
}
