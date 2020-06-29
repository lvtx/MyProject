using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GetResultFromTaskTResult
{
    class Program
    {
       
        static void Main(string[] args)
        {
            Func<object,long> del = delegate(object end)
            {
                long sum = 0;
                for (int i = 1; i <= (int)end; i++)
                    sum += i;
                return sum;
            };

            Task<long> tsk = new Task<long>(del, 1000000);
           
            tsk.Start();
          
            Console.Write("程序运行结果为{0}", tsk.Result);
            Console.ReadKey();
        }
    }
}
