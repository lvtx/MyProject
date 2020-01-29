using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyHelper;
using System.Threading;

namespace IntroduceParallel
{
    class Program
    {
        static void Main(string[] args)
        {
            
            TestParallelInvoke();
            //TestParallelLoop();
            Console.ReadKey();

        }

      

        #region "并行执行"
        static void DoWork1()
        {
            Console.WriteLine("工作一，由线程{0}负责执行……", Thread.CurrentThread.ManagedThreadId);
        }
        static void DoWork2()
        {
            Console.WriteLine("工作二，由线程{0}负责执行……", Thread.CurrentThread.ManagedThreadId);
        }
        static void DoWork3()
        {
            Console.WriteLine("工作三，由线程{0}负责执行……", Thread.CurrentThread.ManagedThreadId);
        }

        static void TestParallelInvoke()
        {
            Console.WriteLine("主线程{0}启动并行操作……", Thread.CurrentThread.ManagedThreadId);
            Parallel.Invoke(
                () => DoWork1(),
                () => DoWork2(),
                () => DoWork3()
                );
            Console.WriteLine("并行操作结束，敲任意键退出主线程{0}……", Thread.CurrentThread.ManagedThreadId);
        }
        #endregion

        #region "并行处理"

        static void VisitDataItem(int i)
        {
            Console.WriteLine("访问第{0}个数据项,由线程{1}负责执行", i, Thread.CurrentThread.ManagedThreadId);
        }

        static void ProcessDataItem(int DataItem)
        {
            Console.WriteLine("处理数据项{0},由线程{1}负责执行", DataItem, Thread.CurrentThread.ManagedThreadId);
        }
        private static void TestParallelLoop()
        {

            var intList = Enumerable.Range(1, 8);
            Console.WriteLine("原始数据");

            foreach (var item in intList)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("\n主线程{0}启动并行操作……\n", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("For并行执行后的数据:");
            Parallel.For(0, intList.Count(), VisitDataItem);
            Console.WriteLine("\nForEach并行执行后的数据:");
            Parallel.ForEach(intList,ProcessDataItem);
            Console.WriteLine("\n并行操作结束，敲任意键退出主线程{0}……\n", Thread.CurrentThread.ManagedThreadId);
        }
        #endregion
    }
}
