using System;
using System.Collections.Generic;
using System.Text;

namespace StaticAndInstanceField
{
    class MyClass
    {
        //¾²Ì¬×Ö¶Î
        public static int staticVar=0;
        //¾²Ì¬·½·¨   
        public static void staticFunc() 
        {
            MyClass obj = new MyClass();
            obj.increaseValue();
            obj.dynamicVar++;
        }
        //ÊµÀý×Ö¶Î
        public int dynamicVar=0;
        //ÊµÀý·½·¨
        public void increaseValue()
        {
            staticVar++;
            dynamicVar++;
        }
    }
}
