using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseAnonymousType
{
    class Program
    {
        static void Main(string[] args)
        {
            var v = new { Amount = 108, Message = "Hello" };
            Console.WriteLine("Amount:{0}, Message:{1}", v.Amount, v.Message);
            Console.WriteLine(v);
            Console.ReadKey();


        }
    }
}
