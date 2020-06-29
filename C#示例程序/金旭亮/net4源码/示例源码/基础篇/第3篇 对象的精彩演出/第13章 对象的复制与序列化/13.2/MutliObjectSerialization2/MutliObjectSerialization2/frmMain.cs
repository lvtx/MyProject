using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;

namespace MutliObjectSerialization2
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            //将学生默认信息显示在窗体上
            this.ClearInputControls();
        }

        //学生集合对象
        private StudentList stus = new StudentList();
        private CollegeStudent curstu = null;   //引用当前正在显示的学生对象


        private bool IsOnShow = false;
        //将学生信息显示在窗体上
        private void ShowStudentInfo(CollegeStudent stu)
        {
            if (stu == null)
                return;
            IsOnShow = true;
            this.txtName.Text = stu.Name;
            IsOnShow = false;
            this.txtScore.Text = Convert.ToString(stu.ScoreForEntranceExamination);
            this.rdoMale.Checked = stu.IsMale;
            this.rdoFemale.Checked = !stu.IsMale;
            this.dateTimePicker1.Value = stu.BirthDay;
            this.lblAge.Text = Convert.ToString(stu.Age);
        }

        //将学生列表显示在列表框中
        private void ShowStudnetList()
        {
            if (stus.Students.Count == 0)
            {
                this.ClearInputControls();
                lstStudent.Items.Clear();
                return;
            }
            lstStudent.Items.Clear();
            foreach (CollegeStudent stu in stus.Students)
                lstStudent.Items.Add(stu.Name);

            //显示第一条记录
            lstStudent.SelectedIndex = 0;
            curstu = stus.Students[0] as CollegeStudent;
            ShowStudentInfo(curstu);
        }

        //清除所有的输入控件到初始状态
        private void ClearInputControls()
        {
            this.txtName.Text = "空";
            this.txtScore.Text = "0";
            this.rdoMale.Checked = true;
            this.lblAge.Text = Convert.ToString(0);
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
            stu.Age = DateTime.Now.Year - stu.BirthDay.Year;
        }

        //保存对象到文件中
        private void SaveToFile()
        {
            if (stus.Students.Count == 0)
                return;
            String FileName = "";
            this.UpdateStudentObj(curstu);
            this.saveFileDialog1.FileName = "StudentList2";

            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileName = saveFileDialog1.FileName;
                SerializeStudentList(FileName);
            }

        }

        //将学生清单序列化到文件中
        private void SerializeStudentList(String FileName)
        {
            using (FileStream writer = new FileStream(FileName, FileMode.Create))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(writer, stus);
                MessageBox.Show("对象成功保存到文件:" + FileName);
            }
        }

        //从文件中装入对象
        private void LoadFromFile()
        {
            try
            {
                String FileName = "";
                if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileName = openFileDialog1.FileName;
                    DeserializeStudentList(FileName);
                    this.ShowStudnetList();
                }
                grpStudentInfo.Enabled = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }
        //从文件中反序列化对象
        private void DeserializeStudentList(String FileName)
        {
            stus.Students.Clear();
            using (FileStream reader = new FileStream(FileName, FileMode.Open))
            {
                IFormatter formatter = new BinaryFormatter();
                stus = (StudentList)formatter.Deserialize(reader);

            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearInputControls();
        }


        //当日期更改时，同步修改年龄
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            int Year = DateTime.Now.Year - this.dateTimePicker1.Value.Year;
            if (Year < 0)
            {
                this.lblAge.Text = "错误！";
                return;
            }
            String newValue = Convert.ToString(Year);
            if (this.lblAge.Text != newValue)
                this.lblAge.Text = newValue;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            grpStudentInfo.Enabled = true;
            CollegeStudent newStu = new CollegeStudent();
            stus.Students.Add(newStu);
            lstStudent.Items.Add(newStu.Name);
            lstStudent.SelectedIndex = lstStudent.Items.Count - 1;
            curstu = newStu;
            txtName.Focus();
        }

        private void lstStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateStudentObj(curstu);//更新原先的对象信息，因为用户有可能修改了
            if ((lstStudent.Items.Count == 0) || (lstStudent.SelectedIndex == -1))
                return;
            curstu = stus.Students[lstStudent.SelectedIndex] as CollegeStudent;
            //显示选中的记录信息
            ShowStudentInfo(curstu);
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            OnDelete();

        }
        //删除记录
        private void OnDelete()
        {
            int CurIndex = lstStudent.SelectedIndex;
            if (CurIndex != -1)
            {
                lstStudent.Items.RemoveAt(CurIndex);
                stus.Students.RemoveAt(CurIndex);
                lstStudent.SelectedIndex = -1;
                curstu = null;
                this.ClearInputControls();
            }
        }

        private void txtName_Leave(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

            if (IsOnShow)
                return;
            if (curstu != null)
            {
                curstu.Name = txtName.Text;
                lstStudent.Items[lstStudent.SelectedIndex] = txtName.Text;

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.SaveToFile();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            this.LoadFromFile();
        }


    }
}