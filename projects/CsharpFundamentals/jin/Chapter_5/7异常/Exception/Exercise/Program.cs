using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//编写一个程序，此程序在运行时要求用户输入一个整数，
//代表某门课的考试成绩，程序接着给出“不及格”、
//“及格”、“中”、“良”、“优”的结论。
namespace Exercise
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("输入成绩");
                string userInput = Console.ReadLine();
                int score = Convert.ToInt32(userInput);
                if (score < 0)
                {
                    Console.WriteLine("输入的成绩小于零，请重新输入");
                }
                else if(score >= 0 && score < 60)
                {
                    Console.WriteLine("不及格");
                }
                else if(score >= 60 && score < 70)
                {
                    Console.WriteLine("及格");
                }
                else if (score >= 70 && score < 80)
                {
                    Console.WriteLine("中");
                }
                else if(score >= 80 && score < 90)
                {
                    Console.WriteLine("良");
                }
                else if(score >= 90 && score <= 100)
                {
                    Console.WriteLine("优");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("输入的不是一个整数的成绩");
            }
            catch (OverflowException)
            {
                Console.WriteLine("输入的数字过大");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
