using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContainerDemo
{
    public partial class frmTabControl : Form
    {
        public frmTabControl()
        {
            InitializeComponent();
            tabCount = tabControl1.TabPages.Count;
        }

        private int tabCount = 0;
        private Random ran = new Random();
        private void btnAddTab_Click(object sender, EventArgs e)
        {
            tabCount++;
            TabPage newPage = new TabPage("tabPage" + tabCount);
            newPage.BackColor = Color.FromArgb(
                ran.Next(0,255),
                ran.Next(0,255),
                ran.Next(0,255));
            tabControl1.TabPages.Add(newPage);

        }

        private void btnActiveLeft_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != 0)
            {
                tabControl1.SelectTab(tabControl1.SelectedIndex - 1);
            }
        }

        private void btnActiveRight_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != tabControl1.TabPages.Count - 1)
            {
                tabControl1.SelectTab(tabControl1.SelectedIndex + 1);
            }
        }
    }
}
