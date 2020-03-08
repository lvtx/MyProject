using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArrayForEachExample
{
    class Program
    {

        static void Main(string[] args)
        {
            //源数组
            int[] SourceArr = new int[]
            {
                0,1,2,3,4,5,6,7,8,9
            };
            //目标数组
            int[] TargetArr = new int[SourceArr.Length];
            int index=0;
            //将施加于每个数组元素的“加工方法”
            Action<int> DoubleElement = delegate(int element)
            {
                TargetArr[index++] = element * 2;
            };
            //完成源数组的“加工”
            Array.ForEach<int>(SourceArr, DoubleElement);
            //显示处理结果
            Array.ForEach<int>(TargetArr, (elelm) => { Console.WriteLine(elelm); });

            Console.ReadKey();
        }
    }
}
