using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLinkInRealWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("程序从Main()方法开始运行……\n");
            try
            {
                throwExceptionMethod();  //有可能抛出异常的方法调用
            }
            catch (MyException e)
            {
                //e.Message是子类调用父类的Message属性，其值在此处
                //throw new MyException("在throwExceptionMethod()方法执行时出现MyException异常",e);
                //赋值给父类中的相关属性
                Console.WriteLine("在Main()方法中捕获到MyException异常，其Message属性值为：{0}",e.Message);
                Console.WriteLine(e.InnerException.Message);

            }
            catch (Exception e)
            {
                Console.WriteLine("在Main()方法中捕获到Exception异常，其Message属性值为：{0}", e.Message);
            }
            doesNotThrowException(); //不抛出异常的方法调用
            Console.WriteLine("\nMain()方法运行结束，敲任意键退出……");
            Console.ReadKey();
        }
        /// <summary>
        /// 自己虽然捕获了异常，但仍然希望外部进一步地处理，因此在简单地
        /// 捕获并处理老异常之后，再抛出一个新的异常供本方法的调用者进行捕获。
        /// </summary>
        public static void throwExceptionMethod()
        {
            try
            {
                Console.WriteLine("throwExceptionMethod()方法开始执行");
                Console.WriteLine("throwExceptionMethod()方法抛出了一个异常");

                // 模拟产生一个异常
                throw new Exception("系统运行时引发的Exception异常");
            }
            catch (Exception e)
            {
                Console.WriteLine("throwExceptionMethod方法捕获并处理了抛出的Exception异常，并将其转换为一个自定义MyException异常再抛出");

                //转换为一个自定义异常，再抛出
                throw new MyException("在throwExceptionMethod()方法执行时出现MyException异常",e);
            }
            finally
            {
                Console.WriteLine("throwExceptionMethod()方法中的finally语句块执行结束\n");

            }           
        }
        /// <summary>
        /// 自己能完全处理异常，不需要外界参与
        /// </summary>
        public static void doesNotThrowException()
        {
            try
            {
                Console.WriteLine("\ndoesNotThrowException()方法开始执行");
                Console.WriteLine("doesNotThrowException()方法虽然包容try/catch/finally，但不会抛出任何异常");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("执行doesNotThrowException()方法中的finally语句块");
            }
            Console.WriteLine("doesNotThrowException()方法运行结束。\n");
        }
    }

    class MyException : Exception
    {
        public MyException(String Message) : base(Message)
        {

        }
        public MyException(string Message, Exception InnerException) : base(Message, InnerException)
        {

        }
    }
}
