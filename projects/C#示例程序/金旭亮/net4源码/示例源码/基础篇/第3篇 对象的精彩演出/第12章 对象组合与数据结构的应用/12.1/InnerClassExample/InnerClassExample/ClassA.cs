using System;
using System.Collections.Generic;
using System.Text;

namespace InnerClassExample
{
    partial class A
    {
        private B b;//�ڲ����ݵ�Ƕ�������
        private int privateField = 0;//����Ƕ��������޸ĵ�˽���ֶ�
        public A()
        {
            b = new B(this); //�������ݵĶ��󣬽��������ô���Ƕ����
        }

    }
}
