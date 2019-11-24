using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    interface Anmial
    {
        void eat();
    }
    
    class Lion : Anmial
    {
        public void eat()
        {
            Console.WriteLine("吃肉");
        }
    }
    class Monkey : Anmial
    {
        public void eat()
        {
            //吃香蕉
            Console.WriteLine("我是猴子，我喜欢偷吃香蕉！");
        }
    }
    //鸽子
    class Pigeon : Anmial
    {
        public void eat()
        {
            //吃大米
            Console.WriteLine("我是一只漂亮的鸽子，为了维持优美的体形，我每餐只吃几粒大米！");
        }
    }
}
