using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zoo
{
    //动物园饲养员
    class Feeder
    {
        public String Name;

        public void FeedAnimal(Animal animal)
        {
            animal.Eat();
        }

        //喂养一群动物
        public void FeedAnimals(Animal[] ans)
        {
            foreach (Animal an in ans)
            {
                an.Eat();
            }
        }
    }
}
