using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OnlyYou5
{

    class OnlyYou
    {
        //构造函数私有，外界不能用new直接创建对象
        private OnlyYou()
        {
        }
        //专用的同步对象
        private static readonly object mylockobj = new object();

        //用于保存“独生子”的静态对象变量
        private static OnlyYou OnlyYouObject = null;
        public static OnlyYou GetOnlyYouObject()
        {
            //只要对象已经创建，所有尝试获取此对象引用的线程都不会被阻塞
            if (OnlyYouObject != null)
                return OnlyYouObject;

            lock (mylockobj) //锁定
            {
                if (OnlyYouObject == null) //对象未创建，则创建对象
                   Interlocked.Exchange(ref OnlyYouObject , new OnlyYou());
                //向外界返回已创建对象的引用
                return OnlyYouObject;
            }//解锁
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
