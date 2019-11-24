using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAndDataType
{
    class Program
    {
        static void Main(string[] args)
        {
            //StringOrstring();
            //UseVarKeyWord();
            //DataTypeLength();
            //ValueTypeConvert();
            //StringAndNumberType();
            UseOperator();
            Console.ReadKey();
        }

        #region "变量与数据类型"
        /// <summary>
        /// 变量的定义与变量的类型
        /// </summary>      
        static void DataType()
        {
            int intValue = 100;
            long longValue = 100;
            float floatValue = 100.5f;
            double doubleValue = 100.5d;
            Console.WriteLine(intValue.GetType());
            Console.WriteLine(longValue.GetType());
            Console.WriteLine(floatValue.GetType());
            Console.WriteLine(doubleValue.GetType());
            Console.WriteLine(intValue.GetType() == typeof(int));
        }
        /// <summary>
        /// 使用string还是String
        /// <summary>
        static void StringOrstring()
        {
            string str1 = "1";
            string str2 = "2";
            Console.WriteLine(str1.GetType());
            Console.WriteLine(str2.GetType());
            Console.WriteLine(typeof(string) == typeof(String));
        }
        /// <summary>
        /// 隐式类型变量定义
        /// <summary>
        static void UseVarKeyWord()
        {
            var value = 100;
            Console.WriteLine(value.GetType());
        }
        /// <summary>
        /// 特定数值类型所占内存单元
        /// </summary>
        static void DataTypeLength()
        {
            Console.WriteLine("int length is {0}",sizeof(int));
            Console.WriteLine("long length is {0}",sizeof(long));
            Console.WriteLine("float length is {0}",sizeof(float));
            Console.WriteLine("double length is {0}",sizeof(double));
        }
        /// <summary>
        /// 数值类型之间的转换
        /// </summary>
        static void ValueTypeConvert()
        {
            int intValue = 100;
            long longValue = 100;
            float floatValue = 100.5f;
            double doubleValue = 100.5d;

            intValue = (int)longValue;//强制类型转换
            longValue = intValue;//隐式类型转换
            doubleValue = floatValue;
        }
        /// <summary>
        /// 字符串与数值类型之间的转换
        /// </summary> 
        static void StringAndNumberType()
        {
            string stringValue = "100";
            int intValue = int.Parse(stringValue);
            double doubleValue = double.Parse(stringValue);
            intValue = Convert.ToInt32(stringValue);
            Console.WriteLine(intValue);
            Console.WriteLine(doubleValue);
            Console.WriteLine();
        }
        #endregion

        #region "运算符与表达式"
        /// <summary>
        /// 使用运算符
        /// </summary>
        static void UseOperator()
        {
            int result = ((5 + 3) * 6) % 7;
            Console.WriteLine(result);
            int a = 1;
            Console.WriteLine(a++);//先取出来，再加一。输出1，a=2
            Console.WriteLine(++a);//先加一，再取出来。输出3，a=3
        }
        /// <summary>
        /// 使用表达式
        /// </summary>
        static void UseExpress()
        {

        }
        #endregion
    }
}

