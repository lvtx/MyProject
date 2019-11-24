using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loop
{
    class Program
    {
        static void Main(string[] args)
        {
            //SumFrom1To100();
            //InputQuitToStop();
            //SumFrom1To100ForUse();
            //BreakAndContinue();
            //SumUseInfinteLoop();
            ForEachDataCollection();
            Console.ReadKey();
        }
        #region "循环结构"
        /// <summary>
        /// while语句 循环求和
        /// </summary>
        static void SumFrom1To100()
        {
            int sum = 0;
            int count = 1;
            while(count <= 100)
            {
                sum = sum + count;
                count++;
            }
            Console.WriteLine(sum);
        }
        /// <summary>
        /// 输入某个特定值时退出 while语句
        /// </summary>
        static void InputQuitToStop()
        {
            string UseInput = " ";
            while (UseInput.ToLower() != "quit")
            {
                Console.WriteLine("请输入字符，输入Quit时退出");
                UseInput = Console.ReadLine();
                if (string.IsNullOrEmpty(UseInput) == false)
                {
                    Console.WriteLine("您输入了{0}",UseInput);
                }
            }
            Console.WriteLine("---------------------------------");
            Console.WriteLine("检测到您输入Quit");
        }
        /// <summary>
        /// For语句 循环求和
        /// </summary>
        static void SumFrom1To100ForUse()
        {
            int sum = 0;
            for (int i = 1; i <= 100; i++)
            {
                sum += i;
            }
            Console.WriteLine("1+2+...+100 = {0}",sum);
        }
        /// <summary>
        /// Break与Continue的不同
        /// </summary>
        static void BreakAndContinue()
        {
            for (int i = 1; i <= 10; i++)
            {
                if(i == 5)
                {
                    continue;
                }
                Console.WriteLine("第{0}轮循环",i);
            }
        }
        /// <summary>
        /// 使用死循环计算整数和
        /// </summary>
        static void SumUseInfinteLoop()
        {
            int count = 1;
            long sum = 0;
            for(; ;)
            {
                sum += count;
                count++;
                if (count > 100000)
                    break;
            }
            Console.WriteLine(sum);
        }
        /// <summary>
        /// 使用ForEach循环遍历数据集合
        /// </summary>
        static void ForEachDataCollection()
        {
            Console.WriteLine("遍历整数集合");
            var intValues = new List<int>() { 1, 2, 3, 4, 5 };
            foreach (var value in intValues)
            {
                //intValues.Remove(value);
                Console.WriteLine("value = {0}",value);
                Console.WriteLine(value);
            }
            /*-----------我是分割线------------*/
            Console.WriteLine("\n遍历对象集合");
            var myClasses = new List<MyClass>();
            for (int i = 0; i < 5; i++)
            {
                myClasses.Add(new MyClass()
                {
                    Id = i,
                    Description = "MyClass对象" + i
                });
            }
            foreach (var obj in myClasses)
            {
                Console.WriteLine("{1}:{0}", obj.Description, obj.Id);
            }
            /*-----------我是分割线------------*/
            Console.WriteLine("\n遍历一个数组");
            int[] numbers = new int[] { 1, 2, 3, 4, 5 };
            foreach (var number in numbers)
            {
                Console.WriteLine(number);
            }
            /*-----------我是分割线------------*/
            Console.WriteLine("\n遍历一个字符串数组");
            string[] str = new string[] { "ab", "ac", "bc" };
            foreach (var item in str)
            {
                Console.WriteLine(item);
            }
        }
        #endregion
    }
}
