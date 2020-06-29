using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;


namespace PropertyImportExport
{
    class Program
    {
        static void Main(string[] args)
        {
 
            //创建部件，并填充其字段集合
            MyPart<int> part = new MyPart<int>();
            for (int i = 0; i < 5; i++)
                part.numbers.Add(i);
            //创建部件宿主
            Host<int> host = new Host<int>();

            //装配部件
            CompositionContainer container = new CompositionContainer();
            container.ComposeParts(host,part);

            ////验证装配是否成功
            foreach (int num in host.collection)
                Console.Write("{0},", num);//输出结果：0,1,2,3,4,

            Console.ReadKey();
        }
    }
}
