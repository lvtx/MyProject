using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace UseMMFBetweenProcess2
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            InitMemoryMappedFile();
        }
       
        /// <summary>
        /// 图片
        /// </summary>
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
       
       /// <summary>
       /// 图片说明
       /// </summary>
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


        private MemoryMappedFile memoryFile = null;

        private MemoryMappedViewStream stream = null;

        /// <summary>
        /// 最大容量:10M
        /// </summary>
        private const int FileSize = 1024 * 1024 * 10;  

        /// <summary>
        /// 创建内存映射文件,获取其读写流
        /// </summary>
        private void InitMemoryMappedFile()
        {
            try
            {
  memoryFile = MemoryMappedFile.CreateOrOpen("UseMMFBetweenProcess2", FileSize);
            stream = memoryFile.CreateViewStream();
            }
            catch (Exception ex )
            {

                MessageBox.Show(ex.Message);
                Close();
            }
          
        }

        /// <summary>
        /// 释放相关资源
        /// </summary>
        private void DisposeMemoryMappedFile()
        {
            if (stream != null)
                stream.Close();
            if (memoryFile != null)
                memoryFile.Dispose();
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

        /// <summary>
        /// 将MyPic对象保存到内存映射文件中
        /// </summary>
        private void SaveToMMF()
        {
            try
            {
            MyPic obj = CreateMyPicObj();
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            formatter.Serialize(stream, obj);
            MessageBox.Show("对象已保存到内存映射文件中");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           

        }

 private void LoadFromMMF()
        {
            try
            {
           // CreateMyPicObj();
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
          MyPic obj =   formatter.Deserialize(stream) as MyPic;
          if (obj != null)
          {
              bmp = obj.pic;
              info = obj.picInfo;
          }
          
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           

        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeMemoryMappedFile();
        }

        private void btnSaveToMMF_Click(object sender, EventArgs e)
        {
            SaveToMMF();
        }

        private void btnLoadFromMMF_Click(object sender, EventArgs e)
        {
            LoadFromMMF();
        }
    }
}
