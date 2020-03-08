using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InvokeMethodDemo
{
    class Program
    {
        static void Main(string[] args)
        {
           // WrapperMethod();
            //ThreadArgumentObject();
            Console.ReadKey();
        }
        //外套方法
        private static void WrapperMethod()
        {
            //设定参数值
            MyThread obj = new MyThread()
            {
                x = 100,
                y = 200
            };
            //启动线程
            Thread th = new Thread(obj.ThreadMethod);
            th.Start();
            //等待线程运行结束
            th.Join();
            //获取函数返回值
            Console.WriteLine(obj.returnVaule);
        }

        private static void ThreadArgumentObject()
        {
            MyThread2 obj = new MyThread2();
            var argu = new ThreadMethodHelper();

            //设定线程函数参数
            argu.x = 100; argu.y = 200;

            //创建线程对象
            Thread t = new Thread(
               new ParameterizedThreadStart(obj.SomeFunc));
            //启动线程,向线程传送线程参数
            t.Start(argu);

            //主线程干其他事……
            t.Join();//等待辅助线程结束

            Console.WriteLine(argu.returnVaule); //取回线程结果

        }
    }

    class MyThread
    {
        public long SomeFunc(int x, int y)
        {
            return x + y;
        }
        //将SomeFunc参数和返回值外化为类的公有字段
        public int x;
        public int y;
        public long returnVaule;

        //供多线程调用的“外套”函数
        public void ThreadMethod()
        {
            returnVaule = SomeFunc(x, y);
        }

    }

    class MyThread2
    {
        public void SomeFunc(object argu)
        {
            int x = (argu as ThreadMethodHelper).x;
            int y = (argu as ThreadMethodHelper).y;
            //使用x和y完成一些工作，结果保存在returnVaule中
            (argu as ThreadMethodHelper).returnVaule = x+y;
        }

    }

    class ThreadMethodHelper
    {
        //线程输入参数
        public int x;
        public int y;
        //函数返回值
        public long returnVaule;
    }

}
