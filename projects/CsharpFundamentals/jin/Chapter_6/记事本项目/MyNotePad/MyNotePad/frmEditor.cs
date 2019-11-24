using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNotePad
{
    public partial class frmEditor : Form
    {
        public frmEditor()
        {
            InitializeComponent();
        }
        private string OriginText = "";
        private string openFileName = "";
        public string OpenFileName
        {
            get 
            { 
                return openFileName; 
            }
            set 
            {
                openFileName = value;
                Text = Path.GetFileName(value)+"-我的记事本";
            }
        }

        private void frmEditor_Load(object sender, EventArgs e)
        {
            toolStripStatusLblShowTime.Text = "";
            toolStripStatusLblShowSaveStatus.Text = "";
            Text = "无标题-我的记事本";

        }
        /// <summary>
        /// 右下角StatusLabel显示时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLblShowTime.Text = DateTime.Now.ToString();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }
        private void frmEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            Save();
        }
        private void Save()
        {
            //重命名文件提醒
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.DefaultExt = ".txt";
            saveFileDialog1.Title = "保存文本文件";
            bool ShouldSave = false;
            //用户打开了一个文件
            if (openFileName != "")
            {
                if (txtEditor.Text != OriginText
                && MessageBox.Show("文件内容已修改是否保存", "保存文件",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ShouldSave = true;
                }              
            }
            //创建一个新文件
            else
            {
                if (txtEditor.Text != null && saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    openFileName = saveFileDialog1.FileName;
                    ShouldSave = true;
                }
            }

            try
            {
                if (ShouldSave == true)
                {
                    File.WriteAllText(openFileName, txtEditor.Text);
                    toolStripStatusLblShowSaveStatus.Text = "文件已保存";
                }
                else
                {
                    toolStripStatusLblShowSaveStatus.Text = "文件未保存";
                }
            }
            catch (Exception)
            {
                txtEditor.Text = "保存文件失败";
            }
        }
        private void Open()
        {
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog1.Title = "打开文件";
            openFileDialog1.DefaultExt = ".txt";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    OpenFileName = openFileDialog1.FileName;
                    OriginText = File.ReadAllText(openFileName);
                    txtEditor.Text = OriginText;
                }
                catch (Exception)
                {
                    txtEditor.Text = "打开文件失败";
                }
            }
        }


    }
}
