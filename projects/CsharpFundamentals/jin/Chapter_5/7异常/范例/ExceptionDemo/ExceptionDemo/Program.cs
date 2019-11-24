using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //WhatIsException();
            InputNumber();

            Console.ReadKey();
        }
        static void WhatIsException()
        {
            int i = 100, j = 0;
            Console.WriteLine(i / j);
        }

        private static void InputNumber()
        {
            Console.Write("请输入一个正整数：");
            try
            {
                //尝试着将用户输入的字符串转换为整数
                int value = Convert.ToInt32(Console.ReadLine());
                if (value <= 0)
                {
                    //发现非法的数据，“主动”抛出一个异常
                    throw new InvalidOperationException("你输入的不是正整数！");
                }
                Console.WriteLine("您输入的数字是：{0}", value);
            }
            catch (FormatException)
            {
                Console.WriteLine("输入的字符串无法转换为数字");
            }
            catch (OverflowException)
            {
                Console.WriteLine("你输入的数字太大了！");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            finally
            {
                Console.WriteLine("敲任意键退出……");
            }
        }


    }
}
