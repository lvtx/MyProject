using System;
using System.Collections.Generic;
using System.Text;

namespace InnerClassExample
{
    partial class A
    {
        private class B  //����������ڲ���Ķ���
        {
            private A container; //�����ⲿ��Ķ�������

            public B(A obj) //�����ⲿ��Ķ�������
            {
                container = obj;
            }
            public void ChangeValue()
            {
                container.privateField += 1;//�����ⲿ���˽���ֶ�
            }

        }
    }
}
