using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
using Common;
using BLL;

namespace Web
{
    public partial class Exit : Form
    {
        public Exit()
        {
            InitializeComponent();
        }
        public Class c = null;
        public Department d = null;
        public ReaderType r = null;
        public Add aa = null;
        public BookType t = null;
        Com com = new Com();

        private void Exit_Load(object sender, EventArgs e)
        {
            if (t != null)
            {
                this.textBox1.Text = t.BookTypeName;
            }
            else if (r != null)
            {
                this.textBox1.Text = r.ReaderTypeName;
            }
            else if (d != null)
            {
                this.textBox1.Text = d.DepartmentName;
            }
            else if (c != null)
            {
                this.textBox1.Text = c.ClassName;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (t != null)
            {
                t.BookTypeName = textBox1.Text.Trim();
                if (aa.booktype.updateBookType(t) > 0)
                {
                    aa.dataGridView1.DataSource = aa.booktype.selectBookType1().Tables[0];
                    //自动找到刚刚修改成功的行，并选中
                    com.AutoFindRow(t.BookTypeId.ToString(), aa.dataGridView1);
                }
                else { MessageBox.Show("修改失败！"); }
            }
            if (r != null)
            {
                r.ReaderTypeName = textBox1.Text.Trim();
                if (aa.readerType_bll.updateReaderType(r) > 0)
                {
                    aa.dataGridView2.DataSource = aa.readerType_bll.selectReaderType1().Tables[0];
                    //自动找到刚刚修改成功的行，并选中
                    com.AutoFindRow(r.ReaderTypeId.ToString(), aa.dataGridView2);
                }
                else { MessageBox.Show("修改失败！"); }
            }
            if (d != null)
            {
                d.DepartmentName = textBox1.Text.Trim();
                if (aa.department_bll.updateDepartment(d) > 0)
                {
                    aa.dataGridView3.DataSource = aa.department_bll.selectDepartment1().Tables[0];
                    //自动找到刚刚修改成功的行，并选中
                    com.AutoFindRow(d.DepartmentId.ToString(), aa.dataGridView3);
                }
                else { MessageBox.Show("修改失败！"); }
            }
            if (c != null)
            {
                c.ClassName = textBox1.Text.Trim();
                if (aa.class_bll.updateClass(c) > 0)
                {
                    aa.dataGridView4.DataSource = aa.class_bll.selectClass1().Tables[0];
                    //自动找到刚刚修改成功的行，并选中
                    com.AutoFindRow(c.ClassId.ToString(), aa.dataGridView4);
                }
                else { MessageBox.Show("修改失败！"); }
            }
            this.Close();
        }
    }
}
