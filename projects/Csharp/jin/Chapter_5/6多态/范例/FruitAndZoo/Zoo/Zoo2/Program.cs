﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo2
{
    class Program
    {
        static void Main(string[] args)
        {
            Monkey m = new Monkey();
            Pigeon p = new Pigeon();
            Lion l = new Lion();

            Feeder f = new Feeder();
            f.Name = "小李";

            f.FeedAnimal(m);//喂猴子
            f.FeedAnimal(p);//喂鸽子
            f.FeedAnimal(l);//喂狮子

            Console.ReadKey();
        }
    }
}
