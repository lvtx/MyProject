using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UseThreadStatic
{
    class ThreadData
    {
        /// <summary>
        /// 每个线程都拥有此字段的一个拷贝,用于保存线程ID
        /// </summary>
        [ThreadStaticAttribute]
        static int ThreadID;

        /// <summary>
        /// 每个线程都拥有此字段的一个拷贝,用于保存线程创建时间
        /// </summary>
        [ThreadStaticAttribute]
        static DateTime CreateTime;

        public static void ThreadStaticDemo()
        {
            //将本线程ID保存到TLS中
            ThreadID = Thread.CurrentThread.ManagedThreadId;
            CreateTime = DateTime.Now;

           //让其它的进程有机会执行，从而展示出每个线程都拥有此字段的一个拷贝
            Thread.Sleep(1000);

            // 显示TLS中的数据
            Console.WriteLine("线程{0}创建于{1}",
                 ThreadID,CreateTime);
        }
    }

}
