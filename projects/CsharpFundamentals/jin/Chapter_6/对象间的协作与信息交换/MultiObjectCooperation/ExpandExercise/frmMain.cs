using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpandExercise
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }        
        public Color MySelectColor { get; set; }
        private List<frmSub> mySubs = new List<frmSub>();

        private void SelectColor()
        {
            colorDialog1.ShowDialog();
            MySelectColor = colorDialog1.Color;            
            foreach (var item in mySubs)
            {
                item.SetColor();
            }
        }

        private void NewSubForm()
        {
            frmSub mySub = new frmSub(this);
            mySubs.Add(mySub);
            mySub.Show();
            mySub.SetColor();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectColor();
        }

        private void btnNewSubForm_Click(object sender, EventArgs e)
        {
            NewSubForm();
        }
    }
}
