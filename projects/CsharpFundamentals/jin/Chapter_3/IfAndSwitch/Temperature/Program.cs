using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temperature
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请输入一个温度");
            string UseInput = Console.ReadLine();
            int UseInputTemperature = int.Parse(UseInput);
            ComfortLevel(UseInputTemperature);
            Console.ReadKey();
        }
        static void ComfortLevel(int temperature)
        {
            if(temperature > 28 && temperature <= 40)
            {
                Console.WriteLine("太热了");
            }
            if(temperature > 20 && temperature <=28)
            {
                Console.WriteLine("真舒服");
            }
            if(temperature > 0 && temperature <= 20)
            {
                Console.WriteLine("太冷了");
            }
        }
    }
}
