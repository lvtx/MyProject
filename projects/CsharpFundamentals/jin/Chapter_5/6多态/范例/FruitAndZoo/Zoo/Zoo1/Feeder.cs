using System;
using System.Collections.Generic;
using System.Text;

namespace Zoo1
{
    //����԰����Ա
    class Feeder
    {
        public String Name;

        //ι����
        public void FeedMonkey(Monkey m)
        {
            m.eat();
        }
        //ι����
        public void FeedPigeon(Pigeon p)
        {
            p.eat();
        }
        //ιʨ��
        public void FeedLion(Lion l)
        {
            l.eat();
        }
     }
}
