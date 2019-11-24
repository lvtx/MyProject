using System;
using System.Collections.Generic;
using System.Text;

namespace SingleObjectSerialization
{
    //学生信息  
    [Serializable]
    class CollegeStudent
    {
        //姓名
        public String Name = "空";
        //性别
        public bool IsMale = true;
        //入学考试成绩
        public int ScoreForEntranceExamination = 0;
    }
}
