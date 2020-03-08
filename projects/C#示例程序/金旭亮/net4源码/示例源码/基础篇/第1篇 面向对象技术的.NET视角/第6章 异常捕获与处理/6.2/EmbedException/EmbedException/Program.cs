using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EmbedException
{
    class Program
    {
        static void Main(string[] args)
        {
            try //Level1
            {
                try //Level2
                {
                    try  //Level3
                    {
                        //to do:每次抛出一种异常，注释掉其余的语句，然后观察输出结果
                        throw new ExceptionA();
                        //throw new ExceptionB();
                        //throw new ExceptionC();
                       // throw new InvalidOperationException();
                    }
                    catch (ExceptionA) //Level3
                    {
                        Console.WriteLine("在Level3中处理ExceptionA。");
                    }
                    finally //Level3
                    {
                        Console.WriteLine("Level3中的finally语句块");
                        //WriteMessageToFile("Level3.txt", "Level3中的finally语句块");
                    }

                }
                catch (ExceptionB)
                {
                    Console.WriteLine("在Level2中处理ExceptionB");
                }
                finally
                {
                    Console.WriteLine("Level2中的finally语句块");
                    //WriteMessageToFile("Level2.txt", "Level2中的finally语句块");
                   
                }
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

        private static void WriteMessageToFile(string FileName,string Message)
        {
            FileStream fs = new FileStream(FileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write("");
            sw.Close();
            fs.Close();
        }


    }

    class ExceptionA : Exception
    {
        
    }

    class ExceptionB : Exception
    {
       
    }

    class ExceptionC : Exception
    {
       
    }

}
