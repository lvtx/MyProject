using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlyYou2
{
    class Program
    {
        static void Main(string[] args)
        {
            OnlyYou One, Two;
            One =OnlyYou.GetOnlyYouObject();
            Two = OnlyYou.GetOnlyYouObject();
            //检查一下两个对象变量是否引用同一对象
            Console.WriteLine(One == Two); //true
            Console.ReadKey();
        }
    }

    class OnlyYou
    {
       private static int ObjectCounter = 0;//对象计数器
       private static OnlyYou OnlyYouObject = null;

       public static OnlyYou GetOnlyYouObject()
        {
            if (ObjectCounter == 0)
            {
                OnlyYouObject = new OnlyYou();
                ObjectCounter++;
            }
            return OnlyYouObject;
        }
    }
}
