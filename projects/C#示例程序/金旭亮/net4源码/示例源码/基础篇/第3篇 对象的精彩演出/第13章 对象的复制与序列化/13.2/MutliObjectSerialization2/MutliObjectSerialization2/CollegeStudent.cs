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
        public String Name="��";//����
        public bool IsMale=true ;    //�Ա�
        public int ScoreForEntranceExamination=0; //��ѧ���Գɼ�
        public DateTime BirthDay=DateTime.Now;   //����
        [NonSerialized]
        public int Age=0; //����
        void IDeserializationCallback.OnDeserialization(Object sender)
        {
            Age=DateTime.Now.Year-BirthDay.Year ;
        }
    }

    [Serializable]
    class StudentList//ѧ���嵥
    {
        public List<CollegeStudent> Students = new List<CollegeStudent>();//ѧ�����󼯺�
    }
}
