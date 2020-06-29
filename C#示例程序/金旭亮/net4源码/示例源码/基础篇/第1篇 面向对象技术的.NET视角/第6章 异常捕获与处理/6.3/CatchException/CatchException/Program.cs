using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatchException
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                int number = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("finally");
            }

        }
    }
}
