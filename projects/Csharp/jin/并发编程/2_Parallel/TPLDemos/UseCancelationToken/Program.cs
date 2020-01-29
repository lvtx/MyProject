using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UseCancelationToken
{

    public class ThreadFuncObject
    {
        //通过构造函数从外界传入取消令牌
        private CancellationToken _token;
        public ThreadFuncObject(CancellationToken token)
        {
            _token = token;
            _token.Register(() =>
            {
                Console.WriteLine("操作己被取消,这是操作取消时被回调的方法");
            });
        }

        public void DoWork()	//将以多线程方式执行的函数
        {
            for (int i = 1; i <= 10; i++)
            {
                if (!_token.IsCancellationRequested)
                {
                    Thread.Sleep(500);
                    Console.WriteLine("正在工作：{0}", i);
                }
            }

        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            ThreadFuncObject threadObj = new ThreadFuncObject(cts.Token);
            Thread thread = new Thread(threadObj.DoWork);
            thread.Start();

            Console.WriteLine("敲任意键取消并行计算任务……");
            Console.ReadKey(true);
            cts.Cancel();
            //为方便观察，主线程在此阻塞等待工作线程执行结束
            thread.Join();
            Console.WriteLine("敲任意键退出");
            Console.ReadKey();
        }

      

        static void DoStep1WithCancelllationToken(CancellationToken token)
        {
            int sleepTime = new Random().Next(1, 5000);
            Console.WriteLine("第一步预计执行时间：{0}毫秒", sleepTime);
            Thread.Sleep(sleepTime);
            if (token.IsCancellationRequested)
            {
                Console.WriteLine("步骤一被取消");
                token.ThrowIfCancellationRequested();
            }
            Console.WriteLine( "第一步执行完毕");
          
        }
        static void DoStep2WithCancelllationToken(CancellationToken token)
        {
            int sleepTime = new Random().Next(1, 5000);
            Console.WriteLine("第二步预计执行时间：{0}毫秒", sleepTime);
            Thread.Sleep(sleepTime);
            if (token.IsCancellationRequested)
            {
                Console.WriteLine("步骤二被取消");
               
                token.ThrowIfCancellationRequested();
            }
            Console.WriteLine("第二步执行完毕");
            
        }
        static void UseCancellationToken()
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                Task.Factory.StartNew(() => DoStep1WithCancelllationToken(cts.Token)).ContinueWith((prevTask) => DoStep2WithCancelllationToken(cts.Token));
                Console.WriteLine("马上敲任意键取消执行");
                Console.ReadKey(true);
                cts.Cancel();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
             
            }


        }
    }
}
