using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace UseATM
{
    class Program
    {
        static ATM OneATM=new ATM(); //共享资源
        static void Main(string[] args)
        {
            //向公共帐号存款2万

            Console.Write("输入公司公共帐户的金额:");
            int PublicAcountMoney =Convert.ToInt32(Console.ReadLine());
            OneATM.Deposit(PublicAcountMoney);

            Console.Write("输入ATM中的现金额:");
            int ATMLeftMoney = Convert.ToInt32(Console.ReadLine());
            OneATM.SetATMLeftMoney(ATMLeftMoney);

            System.Console.WriteLine("\n敲任意键从公共帐户中取钱，ESC键退出……\n");

            while (System.Console.ReadKey(true).Key !=ConsoleKey.Escape)
            {
                System.Console.WriteLine("");
                Thread Jia = new Thread(WithDrawMoney);     //职员甲
                Thread Yi = new Thread(WithDrawMoney);     //职员乙
                Thread Bing = new Thread(WithDrawMoney);   //职员丙

                //随机生成一个要提款的数额，最少100元，最高5000元
                Random ran = new Random();
                Jia.Start(ran.Next(100, 5000));
                Yi.Start(ran.Next(100, 5000));
                Bing.Start(ran.Next(100, 5000));

                //等三人取完钱
                Jia.Join();
                Yi.Join();
                Bing.Join();

                System.Console.WriteLine("公共账号剩余{0}元，ATM中可提现金：{1}", OneATM.QueryPublicAccount(),OneATM.QueryATMLeftAccount());
            }
        }

        //线程函数
        static void WithDrawMoney(object amount)
        {
            switch(OneATM.WithDraw((int)amount))
            {
                case WithDrawState.Succeed:
                    System.Console.WriteLine("成功取出{0}元。",amount );
                    break;
                case WithDrawState.ATMHasNotEnoughCash:
                    System.Console.WriteLine("ATM中现金不足，无法支取{0}元。", amount);
                    break ;
                case WithDrawState.AccountHasNotEnoughMoney:
                    System.Console.WriteLine("帐户中没钱了！无法取出{0}元",amount);
                    break ;
            }
                
        }

    }

    //自助取款机
    class ATM
    {
        private int PublicAcountLeftMoney;//帐户剩余的钱
        private int ATMLeftMoney;//提款机剩余的钱
        //同步信息号量
        private Mutex m = new Mutex();
        //取钱
        public WithDrawState WithDraw(int amount)
        {
            m.WaitOne();
            //公共帐号钱不够
            if (PublicAcountLeftMoney < amount)
            {
                m.ReleaseMutex();
                return WithDrawState.AccountHasNotEnoughMoney;
            }
            //ATM现金不够
            if (ATMLeftMoney < amount)
            {
                m.ReleaseMutex();
                return WithDrawState.ATMHasNotEnoughCash;
            }

            //用户可以提取现金
            ATMLeftMoney -= amount;
            PublicAcountLeftMoney -= amount;
            m.ReleaseMutex();
            return WithDrawState.Succeed;

        }
        //存钱
        public void Deposit(int amount)
        {
            m.WaitOne();
            PublicAcountLeftMoney += amount;
            m.ReleaseMutex();
        }

        /// <summary>
        /// 设置ATM的现金金额
        /// </summary>
        /// <param name="amount"></param>
        public void SetATMLeftMoney(int amount)
        {
            Interlocked.Exchange(ref ATMLeftMoney, amount);
        }
        //获取还剩余多少钱
        public int QueryPublicAccount()
        {
            return PublicAcountLeftMoney;
        }

        /// <summary>
        /// 查询ATM剩余多少钱
        /// </summary>
        /// <returns></returns>
        public int QueryATMLeftAccount()
        {
            return ATMLeftMoney;
        }
    }
    //取款状态
    public enum WithDrawState
    {
        Succeed,        //取钱成功
        AccountHasNotEnoughMoney, //账号中没钱了
        ATMHasNotEnoughCash  //ATM中没有足够的现金
    }
}
