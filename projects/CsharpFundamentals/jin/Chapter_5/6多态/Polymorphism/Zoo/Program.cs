using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    class Program
    {
        static void Main(string[] args)
        {
            Feeder feeder = new Feeder();
            feeder.Name = "小王";
            feeder.FeedAnmial(new Lion());
            Console.ReadKey();
        }
    }
}


