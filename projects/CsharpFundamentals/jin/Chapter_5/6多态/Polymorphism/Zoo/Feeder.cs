using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    class Feeder
    {
        public String Name;

        #region "第一版这里太繁琐了"
        ////喂猴子
        //public void FeedMonkey(Monkey m)
        //{
        //    m.eat();
        //}
        ////喂鸽子
        //public void FeedPigeon(Pigeon p)
        //{
        //    p.eat();
        //}
        ////喂狮子
        //public void FeedLion(Lion l)
        //{
        //    l.eat();
        //}
        #endregion
        #region "第二版"
        //public void FeedAnmial(Anmial anmial)
        //{
        //    anmial.eat();
        //}
        #endregion
        #region "使用接口"
        public void FeedAnmial(Anmial anmial)
        {
            anmial.eat();
        }
        #endregion
    }
}
