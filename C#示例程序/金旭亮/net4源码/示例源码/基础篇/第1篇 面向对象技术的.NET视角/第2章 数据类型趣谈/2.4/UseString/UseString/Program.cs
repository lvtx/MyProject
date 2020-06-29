using System;
using System.Collections.Generic;
using System.Text;

namespace UseString
{
    class Program
    {
        static void Main(string[] args)
        {


            String s1 = "aaaa";//"aaaa"在驻留池中，s1引用它
            String s2 = new String('a', 4);//“aaaa”在托管堆上，s2引用它
            //将"aaaa"加入驻留池，由于原来已有一个同样的字串在池中
            //不再重复加入，s3引用驻留池的这一字串
            String s3 = String.Intern(s2);
            //比对s1和s2是否指向同一字串
            Console.WriteLine((Object)s1 == (object)s2);//输出：False
            //比对s1和s3是否指向同一字串
            Console.WriteLine((Object)s1 == (object)s3);//输出：True

            Console.ReadKey();
        }
    }
}
