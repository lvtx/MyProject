using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseBarrier
{
    /// <summary>
    /// 线程参与者必须实现的接口
    /// </summary>
    public interface IParticipant
    {
        void Go();
    }
    
}
