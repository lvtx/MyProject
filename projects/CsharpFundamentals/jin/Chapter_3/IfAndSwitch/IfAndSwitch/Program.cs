using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfAndSwitch
{
    class Program
    {
        static void Main(string[] args)
        {
            //CompareValue(9);
            //IfAndElse();
            //HowOldAreYou(130);
            //HowOldAreYou2(130);
            DoYouPass();
            Console.ReadKey();
        }
        
        ///<summary>
        ///简单的if else示例
        /// </summary>
        /// <param name="value"></param>
        static void CompareValue(int value)
        {
            if (value > 10)
            {
                Console.WriteLine("{0}比10大",value);
            }
            else
            {
                Console.WriteLine("{0}比10小", value);
            }
        }

        /// <summary>
        /// if else的就近配套原则
        /// </summary>
        static void IfAndElse()
        {
            int value = 90;
            if(value > 10)
            {
                Console.WriteLine("{0}大于10",value);
                if(value % 10 == 0)
                {
                    Console.WriteLine("{0}是10的倍数",value);
                }
            }
            else
            {
                Console.WriteLine("{0}小于10", value);
            }
        }

        /// <summary>
        /// 嵌套的if else 语句
        /// </summary>
        static void HowOldAreYou(int age)
        {
            if(age >= 60)
            {
                Console.WriteLine("老年人");
            }
            else if(age >= 40)
            {
                Console.WriteLine("中年人");
            }
            else if (age >= 18)
            {
                Console.WriteLine("青年人");
            }
            else if(age > 0)
            {
                Console.WriteLine("儿童");
            }
        }

        /// <summary>
        /// 使用组合的逻辑表达式，将深度嵌套的if/else结构“展平”
        /// </summary>
        static void HowOldAreYou2(int age)
        {
            if(age >= 60 && age <= 120)
            {
                Console.WriteLine("老年人");
            }
            else if(age >= 40 && age < 60)
            {
                Console.WriteLine("中年人");
            }
            else if (age >= 18 && age < 40)
            {
                Console.WriteLine("青年人");
            }
            else if(age > 0 && age < 18)
            {
                Console.WriteLine("儿童");
            }
            else
            {
                Console.WriteLine("不是人");
            }
        }

        /// <summary>
        /// switch语句
        /// </summary>
        static void DoYouPass()
        {
            Console.WriteLine("输入你的成绩");
            string UseInput = Console.ReadLine();
            int score = Convert.ToInt32(UseInput);
            switch (score / 10)
            {
                case 10:
                case 9:
                    Console.WriteLine("A");
                    break;
                case 8:
                    Console.WriteLine("A-");
                    break;
                case 7:
                    Console.WriteLine("B");
                    break;
                case 6:
                    Console.WriteLine("C");
                    break;
                default:
                    Console.WriteLine("你凉了");
                    break;
            }
        }
    }
}
