using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using Model;
using Common;

namespace Web
{
    public partial class ReaderExit_UI : Form
    {
        public ReaderExit_UI()
        {
            InitializeComponent();
        }
        private void button3_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).FlatStyle = FlatStyle.Standard;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).FlatStyle = FlatStyle.Flat;
        }

        public ReaderManager_UI reader = null;
        public string ReaderId = null;

        Reader_BLL reader_bll = new Reader_BLL();
        ReaderType_BLL readerType_bll = new ReaderType_BLL();
        Department_BLL department_bll = new Department_BLL();
        Class_BLL class_bll = new Class_BLL();
        Com com = new Com();

        public void ReaderExit_UI_Load(object sender, EventArgs e)
        {
            //读者类型的下拉框绑定
            cboReaderType.DataSource = readerType_bll.selectReaderType();
            cboReaderType.DisplayMember = "ReaderTypeName";
            cboReaderType.ValueMember = "ReaderTypeId";

            //院系的下拉框绑定
            cboDepartment.DataSource = department_bll.selectDepartment();
            cboDepartment.DisplayMember = "DepartmentName";
            cboDepartment.ValueMember = "DepartmentId";
            //班级的下拉框绑定
            cboClass.DataSource = class_bll.selectClass();
            cboClass.DisplayMember = "ClassName";
            cboClass.ValueMember = "ClassId";

            List<Reader> list = reader_bll.selectReader1(ReaderId);
            txtReaderId.Text = list[0].ReaderId;
            txtReaderName.Text = list[0].ReaderName;
            dtTimeIn.Value = list[0].TimeIn;
            dtTimeOut.Value = list[0].TimeOut;
            cboReaderType.SelectedValue = list[0].ReaderTypeId;
            cboDepartment.SelectedValue = list[0].DepartmentId;
            cboClass.SelectedValue = list[0].ClassId;
            txtIdentityCard.Text = list[0].IdentityCard;
            txtGender.Text = list[0].Gender;
            txtSpecial.Text = list[0].Special;
            txtPhone.Text = list[0].Phone;
            txtEmail.Text = list[0].Email;
            txtAddress.Text = list[0].Address;
            txtRemark.Text = list[0].ReaderRemark;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Reader r = new Reader();
            r.ReaderId = txtReaderId.Text.Trim();
            r.ReaderName = txtReaderName.Text.Trim();
            r.TimeIn = dtTimeIn.Value;
            r.TimeOut = dtTimeOut.Value;
            r.ReaderTypeId = (int)cboReaderType.SelectedValue;
            r.DepartmentId = (int)cboDepartment.SelectedValue;
            r.ClassId = (int)cboClass.SelectedValue;
            r.IdentityCard = txtIdentityCard.Text.Trim();
            r.Gender = txtGender.Text.Trim();
            r.Special = txtSpecial.Text.Trim();
            r.Phone = txtPhone.Text.Trim();
            r.Email = txtEmail.Text.Trim();
            r.Address = txtAddress.Text.Trim();
            r.ReaderRemark = txtRemark.Text.Trim();

            if (reader_bll.updateReader(r) > 0)
            {
                MessageBox.Show("修改成功！");
                //单价查询
                reader.btnSelect_Click(null, null);

                //自动找到刚刚修改成功的行，并选中
                com.AutoFindRow(txtReaderId.Text.Trim(), reader.dgvHeaderInfo);
            }
            else
            {
                MessageBox.Show("修改失败！");
            }
        }
        //读者类型编辑
        private void button4_Click(object sender, EventArgs e)
        {
            Add a = new Add();
            a.tabName = "读者类型";
            a.readerExit = this;
            a.readerManager = this.reader;
            a.ShowDialog();
        }
        //院系编辑
        private void button3_Click(object sender, EventArgs e)
        {
            Add a = new Add();
            a.tabName = "院系";
            a.readerExit = this;
            a.readerManager = this.reader;
            a.ShowDialog();
        }
        //班级编辑
        private void button5_Click(object sender, EventArgs e)
        {
            Add a = new Add();
            a.tabName = "班级";
            a.readerExit = this;
            a.readerManager = this.reader;
            a.ShowDialog();
        }

    }
}
