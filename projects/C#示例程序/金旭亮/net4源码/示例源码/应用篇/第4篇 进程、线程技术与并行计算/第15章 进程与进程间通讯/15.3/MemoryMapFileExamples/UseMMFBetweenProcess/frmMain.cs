using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.MemoryMappedFiles;
using System.IO;

namespace UseMMFBetweenProcess
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            InitMemoryMappedFile();
        }

        /// <summary>
        /// 内存映射文件的容量
        /// </summary>
        private const int FileSize = 1024 * 1024;
        private MemoryMappedFile file = null;
        private MemoryMappedViewAccessor accessor = null;

        /// <summary>
        /// 初始化内存映射文件
        /// </summary>
        private void InitMemoryMappedFile()
        {
            file = MemoryMappedFile.CreateOrOpen("UseMMFBetweenProcess", FileSize);
            accessor = file.CreateViewAccessor();
            lblInfo.Text = "内存文件创建或连接成功";
           
        }

        /// <summary>
        /// 要共享的数据对象
        /// </summary>
        private MyStructure data;

        /// <summary>
        /// 显示数据到窗体上
        /// </summary>
        private void ShowData()
        {
            textBox1.Text = data.IntValue.ToString();
            textBox2.Text = data.FloatValue.ToString();
        }

        /// <summary>
        /// 根据用户输入更新数据
        /// </summary>
        private void UpdateData()
        {

            data.IntValue = int.Parse(textBox1.Text);
            data.FloatValue = float.Parse(textBox2.Text);



        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateData();
                accessor.Write<MyStructure>(0, ref data);
                lblInfo.Text = "数据已经保存到内存文件中";
            }
            catch (Exception ex)
            {

                lblInfo.Text = ex.Message;
            }

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            accessor.Read<MyStructure>(0, out data);
            ShowData();
            lblInfo.Text = "成功从内存文件中提取了数据";
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (accessor != null)
                accessor.Dispose();
            if (file != null)
                file.Dispose();
        }
    }
}
