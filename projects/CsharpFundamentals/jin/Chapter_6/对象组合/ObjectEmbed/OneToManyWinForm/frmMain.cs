using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneToManyWinForm
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 添加选项
        /// </summary>
        private void AddItem()
        {
            //不允许添加空项
            if(txtUserInput.Text.Trim() != "")
            {
                listBox1.Items.Add(txtUserInput.Text);
                txtUserInput.Text = "";
            }
        }
        /// <summary>
        /// 删除选项
        /// </summary>
        private void DeleteItem()
        {
            if(listBox1.SelectedIndex != -1)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }
        /// <summary>
        /// 显示列表框中的当前选中项
        /// </summary>
        private void ShowSelectedItem()
        {
            string SelectedItem = listBox1.SelectedItem == null ? "无" : listBox1.SelectedItem.ToString();
            lblInfo.Text = string.Format("当前选中项为{0}", listBox1.SelectedItem);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddItem();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSelectedItem();
        }

        private void txtUserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                AddItem();
            }
        }
    }
}
