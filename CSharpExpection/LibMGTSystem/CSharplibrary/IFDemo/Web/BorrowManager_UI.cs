using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using BLL;
using Model;

namespace Web
{
    public partial class BorrowManager_UI : Form
    {
        public BorrowManager_UI()
        {
            InitializeComponent();
        }

        private void BorrowManager_UI_Load(object sender, EventArgs e)
        {
            #region dgvReaderInfo自动生成列

            //需要添加列的列标题字符串
            string arraysHeaderText = @"读者编号,读者名称,登记时间,有效时间,图书类型,所在院系,所在班级,省份证号,性别,电话,手机,Email,联系地址,描述信息";
            //需要绑定数据库列名称的字符串
            string arraysName = @"ReaderId,ReaderName,TimeIn,TimeOut,ReaderTypeName,DepartmentName,ClassName,IdentityCard,Gender,Special,Phone,Email,Address,ReaderRemark";

            //自动生成columns
            autocoumns.AutoColumn(arraysHeaderText, arraysName, dgvReaderInfo);

            //DataGridView1数据集绑定
            //this.dgvReaderInfo.DataSource = reader_bll.selectReader().Tables[0];
            this.dgvReaderInfo.Columns[13].Visible = false;
            this.dgvReaderInfo.Columns[12].Visible = false;
            this.dgvReaderInfo.Columns[11].Visible = false;
            dgvReaderInfo.DataSource = null;

            #endregion


            #region dgvBorrow表的自动生成列

            //dataGridView3.AutoGenerateColumns = false;
            //需要添加列的列标题字符串
            string arraysHeaderText1 = @"借还ID,读者编号,读者名称,图书编号,图书名称,借出时间,书应归还时间,实际归还时间,应付罚金,续借次数,借还描述";
            //需要绑定数据库列名称的字符串
            string arraysName1 = @"BorrowId,ReaderId,ReaderName,BookId,BookName,BorrowTime,ReturnTime,FactReturnTime,Fine,RenewCount,BorrowRemark";

            //自动生成columns
            autocoumns.AutoColumn(arraysHeaderText1, arraysName1, dgvBorrowed);
            autocoumns.AddColumn("续借", dgvBorrowed);
            autocoumns.AddColumn("还书", dgvBorrowed);

            //dgvBorrowed.Columns[2].Frozen = true;
            //dgvBorrowed.Columns[1].Frozen = true;
            //自动铺满的列宽
            dgvBorrowed.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBorrowed.Columns[0].Visible = false;
            dgvBorrowed.Columns[1].Visible = false;
            dgvBorrowed.Columns[2].Visible = false;
            dgvBorrowed.Columns[7].Visible = false;
            dgvBorrowed.Columns[dgvBorrowed.Columns.Count - 3].Visible = false;
            dgvBorrowed.DataSource = null;
            #endregion


            #region dgvBookInfo的自动生成列

            //需要添加列的列标题字符串
            string arraysHeaderText2 = @"图书编号,图书名称,登记时间,图书类型,作者,拼音码,翻译,语言,页数,价格,印刷版面,存放位置,ISBS码,版本,描述";
            //需要绑定数据库列名称的字符串
            string arraysName2 = @"BookId,BookName,TimeIn,BookTypeName,Author,PinYinCode,Translator,Language,BookNumber,Price,Layout,Address,ISBS,Versions,BookRemark";

            //自动生成columns
            autocoumns.AutoColumn(arraysHeaderText2, arraysName2, dgvBookInfo);
            autocoumns.AddColumn("借书", dgvBookInfo);

            dgvBookInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBookInfo.DataSource = null;

            dgvBookInfo.Columns[dgvBookInfo.Columns.Count - 2].Visible = false;
            dgvBookInfo.Columns[dgvBookInfo.Columns.Count - 3].Visible = false;
            dgvBookInfo.Columns[dgvBookInfo.Columns.Count - 4].Visible = false;
            dgvBookInfo.Columns[dgvBookInfo.Columns.Count - 5].Visible = false;
            dgvBookInfo.Columns[dgvBookInfo.Columns.Count - 6].Visible = false;
            dgvBookInfo.Columns[dgvBookInfo.Columns.Count - 7].Visible = false;
            dgvBookInfo.Columns[dgvBookInfo.Columns.Count - 8].Visible = false;
            #endregion
        }
        private void button5_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).FlatStyle = FlatStyle.Standard;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).FlatStyle = FlatStyle.Flat;
        }
        Com autocoumns = new Com();
        Reader_BLL reader_bll = new Reader_BLL();
        BorrowReturn_BLL borrowReturn_bll = new BorrowReturn_BLL();
        BookInfo_BLL bookInfo_bll = new BookInfo_BLL();

        //读者信息与读者借还表的联动
        private void txtReaderId_TextChanged(object sender, EventArgs e)
        {
            string ReaderId = txtReaderId.Text.Trim();
            dgvReaderInfo.DataSource = reader_bll.selectReader("ReaderId", ReaderId).Tables[0];
        }
        //重新绑定数据是发生
        private void dgvReaderInfo_DataSourceChanged(object sender, EventArgs e)
        {
            if (dgvReaderInfo.Rows.Count > 0)
            {
                string ReaderId = dgvReaderInfo.Rows[0].Cells[0].Value.ToString();
                dgvBorrowed.DataSource = borrowReturn_bll.ReaderBorrowReturn(ReaderId).Tables[0];

                labBorrowBook.Text = "读者编号为:" + ReaderId + " 正在借阅的以下图书";
            }
            else
            {
                dgvBorrowed.DataSource = null;
                labBorrowBook.Text = ".....正在借阅的图书";
            }
        }
        //根据图书编号改变事件
        private void txtBookId_TextChanged(object sender, EventArgs e)
        {
            string BookId = txtBookId.Text.Trim();
            dgvBookInfo.DataSource = bookInfo_bll.selectBookInfo2(BookId).Tables[0];
        }

        private void dgvBorrowed_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int BorrowReturnId = -1;
            try
            {
                //选中行的借阅编号
                BorrowReturnId = (int)dgvBorrowed.Rows[e.RowIndex].Cells[0].Value;

            }
            catch (Exception) { }

            //DataGridView的总列数
            int rows = dgvBorrowed.Columns.Count;

            if (e.ColumnIndex == rows - 2)//修改
            {
                DialogResult result = MessageBox.Show("确定续借吗？", "提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    BorrowReturn b = new BorrowReturn();
                    b.BorrowId = BorrowReturnId;
                    if (borrowReturn_bll.RenewBook(b) > 0)
                    {
                        MessageBox.Show("续借成功！");
                        dgvReaderInfo_DataSourceChanged(null, null);
                        autocoumns.AutoFindRow(b.BorrowId, dgvBorrowed);
                    }
                    else
                    {
                        MessageBox.Show("续借失败！");
                    }
                }
            }
            else if (e.ColumnIndex == rows - 1)//还书
            {
                DialogResult result = MessageBox.Show("确定还书吗？", "提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    if (borrowReturn_bll.ReturnBook(BorrowReturnId) > 0)
                    {
                        dgvReaderInfo_DataSourceChanged(null, null);
                        MessageBox.Show("还书成功！");
                        txtBookId_TextChanged(null, null);
                    }
                    else
                    {
                        MessageBox.Show("还书失败！");
                    }
                }
            }
        }
        //借书
        private void dgvBookInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string BookId = "";
            try
            {
                //选中行的图书编号
                BookId = dgvBookInfo.Rows[e.RowIndex].Cells[0].Value.ToString();

            }
            catch (Exception) { }

            //DataGridView的总列数
            int rows = dgvBookInfo.Columns.Count;

            if (e.ColumnIndex == rows - 1)//借书
            {
                if (dgvReaderInfo.Rows.Count < 1)
                {
                    MessageBox.Show("没有读者信息！");
                    return;
                }
                BorrowReturn b = new BorrowReturn();
                b.BookId = BookId;
                b.ReaderId = dgvReaderInfo.Rows[0].Cells[0].Value.ToString();
                b.BorrowTime = DateTime.Now;
                b.ReturnTime = DateTime.Now;//数据库中存储过程根据借书时间自动计算应还书日期
                b.Fine = 0;
                b.RenewCount = 0;
                b.BorrowRemark = "";


                DialogResult result = MessageBox.Show("确定借书吗？", "提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    if (borrowReturn_bll.BorrowBook(b) == 0)
                    {


                        MessageBox.Show("借书成功！");
                        //刷新图书借还表
                        dgvReaderInfo_DataSourceChanged(null, null);
                        //选中添加成功的新行
                        autocoumns.AutoFindRow(b.BorrowId, dgvBorrowed);
                        //刷新 //读者信息与读者借还表的联动
                        txtBookId_TextChanged(null, null);
                    }
                    else
                    {
                        MessageBox.Show("借书失败！");
                    }
                }

            }
        }
        //查询读者
        private void btnSelctReader_Click(object sender, EventArgs e)
        {
            Info_UI i = new Info_UI();
            i.txtName = "读者信息";
            i.borrowManager = this;
            i.ShowDialog();
        }

        private void btnSelectBook_Click(object sender, EventArgs e)
        {
            Info_UI i = new Info_UI();
            i.txtName = "图书信息";
            i.borrowManager = this;
            i.ShowDialog();
        }

        private void dgvBorrowed_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dgvBorrowed.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvBorrowed.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvBorrowed.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }


    }
}
