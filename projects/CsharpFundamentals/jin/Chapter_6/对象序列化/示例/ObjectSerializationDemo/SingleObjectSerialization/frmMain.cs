using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary ;
using System.IO;

namespace SingleObjectSerialization
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            stu = new CollegeStudent();
            //将学生默认信息显示在窗体上
            this.ClearInputControls();
        }

        //学生对象
        private CollegeStudent stu ;

        //将学生信息显示在窗体上
        private void ShowStudentInfo(CollegeStudent obj)
        {
            if (obj == null)
                return;
            this.txtName.Text = obj.Name;
            this.txtScore.Text = Convert.ToString(obj.ScoreForEntranceExamination);
            this.rdoMale.Checked = obj.IsMale ;
            this.rdoFemale.Checked = !obj.IsMale;
        }

        //清除所有的输入控件到初始状态
        private void ClearInputControls()
        {
            this.txtName.Text = "空";
            this.txtScore.Text ="0";
            this.rdoMale.Checked = true;
        }

        //更新对象信息
        private void UpdateStudentObj(CollegeStudent stu)
        {
            if (stu == null)
                return;
            stu.IsMale = rdoMale.Checked;
            stu.Name = txtName.Text;
            stu.ScoreForEntranceExamination = Convert.ToInt32(txtScore.Text);
        }

        //保存对象到文件中
        private void SaveToFile()
        {
            String FileName = "";
            this.saveFileDialog1.FileName = "Student";
            this.UpdateStudentObj(stu);
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileName = saveFileDialog1.FileName;
                SerializeObj(FileName,stu);
            }

        }

        //将CollegeStudent对象序列化到文件中
        private void SerializeObj(String FileName,CollegeStudent stu)
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
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileName = openFileDialog1.FileName;
                this.ShowStudentInfo(DeserializeObj(FileName));              
            }
  
        }
        //从文件中反序列化对象
        private CollegeStudent  DeserializeObj(String FileName)
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