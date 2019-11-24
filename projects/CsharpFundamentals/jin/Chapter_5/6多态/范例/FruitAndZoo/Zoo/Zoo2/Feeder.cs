using System;
using System.Collections.Generic;
using System.Text;

namespace Zoo2
{
   

        //动物园饲养员
        class Feeder
        {
            public String Name;
            
            public void FeedAnimal(Animal animals)
            {
                animals.eat();
            }
        }

    
}
