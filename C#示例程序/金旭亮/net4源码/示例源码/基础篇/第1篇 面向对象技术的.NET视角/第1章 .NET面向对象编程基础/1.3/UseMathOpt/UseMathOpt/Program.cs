using System;
using System.Collections.Generic;
using System.Text;

namespace UseMathOpt
{
    class Program
    {
        static void Main(string[] args)
        {
            MathOpt mathobj = null;    //定义MathOpt对象变量
            mathobj = new MathOpt();    //创建对象
            int IResult = mathobj.Add(100, 200);   //调用类的整数相加方法
            double FResult = mathobj.Add(5.5, 9.2);
            Console.WriteLine("100+200=" + IResult); //输出结果
            Console.WriteLine("5.5+9.2=" + FResult); //输出结果
            Console.ReadKey();  //敲任意键结束整个程序
        }
    }

    
}
