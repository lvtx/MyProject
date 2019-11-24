
using System.Collections.Generic;
namespace ObjectEmbodied
{

    class Program
    {
        static void Main(string[] args)
        {
            One2OneTest();

            One2ManyTest();
        }

        private static void One2OneTest()
        {
            //创建第一种包容方式的对象
            //其内部已自动创建好包容的B对象
            //但外界不能直接访问
            OneToOneClass obj = new OneToOneClass();

            //使用第一种包容方式的对象
            //……
            //当obj所引用的对象被销毁时，其内部对象也同时被销毁

            //实现第二种对象组合方式
            OneToOneClass2 container = new OneToOneClass2(new InnerClass());

            //使用第二种包容方式的对象
            //……
            //container和obj所引用的对象其生命周期是相互独立的，
            //一个对象被销毁不会自动导致另一个对象也被销毁
        }

        private static void One2ManyTest()
        {
            OneToManyClass outer = new OneToManyClass();
            //添加5个对象
            for (int i = 0; i < 5; i++)
            {
                outer.Add(new InnerClass());
            }
            //移除第1个对象
            outer.Remove(0);

            List<InnerClass> innerObjs = new List<InnerClass>();
            //添加5个对象
            for (int i = 0; i < 5; i++)
            {
                innerObjs.Add(new InnerClass());
            }
            OneToManyClass2<InnerClass> outer2 = new OneToManyClass2<InnerClass>(innerObjs);
        }


    }


}
