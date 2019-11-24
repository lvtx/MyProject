using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/*用户点击“>>>”或“<<<”，将当前选中的指定的列表项移到另一个列表控件中*/
namespace Exercise
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            AddItem("one");
            AddItem("two");
            AddItem("three");
            AddItem(1);
            AddItem(2);
            AddItem(3);
        }
        private void AddItem<T>(T item)
        {
            if (typeof(T) == typeof(int))
                lbRight.Items.Add(item);
            
            if (typeof(T) == typeof(string))
                lbLeft.Items.Add(item);
            else
                Console.WriteLine("添加元素时出错");
        }

        private void btnLeftToRight_Click(object sender, EventArgs e)
        {
            var tempItem = lbLeft.SelectedItem;
            lbLeft.Items.RemoveAt(lbLeft.SelectedIndex);
            lbRight.Items.Add(tempItem);
        }

        private void btnRightToLeft_Click(object sender, EventArgs e)
        {
            var tempItem = lbRight.SelectedItem;
            lbRight.Items.RemoveAt(lbRight.SelectedIndex);
            lbLeft.Items.Add(tempItem);
        }
    }
}
