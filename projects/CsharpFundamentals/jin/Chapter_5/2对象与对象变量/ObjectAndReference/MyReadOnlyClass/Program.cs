using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadOnlyType
{
    class Program
    {
        static void Main(string[] args)
        {
            #region "不做处理字段值可改"
            //MyClass myClass = new MyClass();
            //myClass.Add(1);
            //myClass.printValue();
            #endregion
            #region "让字段只读，不可修改里面的值"
            MyReadOnlyClass obj = new MyReadOnlyClass();
            obj.Add(1);
            obj.printValue();
            #endregion
            Console.ReadKey();
        }
    }
    class MyClass
    {
        private int value = 100;
        public void Add(int step)
        {
            value += step;
        }
        public void printValue()
        {
            Console.WriteLine(value);
        }
    }
    class MyReadOnlyClass
    {
        int value = 100;
        public void Add(int step)
        {
            MyReadOnlyClass objValue = new MyReadOnlyClass();
            objValue.value += step;
        }
        public void printValue()
        {
            Console.WriteLine(value);
        }
    }
}
