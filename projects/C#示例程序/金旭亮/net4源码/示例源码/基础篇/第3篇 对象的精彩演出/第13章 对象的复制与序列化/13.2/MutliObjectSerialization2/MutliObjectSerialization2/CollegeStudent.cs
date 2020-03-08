using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

namespace MutliObjectSerialization2
{
    [Serializable]
    class CollegeStudent:IDeserializationCallback
    {
        public String Name="空";//姓名
        public bool IsMale=true ;    //性别
        public int ScoreForEntranceExamination=0; //入学考试成绩
        public DateTime BirthDay=DateTime.Now;   //生日
        [NonSerialized]
        public int Age=0; //年龄
        void IDeserializationCallback.OnDeserialization(Object sender)
        {
            Age=DateTime.Now.Year-BirthDay.Year ;
        }
    }

    [Serializable]
    class StudentList//学生清单
    {
        public List<CollegeStudent> Students = new List<CollegeStudent>();//学生对象集合
    }
}
