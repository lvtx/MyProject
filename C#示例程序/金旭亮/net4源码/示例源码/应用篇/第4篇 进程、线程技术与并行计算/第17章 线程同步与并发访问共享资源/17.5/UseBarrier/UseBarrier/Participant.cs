using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UseBarrier
{
    /// <summary>
    /// 张三
    /// </summary>
    public class ZhangSan:IParticipant
    {
        /// <summary>
        /// 用于生成随机暂停的时间
        /// </summary>
        private Random ran = new Random();

        public void Go()
        {

            Console.WriteLine("张三出门,挤公交车……");
            //随机暂停1到3秒
            Thread.Sleep(ran.Next(1000,3000));
            Console.WriteLine("张三下了公交车,到了地铁站,买票坐地铁……");
             //随机暂停1到3秒
            Thread.Sleep(ran.Next(1000,3000)); 
            Console.WriteLine("张三到了鸟巢大门……");
            Program.barrier.SignalAndWait();  //等待会合
            //开始参观
            Console.WriteLine("张三在参观……");
            //随机暂停1到3秒
            Thread.Sleep(ran.Next(1000, 3000)); 
             Program.barrier.SignalAndWait(); //参观结束
             Console.WriteLine("张三离开了鸟巢,到了地铁站,买票坐地铁回家……");
             //随机暂停1到3秒
             Thread.Sleep(ran.Next(1000, 3000));
             Console.WriteLine("张三离开了地铁站,坐公交车回家……");
 //随机暂停1到3秒
             Thread.Sleep(ran.Next(1000, 3000));
              Console.WriteLine("张三回到家了！");
              Program.barrier.SignalAndWait();//回到家了！

        }
    }

    /// <summary>
    /// 李四
    /// </summary>
    public class LiSi : IParticipant
    {
        /// <summary>
        /// 用于生成随机暂停的时间
        /// </summary>
        private Random ran = new Random();

        public void Go()
        {

            Console.WriteLine("李四出门,到了地铁站,买票坐地铁……");
            //随机暂停1到3秒
            Thread.Sleep(ran.Next(1000, 3000));
            Console.WriteLine("李四到了鸟巢大门……");
            Program.barrier.SignalAndWait();  //等待会合
            //开始参观
            Console.WriteLine("李四在参观……");
            //随机暂停1到3秒
            Thread.Sleep(ran.Next(1000, 3000));
            Program.barrier.SignalAndWait(); //参观结束
            Console.WriteLine("李四离开了鸟巢,到了地铁站,买票坐地铁回家……");
            //随机暂停1到3秒
            Thread.Sleep(ran.Next(1000, 3000));
            Console.WriteLine("李四回到家了！");
            Program.barrier.SignalAndWait();//回到家了！
        }
    }

    /// <summary>
    /// 王五
    /// </summary>
    public class WangWu : IParticipant
    {
        /// <summary>
        /// 用于生成随机暂停的时间
        /// </summary>
        private Random ran = new Random();

        public void Go()
        {
 
            Console.WriteLine("王五出门,步行去鸟巢……");
            //随机暂停1到3秒
            Thread.Sleep(ran.Next(1000, 3000));
            Console.WriteLine("王五到了鸟巢大门……");
            Program.barrier.SignalAndWait();  //等待会合
            //开始参观
            Console.WriteLine("王五在参观……");
            //随机暂停1到3秒
            Thread.Sleep(ran.Next(1000, 3000));
            Program.barrier.SignalAndWait(); //参观结束
            Console.WriteLine("王五离开了鸟巢,步行回家……");
            //随机暂停1到3秒
            Thread.Sleep(ran.Next(1000, 3000));
            Console.WriteLine("王五回到家了！");
            Program.barrier.SignalAndWait();//回到家了！
        }
    }
}
