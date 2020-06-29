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
        //ͼƬ
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
        //ͼƬ˵��
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

        //ѡ��ͼƬ
        private void ChooseImageFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmp = new Bitmap(openFileDialog1.FileName);
            }
        }
        //�����û��趨����Ϣ��������
        private MyPic CreateMyPicObj()
        {
            MyPic obj = new MyPic();
            obj.pic = bmp;
            obj.picInfo = info;
            return obj;
        }

        //�������Ƶ���������
        private void CopyToClipboard()
        {
            //����MyPic����
            MyPic obj = CreateMyPicObj();
         
            //����һ�����ݶ��󣬽�MyPic���͵Ķ���װ��
            IDataObject dataobj = new DataObject(obj);
            //�������͵�����Ҳ����װ�뵽���ݶ�����
            dataobj.SetData(DataFormats.UnicodeText, info);
            dataobj.SetData(DataFormats.Bitmap, bmp);
            //���Ƶ��������ϣ��ڶ����������������˳�ʱ����ռ�����
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

        //�Ӽ������ȡ����
        private void PasteFromClipboard()
        {
            //��������������Ҫ�������𣿸�ʽΪ����Ŀ����.���ݸ�ʽ����
            if (Clipboard.ContainsData("UseClipboard.MyPic") == false)
                return;
            //��ȡ����
            IDataObject clipobj = Clipboard.GetDataObject();
            //������ת��Ϊ��Ҫ������
            MyPic mypicobj = clipobj.GetData("UseClipboard.MyPic") as MyPic;
            //�����ݶ����зֽ����Ҫ������
            info = mypicobj.picInfo;
            pictureBox1.Image = mypicobj.pic;
        }

        private void btnPasteFromClipboard_Click(object sender, EventArgs e)
        {
            PasteFromClipboard();
        }

    
    }
}