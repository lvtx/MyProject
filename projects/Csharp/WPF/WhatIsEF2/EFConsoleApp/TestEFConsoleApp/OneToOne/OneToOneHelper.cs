using EFConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEFConsoleApp.OneToOne
{
    class OneToOneHelper
    {
        public static Random ran = new Random();
        public static Person CreatePersonWithoutIDCard()
        {
            Person p = new Person
            {
                Name = "普通人" + ran.Next(1, 10000)
            };
            return p;
        }
        public static IdentityCard CreateIndependentIdentityCard()
        {
            IdentityCard card = new IdentityCard()
            {
                IDNumber = CreateIDNumber()
            };
            return card;
        }
        //生成18位随机身份证号码
        public static String CreateIDNumber()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 18; i++)
            {
                sb.Append(ran.Next(0, 10));
            }
            return sb.ToString();
        }
    }
}
