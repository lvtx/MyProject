using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UseClipboard
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        //图片
        private Image bmp
        {
            get
            {
                return pictureBox1.Image;
            }
            set
            {
                pictureBox1.Image = value;
            }
        }
        //图片说明
        private string info
        {
            get
            {
                return txtImageInfo.Text;
            }
            set
            {
                txtImageInfo.Text = value;
            }
        }

       

        private void btnLoadPic_Click(object sender, EventArgs e)
        {
            ChooseImageFile();
        }

        //选择图片
        private void ChooseImageFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmp = new Bitmap(openFileDialog1.FileName);
            }
        }
        //根据用户设定的信息创建对象
        private MyPic CreateMyPicObj()
        {
            MyPic obj = new MyPic();
            obj.pic = bmp;
            obj.picInfo = info;
            return obj;
        }

        //将对象复制到剪贴板上
        private void CopyToClipboard()
        {
            //创建MyPic对象
            MyPic obj = CreateMyPicObj();
         
            //创建一个数据对象，将MyPic类型的对象装入
            IDataObject dataobj = new DataObject(obj);
            //其它类型的数据也可以装入到数据对象中
            dataobj.SetData(DataFormats.UnicodeText, info);
            dataobj.SetData(DataFormats.Bitmap, bmp);
            //复制到剪贴板上，第二个参数表明程序退出时不清空剪贴板
            Clipboard.SetDataObject(dataobj,true );
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
           
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            CopyToClipboard();
        }

        //从剪贴板获取数据
        private void PasteFromClipboard()
        {
            //剪贴板上有我需要的数据吗？格式为“项目名称.数据格式名”
            if (Clipboard.ContainsData("UseClipboard.MyPic") == false)
                return;
            //读取数据
            IDataObject clipobj = Clipboard.GetDataObject();
            //将数据转换为需要的类型
            MyPic mypicobj = clipobj.GetData("UseClipboard.MyPic") as MyPic;
            //从数据对象中分解出需要的数据
            info = mypicobj.picInfo;
            pictureBox1.Image = mypicobj.pic;
        }

        private void btnPasteFromClipboard_Click(object sender, EventArgs e)
        {
            PasteFromClipboard();
        }

    
    }
}