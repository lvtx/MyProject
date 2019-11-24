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
    public partial class frmTableLayout : Form
    {
        public frmTableLayout()
        {
            InitializeComponent();
        }

        private int counter = 5;
        private int colCount = 2;
        private int rolCount = 3;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            counter++;
            Button btn = new Button();
            btn.Text = "button" + counter;
            
            
            tableLayoutPanel1.Controls.Add(btn,0,rolCount);
            rolCount++;

        }
    }
}
