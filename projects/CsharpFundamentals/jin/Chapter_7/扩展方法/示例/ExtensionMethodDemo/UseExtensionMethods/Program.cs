using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseExtensionMethods
{
    /// <summary>
    /// 将被“动态”扩展的原始类型
    /// </summary>
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    /// <summary>
    /// 存放扩展方法的类
    /// </summary>
    static class PersonExtensions
    {
        public static void SayHello(this Person person)
        {
            Console.WriteLine("{0}说：“你好！”", person.Name);
        }
        public static void SayHelloTo(this Person p1, Person p2)
        {
            Console.WriteLine("{0}对{1}说：“您吃了吗？”",p1.Name,p2.Name);
        }

        public static bool IsOlderThan(this Person p1, Person p2)
        {
            return p1.Age > p2.Age;
        }
    }
   
    

    class Program
    {
        static void Main(string[] args)
        {
            var person = new Person()
            {
                Name = "张三",
                Age = 30
            };
            var person2 = new Person()
            {
                Name = "李四",
                Age = 40
            };
            person.SayHello();
            person.SayHelloTo(person2);
            if (person.IsOlderThan(person2))
            {
                Console.WriteLine("{0}比{1}年纪大",person.Name,person2.Name);
            }
            else
            {
                Console.WriteLine("{0}并不比{1}年纪大", person.Name, person2.Name);
            }
            Console.ReadKey();

        }
    }
}
