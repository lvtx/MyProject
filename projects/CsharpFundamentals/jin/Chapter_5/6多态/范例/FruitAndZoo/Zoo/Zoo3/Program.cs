using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo3
{
    class Program
    {
        static void Main(string[] args)
        {
            //动物数组
            var ans = new List<Animal> { 
                new Monkey(), new Pigeon(), new Lion() 
            };

            Feeder f = new Feeder();
            f.Name = "小李";
            //喂养一群动物
            f.FeedAnimals(ans);

        }
    }
}
