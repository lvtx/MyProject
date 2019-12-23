using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypicalDelegateExample
{
    //1.定义一个委托
    public delegate int AddDelegate(int i, int j);

    //2.定义一个MyClass类，放置一个满足AddDelegate委托要求的方法
    class MyClass
    {
        public int Add(int i,int j)
        {
            return i + j;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //3.定义一个委托变量
            AddDelegate del = null;
            //4.实例化一个MyClass对象，并将其Add方法引用传给委托变量del
            MyClass obj = new MyClass();
            del = obj.Add;
            //5.通过委托变量调用MyClass对象的Add()方法
            int result=del(100, 200);
            Console.WriteLine("{0}+{1}={2}",100,200,result);
            Console.ReadKey(true);

        }
    }
}
