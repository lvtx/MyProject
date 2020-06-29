using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace SingleObjectSerialization2
{
    [Serializable]
    class CollegeStudent:IDeserializationCallback
    {
        public String Name="空";//姓名
        public bool IsMale=true ;    //性别
        public int ScoreForEntranceExamination=0; //入学考试成绩
        public DateTime BirthDay=DateTime.Now;   //生日
        [NonSerialized]
        public int Age=0; //年龄字段不序列化
        //定义反序列化完成时自动调用的函数
        void IDeserializationCallback.OnDeserialization(Object sender)
        {
            //计算年龄
            Age=DateTime.Now.Year-BirthDay.Year ;
        }
      
    }
}
