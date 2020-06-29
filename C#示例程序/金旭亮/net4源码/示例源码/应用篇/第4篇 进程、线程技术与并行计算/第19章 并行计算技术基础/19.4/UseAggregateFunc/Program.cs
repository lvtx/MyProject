using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UseAggregateFunc
{
    class Program
    {
        static void Main(string[] args)
        {
            //生成测试数据
            int[] source = new int[5];
            Random rand = new Random();
            for (int x = 0; x < source.Length; x++)
            {
                source[x] = rand.Next(1, 10);
            }
            //输出原始数据：
            Console.WriteLine("要计算方差的原始数据为：");
            foreach (int num in source)
            {
                Console.Write("{0} ",num);
            }
            Console.WriteLine();
            //计算平均值
            double mean = source.AsParallel().Average();
            Console.WriteLine("总体数据平均值={0}", mean);
            //并行执行的聚合函数
            double VariantOfPopulation = source.AsParallel().Aggregate(
                0.0,   //聚合变量初始值
                //针对每个分区的每个数据项调用此函数
                (aggValue, number) => {
                    double result = aggValue + Math.Pow((number - mean), 2);
                    Console.WriteLine("线程{0}调用updateAccumulatorFunc:aggValue={1},number={2},返回值={3}",
                        Thread.CurrentThread.ManagedThreadId,
                        aggValue, number, result);
                    return result; 
                },
                //针对分区处理结果调用此函数
                (aggValue, thisDataPartition) =>
                {
                    double result = aggValue + thisDataPartition;
                    Console.WriteLine("调用combineAccumulatorsFunc:aggValue={0},thisDataPartition={1},返回值={2}",
                        aggValue, thisDataPartition, result);
                    return result;
                },
                //得到最终结果
                (result) => result / source.Length
                );

            Console.WriteLine("数据的方差为：{0}", VariantOfPopulation);
            Console.ReadKey();


        }
    }
}
