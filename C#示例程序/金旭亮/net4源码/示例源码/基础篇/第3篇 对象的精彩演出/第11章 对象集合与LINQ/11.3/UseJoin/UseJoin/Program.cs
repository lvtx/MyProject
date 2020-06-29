using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseJoin
{
   
    class A
    {
        public int AID;
        public string AName;
    }

    class B
    {
        public int BID;
        public string BName;
    }

    class Program
    {
        static void Main(string[] args)
        {
            A[] arrA = new A[10];
            B[] arrB = new B[10];
            Random ran = new Random();
            for (int i = 0; i < 10; i++)
            {
                arrA[i] = new A() { AID = i, AName = "A" + i.ToString() };
                arrB[i] = new B() { BID = i, BName = "B" + i.ToString() };
            }
            var result = arrA.Join(   //指明连接的第一个集合对象arrA
                arrB,                 //指明连接的第二个集合对象arrB
                Aobj => Aobj.AID,     //指明第一个对象集合中的对象A的AID字段作为连接字段
                Bobj => Bobj.BID,     //指明第二个对象集合中的对象B的BID字段作为连接字段
                (Aobj, Bobj) => new 　//指明结果以哪种方式返回
                {
                    Aobj.AID,
                    Aobj.AName,
                    Bobj.BName
                }
            );


            foreach (var elem in result)
                Console.WriteLine("{0},{1},{2}", elem.AID, elem.AName, elem.BName);
            Console.ReadKey();

        }
    }
}
