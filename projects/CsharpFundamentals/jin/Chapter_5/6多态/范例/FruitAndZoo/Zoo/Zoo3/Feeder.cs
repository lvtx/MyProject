using System;
using System.Collections.Generic;
using System.Text;

namespace Zoo3
{
    //����԰����Ա
    class Feeder
    {
        public String Name;
        //ι��һȺ����
        public void FeedAnimals(IEnumerable<Animal> ans)
        {
            foreach (Animal an in ans)
            {
                an.eat();
            }
        }
     }
}
