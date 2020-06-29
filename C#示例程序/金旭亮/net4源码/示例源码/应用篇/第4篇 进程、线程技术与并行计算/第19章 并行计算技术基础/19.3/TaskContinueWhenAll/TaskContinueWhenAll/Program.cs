using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TaskContinueWhenAll
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建“前期”任务数组
            Task[] tasks = new Task[]{
            Task.Factory.StartNew(() =>
                {
                        Thread.Sleep(1000);  //模拟任务的延迟
                      Console.WriteLine("前期任务1");
                }),
                Task.Factory.StartNew(()=>
                    {
                        Console.WriteLine("前期任务2");
                    })
            };
            //所有前期任务完成之后，启动下一个任务
            //to do:可以把ContinueWhenAll换成ContinueWhenAny进行试验，看看结果有何不同？
            Task.Factory.ContinueWhenAll(tasks, _ =>
            {
                Console.WriteLine("收尾任务！");
            });

            Console.ReadKey();
        }
    }
}
