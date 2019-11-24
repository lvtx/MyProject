using System;
using System.Collections.Generic;
using System.Text;

namespace Zoo1
{
    //动物园饲养员
    class Feeder
    {
        public String Name;

        //喂猴子
        public void FeedMonkey(Monkey m)
        {
            m.eat();
        }
        //喂鸽子
        public void FeedPigeon(Pigeon p)
        {
            p.eat();
        }
        //喂狮子
        public void FeedLion(Lion l)
        {
            l.eat();
        }
     }
}
