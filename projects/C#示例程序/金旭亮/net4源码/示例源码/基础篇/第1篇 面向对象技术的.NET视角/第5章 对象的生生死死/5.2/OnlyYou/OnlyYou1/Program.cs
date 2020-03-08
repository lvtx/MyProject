using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlyYou1
{
    class Program
    {
        static void Main(string[] args)
        {
            OnlyYou One, Two;
            One = GetOnlyYouObject();
            Two = GetOnlyYouObject();
            //检查一下两个对象变量是否引用同一对象
            Console.WriteLine(One == Two); //true
            Console.ReadKey();
        }

        static OnlyYou OnlyYouObject = null;

        static OnlyYou GetOnlyYouObject()
        {
            if (OnlyYouObject == null)
                OnlyYouObject=new OnlyYou();

                return OnlyYouObject;
         
        }
    }

    class OnlyYou
    {
    }
}
