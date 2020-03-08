using System;
using System.Collections.Generic;
using System.Text;

namespace SingleObjectSerialization
{
    [Serializable]
    class CollegeStudent    //学生信息
    {
        public String Name="空";//姓名
        public bool IsMale=true ;    //性别
        public int ScoreForEntranceExamination=0; //入学考试成绩
    }
}
