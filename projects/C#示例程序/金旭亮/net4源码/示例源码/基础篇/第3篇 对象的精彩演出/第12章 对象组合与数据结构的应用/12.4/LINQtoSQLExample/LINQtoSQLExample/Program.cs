using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LINQtoSQLExample
{
    class Program
    {
        static void Main(string[] args)
        {
            NorthWndDataContext db = new NorthWndDataContext();
            var query = from cust in db.Customers
                        where cust.City == "Berlin"
                        select cust;
            foreach (var cust in query)
                Console.WriteLine(cust.CompanyName);
            Console.ReadKey();
        }
    }
}
