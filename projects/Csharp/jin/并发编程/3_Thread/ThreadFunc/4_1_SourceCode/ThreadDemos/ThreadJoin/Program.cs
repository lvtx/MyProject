using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadJoin
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("���߳̿�ʼ����");
            Thread th = new Thread(new ThreadStart(ThreadAMethod));
            th.Start();
            th.Join();
            Console.WriteLine("���߳��˳�");
            Console.ReadKey();
        }

        static void ThreadAMethod()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("�����߳�����ִ��:"+i.ToString());
                Thread.Sleep(200);
            }
            Console.WriteLine("�����߳�ִ�н���");
        }
    }
}
