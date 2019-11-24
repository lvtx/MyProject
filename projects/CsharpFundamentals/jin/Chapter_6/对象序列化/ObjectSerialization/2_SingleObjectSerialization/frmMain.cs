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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace _2_SingleObjectSerialization
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            stu = new CollegeStudent();
            this.ClearInputControls();
        }

        private CollegeStudent stu;

        private void ShowStudentInfo(CollegeStudent obj)
        {
            if (obj == null)
                return;
            this.txtName.Text = obj.Name;
            this.txtScore.Text = Convert.ToString(obj.ScoreForEntranceExamination);
            this.rdoMale.Checked = obj.IsMale;
            this.rdoFemale.Checked = !obj.IsMale;
        }

        private void ClearInputControls()
        {
            this.txtName.Text = "空";
            this.txtScore.Text = "0";
            this.rdoMale.Checked = true;
        }

        private void UpdateStudentObj(CollegeStudent stu)
        {
            if (stu == null)
                return;
            stu.IsMale = rdoMale.Checked;
            stu.Name = txtName.Text;
            stu.ScoreForEntranceExamination = Convert.ToInt32(txtScore.Text);
        }

        private void SaveToFile()
        {
            string FileName = "";
            this.saveFileDialog1.FileName = "Student";
            this.UpdateStudentObj(stu);
            if(this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileName = saveFileDialog1.FileName;
                SerializeObj(FileName, stu);
            }
        }
        //将CollegeStudent对象序列化到文件中
        private void SerializeObj(string FileName,CollegeStudent stu)
        {
            //创建FileStream对象
            using (FileStream writer = new FileStream(FileName, FileMode.Create))
            {
                //创建格式化器对象
                IFormatter formatter = new BinaryFormatter();
                //格式化器对象使用FileStream对象序列化对象
                formatter.Serialize(writer, stu);
                MessageBox.Show("对象成功保存到文件:" + FileName);
            }          
        }

        //从文件中装入对象
        private void LoadFromFile()
        {
            String FileName = "";
            if(this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileName = openFileDialog1.FileName;
                this.ShowStudentInfo(DeserializeObj(FileName));
            }
        }

        private CollegeStudent DeserializeObj(String FileName)
        {
            using (FileStream reader = new FileStream(FileName, FileMode.Open))
            {
                IFormatter formatter = new BinaryFormatter();
                return (CollegeStudent)formatter.Deserialize(reader);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearInputControls();
        }

        private void btnSaveObject_Click(object sender, EventArgs e)
        {
            this.SaveToFile();
        }

        private void btnLoadObject_Click(object sender, EventArgs e)
        {
            this.LoadFromFile();
        }
    }
}
