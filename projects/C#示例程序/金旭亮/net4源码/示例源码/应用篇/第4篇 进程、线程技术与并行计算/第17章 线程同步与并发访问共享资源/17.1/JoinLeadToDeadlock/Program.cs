using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace JoinLeadToDeadlock
{
    class Program
    {
        static Thread mainThread;

        static void Main(string[] args)
        {
            Console.WriteLine("���߳̿�ʼ����");
            mainThread = Thread.CurrentThread;

            Thread ta = new Thread(new ThreadStart(ThreadAMethod));
            ta.Start();
              Console.WriteLine("���̵߳ȴ��߳�A��������");
            ta.Join(); //�ȴ��߳�A����
            Console.WriteLine("���߳��˳�");

        }

        static void ThreadAMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(Convert.ToString(i) + ": �߳�A����ִ��");
                Thread.Sleep(1000);
            }
            Console.WriteLine("�߳�A�ȴ����߳��˳�����");
            //�ȴ����߳̽���
            mainThread.Join();
        }

    }
}
