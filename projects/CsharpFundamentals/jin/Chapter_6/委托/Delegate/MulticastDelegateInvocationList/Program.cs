using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MulticastDelegateInvocationList
{
    public delegate int MyDelegate(int value);
    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj = new MyClass();
            MyDelegate funDel1 = new MyDelegate(obj.Func1);
            funDel1 += new MyDelegate(obj.Func2);
            //以上两句可简写为
            //funDel1 = obj.Func1;
            //funDel1 += obj.Func2;
            Delegate[] ds = funDel1.GetInvocationList();
            Console.WriteLine("fundel1委托调用列表中包含{0}个方法", ds.GetLength(0));
            funDel1(8);//先调func1，再调func2

            Console.WriteLine("==========第二个委托=========");
            MyDelegate funDel2 = obj.Func1;
            funDel2 += obj.Func2;
            Console.WriteLine("fundel2委托调用列表中包含{0}个方法", funDel2.GetInvocationList().GetLength(0));
            funDel2(8);//先调func1，再调func2

            Console.WriteLine("===========组合两个委托变量============");
            MyDelegate mulDel = funDel1 + funDel2;
            Delegate[] mulFuncDs = mulDel.GetInvocationList();
            Console.WriteLine("组合之后委托调用列表中包含{0}个方法", mulFuncDs.GetLength(0));
            int getMulDelValue = mulDel(8);//返回值为委托变量最后一个方法的返回值
            Console.WriteLine("合并之后委托变量的返回值{0}",getMulDelValue);
            mulDel -= obj.Func2;
            Console.WriteLine("移除Func1后委托调用列表中包含{0}个方法", mulDel.GetInvocationList().GetLength(0));
            getMulDelValue = mulDel(8);
            Console.WriteLine("最后一个方法的返回值为{0}",getMulDelValue);
            Console.ReadLine();
        }
    }
    class MyClass
    {
        public int Func1(int value)
        {
            Console.WriteLine("func1的值为:{0}", value);
            return value;
        }
        public int Func2(int value)
        {
            Console.WriteLine("func2的值为:{0}", value - 3);
            return value - 3;
        }
        public int Func3(int value)
        {
            Console.WriteLine("Func3的值为:{0}", value);
            return value;
        }

    }
}
