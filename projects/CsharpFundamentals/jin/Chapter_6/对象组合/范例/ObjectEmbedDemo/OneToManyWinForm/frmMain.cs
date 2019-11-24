using System;
using System.Windows.Forms;

namespace OneToManyWinForm
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddItem();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }
        /// <summary>
        /// 添加选项
        /// </summary>
        private void AddItem()
        {
            //不允许加空项
            if (txtUserInput.Text.Trim() != "")
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
            if (listBox1.SelectedIndex != -1)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }
        /// <summary>
        /// 显示列表框中的当前选中项
        /// </summary>
        private void ShowSelectedItem()
        {
            String SelectedItem = listBox1.SelectedItem == null ? "无" : listBox1.SelectedItem.ToString();
            lblInfo.Text = String.Format("当前选中项为：{0}", SelectedItem);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSelectedItem();
        }

        private void txtUserInput_KeyDown(object sender, KeyEventArgs e)
        {
            //回车时直接加入列表
            if (e.KeyCode == Keys.Enter)
            {
                AddItem();
            }
        }
    }
}
