using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLib
{
    public class Class1
    {
        //求一个数的倍数
        public int DoubleValue(int i)
        {
           // return i * i;
           return i * 2;
        }

        //将字串转为大写
        private string ToUpper(string str)
        {
            return str.ToUpper();
        }

    }
}
