using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticAndInstanceField
{
    // 静态类
    static class StaticClass
    {
        // 公有静态字段
        public static int staticField = 100;
        // 公有静态属性（同时初始化为“Hello”，适用于C# 6）
        public static string staticProp
        {
            get; set;
        } = "Hello";
        
        // 静态方法
        public static void staticFunc()
        {
            Console.WriteLine("调用静态方法");
        }
   
        // 私有静态字段，不能被外界访问
        // 初始化主要通过静态构造方法或直接赋初值。
        private static int SecretField = 0;
        
        // 静态构造方法
        static StaticClass()
        {
            SecretField = 100;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            #region ".NET基类库中的数学函数"
            //计算90度的余弦值
            int angle = 90;
            //转为弧度
            double radian = angle * Math.PI / 180.0;
            //调用数学库函数进行计算
            Console.WriteLine(Math.Cos(radian));
            #endregion

            #region "自定义静态类型"
            //静态类型无法创建对象
            //var obj = new StaticClass();

            StaticClass.staticField += 100;
            Console.WriteLine("staticField={0}",StaticClass.staticField);
            //访问静态属性
            StaticClass.staticProp +=" World";
            Console.WriteLine("staticProp={0}",StaticClass.staticProp);
            //访问静态方法
            StaticClass.staticFunc();
            #endregion

            Console.ReadKey();
        }
    }
}
