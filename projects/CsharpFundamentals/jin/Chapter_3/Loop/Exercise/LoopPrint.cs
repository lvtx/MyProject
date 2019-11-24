using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise
{
    
    class LoopPrint
    {
        #region "打印直角三角形排列的星号"
        public static void PrintAngle()
        {
            for (int i = 1; i <= 6; i++)
            {
                for (int j = 1; j <= i; j++)
                {
                    Console.Write("* ");
                }
                Console.WriteLine();
            }
        }
        #endregion

        #region "打印我也不知道怎么描述的排列"
        public static void PrintRectangle()
        {
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j < 14; j++)
                {
                    if (j == 7)
                    {
                        Console.Write(" \n ");
                        continue;
                    }
                    Console.Write("* ");
                }
                Console.Write("\n");
            }
        }
        #endregion
    }
}
