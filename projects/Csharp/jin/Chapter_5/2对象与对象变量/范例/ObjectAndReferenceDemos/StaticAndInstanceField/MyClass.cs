using System;
using System.Collections.Generic;
using System.Text;

namespace StaticAndInstanceField
{
    class MyClass
    {
        //��̬�ֶ�
        public static int staticVar=0;
        //��̬����   
        public static void staticFunc() 
        {
            MyClass obj = new MyClass();
            obj.increaseValue();
            obj.dynamicVar++;
        }
        //ʵ���ֶ�
        public int dynamicVar=0;
        //ʵ������
        public void increaseValue()
        {
            staticVar++;
            dynamicVar++;
        }
    }
}
