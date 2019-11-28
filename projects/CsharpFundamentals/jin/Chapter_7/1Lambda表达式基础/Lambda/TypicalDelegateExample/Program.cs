using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypicalDelegateExample
{
    //1.定义一个委托
    delegate int AddDelegate(int x, int y);
    //2.定义一个MyClass类，放置一个满足AddDelegate委托要求的方法
    class MyClass
    {
        public int Add(int x,int y)
        {
            return x + y;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //3.定义一个委托变量
            AddDelegate addDel = null;
            //4.实例化一个MyClass对象，并将其Add方法引用传给委托变量del
            MyClass myClass = new MyClass();
            addDel = myClass.Add;
            //5.通过委托变量调用MyClass对象的Add()方法
            int result = addDel(5, 10);
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
