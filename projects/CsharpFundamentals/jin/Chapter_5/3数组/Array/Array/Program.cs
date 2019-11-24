using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Array
{
    #region "问题1"
    //编写一个控制台程序，程序运行时随机生成10个数，填充
    //一个数组，然后遍历显示数组内容，接着计算数组元素的
    //和，求出其平均值，最大值，最小值，中位数，显示在控
    //制台窗口中。
    #endregion
    #region "问题二"
    //请编写一个程序将一个整数转换为汉字读法字符串。
    //比如“1123”转换为“一千一百二十三”。
    #endregion
    class Program
    {
        static void Main(string[] args)
        {
            #region "问题一的调用"
            //Random ran = new Random(System.Environment.TickCount);
            //int[] intArray = new int[10];
            //for (int i = 0; i < intArray.Length; i++)
            //{
            //    intArray[i] = ran.Next(1, 100);
            //}
            //ShowArray(intArray);
            //double avg = MathFunctionClass.Avg(intArray);
            //Console.WriteLine("平均值 = {0}",avg);
            //int max = MathFunctionClass.Max(intArray);
            //Console.WriteLine("最大值 = {0}",max);
            //int min = MathFunctionClass.Min(intArray);
            //Console.WriteLine("最小值 = {0}",min);
            //double med = MathFunctionClass.Median(intArray);
            //Console.WriteLine("中位数 = {0}",med);
            #endregion
            NumConvertToCHS.UserInput();
            NumConvertToCHS.ConsoleOutput();
            NumConvertToCHS numConvertToCHS = new NumConvertToCHS();
            numConvertToCHS.Initialization();//先初始化
            //string chsNumber = numConvertToCHS.ConvertNum();
            //Console.WriteLine(chsNumber);
            Console.ReadKey();
        }
        #region "遍历数组打印"
        private static void ShowArray(int[] intArray)
        {
            foreach (var item in intArray)
            {
                Console.Write("{0},",item);
            }
        }
        #endregion
    }
}
