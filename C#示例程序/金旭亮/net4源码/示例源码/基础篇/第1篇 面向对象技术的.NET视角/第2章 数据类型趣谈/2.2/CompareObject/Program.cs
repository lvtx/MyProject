using System;
using System.Collections.Generic;
using System.Text;

namespace CompareObject
{
    class A
    {
        public int i;
        //比对对象内容
        public bool Equals(A a)
        {
            return i == a.i;
          
        }
    }
    class Program
    {
       
        static void Main(string[] args)
        {
            A a1 = new A();
            A a2 = new A();
          
            //两个对象的值不等
            a1.i = 100;
            a2.i = 200;
            Console.WriteLine(a1.Equals(a2)); //输出：false
            //两个对象的值相等
            a1.i = 100;
            a2.i = 100;
            Console.WriteLine(a1.Equals(a2)); //输出：true
            //a1和a2指向同一对象
            a2 = a1;
            Console.WriteLine(a1.Equals(a2)); //输出：true

           
            //程序暂停       
            Console.ReadKey();
        }
    }
}
