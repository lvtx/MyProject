using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsOrdered
{
    class Program
    {
        static void Main(string[] args)
        {
            //生成测试数据
            List<int> source = new List<int>();
            Random ran=new Random();
            for (int i = 0; i < 10000; i++)
                source.Add(ran.Next(1, 100));

            Console.Write("串行执行的结果为：");

            // 串行执行
            var parallelQuery = from num in source
                                where num % 2 == 0
                                select num;

           
            //取出开头的10个偶数
            var First10Numbers = parallelQuery.Take(10);

            //输出结果
            foreach (var v in First10Numbers)
                Console.Write("{0} ", v);

            Console.Write("\n======================\n");

            Console.Write("并行执行的结果为：");
             
            //如果去掉下面的AsOrdered()，则结果出错
             parallelQuery = from num in source.AsParallel()
                                where num % 2 == 0
                                select num;


            // Some operators expect an ordered source sequence.
             First10Numbers = parallelQuery.Take(10);

            // Use foreach to preserve order at execution time.
            foreach (var v in First10Numbers)
                Console.Write("{0} ", v);

            Console.ReadKey();

        }
    }
}
