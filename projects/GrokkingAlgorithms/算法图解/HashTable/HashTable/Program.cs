using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{
    class Program
    {
        static void Main(string[] args)
        {
            Hashtable ht = new Hashtable();
            ht.Add("apple", 0.67);//添加一个键值对
            ht.Add("milk", 1.49);
            ht.Add("avocado", 1.49);
            Console.WriteLine(ht["apple"]);
            Console.WriteLine(ht["milk"]);
            if (ht.Contains("avocado"))//检查表里是否有特定的键
            {
                Console.WriteLine("gui");
            }
        }
    }
}
