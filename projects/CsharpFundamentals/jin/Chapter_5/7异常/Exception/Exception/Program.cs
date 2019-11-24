using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyException
{
    class Program
    {
        static void Main(string[] args)
        {
            InputNumber();
            Console.ReadKey();
        }
        private static void InputNumber()
        {
            try
            {
                Console.WriteLine("请输入一个正整数");
                string userInput = Console.ReadLine();
                int value = Convert.ToInt32(userInput);
                if (value < 0)
                {
                    Console.WriteLine("你输入的不是一个正整数");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("输入的字符串无法转换为数字");
            }
            catch (OverflowException)
            {
                Console.WriteLine("你输入的数字太大了");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("敲任意键退出");
            }
        }
    }
}
