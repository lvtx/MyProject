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
    public partial class ReaderAdd_UI : Form
    {
        public ReaderAdd_UI()
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

        Reader_BLL reader_bll = new Reader_BLL();
        ReaderType_BLL readerType_bll = new ReaderType_BLL();
        Department_BLL department_bll = new Department_BLL();
        Class_BLL class_bll = new Class_BLL();
        Com com = new Com();

        //加载
        private void ReaderAdd_UI_Load(object sender, EventArgs e)
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
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        //添加读者类型
        private void btnReaderType_Click(object sender, EventArgs e)
        {
            Add a = new Add();
            a.tabName = "读者类型";
            a.readerManager = this.reader;
            a.readerAdd = this;
            a.ShowDialog();
        }
        //添加院系
        private void btnDepartment_Click(object sender, EventArgs e)
        {
            Add a = new Add();
            a.tabName = "院系";
            a.readerManager = this.reader;
            a.readerAdd = this;
            a.ShowDialog();
        }
        //添加班级
        private void btnClass_Click(object sender, EventArgs e)
        {
            Add a = new Add();
            a.tabName = "班级";
            a.readerManager = this.reader;
            a.readerAdd = this;
            a.ShowDialog();
        }
        //新增读者信息
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //图书编号必须唯一
            int rows = reader.dgvHeaderInfo.RowCount;
            lab.Text = "";
            for (int i = 0; i < rows; i++)
            {
                string Columns1 = reader.dgvHeaderInfo.Rows[i].Cells[0].Value.ToString();
                if (Columns1 == txtReaderId.Text.Trim())
                {
                    lab.Text = "读者编号已存在！";
                    return;
                }
            }
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
            r.Phone = txtPhone.Text.Trim();
            r.Special = txtSpecial.Text.Trim();
            r.Email = txtEmail.Text.Trim();
            r.Address = txtAddress.Text.Trim();
            r.ReaderRemark = txtRemark.Text.Trim();

            if (reader_bll.addReader(r) == 0)
            {
                MessageBox.Show("新增成功！");
                //单击查询 刷新读者信息表
                reader.btnSelect_Click(null, null);

                //自动找到刚刚添加成功的新行，并选中
                com.AutoFindRow(txtReaderId.Text.Trim(), reader.dgvHeaderInfo);
            }
            else
            {
                MessageBox.Show("新增失败！");
            }
        }

    }
}
