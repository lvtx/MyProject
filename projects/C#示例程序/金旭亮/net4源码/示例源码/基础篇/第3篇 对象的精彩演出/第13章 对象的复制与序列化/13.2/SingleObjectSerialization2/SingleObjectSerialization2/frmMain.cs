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

namespace SingleObjectSerialization2
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
        private void ShowStudentInfo(CollegeStudent stu)
        {
            if (stu == null)
                return;
            this.txtName.Text = stu.Name;
            this.txtScore.Text = Convert.ToString(stu.ScoreForEntranceExamination);
            this.rdoMale.Checked = stu.IsMale ;
            this.rdoFemale.Checked = !stu.IsMale;
            this.dateTimePicker1.Value = stu.BirthDay;
            this.lblAge.Text = Convert.ToString(stu.Age);
        }

        //清除所有的输入控件到初始状态
        private void ClearInputControls()
        {
            this.txtName.Text = "空";
            this.txtScore.Text ="0";
            this.rdoMale.Checked = true;
            this.lblAge.Text  =Convert.ToString(0);
            this.dateTimePicker1.Value = DateTime.Now;
        }

        //更新对象信息
        private void UpdateStudentObj(CollegeStudent stu)
        {
            if (stu == null)
                return;
            stu.IsMale = rdoMale.Checked;
            stu.Name = txtName.Text;
            stu.ScoreForEntranceExamination = Convert.ToInt32(txtScore.Text);
            stu.BirthDay = dateTimePicker1.Value;
        }

        //保存对象到文件中
        private void SaveToFile()
        {
            String FileName = "";
            this.saveFileDialog1.FileName = "Student2";
            this.UpdateStudentObj(stu);
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileName = saveFileDialog1.FileName;
                SerializeStudentObj(FileName,stu);
            }

        }

        //将CollegeStudent序列化到文件中
        private void SerializeStudentObj(String FileName,CollegeStudent stu)
        {
            using (FileStream writer = new FileStream(FileName, FileMode.Create))
            {
                IFormatter formatter = new BinaryFormatter();
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
                this.ShowStudentInfo(DeserializeStudentObj(FileName));              
            }
  
        }
        //从文件中反序列化对象
        private CollegeStudent  DeserializeStudentObj(String FileName)
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

        //当日期更改时，同步修改年龄
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            int Year = DateTime.Now.Year - this.dateTimePicker1.Value.Year;
            if (Year<0)
            {
                this.lblAge.Text = "错误！";
                return ;
            }
            String newValue=Convert.ToString(Year);
            if (this.lblAge.Text != newValue)
                this.lblAge.Text = newValue;
        }
    }
}