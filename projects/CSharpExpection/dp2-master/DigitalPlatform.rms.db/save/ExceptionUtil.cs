using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalPlatform.rms
{
    //�Զ�����쳣����
    //ReversePolishStackջPop()��Seek()��SeekType()ʱ���ܻ��׳����쳣
    public class StackUnderflowException : Exception
    {
        //���캯��
        //strEx: ��Ϣ
        public StackUnderflowException(string strEx)
            : base(strEx)
        { }
    }
    //�Զ�����쳣����:����ʱ���Ͳ������׳����쳣
    public class NoMatchException : Exception
    {
        //���캯��
        public NoMatchException(string strEx)
            : base(strEx)
        { }
    }
}
