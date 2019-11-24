using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Filter = "所支持的图片格式|*.gif;*.bmp;*.jpg;*.png|任意文件(*.*)|*.*";
            //openFileDialog1.FileName = "";
            //openFileDialog1.Title = "选择一张图片";
            //openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            //openFileDialog1.CheckFileExists = true;
            //openFileDialog1.CheckPathExists = true;
            //openFileDialog1.Multiselect = true;
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    if(openFileDialog1.Multiselect == false)
            //    {
            //        lblInfo.Text = openFileDialog1.FileName;
            //    }
            //    else
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        /*
            //         * 这两者之间是有区别的
            //        openFileDialog1.FileName;
            //        openFileDialog1.FileNames;
            //        */
            //        foreach (var file in openFileDialog1.FileNames)
            //        {
            //            sb.Append(file);
            //            sb.Append("\n");
            //        }
            //        lblInfo.Text = sb.ToString();
            //    }
            //}
            openFileDialog1.ShowDialog();
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = ".txt";
            saveFileDialog1.Filter = "文本文件|*.txt|任意文件|*.*";
            saveFileDialog1.OverwritePrompt = true;//重名文件提醒
            saveFileDialog1.Title = "保存文件";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                lblInfo.Text = "文件已保存到：" + saveFileDialog1.FileName;
            }
        }

        private void btnFontDialog_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                lblInfo.Font = fontDialog1.Font;
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                lblInfo.ForeColor = colorDialog1.Color;
            }
        }
    }
}
