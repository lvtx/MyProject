using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountDown
{
    class Program
    {
        static void Main(string[] args)
        {
            CountDown countDown = new CountDown();
            countDown.Print(10);
        }
    }

    class CountDown
    {
        public void Print(int index)
        {
            Console.WriteLine(index);
            if(index <= 0)//基线条件
            {
                return;
            }
            Print(index - 1);//递归条件
        }
    }
}
