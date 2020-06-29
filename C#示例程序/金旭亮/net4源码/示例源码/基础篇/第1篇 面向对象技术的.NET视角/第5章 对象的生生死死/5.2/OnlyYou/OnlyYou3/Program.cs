using System;
using System.Collections.Generic;
using System.Text;

namespace OnlyYou
{
    class OnlyYou
    {
        //构造函数私有，外界不能用new直接创建对象
        private OnlyYou()
        {
        }
        //用于保存“独生子”的静态对象变量
        private static OnlyYou OnlyYouObject = null;
        public static OnlyYou GetOnlyYouObject()
        {
            lock (typeof(OnlyYou)) //锁定整个类型
            {
                if (OnlyYouObject == null) //对象未创建，则创建对象
                    OnlyYouObject = new OnlyYou();
                //向外界返回已创建对象的引用
                return OnlyYouObject;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            OnlyYou One, Two;
            One = OnlyYou.GetOnlyYouObject();
            Two = OnlyYou.GetOnlyYouObject();

            //检查一下两个对象变量是否引用同一对象
            Console.WriteLine(One == Two); //true
            Console.ReadKey();
        }
    }
}
