using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseMMFBetweenProcess
{
    /// <summary>
    /// 要共享的数据结构，注意，其成员不能是引用类型
    /// </summary>
    public struct MyStructure
    {
        public int IntValue
        {
            get;
            set;
        }
        public float FloatValue
        {
            get;
            set;
        }
       
    }

   
}
