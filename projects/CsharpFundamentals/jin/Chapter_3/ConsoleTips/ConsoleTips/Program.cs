using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTips
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConsoleRelevant.TestKey();
            //ConsoleRelevant.QuitConsoleApp();
            ConsoleRelevant.DisableControlC();
            Console.ReadKey(true);
        }
    }
}
