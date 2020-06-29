using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace OrderablePartitionerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = 30000000;
            int[] data = new int[size];  //要并行处理的数据源
            Parallel.ForEach(Partitioner.Create(0, size),
                (Tuple<int, int> chunk) =>
                {
                    int ThreadID = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine("线程{0}正在处理数据分区：[{1},{2})。",
                                        ThreadID, chunk.Item1, chunk.Item2);
                    for (int i = chunk.Item1; i < chunk.Item2; i++)
                        data[i]++;  //完成数据处理工作
                });
            Console.ReadKey();
        }
    }
}
