using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ClipboardInfo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnGetClipboardInfo_Click(object sender, EventArgs e)
        {
            //获取剪贴板数据
            IDataObject data=Clipboard.GetDataObject();
            richTextBox1.Text = ""; //清空信息
            //没有数据则退出
            if(data==null) return ;
            //获取剪贴板数据所支持的格式清单
            string[] strs=data.GetFormats();
            //显示格式清单
            foreach (string str in strs)
            {
                richTextBox1.AppendText(str + "\n");
            }
         }
    }
}