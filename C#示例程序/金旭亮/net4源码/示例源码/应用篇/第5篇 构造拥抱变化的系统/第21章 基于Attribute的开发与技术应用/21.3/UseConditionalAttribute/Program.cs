using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace UseConditionalAttribute
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintDebugInfo("程序出错了！");
            Console.WriteLine("不管是Debug还是Release，这条信息都看得见！");
            Console.ReadKey();

        }

        [Conditional("DEBUG")]
        static void PrintDebugInfo(string info)
        {
            Console.WriteLine("调试阶段信息:{0}", info);
        }



    }
}
