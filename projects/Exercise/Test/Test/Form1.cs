using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入数据库名称");
            }
            else
            {
                try
                {
                    //声明一个字符串，用于存储连接数据库字符串
                    string ConStr = "server=DESKTOP-B79BJKE\\SQLEXPRESS;database=" + textBox1.Text.Trim() + ";uid=wang;pwd=";//Trim()移除头尾空格
                    //创建一个SqlConnection对象
                    SqlConnection conn = new SqlConnection(ConStr);
                    conn.Open();//打开连接
                    if (conn.State == ConnectionState.Open)//判断当前的连接状态
                    {
                        //显示状态
                        label2.Text = "数据库【" + textBox1.Text.Trim() + "】已经连接并打开";
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("数据库连接失败");
                }
            }
        }
    }
}
