using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEmbed
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }
        private static void TestOneToOne()
        {
            OneToOneClass obj = new OneToOneClass();
            OneToOneClass2 container = new OneToOneClass2(new InnerClass());
        }
        private static void TestOneToMany()
        {
            OneToManyClass outer = new OneToManyClass();
            for (int i = 0; i < 5; i++)
            {
                outer.Add(new InnerClass());
            }
            outer.Remove(0);

            List<InnerClass> inner = new List<InnerClass>();
            for (int i = 0; i < 5; i++)
            {
                inner.Add(new InnerClass());
            }
            OneToManyClass2<InnerClass> outer2 = new OneToManyClass2<InnerClass>(inner);
        }
    }
}
