using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zoo
{
    class Program
    {
        static void Main(string[] args)
        {
            //动物数组
            Animal[] ans = { new Monkey(), new Pigeon(), new Lion() };

            Feeder f = new Feeder() { Name = "小李" };

            f.FeedAnimals(ans);

            Console.ReadKey();
        }
    }
}
