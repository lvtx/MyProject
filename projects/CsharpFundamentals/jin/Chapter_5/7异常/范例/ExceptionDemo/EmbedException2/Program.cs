using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4_EmbedException2
{
    #region "自定义异常类型"
    class ExceptionA : Exception
    {

    }

    class ExceptionB : Exception
    {

    }

    class ExceptionC : Exception
    {

    }

    #endregion

    class Program
    {
        static void Main(string[] args)
        {

            try //Level1
            {
                Console.WriteLine("进入Level1的try语句块");
                try //Level2
                {
                    Console.WriteLine("进入Level2的try语句块");
                    //如果在Level3代码执行之前（和之后）出现异常，Level3的finally语句块还会执行吗？
                    //throw new ExceptionB();
                    try  //Level3
                    {
                        Console.WriteLine("进入Level3的try语句块");
                        //to do:每次抛出一种异常，注释掉其余的语句，然后观察输出结果
                        //throw new ExceptionA();
                        //throw new ExceptionB();
                        //throw new ExceptionC();
                        //throw new InvalidOperationException();
                        Console.WriteLine("退出Level3的try语句块");
                    }
                    catch (ExceptionA) //Level3
                    {
                        Console.WriteLine("在Level3中处理ExceptionA。");
                    }
                    finally //Level3
                    {
                        Console.WriteLine("Level3中的finally语句块");
                        WriteMessageToFile("Level3.txt", "Level3中的finally语句块");
                    }

                    //throw new ExceptionB();
                    Console.WriteLine("退出Level2的try语句块");
                }
                catch (ExceptionB)
                {
                    Console.WriteLine("在Level2中处理ExceptionB");
                }
                finally
                {
                    Console.WriteLine("Level2中的finally语句块");
                    WriteMessageToFile("Level2.txt", "Level2中的finally语句块");

                }
                Console.WriteLine("退出Level1的try语句块");
            }
            catch (ExceptionC)
            {
                Console.WriteLine("在Level1中处理ExceptionC ");
            }
            //当示例抛出注释InvalidOperationException时，注释掉此块，查看CLR的异常处理策略
            catch (Exception)
            {
                Console.WriteLine("在Level1中处理Exception ");
            }
            finally
            {
                Console.WriteLine("Level1中的finally语句块");
                WriteMessageToFile("Level1.txt", "Level1中的finally语句块");
            }
            Console.ReadKey();//程序暂停
        }

        /// <summary>
        /// 将异常信息写入到文件中，以备日后查询
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Message"></param>
        private static void WriteMessageToFile(string FileName, string Message)
        {
            FileStream fs = new FileStream(FileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(Message);
            sw.Close();
            fs.Close();
        }
    }
}