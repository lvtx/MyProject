using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace SingleObjectSerialization2
{
    [Serializable]
    class CollegeStudent:IDeserializationCallback
    {
        public String Name="��";//����
        public bool IsMale=true ;    //�Ա�
        public int ScoreForEntranceExamination=0; //��ѧ���Գɼ�
        public DateTime BirthDay=DateTime.Now;   //����
        [NonSerialized]
        public int Age=0; //�����ֶβ����л�
        //���巴���л����ʱ�Զ����õĺ���
        void IDeserializationCallback.OnDeserialization(Object sender)
        {
            //��������
            Age=DateTime.Now.Year-BirthDay.Year ;
        }
      
    }
}
