using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise
{
    #region "练习一 2-4+6-8+10-...+100 = 50"
    public struct ElementOfArithmeticProgression
    {
        public int beginNumber;
        public int endNumber;
        public int commonDifference;
    }
    class SumOfArithmeticProgression//等差数列的和
    {
        public static int Sum(ElementOfArithmeticProgression e)
        {
            int sum = 0;
            for (int i = e.beginNumber; i <= e.endNumber; i += e.commonDifference)
            {
                sum += i;
            }
            return sum;
        }
    }
    #endregion
}
