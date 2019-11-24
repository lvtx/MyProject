using System;
using System.Collections.Generic;
using System.Text;

namespace Zoo3
{
    //动物园饲养员
    class Feeder
    {
        public String Name;
        //喂养一群动物
        public void FeedAnimals(IEnumerable<Animal> ans)
        {
            foreach (Animal an in ans)
            {
                an.eat();
            }
        }
     }
}
