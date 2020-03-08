using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            UseFindMethod();
        }
        static void UseFindMethod()
        {
            using (var context = new MyDBEntities())
            {
                var clients = from q in context.OrderClients
                            where q.ClientID > 10
                            select q;
                Console.WriteLine("共有{0}条记录",clients.Count());
            }
            Console.ReadLine();
        }
    }
}
