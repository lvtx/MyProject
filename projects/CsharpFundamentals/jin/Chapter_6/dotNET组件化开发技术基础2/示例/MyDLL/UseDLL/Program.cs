//չʾ�������ʹ��

using System;
using System.Collections.Generic;
using System.Text;
using MyDLL;    //�������������ռ�
using MyDLL.MyChildDLL;

namespace UseDLL
{
    class Program
    {
        static void Main(string[] args)
        {
            //���������е���Ķ���
            MyPublicClass obj = new MyPublicClass();
            //������Ļ�������
            obj.i = 100;
            obj.f();
            //����������书��
            obj.j = 200;
            obj.g();
        }
    }
}
