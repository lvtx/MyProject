using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadOnlyType
{
    class MyClass
    {
        private int Value = 100;
        public void add(int step)
        {
            this.Value += step;
        }
        public void printValue()
        {
            Console.WriteLine("Value={0}", Value);
        }
    }

    class MyReadOnlyClass
    {
        private int Value = 100;

        public MyReadOnlyClass add(int step)
        {
            MyReadOnlyClass obj = new MyReadOnlyClass();
            obj.Value = this.Value;
            obj.Value += step;
            return obj;
        }

        public void printValue()
        {
            Console.WriteLine("Value={0}", Value);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //不作特殊处理，类的字段值是可改的

            MyClass obj = new MyClass();
            obj.add(1);  //字段值加一
            obj.printValue();  //输出:101
            
          
            //DateTime是只读的

            DateTime date = new DateTime(2015, 10, 1);
            date.AddDays(1);  //增加一天
            Console.WriteLine(date);  //输出：2015/10/1

            //String是只读的

            string str = "abcd";
            str.ToUpper();  //改为大写
            Console.WriteLine(str);  //输出：abcd
       
            //使用只读的类

            MyReadOnlyClass readonlyObj = new MyReadOnlyClass();
            MyReadOnlyClass readonlyObj2 = readonlyObj.add(1);
            Console.WriteLine(readonlyObj2 == readonlyObj); //false
            readonlyObj.printValue(); //输出：100
            readonlyObj2.printValue(); //输出：101

            Console.ReadKey();
        }
    }
}
